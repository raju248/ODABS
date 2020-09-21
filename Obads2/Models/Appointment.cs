using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Obads2.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public virtual Patient patient { get; set; }
        public virtual Doctor doctor { get; set; }
        public virtual Payment payment { get; set; }
        public virtual Prescription prescription { get; set; }
    }
}