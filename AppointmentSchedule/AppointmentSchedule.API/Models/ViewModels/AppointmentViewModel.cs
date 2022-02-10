using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Models.ViewModels
{
    public class AppointmentViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ArrangedAt { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }

        public bool IsApproved { get; set; }
        public string AdminId { get; set; }
        
        #nullable enable
        public string? DoctorName { get; set; }
        #nullable enable
        public string? PatientName { get; set; }
        #nullable enable
        public string? AdminName { get; set; }
    }
}
