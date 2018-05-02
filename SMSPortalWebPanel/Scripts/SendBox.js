

$(document).ready(function () {
    waitingDialog.show('لطفا صبر کنید...');
    getSents();
});



var SentDT;
function getSents() {

    $.ajax({
        type: "POST",
        url: "/SendBox/getSents",
        datatype: "json",
        timeout: 10000,
        success: function (data) {

            waitingDialog.hide()

            body = "";
            var jsonData = JSON.parse(data);

            if (SentDT == null)
                SentDT = myDataTable('#SentDT');

            SentDT.clear();

            //if (jsonData.length == 0) {
            //    $("#sectionSents").hide();
            //    return;
            //}
            //else
            //    $("#sectionSents").show();

            for (var i = 0; i < jsonData.length; i++) {
                var sent = jsonData[i];

                var message = '<span style="cursor:pointer;" onclick="showGeneralInfoModal(\'' + sent.Message.replace(/\n/g, '') + '\');" >' + sent.Message.substring(0, 20) + "...</span>";
                var counter = '<span class="badge badge-success" style="cursor:pointer" onclick="showSentContacts(\'' + sent.Id + '\');">' + sent.Counter + '</span>';

                var type;
                if (sent.Type == 0 || sent.Type == 1) {
                    type = "یکبار"
                } else if (sent.Type == 2 || sent.Type == 5) {
                    type = "روزانه"
                } else if (sent.Type == 3 || sent.Type == 6) {
                    type = "هفتگی"
                } else if (sent.Type == 4 || sent.Type == 7) {
                    type = "ماهانه"
                }

                SentDT.row.add([
                    "",
                    message,
                    counter,
                    sent.User,
                    sent.DTCreate,
                    type,
                    '<a href="../NewMessage/CopySent/' + sent.Id + '" ><img src="../img/copy.png" title="کپی" /> </a>',
                ]).draw(false);
            }
        },
        error: function (data) {
            waitingDialog.hide();
        }

    });
}

var SendBoxContactsDT;
function showSentContacts(sentId) {

    
    $("#sectionSentContacts").show();

    $.ajax({
        type: "POST",
        url: "/SendBox/getSentContacts",
        datatype: "json",
        data: { id: sentId },
        timeout: 10000,
        success: function (data) {
            body = "";
            var jsonData = JSON.parse(data);

            if (SendBoxContactsDT == null)
            SendBoxContactsDT = myDataTable('#SendBoxContactsDT');

            SendBoxContactsDT.clear();

            if (jsonData.length == 0) {
                $("#sectionSentContacts").hide();
                return;
            } else
                $("#sectionSentContacts").show();

            for (var i = 0; i < jsonData.length; i++) {
                var sent = jsonData[i];

                var delivered;

                if (sent.Delevered)
                    delivered = '<img src="../img/check-mark.png" title="دلیور شده" />';
                else
                    delivered = '<img src="../img/wait.png" title="انتظار دریافت پیام دلیوری" />';


                SendBoxContactsDT.row.add([
                    "",
                    sent.Name,
                    sent.Family,
                    sent.Number,
                    sent.SIM,
                    sent.DTSend,
                    delivered,
                    sent.DTDeliver,
                    '<a href="../NewMessage/CopySentPhone/' + sent.Id + '" ><img src="../img/copy.png" title="کپی"/></a>',
                ]).draw(false);
            }
        },
        error: function (data) {
            $("#sectionSentContacts").hide();
        }

    });

}

