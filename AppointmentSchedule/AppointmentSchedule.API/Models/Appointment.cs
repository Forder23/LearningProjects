using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ArrangedAt { get; set; }
        public string DoctorId{ get; set; }
        public string PatientId { get; set; }

        public bool IsApproved { get; set; }
        public string AdminId{ get; set; }
    }
}
