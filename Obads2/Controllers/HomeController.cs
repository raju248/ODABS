using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Obads2.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Odabs2.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext _db;

        public HomeController()
        {
            _db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult WhatWeTreat()
        {
            return View();
        }

        public ActionResult LoginOption()
        {
            return View();
        }

        public ActionResult Doctor(string search, string sp, int? page)
        {

            /*if (String.IsNullOrEmpty(date) == false)
            {
                DateTime dx = DateTime.Parse(date);
                var user = _db.Users.Where(u => u.Id.Equals(User.Identity.GetUserId())).FirstOrDefault();
                var model1 = _db.Appointments.Where(x => (DbFunctions.TruncateTime(x.AppointmentTime) == dx && x.patient.Id == user.patient.Id) || date == null).ToList().ToPagedList(page ?? 1, 3);
                return View(model1);
            }*/

            /*var model = _db.Appointments.ToList().ToPagedList(page ?? 1, 3);
            return View(model);*/


            if (!String.IsNullOrEmpty(search))
            {
                var model = _db.Doctors.Where(d => d.User.Name.ToLower().Contains(search.ToLower()) ||
                d.User.doctor.Speciality.ToLower().Contains(search.ToLower()) || d.User.Email.ToLower().Contains(search.ToLower())).ToList().ToPagedList(page ?? 1, 6);
                return View(model);
            }

            if (!String.IsNullOrEmpty(sp))
            {
                var model = _db.Doctors.Where(d => d.Speciality.ToLower().Contains(sp.ToLower())).ToList().ToPagedList(page ?? 1, 6);
                return View(model);
            }



            var doctors = _db.Doctors.Include(d => d.User).ToList().ToPagedList(page ?? 1, 6);
            return View(doctors);
        }

        public ActionResult DoctorProfile(int id)
        {
            var model = _db.Doctors.Where(d => d.Id == id).FirstOrDefault();
            return View(model);
        }

        [Authorize(Roles = "Patient")]

        [HttpPost]
        public ActionResult Pay(Booking b)
        {
            if (ModelState.IsValid)
            {
                string strCurrentUserId = User.Identity.GetUserId();
                int docID = Convert.ToInt32(b.id);

                var patient = _db.Patients.Where(p => p.User.Id.Equals(strCurrentUserId)).FirstOrDefault();
                var doctor = _db.Doctors.Where(d => d.Id == docID).FirstOrDefault();

                Appointment appointment = new Appointment
                {
                    patient = patient,
                    doctor = doctor,
                    AppointmentTime = DateTime.Parse(b.date),
                    payment = new Payment
                    {
                        amount = 500,
                        paymentMethod = b.paymentMethod
                    },
                    prescription = new Prescription
                    {
                        FileURL = ""
                    }
                };

                _db.Appointments.Add(appointment);
                _db.SaveChanges();

                return Json(new { success = true });
            }



            return RedirectToAction("Doctor", "Home");
        }

        [Authorize(Roles = "Patient")]

        [HttpPost]
        public ActionResult getNextFreeTime(Booking b)
        {
            DateTime oDate = DateTime.Parse(b.date);
            int cid = Convert.ToInt32(b.id);
            var doctor = _db.Doctors.Where(d => d.Id == cid).FirstOrDefault();
            List<Slots> slots = new List<Slots>();

            if (doctor != null)
            {
                DateTime ds = DateTime.Parse(doctor.StartTime);
                DateTime de = DateTime.Parse(doctor.EndTime);

                int duration = Convert.ToInt32(de.Subtract(ds).TotalMinutes.ToString());

                Slots slot = new Slots();
                var sx = ds.ToString("HH:mm");


                for (int i = 0; i < duration / 30; i++)
                {
                    TimeSpan ts = TimeSpan.Parse(sx).Add(new TimeSpan(0, i * 30, 0));
                    TimeSpan te = TimeSpan.Parse(sx).Add(new TimeSpan(0, (i + 1) * 30, 0));

                    DateTime formattedStartTime = DateTime.Parse(ts.ToString(@"hh\:mm"));
                    DateTime formattedEndTime = DateTime.Parse(te.ToString(@"hh\:mm"));

                    DateTime QueryDate = DateTime.Parse(b.date);


                    QueryDate += ts;
                    var datesss = QueryDate.ToString();


                    //var slotEmpty = _db.Appointments.Where(ap => ap.AppointmentTime.ToString().Equals(QueryDate.ToString()) && ap.doctor.Id == b.id).FirstOrDefault();
                    var slotEmpty = _db.Appointments.Where(m => m.AppointmentTime.Year == QueryDate.Year && m.AppointmentTime.Month == QueryDate.Month
                    && m.AppointmentTime.Day == QueryDate.Day && m.AppointmentTime.Hour == QueryDate.Hour && m.AppointmentTime.Minute == QueryDate.Minute && m.doctor.Id == b.id).FirstOrDefault();
                    //var slotEmpty = _db.Database.ExecuteSqlCommand("Select * from Appointments where AppointmentTime =@date", new SqlParameter("@date", QueryDate.ToString()));
                    if (slotEmpty == null)
                    {
                        Slots s = new Slots
                        {
                            StartTimeStamp = QueryDate.ToString(),
                            StartTime = formattedStartTime.ToString("hh:mm tt"),
                            EndTime = formattedEndTime.ToString("hh:mm tt")
                        };

                        slots.Add(s);
                    }

                }
            }

            return Json(slots);
        }
    }
}