using Bai4_lab1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bai4_lab1.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpGet]
        [Route("api/Attendances")]
        public IHttpActionResult Attendances(int id)
        {
            var userID = User.Identity.GetUserId();
            BigSchoolContext context = new BigSchoolContext();
            
            var attendance = new Attendance()
            {
                CourseId = id,
                Attendee = userID
            };
            try
            {
                if( context.Attendances.Any(a => a.Attendee== attendance.Attendee && a.CourseId == attendance.CourseId))
                {
                    var obj = context.Attendances.First(a => a.Attendee == attendance.Attendee && a.CourseId == attendance.CourseId);
                    context.Attendances.Remove(obj);
                    context.SaveChanges();
                    return Ok("remove");
                }
                else
                {
                    context.Attendances.Add(attendance);
                    context.SaveChanges();
                }
                
                return Ok("add");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
            
            
        }
    }
}
