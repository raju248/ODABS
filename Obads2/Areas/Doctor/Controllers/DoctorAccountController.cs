using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Obads2.Areas.Doctor.Models;
using Obads2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;
using Obads2.Areas.Admin.Models;
using System.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System.Data;
using System.IO;
using Syncfusion.Pdf.Graphics;

namespace Obads2.Areas.Doctor.Controllers
{
    public class DoctorAccountController : Controller
    {
        // GET: Doctor/Doctor

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationDbContext _db;

        public DoctorAccountController()
        {
            _db = new ApplicationDbContext();
        }

        public DoctorAccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [Authorize(Roles = "Doctor")]

        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var model = _db.Appointments.Where(x => x.doctor.User.Id.Equals(userId)).ToList();
            return View(model);
        }
        [Authorize(Roles = "Doctor")]

        public ActionResult Edit()
        {
            string userId = User.Identity.GetUserId();
            var user = _db.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();

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
        [Authorize(Roles = "Doctor")]

        [HttpPost]
        public ActionResult Edit(DoctorEditViewModel model)
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

            _db.SaveChanges();

            return RedirectToAction("UserProfile", "DoctorAccount");
        }

        [Authorize(Roles = "Doctor")]

        [HttpPost]
        public string GetAppointments()
        {
            var model = _db.Appointments.ToList();
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.DateFormatString = "dd/MM/yyy hh:mm:ss";
            string s = JsonConvert.SerializeObject(model);
            return s;
        }

        [Authorize(Roles = "Doctor")]

        public ActionResult Cancel(int id)
        {
            return RedirectToAction("Index", "DoctorAccount", new { area = "Doctor" });
        }

        [Authorize(Roles = "Doctor")]

        public ActionResult UserProfile()
        {
            string userId = User.Identity.GetUserId();

            var model = _db.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();

            return View(model);
        }
        [AllowAnonymous]

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(DoctorLoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    if (UserManager.IsInRole(user.Id, "Doctor"))
                    {
                        await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

                        if (String.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("Index", "DoctorAccount", new { area = "Doctor" });
                        }

                        else
                        {
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");

                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "DoctorAccount", new { area = "Doctor" });
        }


        [Authorize(Roles = "Doctor")]

        public ActionResult AddPrescription(int id)
        {
            var prescription = _db.Prescriptions.Where(a => a.PrescriptionId == id).FirstOrDefault();
            var model = new PrescriptionViewModel
            {
                Id = id,
                patientName = prescription.appointment.patient.User.Name,
                patientPhoneNumber = prescription.appointment.patient.User.PhoneNumber,
                appointmentTime = prescription.appointment.AppointmentTime.ToString("dd-MMMM-yy hh:mm tt"),
                medicines = new List<string>(),
                days = new List<string>(),
                times = new List<string>(),
                quantity = new List<string>(),
            };

            return View(model);
        }


        [Authorize(Roles = "Doctor")]

        [HttpPost]
        public ActionResult AddPrescription(PrescriptionViewModel model)
        {
            if (ModelState.IsValid && model.days!=null)
            {
                var prescription = _db.Prescriptions.Where(p => p.PrescriptionId == model.Id).FirstOrDefault();

                PdfDocument doc = new PdfDocument();


                //Add a page.
                PdfPage page = doc.Pages.Add();
                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;
                //Set the standard font
                PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

                graphics.DrawString("Patient Name : " + prescription.appointment.patient.User.Name, font, PdfBrushes.Black, new PointF(0, 0));

                graphics.DrawString("Appoiment Time " + prescription.appointment.AppointmentTime.ToString("dd-MMMM-yyyy hh:mm tt"), font, PdfBrushes.Black, new PointF(0, 12));

                graphics.DrawString("Doctor Name : " + prescription.appointment.doctor.User.Name, font, PdfBrushes.Black, new PointF(0, 24));

                graphics.DrawString("Doctor Contact No : " + prescription.appointment.doctor.User.PhoneNumber, font, PdfBrushes.Black, new PointF(0, 36));

                //Create a PdfGrid.
                PdfGrid pdfGrid = new PdfGrid();
                //Create a DataTable.
                DataTable dataTable = new DataTable();
                //Add columns to the DataTable
                dataTable.Columns.Add("Medicine Name");
                dataTable.Columns.Add("Quantity");
                dataTable.Columns.Add("Days");
                dataTable.Columns.Add("Times");

                for (int i = 0; i < model.times.Count; i++)
                {
                    dataTable.Rows.Add(new object[] { model.medicines[i], model.quantity[i], model.days[i], model.times[i] });
                }
                //Assign data source.
                pdfGrid.DataSource = dataTable;

                PdfGridBuiltinStyleSettings tableStyleOption = new PdfGridBuiltinStyleSettings();
                tableStyleOption.ApplyStyleForBandedRows = true;
                tableStyleOption.ApplyStyleForHeaderRow = true;
                pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent4, tableStyleOption);
                //Draw grid to the page of PDF document.
                pdfGrid.Draw(page, new PointF(0, 50));

                string filePathString = "~/Images/" + model.Id + ".pdf";

                // Open the document in browser after saving it
                doc.Save(Server.MapPath(filePathString));
                //close the document
                doc.Close(true);


                _db.Entry(prescription).State = EntityState.Modified;
                prescription.FileURL = filePathString;
                _db.SaveChanges();

                TempData["Success"] = "Prescription added for appointment no. " + model.Id;

                return RedirectToAction("Index", "DoctorAccount");
            }


            return View(model);

        }

        [Authorize(Roles = "Doctor")]

        public FileResult DownloadPrescription(int id)
        {
            var prescription = _db.Prescriptions.Where(p => p.PrescriptionId == id).FirstOrDefault();
            string path = Server.MapPath(prescription.FileURL);

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, prescription.FileURL);

        }


        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "Doctor" });
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

        }


    }
}
