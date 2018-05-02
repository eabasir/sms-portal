

$(document).ready(function () {
    //waitingDialog.show('لطفا صبر کنید...');

    //setInterval(rfresh_queue, 1000);

    getQueues();
});


//var queueContactsRefresh;
var queueId;


//function rfresh_queue() {
//    getQueues();
//}

var QueueDT;


function getQueues() {

    waitingDialog.show('لطفا صبر کنید...');

    $.ajax({
        type: "POST",
        url: "/Queue/getQueues",
        datatype: "json",
        timeout: 100000,
        success: function (data) {
            waitingDialog.hide()

            body = "";
            var jsonData = JSON.parse(data);
            if (QueueDT == null)
                QueueDT = myDataTable('#QueueDT');

            QueueDT.clear();

            //if (jsonData.length == 0) {
            //    $("#sectionQueues").hide();
            //    return;
            //} else
            //    $("#sectionQueues").show();


            for (var i = 0; i < jsonData.length; i++) {
                var queue = jsonData[i];

                var message = '<span style="cursor:pointer;" onclick="showGeneralInfoModal(\'' + queue.Message.replace(/\n/g, '') + '\');" >' + queue.Message.substring(0, 20) + "...</span>";
                var counter = '<span class="badge badge-danger" style="cursor:pointer" onclick="showQueueContacts(\'' + queue.Id + '\');">' + queue.Counter + '</span>';

                var type;
                if (queue.Type == 0 || queue.Type == 1) {
                    type = "یکبار"
                } else if (queue.Type == 2 || queue.Type == 5) {
                    type = "روزانه"
                } else if (queue.Type == 3 || queue.Type == 6) {
                    type = "هفتگی"
                } else if (queue.Type == 4 || queue.Type == 7) {
                    type = "ماهانه"
                }

                var status;
                if (queue.Status)
                    status = '<img src="../img/deactive.png" title="غیر فعال سازی" onclick="changeQueueStatus(\'' + queue.Id + '\' , false);" style="cursor:pointer;"/>'
                else
                    status = '<img src="../img/active.png" title="فعال سازی" style="cursor:pointer;" onclick="changeQueueStatus(\'' + queue.Id + '\' , true);"/>'

                QueueDT.row.add([
                    "",
                    message,
                    counter,
                    queue.User,
                    queue.DTCreate,
                    queue.DTRequest,
                    type,
                    status,
                   '<button class="btn btn-default btn-xs"  onclick="showModalWithId(\'removeQueue\' ,\'' + queue.Id + '\', \'آیا این صف حذف شود؟\')" ><i class="fa fa-times"></i></button>'
                ]).draw(false);
            }
        },
        error: function (data) {
            waitingDialog.hide();
        }

    });

}

var QueueContactsDT;
function showQueueContacts(queueId) {

    if (queueId != null)
        this.queueId = queueId;

    //if (queueContactsRefresh != null) {
    //    clearInterval(queueContactsRefresh);
    //}
    //queueContactsRefresh = setInterval(function () { showQueueContacts(queueId) }, 1000);

    waitingDialog.show('لطفا صبر کنید...');

    $("#sectionQueueContacts").show();

    $.ajax({
        type: "POST",
        url: "/Queue/getQueueContacts",
        datatype: "json",
        data: { id: this.queueId },
        timeout: 100000,
        success: function (data) {

            waitingDialog.hide();

            body = "";
            var jsonData = JSON.parse(data);
            if (QueueContactsDT == null)
                QueueContactsDT = myDataTable('#QueueContactsDT');

            QueueContactsDT.clear();

            if (jsonData.length == 0) {
                $("#sectionQueueContacts").hide();
                //clearInterval(queueContactsRefresh);
                return;
            } else
                $("#sectionQueueContacts").show();

            for (var i = 0; i < jsonData.length; i++) {
                var contact = jsonData[i];


                var status;
                if (contact.Status)
                    status = '<img src="../img/deactive.png" title="غیر فعال سازی" onclick="changeQueueContactStatus(\'' + contact.Id + '\' , false);" style="cursor:pointer;"/>'
                else
                    status = '<img src="../img/active.png" title="فعال سازی" style="cursor:pointer;" onclick="changeQueueContactStatus(\'' + contact.Id + '\' , true);"/>'

                var process;
                if (contact.Proccess)
                    process = '<img src="../img/process.png" title="در حال پردازش" />'
                else
                    process = '<img src="../img/wait.png" title="انتظار برای ارسال" />'

                QueueContactsDT.row.add([
                    "",
                    contact.Name,
                    contact.Family,
                    contact.Number,
                    process,
                    status,
                   '<button class="btn btn-default btn-xs"  onclick="showModalWithId(\'removeQueueContact\' ,\'' + contact.Id + '\', \'آیا این صف حذف شود؟\')" ><i class="fa fa-times"></i></button>'
                ]).draw(false);
            }
        },
        error: function (data) {
            $("#QueueContactsDT").hide();
            waitingDialog.hide();

        }

    });

}


function changeQueueStatus(queueId, status) {
    waitingDialog.show('لطفا صبر کنید...');
    $.ajax({
        type: "POST",
        url: "/Queue/changeQueueStatus",
        datatype: "json",
        data: { Id: queueId, Status: status },
        timeout: 100000,
        success: function (data) {
            waitingDialog.hide();
            getQueues();
        },
        error: function (data) {
            waitingDialog.hide();
        }

    });
}
function removeQueue(queueId) {
    waitingDialog.show('لطفا صبر کنید...');
    $.ajax({
        type: "POST",
        url: "/Queue/removeQueue",
        datatype: "json",
        data: { Id: queueId },
        timeout: 100000,
        success: function (data) {
            waitingDialog.hide();
            getQueues();
        },
        error: function (data) {
            waitingDialog.hide();
        }

    });
}
function changeQueueContactStatus(queueContactId, status) {
    waitingDialog.show('لطفا صبر کنید...');
    $.ajax({
        type: "POST",
        url: "/Queue/changeQueueContactStatus",
        datatype: "json",
        data: { Id: queueContactId, Status: status },
        timeout: 100000,
        success: function (data) {
            waitingDialog.hide();
            var jsonData = JSON.parse(data);
            showSuccessModal("شماره زیر با موفقیت تغییر وضعیت داد: " + jsonData.number);
            showQueueContacts();
        },
        error: function (data) {
            waitingDialog.hide();
        }

    });
}
function removeQueueContact(queueContactId) {
    waitingDialog.show('لطفا صبر کنید...');
    $.ajax({
        type: "POST",
        url: "/Queue/removeQueueContact",
        datatype: "json",
        data: { Id: queueContactId },
        timeout: 100000,
        success: function (data) {
            waitingDialog.hide();
            var jsonData = JSON.parse(data);
            showSuccessModal("شماره زیر با موفقیت از صف ارسال حذف گردید: " + jsonData.number);
            showQueueContacts();
        },
        error: function (data) {
            waitingDialog.hide();
        }

    });
}
function RestartQueues() {
    waitingDialog.show('لطفا صبر کنید...');
    $.ajax({
        type: "GET",
        url: "/Queue/restartQueues",
        timeout: 100000,
        success: function (data) {
            waitingDialog.hide();
        },
        error: function (data) {
            waitingDialog.hide();
        }

    });
}



$(document).on('confirmation', '.remodal', function () {

    switch (modalId) {
        case 'removeQueue':
            removeQueue(modalObjId);
            break;
        case 'removeQueueContact':
            removeQueueContact(modalObjId)
            break;

    }



});