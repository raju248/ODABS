using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Obads2.Models
{
    public class Booking
    {
        public int id { get; set; }
        public string date { get; set; }
        public string paymentMethod { get; set; }
    }
}