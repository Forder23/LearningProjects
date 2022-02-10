using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentSchedule.API.Models.ViewModels
{
    public class CommonResponseViewModel<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T DataEnum { get; set; }
    }
}
