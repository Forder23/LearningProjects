using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Enter email address")]
        [DataType(DataType.EmailAddress)]
        [MinLength(8,ErrorMessage ="Email too short!")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Enter password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password too short!")]
        public string Password { get; set; }

        [Display(Name ="Remember me?")]
        public bool RememberMe { get; set; }
    }
}
