using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Obads2.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        public string ImagePath { set; get; }

        public virtual ApplicationUser User { get; set; }
        [JsonIgnore]
        public virtual List<Appointment> appointments { get; set; }

    }
}