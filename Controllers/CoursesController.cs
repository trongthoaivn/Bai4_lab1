using Bai4_lab1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
            var userID = User.Identity.GetUserId();
            foreach (Course c in upcoming)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(c.LecturerId);
                

                if (userID != null)
                {
                    c.isLogin = true;
                    //ktra user đó chưa tham gia khóa học
                    Attendance find = schoolContext.Attendances.FirstOrDefault(p =>
                    p.CourseId == c.Id && p.Attendee == userID);
                    if (find == null)
                        c.isGoing = true;
                    //ktra user đã theo dõi giảng viên của khóa học ?
                    Following findFollow = schoolContext.Followings.FirstOrDefault(p =>
                    p.FollowerId == userID && p.FolloweeId == c.LecturerId);
                    if (findFollow == null)
                        c.isFollowing = true;
                }
                c.LecturerId = user.Name;

            }
            
            return View(upcoming);
        }
        [Authorize]
        public ActionResult Mine()
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var courses = context.Courses.Where(c => c.LecturerId == currentUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach( Course i in courses)
            {
                i.LecturerId = currentUser.Name;
            }
            return View(courses);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            BigSchoolContext context = new BigSchoolContext();
            var course = context.Courses.Single(c => c.Id == id && c.LecturerId == userId);
            course.ListCategory = context.Categories.ToList();
            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Course course)
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            course.LecturerId = user.Id;
            context.Courses.AddOrUpdate(course);
            context.SaveChanges();

            var upcoming = context.Courses.Where(p => p.DateTime > DateTime.Now).OrderBy(p => p.DateTime).ToList();
            foreach (Course c in upcoming)
            {
                c.LecturerId = user.Name;
            }
            return View("Index", upcoming);
        }

        [Authorize]
        public ActionResult LectureIamGoing()
        {

            ApplicationUser currentUser =
            System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            //danh sách giảng viên được theo dõi bởi người dùng (đăng nhập) hiện tại
            var listFollwee = context.Followings.Where(p => p.FollowerId ==
            currentUser.Id).ToList();
            //danh sách các khóa học mà người dùng đã đăng ký
            var listAttendances = context.Attendances.Where(p => p.Attendee ==
            currentUser.Id).ToList();
            
            var courses = new List<Course>();
            foreach (var attendance in listAttendances)
            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeId == attendance.Attendee)
                    {
                        var objCourse = context.Courses.First(p => p.DateTime > DateTime.Now && attendance.CourseId == p.Id);
                        ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(objCourse.LecturerId);
                        objCourse.LecturerId = user.Name;
                        courses.Add(objCourse);
                    }
                }
            }
            return View(courses);
        }
    }
    
}