using AppointmentSchedule.API.Database;
using AppointmentSchedule.API.Helper_Extension;
using AppointmentSchedule.API.Models;
using AppointmentSchedule.API.Models.ViewModels;
using AppointmentSchedule.API.Service.IServices;
using AppointmentSchedule.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Service.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _Context;
        private readonly IEmailSenderService _EmailService;
        private readonly IConfiguration _Configuration;

        public AppointmentService(ApplicationDbContext Context, 
            IEmailSenderService EmailService, 
            IConfiguration Configuration)
        {
            _Context = Context;
            _EmailService = EmailService;
            _Configuration = Configuration;
        }

        public async Task<int> AddEdit(AppointmentViewModel Model)
        {
            if ((Model != null) && (Model.Id > 0))
            {
                Appointment _FoundAppointment = await _Context.Appointments.FindAsync(Model.Id);
                _FoundAppointment.Title = Model.Title;
                _FoundAppointment.Description = Model.Description;
                _FoundAppointment.ArrangedAt = DateTime.Parse(Model.ArrangedAt);
                _FoundAppointment.DoctorId = Model.DoctorId;
                _FoundAppointment.PatientId = Model.PatientId;
                _FoundAppointment.IsApproved = Model.IsApproved;
                _FoundAppointment.AdminId = Model.AdminId;
                await _Context.SaveChangesAsync();

                return 200;
            }
            else
            {
                Appointment newAppointment = new()
                {
                    Title = Model.Title,
                    Description = Model.Description,
                    ArrangedAt = DateTime.Parse(Model.ArrangedAt),
                    DoctorId = Model.DoctorId,
                    PatientId = Model.PatientId,
                    IsApproved = false,
                    AdminId = Model.PatientId
                };

                var htmlCode =
                    "<h3>New Appointment arrived</h3> <br/>" +
                    "<button class= 'btn btn-primary' onclick='onConfirmAppointment()'>Click if you want to approve it! </button>";


                await _EmailService.SendMail("admirsitnic@gmail.com",
                                             htmlCode,
                                             _Configuration.GetValue<string>("MailJetConfig:Subject"));
                _Context.Appointments.Add(newAppointment);
                await _Context.SaveChangesAsync();
                //return 2;  --> u tutorialu, povratni tip u tutorialu je Task<int>
                return 201;
            }
        }

        public async Task<Appointment> ConfirmAppointment(int id)
        {
            Appointment ToApproveAppointment = new();
            if (id > 0)
            {
                ToApproveAppointment = await _Context.Appointments.FindAsync(id);
                if (ToApproveAppointment == null)
                {
                    return null;
                }
                else
                {
                    ToApproveAppointment.IsApproved = true;
                    await _Context.SaveChangesAsync();
                    return ToApproveAppointment;
                }
            }
            return ToApproveAppointment;
        }

        public AppointmentViewModel GetById(int id)
        {
            if (id > 0)
            {
                var Appointment = _Context.Appointments.Select(x => new AppointmentViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ArrangedAt = x.ArrangedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    DoctorId = x.DoctorId,
                    PatientId = x.PatientId,
                    IsApproved = x.IsApproved,
                    DoctorName = _Context.Users.SingleOrDefault(u=> u.Id == x.DoctorId).Name,
                    PatientName = _Context.Users.SingleOrDefault(u => u.Id == x.PatientId).Name
                })
                .Where(x => x.Id == id)
                .SingleOrDefault();

                if (Appointment != null)
                {
                    return Appointment;
                }
            }
            
            return null;
        }

        public List<DoctorViewModel> GetDoctors()
        {
            var Doctors =
         _Context.Users
            .Join(_Context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new { user, userRole })
            .Join(_Context.Roles, userRole => userRole.userRole.RoleId, role => role.Id, (userRole, role) => new { userRole, role })
            .Where(x => x.role.Name == Helper.Doctor)
            .Select(x => new DoctorViewModel
            {
                Id = x.userRole.user.Id,
                Name = x.userRole.user.Name,
            })
             .ToList();

            return Doctors;                                                            
        }

        public List<AppointmentViewModel> GetDoctorsAppointments(string doctorId)
        {
            List<AppointmentViewModel> AppointmentList = new();
            if (doctorId == null)
            {
                AppointmentList = _Context.Appointments.Select(x => new AppointmentViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ArrangedAt = x.ArrangedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    DoctorId = x.DoctorId,
                    PatientId = x.PatientId,
                    IsApproved = x.IsApproved,
                    AdminId = x.AdminId,
                    DoctorName = _Context.Users.SingleOrDefault(n => n.Id == doctorId).Name,
                    PatientName = _Context.Users.SingleOrDefault(n => n.Id == x.PatientId).Name,
                    AdminName = _Context.Users.SingleOrDefault(n => n.Id == x.AdminId).Name
                }).ToList();
            }

             AppointmentList = _Context.Appointments
                .Select(x => new AppointmentViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ArrangedAt = x.ArrangedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                DoctorId = x.DoctorId,
                PatientId = x.PatientId,
                IsApproved = x.IsApproved,
                AdminId = x.AdminId,
                DoctorName = _Context.Users.SingleOrDefault(n => n.Id == doctorId).Name,
                PatientName = _Context.Users.SingleOrDefault(n => n.Id == x.PatientId).Name,
                AdminName = _Context.Users.SingleOrDefault(n => n.Id == x.AdminId).Name
            })
                .Where(n => n.DoctorId == doctorId)
                .ToList();

            if (AppointmentList.Count > 0 && AppointmentList != null)
            {
                return AppointmentList;
            }
            return null;
        }

        public List<PatientViewModel> GetPatients()
        {
            var Patients =
         _Context.Users
            .Join(_Context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new { user, userRole })
            .Join(_Context.Roles, userRole => userRole.userRole.RoleId, role => role.Id, (userRole, role) => new { userRole, role })
            .Where(x => x.role.Name == Helper.Patient)
            .Select(x => new PatientViewModel
            {
                Id = x.userRole.user.Id,
                Name = x.userRole.user.Name,
            })
             .ToList();

            return Patients;
        }

        public List<AppointmentViewModel> GetPatientsAppointments(string patientId)
        {
            var AppointmentList = _Context.Appointments
                .Select(x => new AppointmentViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ArrangedAt = x.ArrangedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    DoctorId = x.DoctorId,
                    PatientId = x.PatientId,
                    IsApproved = x.IsApproved,
                    AdminId = x.AdminId,
                    DoctorName = _Context.Users.SingleOrDefault(n => n.Id == x.DoctorId).Name,
                    PatientName = _Context.Users.SingleOrDefault(n => n.Id == patientId).Name,
                    AdminName = _Context.Users.SingleOrDefault(n => n.Id == x.AdminId).Name
                })
                .Where(n => n.PatientId == patientId)
                .ToList();

            if (AppointmentList.Count > 0 && AppointmentList != null)
            {
                return AppointmentList;
            }
            return null;
        }

        public async Task<Appointment> RemoveById(int id)
        {
            if (id > 0)
            {
                Appointment ToRemoveAppointment = await _Context.Appointments.FindAsync(id);
                _Context.Appointments.Remove(ToRemoveAppointment);
                await _Context.SaveChangesAsync();
                return ToRemoveAppointment;
            }
            return null;
        }
    }
}
