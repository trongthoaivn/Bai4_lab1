using Bai4_lab1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bai4_lab1.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();
            Course objcourse = new Course();
            objcourse.ListCategory = context.Categories.ToList();

            return View(objcourse);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objcourse)
        {
            BigSchoolContext context = new BigSchoolContext();
            ModelState.Remove("LecturerId");

            if (!ModelState.IsValid)
            {
                objcourse.ListCategory = context.Categories.ToList();
                context.Categories.ToList();
                return View("Create", objcourse);
            }

            Course last = context.Courses.OrderByDescending(p => p.Id).FirstOrDefault();
            if (last != null)
            {
                objcourse.Id = last.Id + 1;
            }
            else
            {
                objcourse.Id = 0;
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objcourse.LecturerId = user.Id;
            context.Courses.Add(objcourse);
            context.SaveChanges();
            return RedirectToAction("index", "Courses");
        }

        public ActionResult Index()
        {
            BigSchoolContext schoolContext = new BigSchoolContext();
            var upcoming = schoolContext.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            foreach(Course c in upcoming)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(c.LecturerId);
                c.Category.Name = user.Name;
            }
            Console.WriteLine(upcoming);
            return View(upcoming);
        }
    }
}