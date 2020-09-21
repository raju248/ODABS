using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Obads2.Models
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        public int status { get; set; }
        public string Address { get; set; }
        public string Qualification { get; set; }
        public string Speciality { get; set; }
        public string Education { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        //public string Hospital { get; set; }
        public string ImageFilePath { get; set; }
        [JsonIgnore]
        public virtual Room room { get; set; }

        public virtual ApplicationUser User { get; set; }

        [JsonIgnore]
        public virtual List <Appointment> appointments { get; set; }
    }
}