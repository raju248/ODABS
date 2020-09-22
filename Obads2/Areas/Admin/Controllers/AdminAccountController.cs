using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Obads2.Areas.Admin.Models;
using Obads2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using RegisterViewModel = Obads2.Areas.Admin.Models.RegisterViewModel;

namespace Obads2.Areas.Admin.Controllers
{
    public class AdminAccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext _db;

        public AdminAccountController()
        {
            _db = new ApplicationDbContext();
        }

        public AdminAccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        [Authorize(Roles ="Admin")]
        public ActionResult MyProfile()
        {
            string id = User.Identity.GetUserId();
            var model = _db.Users.Where(u => u.Id.Equals(id)).FirstOrDefault();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit()
        {
            var userId = User.Identity.GetUserId();
            var user = _db.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();

            var model = new AdminAccountEditViewModel
            {
                id = user.Id,
                Name = user.Name,
                DateOfBirth = user.dateOfBirth.ToShortDateString(),
                PhoneNumber = user.PhoneNumber,
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
        public ActionResult Edit(AdminAccountEditViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = _db.Users.Where(u => u.Id.Equals(userId)).FirstOrDefault();

            _db.Entry(user).State = EntityState.Modified;
            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            user.dateOfBirth = DateTime.Parse(model.DateOfBirth);
            user.Gender = model.Gender;

            _db.SaveChanges();
            return RedirectToAction("MyProfile", "AdminAccount");

        }


        [AllowAnonymous]
        public ActionResult AdminLogin()
        {
            //ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("AdminLogin", "AdminAccount", new { area = "Admin" });
        }



        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminLogin(AdminLoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {

                    if (UserManager.IsInRole(user.Id, "Admin"))
                    {
                        await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

                        if (String.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
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

        //
        // GET: Admin/Account/Register
        [AllowAnonymous]
        public ActionResult AdminRegister()
        {
            return View();
        }

        //
        // GET: Admin/Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                string fileName = Path.GetFileNameWithoutExtension(model.DisplayPicture.FileName);
                string fileExtension = Path.GetExtension(model.DisplayPicture.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                string filePathString = "/Images/Doctor_Pictures/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/Doctor_Pictures/"), fileName);
                model.DisplayPicture.SaveAs(fileName);

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    dateOfBirth = model.DateOfBirth,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    admin = new Obads2.Models.Admin
                    {
                        ImagePath = filePathString
                    }
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Admin");
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: Admin/Account/Register
        [Authorize(Roles = "Admin")]
        public ActionResult DocRegister()
        {
            var model = new DocRegisterViewModel();
            return View(model);
        }


        //
        // GET: Admin/Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DocRegister(DocRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(model.DisplayPicture.FileName);
                string fileExtension = Path.GetExtension(model.DisplayPicture.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                string filePathString = "/Images/Doctor_Pictures/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/Doctor_Pictures/"), fileName);
                model.DisplayPicture.SaveAs(fileName);

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    dateOfBirth = model.DateOfBirth,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    doctor = new Obads2.Models.Doctor
                    {
                        Address = model.Address,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        Education = model.Education,
                        Qualification = model.Qualification,
                        ImageFilePath = filePathString,
                        Speciality = model.Speciality,
                        room = new Room()
                    }
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Doctor");
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    TempData["Success"] = "Registration Successful";
                    return RedirectToAction("DocRegister", "AdminAccount", new { area = "Admin" });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public FileResult DownloadPrescription(int id)
        {
            var prescription = _db.Prescriptions.Where(p => p.PrescriptionId == id).FirstOrDefault();
            string path = Server.MapPath(prescription.FileURL);

            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, prescription.FileURL);

        }

        [Authorize(Roles = "Admin")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                model = new ChangePasswordViewModel();
                TempData["Success"] = "Password changed successfully!";
                return RedirectToAction("MyProfile", "AdminAccount");
            }
            AddErrors(result);
            return View(model);
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
            return RedirectToAction("Index", "Home", new { area = "Admin" });
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