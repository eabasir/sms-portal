var table;
$(function () {
    $("input#importFile").change(function () {

        if (window.FormData !== undefined) {
            var fileUpload = $("#importFile").get(0);
            var files = fileUpload.files;

            if (files.length > 0) {
                waitingDialog.show('لطفا صبر کنید...');


                if (table == null)
                    table = myDataTable("#myDataTable");

                var fileData = new FormData();

                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i]);
                }

                $.ajax({
                    url: '../FileUpload/getFileData',
                    type: "POST",
                    timeout: 60000,
                    contentType: false, // Not to set any content header
                    processData: false, // Not to process data
                    data: fileData,
                    success: function (data) {
                        waitingDialog.hide();
                        try {

                            var jsonData = JSON.parse(data);

                            table.clear();

                            if (jsonData.DuplicatesLink != null || jsonData.NonMobileLink != null) {


                                showFileErrorsModal(jsonData.DuplicatesLink, jsonData.NonMobileLink);

                                if (jsonData.DuplicatesLink != null)
                                    return;
                            }


                            if (jsonData.Persons.length > 0) {

                                for (var i = 0; i < jsonData.Persons.length; i++) {
                                    var contact = jsonData.Persons[i];

                                    var numbers = "";
                                    for (var j = 0 ; j < contact.Numbers.length ; j++) {
                                        numbers += '<li>' + contact.Numbers[j] + '</li>';
                                    }

                                    table.row.add([
                                        "",
                                        contact.Name,
                                        contact.Family,
                                        contact.Email,
                                        '<ul>' + numbers + '</ul>',
                                    ]).draw(false);
                                }
                            }
                        }
                        catch (Err) {
                            console.log(Err);
                        }

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

function SaveContacts() {

    waitingDialog.show('لطفا صبر کنید...');

    var rows = $('#myDataTable').dataTable().api().rows();
    var contacts = [];

    var i = 0
    rows.every(function () {

        var data = this.data();


        var numbers = data[4].replace("<ul>", "").replace("</ul>", "").replace(new RegExp("<li>", "g"), '').split("</li>");

        contacts[i] = {
            Name: data[1],
            Family: data[2],
            Email: data[3],
            Numbers: numbers
        }
        i++;
    });

    if (i == 0) {
        waitingDialog.hide();
        showAlertModal('جدول مخاطبین نمی تواند خالی باشد');

        return;
    }


    var Companies = [];
    i = 0;
    $('#companyContainer input[type=text]').each(function () {

        Companies[i] = $(this).val();
        i++;
    });


    total = {
        Contacts: contacts,
        Companies: Companies
    }

    $.ajax({
        type: "POST",
        url: '../Contact/SaveGroup',
        data: { data: JSON.stringify(total) },
        datatype: "json",
        timeout: 60000,

        success: function (data) {

            var jsonData = JSON.parse(data);
            waitingDialog.hide();

            if (jsonData.Error != "" && jsonData.Error != null) {
                showAlertModal(jsonData.Error);
            }
            else {
                showSuccessModal(jsonData.Result);
            }

            waitingDialog.hide();
        },
        error: function (err) {
            waitingDialog.hide();
        }

    });

}

function addCompany() {
    if ($("#companyDRP option:selected").val() != '0') {
        name = $("#companyDRP option:selected").text();

        var str = '<div class="form-group"><input type="text" class="form-control" value="' + name + '" readonly>' +
                  '<button type="button" class="btn btn-default btn-xs" onclick="removeCompany(this);"><i class="fa fa-times"></i></button></div>';

        $("#companyContainer").append(str);
    }

}

function removeCompany(e) {

    e.parentNode.parentNode.removeChild(e.parentNode);

}


function showGeneralInfoModal(message) {
    $('.GeneralInfoModalBody').html(message);
    $('#GeneralInfoModal').modal('show');
}


function showFileErrorsModal(duplicteFN, nonMobileFN) {


    var content = '<div>';

    if (duplicteFN != null && duplicteFN != '')
        content += '<a href="../Contact/DownloadErrors/' + duplicteFN + '" >'+
        '<button type="submit" class="btn btn-danger" style="margin:20px">شماره های تکرار شده</button></a>';
    if (nonMobileFN != null && nonMobileFN != '')
        content += '<a href="../Contact/DownloadErrors/' + nonMobileFN + '" >' +
            '<button type="submit" class="btn btn-info" style="margin:20px">شماره های غیر همراه</button></a>';

    content += '</div>';

    $('.GeneralInfoModalBody').html(content);
    $('#GeneralInfoModal').modal('show');
}