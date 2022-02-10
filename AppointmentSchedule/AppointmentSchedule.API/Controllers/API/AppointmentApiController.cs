using AppointmentSchedule.API.Helper_Extension;
using AppointmentSchedule.API.Models;
using AppointmentSchedule.API.Models.ViewModels;
using AppointmentSchedule.API.Service.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Controllers.API
{
    [Route("/api/Appointment")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentService _AppointmentService;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly string LoggedUserId;
        private readonly string Role;
        public AppointmentApiController(IAppointmentService AppointmentService, 
            IHttpContextAccessor HttpContextAccessor)
        {
            _AppointmentService = AppointmentService;
            _HttpContextAccessor = HttpContextAccessor;
            LoggedUserId = _HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Role = _HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            CommonResponseViewModel<AppointmentViewModel> Appointment = new();
            if (id > 0)
            {
                Appointment.DataEnum = _AppointmentService.GetById(id);
                Appointment.StatusCode = 200;
                Appointment.Message = "Succesfully retrieved!";
                return Ok(Appointment);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("calendardata")]
        public IActionResult GetAppointments(string doctorId)
        {
            CommonResponseViewModel<List<AppointmentViewModel>> Appointments = new();
            if (Role == Helper.Doctor)
            {
                Appointments.DataEnum = _AppointmentService.GetDoctorsAppointments(LoggedUserId);
                Appointments.StatusCode = 200;
            }
            else if (Role == Helper.Patient)
            {
                Appointments.DataEnum = _AppointmentService.GetPatientsAppointments(LoggedUserId);
                Appointments.StatusCode = 200;
            }
            else
            {
                Appointments.DataEnum = _AppointmentService.GetDoctorsAppointments(doctorId);
                Appointments.StatusCode = 200;
            }
            return Ok(Appointments);
        }

        [HttpPost("SaveCalendarData")]
        public IActionResult SaveAppointment(AppointmentViewModel Model)
        {
            CommonResponseViewModel<int> CommonResponse = new();
            try
            {
                CommonResponse.StatusCode = _AppointmentService.AddEdit(Model).Result;
                if (CommonResponse.StatusCode == 200)
                {
                    CommonResponse.Message = Helper.AppointmentUpdated;
                }
                else if (CommonResponse.StatusCode == 201)
                {
                    CommonResponse.Message = Helper.AppointmentAdded;
                }
            }
            catch (Exception e)
            {

                CommonResponse.Message = e.Message;
            }
           
            return Ok(CommonResponse);
        }

        [HttpPost]
        [Route("ConfirmEvent/{id}")]
        public async Task<IActionResult> ConfirmEvent(int id)
        {
            var ToApproveAppointment = await _AppointmentService.ConfirmAppointment(id);
            if (ToApproveAppointment == null )
            {
                return NotFound();
            }
            return Ok(ToApproveAppointment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveById(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            else
            {
                Appointment DeletedAppointment = await _AppointmentService.RemoveById(id);
                if (DeletedAppointment == null)
                {
                    return NotFound();
                }
                return Ok(DeletedAppointment);
            }
        }
    }
}
