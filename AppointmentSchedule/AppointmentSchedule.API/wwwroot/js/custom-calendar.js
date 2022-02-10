var route = $(location).attr('protocol') + "//" + $(location).attr('hostname') + ':' + $(location).attr('port');
var calendar;
$(document).ready(function () {

    $('#arrangedAt').kendoDateTimePicker({
        value: new Date(),
        dateInput: false
    });

    initializeCalendar();
});

function initializeCalendar() {

    var calendarElement = document.getElementById('calendar');
    if (calendarElement != null) {
        calendar = new FullCalendar.Calendar(calendarElement, {
            initialView: 'dayGridMonth',
            headerToolbar: {
                left: 'prev, next, today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek, timeGridDay'
            },
            selectable: true,
            editable: false,
            select: function (event) {
                onShowModal(event, null);
            },
            eventDisplay: 'block',
            events: function (fetchInfo, successfulCallback, failureCallback) {
                $.ajax({
                    url: route + '/api/Appointment/calendardata?doctorId=' + $('#doctorId').val(),
                    type: 'GET',
                    dataType: 'JSON',
                    success: function (response) {
                        var events = [];
                        if (response.statusCode === 200) {
                            $.each(response.dataEnum, function (i, data) {
                                events.push({
                                    title: data.title,
                                    description: data.description,
                                    start: data.arrangedAt,
                                    backgroundColor: data.isApproved ? "#28a745" : "#dc3545",
                                    borderColor: "#162466",
                                    textColor: "white",
                                    id: data.id
                                });
                            })
                            successfulCallback(events);
                        }
                    }
                })
            },
            eventClick: function (info) {
                getAppointmentById(info.event);
            }
        });
        calendar.render();
    }

}

function onShowModal(obj, isEventDetail) {
    if ((isEventDetail != null) || (isEventDetail != undefined)) {
        $('#id').val(obj.id);
        $('#title').val(obj.title);
        $('#description').val(obj.description);
        $('#arrangedAt').val(obj.arrangedAt);
        $('#doctorId').val(obj.doctorId);
        $('#patientId').val(obj.patientId);
        $('#lblPatientName').val(obj.patientName);
        $('#lblDoctorName').val(obj.doctorName);

        if (obj.isApproved) {
            $('#lblStatus').val('Approved');
            $('#btnSubmitAppointment').addClass('d-none');
            $('#btnConfirmAppointment').addClass('d-none');
        }
        else {
            $('#lblStatus').val('Pending');
            $('#btnSubmitAppointment').removeClass('d-none');
            $('#btnConfirmAppointment').removeClass('d-none');
        }
        $('#btnDeleteAppointment').removeClass('d-none');
    }
    else {
        $('#arrangedAt').val(obj.startStr + " " + new moment().format("hh:mm A"));
        $('#btnDeleteAppointment').addClass('d-none');
        $('#btnSubmitAppointment').removeClass('d-none');
    }
    $("#appointmentInput").modal("show");
}

function onClickCloseButton() {
    $("#appointmentInput").modal("hide");
    $('#appointmentForm')[0].reset();
    $('#id').val(null);
    $('#title').val(null);
    $('#description').val(null);
    $('#arrangedAt').val(null);
    $('#doctorId').val(obj.doctorId);
}

function onSubmitForm() {
    if (checkValidation()) {


    var request = {
        id: parseInt($('#id').val()),
        title: $('#title').val(),
        description: $('#description').val(),
        arrangedAt: $('#arrangedAt').val(),
        doctorId: $('#doctorId').val(),
        patientId: $('#patientId').val(),
        isApproved: false,
        adminId: $('#patientId').val()
        };

    var url = route + '/api/Appointment/SaveCalendarData';

    fetch(url, {
        method: "post",
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
    })
        
        .then((response) => {
            console.log("Success");
            if (response.status === 200) {
                calendar.refetchEvents();
                onClickCloseButton();
                $.notify("Appointment updated successfully", "success");
            }
            else if (response.status === 201) {
                calendar.refetchEvents();
                onClickCloseButton();
                $.notify("Appointment added successfully", "success");
            }
        })
    }
}

function checkValidation() {
    var isValid = true;
    var description = $('#description').val();

    if ($('#title').val() === undefined || $('#title').val() ==="") {
        isValid = false;
        $("#title").addClass('error');
    }
    else {
        $('#title').removeClass('error');
    }

    if ($('#description').val() === undefined || $('#description').val() === "") {
        isValid = false;
        $('#description').addClass('error');
    }
    else {
        $('#description').removeClass('error');
    }
    return isValid;
}

function getAppointmentById(info) {
    $.ajax({
        url: route + '/api/Appointment/' + info.id,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            if (response.statusCode === 200) {
                if ((response.dataEnum !== null) || (response.dataEnum !== undefined)) {
                    onShowModal(response.dataEnum, true);
                }
            }
        }
    })
}

function onDoctorChange() {
    calendar.refetchEvents();
}

function onDeleteAppointment() {
    var id = $('#id').val();
    $.ajax({
        url: route + '/api/Appointment/' + id,
        type: 'DELETE',
        async: true,
        dataType: 'JSON',
        success: function (response) {
            console.log("Deleted element with id " + id);
            console.log(response);
            calendar.refetchEvents();
            $.notify("Succesfully deleted!", "success");
            onClickCloseButton();
        },
        error: function (response) {
            $.notify("Error while deleting", "error");

        }
    })
}

function onConfirmAppointment() {
    var id = $('#id').val();
    $.ajax({
        url: route + '/api/Appointment/ConfirmEvent/' + id,
        type: 'POST',
        dataType: 'JSON',
        success: function (response) {
            console.log('Confirmed appointment');
            $.notify("Confirmed appointment", "success");
            calendar.refetchEvents();
            onClickCloseButton();
        },
        error: function (response) {
            console.log("Error while confirming");
        }
    })

}