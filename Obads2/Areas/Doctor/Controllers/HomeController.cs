using Obads2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Obads2.Areas.Doctor.Controllers
{
    [Authorize(Roles ="Doctor")]
    public class HomeController : Controller
    {

        ApplicationDbContext _db;

        public HomeController()
        {
            _db = new ApplicationDbContext();
        }

        // GET: Doctor/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PatientProfile(int id)
        {
            var patient = _db.Patients.Where(p => p.Id == id).FirstOrDefault();
            return View(patient);
        }
    }
}