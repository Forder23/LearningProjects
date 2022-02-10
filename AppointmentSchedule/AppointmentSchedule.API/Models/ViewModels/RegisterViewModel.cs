using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name ="First name")]
        [MinLength(2,ErrorMessage ="First name must have at least two characters!")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name ="Last name")]
        [MinLength(3, ErrorMessage = "Last name must have at least three characters!")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(8, ErrorMessage = "Enter your valid email!")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage ="Password must have at least eight characters!")]

        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password and Confirm password must have the same value")]
        [Display(Name ="Confirm password")]
        [MinLength(8, ErrorMessage = "Password must have at least eight characters!")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name ="Role")]
        public string RoleName { get; set; }

    }
}
