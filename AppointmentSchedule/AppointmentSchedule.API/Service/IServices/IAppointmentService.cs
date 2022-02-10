using AppointmentSchedule.API.Models;
using AppointmentSchedule.API.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Service.IServices
{
    public interface IAppointmentService
    {
        AppointmentViewModel GetById(int id);
        List<DoctorViewModel> GetDoctors();
        List<AppointmentViewModel> GetDoctorsAppointments(string doctorId);
        List<PatientViewModel> GetPatients();
        List<AppointmentViewModel> GetPatientsAppointments(string patientId);
        Task<int> AddEdit(AppointmentViewModel Model = null);
        Task<Appointment> RemoveById(int id); 
        Task<Appointment> ConfirmAppointment(int id); 
    }
}
