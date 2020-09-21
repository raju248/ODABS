using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Obads2.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int amount { get; set; }
        public string paymentMethod { get; set; }
        [JsonIgnore]

        public virtual Appointment appointment { get; set; }
    }
}