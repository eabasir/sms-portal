
var table;
function getContactsDataFromDB() {

    waitingDialog.show('لطفا صبر کنید...');
    $('#fileContainer').hide();

    if (table == null)
        table = myDataTable('#myDataTable');

    $.ajax({
        type: "POST",
        url: getContactsFromDBURL,
        datatype: "json",
        timeout: 600000,
        success: function (data) {
            body = "";
            var jsonData = JSON.parse(data);

            table.clear().draw();

            for (var i = 0; i < jsonData.length; i++) {
                var contact = jsonData[i];

                for (var j = 0 ; j < contact.Numbers.length; j++) {

                    var Number = contact.Numbers[j];

                    var addButton = "<td><button type='button' class='btn btn-primary btn-addon btn-sm' onClick='addToTags( \"" + contact.Name + " " + contact.Family + "\",\"" + Number + "\");'><i class='fa fa-plus'></i></button></td>";
                    table.row.add([
                       "",
                       contact.Name,
                       contact.Family,
                       contact.Email,
                       Number,
                       addButton
                    ]).draw(false);
                }

            }
            waitingDialog.hide()
        },
        error: function (data) {
            waitingDialog.hide();
        }

    });
}

$(function () {
    $("input#importFile").change(function () {

        if (window.FormData !== undefined) {
            var fileUpload = $("#importFile").get(0);
            var files = fileUpload.files;

            if (files.length > 0) {
                waitingDialog.show('لطفا صبر کنید...');


                var fileData = new FormData();

                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i]);
                }

                $.ajax({
                    url: uploadFileURL,
                    type: "POST",
                    timeout: 600000,
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function (data) {
                        try {

                            body = "";
                            var jsonData = JSON.parse(data);

                            for (var i = 0; i < jsonData.length; i++) {
                                var contact = jsonData[i];

                                for (var j = 0 ; j < contact.Numbers.length ; j++) {

                                    var addButton = "<td><button type='button' class='btn btn-primary btn-addon btn-sm' onClick='addToTags( \"" + contact.Name + " " + contact.Family + "\",\"" + contact.Numbers[j] + "\");'><i class='fa fa-plus'></i></button></td>";

                                    table.row.add([
                                       "",
                                       contact.Name,
                                       contact.Family,
                                       contact.Email,
                                       contact.Numbers[j],
                                       addButton
                                    ]).draw(false);
                                }
                            }

                        }
                        catch (Err) {
                            console.log(Err);
                        }
                        waitingDialog.hide();
                    },
                    error: function (err) {
                        waitingDialog.hide();
                        alert(err.statusText);
                    }

                });
            }

        } else {
            alert("ورژن مرورگر شما برای استفاده از این پرتال قدیمی است.");
        }


    });
});


function getCompanyList() {

    waitingDialog.show('لطفا صبر کنید...');
    $.ajax({
        type: "POST",
        url: getCompanyListURL,
        datatype: "json",
        timeout: 600000,
        success: function (data) {

            var jsonData = JSON.parse(data);
            body = "<option value = '0'>سازمان مورد نظر خود را انتخاب کنید</option>";
            for (var i = 0; i < jsonData.length; i++) {
                var company = jsonData[i];
                body += "<option value='" + company.Id + "'>" + company.Name + "</option>";

            }
            $('#companyDRP').html(body);
            $("#companyDRP").select2();

            waitingDialog.hide();
        },
        error: function () {
            waitingDialog.hide();
        }
    });
}

var companyId;

function getCompanyContactsAndAddToList(_id) {
    waitingDialog.show('لطفا صبر کنید...');
    $.ajax({
        type: "POST",
        url: getCompanyContactsURL,
        data: { id: _id },
        datatype: "json",
        timeout: 600000,

        success: function (data) {

            var jsonData = JSON.parse(data);
            for (var i = 0; i < jsonData.length; i++) {
                addToTags(jsonData[i].Name, jsonData[i].Number);
            }

            waitingDialog.hide();
        },
        error: function () {
            waitingDialog.hide();
        }
    });
}

function sendNewMessage() {
    waitingDialog.show('لطفا صبر کنید...');
    var Numbers = [];

    var tags = $("#tags").val();

    var data = {

        Message: $("#Message").val(),
        StrDTSend: $("#StrDTSend").val(),
        RepeatType: parseInt($('input[name=RepeatType]:checked').val(), 10),
        Numbers: tags.split(','),
        StartRange: $('#StartNumber').val(),
        FinishRange: $('#FinishNumber').val()
    }

    $.ajax({
        type: "POST",
        url: sendNewMessageURL,
        data: { data: JSON.stringify(data) },
        datatype: "json",
        timeout: 600000,

        success: function (data) {

            var jsonData = JSON.parse(data);

            waitingDialog.hide();

            if (jsonData.Error != "" && jsonData.Error != null) {
                showAlertModal(jsonData.Error);
            }
            else {
                showSuccessModal(jsonData.Result);
            }
        },
        error: function () {
            waitingDialog.hide();
        }
    });




}



$(document).on('confirmation', '.remodal', function () {

    switch (modalId) {
        case 'AddAllNumbers':
            getAllData();
            break;
        case 'AddCompanyContacts':
            getCompanyContactsAndAddToList(companyId);
            break;
        case 'AddRange':
            checkRange();
            break;
        case 'ClearTags':
            elt.tagsinput('removeAll');
            break;

    }



});

function showclearTagsModal() {
    var count = $("#tags").val().length;
    if (count > 0) {
        showModal('ClearTags', 'همه شماره ها پاک شود؟');
    }
}

function showAddAllModal() {
    var count = table.rows().data().length;
    if (count > 0) {
        showModal('AddAllNumbers', 'همه شماره ها به لیست ارسال اضافه شود؟');
    }
}

function checkRange() {
    start = $('#StartNumber').val();
    finish = $('#FinishNumber').val();

    startIsNum = /^\d+$/.test(start);
    finishIsNum = /^\d+$/.test(finish);

    if (start.substring(0, 2) != '09' || finish.substring(0, 2) != '09'
        || start.length < 10 || finish.length < 10
        || start.length > 11 || finish.length > 11
        || !startIsNum || !finishIsNum) {

        showAlertModal('شماره های ابتدایی و انتهایی بدرستی وارد نشده اند.');
    }
    else if (start == finish) {
        showAlertModal('شماره های ابتدایی و انتهایی نمی توانند یکی باشند');
    }
    else {
        showSuccessModal('بازه شماره با موفقیت ثبت شد.');
    }


}

function showFileContainer() {
    $("input#importFile").val('');
    $('#fileContainer').show();

    table.clear().draw();

}
function addToTags(name, number) {

    isDigit = /^\d+$/.test(number);

    if (!isDigit)
        return;

    if (number.substring(0, 3) == '98')
        number = '0' + number.substring(2);

    elt.tagsinput('add', { "value": number, "text": name });

}

function getAllData() {

    var data = table.rows()
            .data();

    for (var i = 0 ; i < data.length ; i++) {
        addToTags(data[i][1] + " " + data[i][2], data[i][4]);

    }

}
