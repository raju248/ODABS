using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Obads2.Areas.Doctor.Models
{
    public class DoctorViewModels
    {
    }

    public class DoctorLoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class Times
    {
        public int id { get; set; }
        public string time { get; set; }
        public bool IsSelected { get; set; }
    }

    public class TimesGroup
    {
        public List<Times> times = new List<Times>
        {
            new Times{ id = 0, time = "Morning", IsSelected = false },
            new Times{ id = 1, time = "Afternoon", IsSelected = false },
            new Times{ id = 2, time = "Evening", IsSelected = false },
            new Times{ id = 3, time = "Night", IsSelected = false },
        };
    }

    public class PrescriptionViewModel
    {
        public string patientName { get; set; } 
        public string patientPhoneNumber { get; set; } 
        public string appointmentTime { get; set; } 

        public int Id { get; set; }
        public List<string> medicines { get; set; }

        public List<string> quantity { get; set; }

        public List<string> days { get; set; }

        public List<string> times { get; set; }
    }


}