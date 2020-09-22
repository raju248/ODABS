using Newtonsoft.Json;
using Obads2.Areas.Admin.Models;
using Obads2.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Obads2.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {

        ApplicationDbContext _db;

        public HomeController()
        {
            _db = new ApplicationDbContext();
        }

        // GET: Admin/Home
        public ActionResult Index()
        {

            var doctors = _db.Doctors.ToList();
            var patients = _db.Patients.ToList();
            var appoinments = _db.Appointments.ToList();

            var model = new HomeViewModel
            {
                numberOfDoctors = doctors.Count(),
                numberOfPatients = patients.Count(),
                numberOfAppointments = appoinments.Count()
            };

            
            return View(model);
        }


        public ActionResult Doctors(string search,int? page)
        {

            if(!String.IsNullOrEmpty(search))
            {
                var doctors1 = _db.Doctors.Where(d=>d.User.Name.ToLower().Contains(search.ToLower()) ||
                    d.Speciality.ToLower().Contains(search.ToLower())  ||
                    d.User.Email.ToLower().Contains(search.ToLower())).ToList().ToPagedList(page ?? 1, 6);
                return View(doctors1);
            }

            var doctors = _db.Doctors.ToList().ToPagedList(page ?? 1, 6);
            return View(doctors);
        }


        public ActionResult EditDoctorAccount(string id)
        {

            var user = _db.Users.Where(u => u.Id.Equals(id)).FirstOrDefault();

            var model = new DoctorEditViewModel
            {
                id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.dateOfBirth.ToString("dd-MMMM-yyyy"),
                Address = user.doctor.Address,
                Education = user.doctor.Education,
                StartTime = user.doctor.StartTime,
                EndTime = user.doctor.EndTime,
                Qualification = user.doctor.Qualification,
                Speciality = user.doctor.Speciality,
                Gender = user.Gender
            };

            ViewBag.Gender = new List<SelectListItem>
            {
                new SelectListItem(){ Text = "Male", Value = "Male",},
                new SelectListItem(){ Text = "Female", Value = "Female",}
            };


            return View(model);
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public ActionResult EditDoctorAccount(DoctorEditViewModel model)
        {
            var user = _db.Users.Where(d => d.Id.Equals(model.id)).FirstOrDefault();

            _db.Entry(user).State = EntityState.Modified;

            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            user.dateOfBirth = DateTime.Parse(model.DateOfBirth);
            user.doctor.Address = model.Address;
            user.doctor.Education = model.Education;
            user.doctor.StartTime = model.StartTime;
            user.doctor.EndTime = model.EndTime;
            user.doctor.Qualification = model.Qualification;
            user.doctor.Speciality = model.Speciality;
            user.Gender = model.Gender;

            TempData["Success"] = "Changes Saved!";

            ViewBag.Gender = new List<SelectListItem>
            {
                new SelectListItem(){ Text = "Male", Value = "Male",},
                new SelectListItem(){ Text = "Female", Value = "Female",}
            };

            _db.SaveChanges();

            return View(model);
        }

        public ActionResult Appointments()
        {
            var model = _db.Appointments.Where(ap=>ap.doctor!=null).ToList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]

        [HttpGet]
        public String getAppointments()
        {
            var model = _db.Appointments.ToList();
            string jsonString = JsonConvert.SerializeObject(model, Formatting.Indented);
            return jsonString;
        }

        [Authorize(Roles = "Admin")]

        public ActionResult DoctorProfile(int id)
        {
            var model = _db.Doctors.Where(d => d.Id == id).FirstOrDefault();
            return View(model);
        }

        public ActionResult Patients(string search, int? page)
        {
            if(!String.IsNullOrEmpty(search))
            {
                var patients = _db.Patients.Where(p=>p.User.Name.ToLower().Contains(search.ToLower())||
                p.User.Email.ToLower().Contains(search.ToLower())||
                p.User.PhoneNumber.ToLower().Contains(search.ToLower())
                ).ToList().ToPagedList(page ?? 1, 6);

                return View(patients);
            }

            var patient = _db.Patients.ToList().ToPagedList(page ?? 1, 6);

            return View(patient);
        }

        [Authorize(Roles = "Admin")]

        public ActionResult PatientProfile(int id)
        {
            var model = _db.Patients.Where(d => d.Id == id).FirstOrDefault();
            return View(model);
        }

        [Authorize(Roles = "Admin")]

        public ActionResult EditPatientAccount(string id)
        {
            var user = _db.Users.Where(u => u.Id.Equals(id)).FirstOrDefault();

            var model = new EditViewModel
            {
                id = user.Id,
                Name = user.Name,
                DateOfBirth = user.dateOfBirth.ToShortDateString(),
                PhoneNumber = user.PhoneNumber
            };

            ViewBag.Gender = new List<SelectListItem>
            {
                new SelectListItem(){ Text = "Male", Value = "Male",},
                new SelectListItem(){ Text = "Female", Value = "Female",}
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public ActionResult EditPatientAccount(EditViewModel model)
        {
            var user = _db.Users.Where(u => u.Id.Equals(model.id)).FirstOrDefault();

            _db.Entry(user).State = EntityState.Modified;
            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            user.dateOfBirth = DateTime.Parse(model.DateOfBirth);

            TempData["Success"] = "Changes Saved!";

            ViewBag.Gender = new List<SelectListItem>
            {
                new SelectListItem(){ Text = "Male", Value = "Male",},
                new SelectListItem(){ Text = "Female", Value = "Female",}
            };

            _db.SaveChanges();

            return View(model);
        }
    }
}