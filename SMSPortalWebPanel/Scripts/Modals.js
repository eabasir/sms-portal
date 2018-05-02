var modalId;
var modalObjId;
function showModal(_modalId, message) {
    modalId = _modalId;
    $('#modalMessage').html(message);
    var inst = $('[data-remodal-id =Modal]').remodal();
    inst.open();
}

function showModalWithId(_modalId, _modalObjId, message) {
    modalId = _modalId;
    modalObjId = _modalObjId;
    $('#modalMessage').html(message);
    var inst = $('[data-remodal-id =Modal]').remodal();
    inst.open();
}

function showAlertModal(message) {
    $('#alertModalMessage').html(message);
    var inst = $('[data-remodal-id =AlertModal]').remodal();
    inst.open();
}
function showSuccessModal(message) {
    $('#successModalMessage').html(message);
    var inst = $('[data-remodal-id =SuccessModal]').remodal();
    inst.open();
}

function showGeneralInfoModal(message) {
    $('.GeneralInfoModalBody').html(message);
    $('#GeneralInfoModal').modal('show');
}
