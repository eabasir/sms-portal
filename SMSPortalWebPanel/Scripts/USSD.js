var tblRecent;
var tblPrefered;

function makeNewQuery() {
    var query = $("#txtSendMessage").val();
    sendQuery(query);
}


function reSendPrefred(query) {

    $('#chkPrefered').prop('checked', false);
    sendQuery(query);
}

function sendQuery(query) {


    if (query == null || query.length > 50) {
        showAlertModal('دستور ارسالی بدرستی وارد نشده است');
        return;
    }

    waitingDialog.show('لطفا صبر کنید...');
    var ussd = {
        SendMessage: query,
        SIM_Number: $("#spanNumber").html()
    }

    if ($('#chkPrefered').is(':checked')) {
        var title = $("#txtPreferedTitle").val();

        if (title == null || title.length > 50) {
            showAlertModal('عنوان دستور مورد علاقه بدرستی وارد نشده است');
            return;
        }

        ussd.ISPrefered = true;
        ussd.PreferedTitle = title;
    } else {
        ussd.ISPrefered = false;
    }

    $.ajax({
        type: "POST",
        url: sendNewQueryURL,
        data: { data: JSON.stringify(ussd) },
        datatype: "json",
        timeout: 60000,
        success: function (data) {
            waitingDialog.hide()

            getList();
            showSuccessModal("دستور وارد شده با موفقیت در صف ارسال قرار گرفت. چنانچه صف ارسال خالی باشد پاسخ تا یک دقیقه دیگر قابل رویت خواهد بود")


        },
        error: function (data) {
            waitingDialog.hide();
        }

    });






}


function getList() {

    var number = $("#spanNumber").html();

    $.ajax({
        type: "POST",
        url: getUSSDListURL,
        data: { number: number },
        datatype: "json",
        timeout: 15000,
        success: function (data) {

            if (tblRecent == null)
                tblRecent = myDataTable('#recentTable');

            if (tblPrefered == null)
                tblPrefered = myDataTable('#preferedTable');

            tblRecent.clear();
            tblPrefered.clear();


            var jsonData = JSON.parse(data);

            var USSDs = jsonData.UssdVMs;
            if (USSDs != null && !USSDs.length > 0)
                return;

            for (var i = 0; i < USSDs.length; i++) {
                var ussd = USSDs[i];

                if (ussd.ISPrefered) {

                    tblPrefered.row.add([
                  "",
                  '<span style="direction:ltr">' + ussd.SendMessage + '</span>',
                  ussd.PreferedTitle,
                  '<button class="btn btn-success" type="button" onClick="reSendPrefred(\'' + ussd.SendMessage + '\')">ارسال مجدد</button>',
                    ]).draw(false);
                }

                var status = "";

                if (ussd.IsSent)
                    status = '<img src="../../img/check-mark.png" title="ارسال شده" />';
                else
                    status = '<img src="../../img/process.png" title="در صف انتظار" />';


                tblRecent.row.add([
                "",
               '<span style="direction:ltr">' + ussd.SendMessage + '</span>',
                ussd.ReceivedMessage,
                ussd.DTSend,
                status
                ]).draw(false);


            }
        },
        error: function (data) {
        }

    });






}



