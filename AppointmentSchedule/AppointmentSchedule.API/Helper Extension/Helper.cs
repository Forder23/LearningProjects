using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Helper_Extension
{
    public static class Helper
    {
        //ROLES

        public static string Admin = "Administrator";
        public static string Patient = "Patient";
        public static string Doctor = "Doctor";

        //STRINGS
        public static string AppointmentAdded = "Successfuly added appointment!";
        public static string AppointmentUpdated = "Successfuly updated appointment!";
        public static string AppointmentDeleted = "Successfuly deleted appointment!";
        public static string AppointmentDoesntExist = "Appointment doesn't exist!";

        public static string AppointmentError = "There was an error with an appointment. Try again!";


        public static List<SelectListItem> GetRoles(bool isAdmin)
        {
            if (isAdmin == true)
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem{ Value = Admin, Text = Helper.Admin }
                };
            }
            else
            {
                return new List<SelectListItem>()
                {
                    new SelectListItem{ Value = Patient, Text = Helper.Patient },
                    new SelectListItem{ Value = Doctor, Text = Helper.Doctor}
                };
            }
            
        }
    }
}
