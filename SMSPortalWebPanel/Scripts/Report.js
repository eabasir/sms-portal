var table;
function srearch() {


    if ($("#RportTypeDRP option:selected").val() == 0) {
        showAlertModal('نوع گزارش مورد نظر خود را انتخاب نمایید');

        return;
    }

    waitingDialog.show('لطفا صبر کنید...');
    var search = {
        Type: $("#RportTypeDRP option:selected").val(),
        Text: $("#txtSearch").val(),
        DTStart: $("#DTStart").val(),
        DTFinish: $("#DTFinish").val(),

    }

    $.ajax({
        type: "POST",
        url: '../Report/getReport',
        data: { data: JSON.stringify(search) },
        datatype: "json",
        timeout: 60000,
        success: function (data) {
            waitingDialog.hide()
            
            if (table == null)
            table = myDataTable('#myDataTable');

            table.clear();

            var jsonData = JSON.parse(data);

            if (!jsonData.length > 0)
                return;
            
            for (var i = 0; i < jsonData.length; i++) {
                var report = jsonData[i];

                var message = '<span style="cursor:pointer;" onclick="showGeneralInfoModal(\'' + report.Message.replace(/\n/g, '') + '\');" >' + report.Message.substring(0, 20) + "...</span>";
                
                var type;
                if (report.iSSent) 
                    type = '<img src="../img/out.png" title="ارسال شده" />'
                else
                    type = '<img src="../img/in.png" title="دریافت شده" />'

                var details = "<ul>";
                for (var j = 0 ; j < report.Details.length ; j++){
                    details +="<li>" + report.Details[j] + "</li>";
                }
                details += "</ul>";

                table.row.add([
                    "",
                    message,
                    type,
                    report.DateTimeFa,
                    report.SIM,
                    report.Contact,
                    '<img src="../img/detail.png" style="cursor:pointer;" onclick="showGeneralInfoModal(\'' + details + '\');" >',
                ]).draw(false);
            }
        },
        error: function (data) {
            waitingDialog.hide();
        }

    });


    



}

