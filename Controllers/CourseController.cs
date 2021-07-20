using Bai4_lab1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bai4_lab1.Controllers.Api
{
    public class CourseController : ApiController
    {
        

        [HttpGet]
        [Route("api/DeteleCourse")]
        public IHttpActionResult DeteleCourse(int id)
        {
            BigSchoolContext schoolContext = new BigSchoolContext();
            var userId = User.Identity.GetUserId();
            var course = schoolContext.Courses.Single(c => c.Id == id && c.LecturerId == userId);
           
            try
            {
                var Attend = schoolContext.Attendances.Single(p => p.Attendee == userId && p.CourseId == id);
                schoolContext.Attendances.Remove(Attend);
                schoolContext.Courses.Remove(course);
                schoolContext.SaveChanges();
                return Json(new
                {
                    check = "Ok"
                });
            }
            catch(Exception e)
            {
                e.Message.ToString();
                return Json(new
                {
                    check = "Null"
                });
            }
        }
    }
}