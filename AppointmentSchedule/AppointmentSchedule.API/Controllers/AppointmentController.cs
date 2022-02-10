using AppointmentSchedule.API.Service.IServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _AppointmentService;
        public AppointmentController(IAppointmentService AppointmentService)
        {
            _AppointmentService = AppointmentService;
        }
        public IActionResult Index()
        {
            var Doctors = _AppointmentService.GetDoctors();
            var Patients = _AppointmentService.GetPatients();
            ViewBag.Doctors = Doctors;
            ViewBag.Patients = Patients;
            return View();
        }
    }
}
