using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Obads2.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        public string ImagePath { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}