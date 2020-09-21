using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Obads2.Areas.Admin.Models
{
    public class AccountViewModels
    {
    }


    public class AdminLoginViewModel
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

    public class AdminAccountEditViewModel
    {
        public string id { get; set; }
        [Required]
        public string Name { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Contact no")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Date of birth")]
        public String DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Contact No")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Display Picture")]
        public HttpPostedFileBase DisplayPicture { get; set; }
    }

    public class Day
    {
        public int ID { get; set; }
        public string DayName { get; set; }
        public bool IsCheked { get; set; }
    }


    public class DocRegisterViewModel
    {

        public DocRegisterViewModel()
        {
            test = new List<string>();
        }

        [Required]
        [Display(Name = "Available Days")]
        public List<Day> days = new List<Day>
        {
            new Day{ ID = 1, IsCheked = false, DayName = "Sunday"},
            new Day{ ID = 2, IsCheked = false, DayName = "Monday"},
            new Day{ ID = 3, IsCheked = false, DayName = "Tuesday"},
            new Day{ ID = 4, IsCheked = false, DayName = "Wednesay"},
            new Day{ ID = 5, IsCheked = false, DayName = "Thursday"},
            new Day{ ID = 6, IsCheked = false, DayName = "Friday"},
            new Day{ ID = 7, IsCheked = false, DayName = "Saturday"}
        };



        [Required]
        [Display(Name = "Name")]

        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Contact No")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }


        [Required]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]

        [Display(Name = "Education")]
        public string Education { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }


        [Required]
        [Display(Name = "End Time")]
        public string EndTime  { get; set; }

        [Required]
        [Display(Name = "Qualification")]

        public string Qualification { get; set; }
        [Required]
        [Display(Name = "Speciality")]

        public string Speciality { get; set; }


        /*public IEnumerable<Day> Days
        {
            get; set;
        }*/

        [Required]
        [Display(Name = "Display Picture")]
        public HttpPostedFileBase DisplayPicture { get; set; }

        public List<string> test { get; set; }

    }


    public class HomeViewModel
    {
        public int numberOfDoctors { get; set; }
        public int numberOfPatients { get; set; }
        public int numberOfAppointments { get; set; }
    }


    public class DoctorEditViewModel
    {
        [Required]
        public string id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Contact No")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Gender { set; get; }

        [Required]
        [Display(Name = "Date of Birth")]

        public String DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Education")]
        public string Education { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public string EndTime { get; set; }

        [Required]
        [Display(Name = "Qualification")]
        public string Qualification { get; set; }
        [Required]
        [Display(Name = "Speciality")]
        public string Speciality { get; set; }

        [Required]
        [Display(Name = "Display Picture")]
        public HttpPostedFileBase DisplayPicture { get; set; }


    }
}