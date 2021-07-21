using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Bai4_lab1.Models
{
    public class FollowingController : ApiController
    {
        [HttpGet]
        [Route("api/Following")]
        public IHttpActionResult Following(int id)
        {
            BigSchoolContext schoolContext = new BigSchoolContext();
            var Follower = User.Identity.GetUserId();
            var course = schoolContext.Courses.Single(c => c.Id == id);
            var Followee = course.LecturerId;
            Following following = new Following()
            {
                FollowerId = Follower,
                FolloweeId = Followee
            };
            try
            {
                if(schoolContext.Followings.Any( f => f.FolloweeId == following.FolloweeId && f.FollowerId ==following.FollowerId))
                {
                    var obj = schoolContext.Followings.First(f => f.FolloweeId == following.FolloweeId && f.FollowerId == following.FollowerId);
                    schoolContext.Followings.Remove(obj);
                    schoolContext.SaveChanges();
                    return Ok("remove");
                }
                else
                {
                    schoolContext.Followings.Add(following);
                    schoolContext.SaveChanges();
                    return Ok("add");
                }
                
            }
            catch(Exception e)
            {
                return BadRequest(e.Message.ToString());
            }
        }
    }
}