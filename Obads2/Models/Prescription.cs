using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Obads2.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }
        public string FileURL { get; set; }
        [JsonIgnore]

        public virtual Appointment appointment { get; set; }
    }
}