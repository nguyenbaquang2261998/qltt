﻿using InternManagement.DTOs.RegisterTopic;
using InternManagement.DTOs.Team;
using InternManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternManagement.Controllers
{
    [Route("register-topic")]
    public class RegisterTopicController : Controller
    {
        private readonly ILogger<RegisterTopicController> _logger;
        private readonly InternManagementContext _context;

        public RegisterTopicController(ILogger<RegisterTopicController> logger, InternManagementContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("list")]
        public IActionResult List([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            if (pageSize == 0 || pageIndex == 0)
            {
                pageIndex = 1;
                pageSize = 10;
            }

            var data = from r in _context.RegisterTopics
                        join s in _context.Students on r.StudentId equals s.Id
                        join tp in _context.Topics on r.TopicId equals tp.Id
                        join t in _context.Teams on r.TeamId equals t.Id
                        select new RegisterTopicOutput()
                        {
                            Id = t.Id,
                            StudentName = s.UserName,
                            TopicName = tp.Name,
                            Team = t.Id,
                            CreatedDate = r.CreatedDate
                        };
            var res = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return View(res);
        }

        //[HttpGet("detail")]
        //public IActionResult Detail([FromQuery] int id)
        //{
        //    var student = _context.Teams.Where(x => x.Id == id).FirstOrDefault();
        //    return View(student);
        //}

        [HttpGet("team")]
        public IActionResult Team(int id)
        {
            ViewBag.Show = false;
            var teams = _context.Teams.ToList();
            ViewBag.DataTeams = new SelectList(teams, "Id", "Id");
            var studentId = HttpContext.Session.GetInt32("studentId");
            var student = _context.Students.FirstOrDefault(x => x.Id == studentId);
            if (id == 0)
            {
                return View();
            }
            else
            {
                ViewBag.Show = true;
                ViewBag.IsRegisted = false;
                var team = (from t in _context.Teams
                           join s in _context.Semesters on t.SemesterId equals s.Id
                           join tp in _context.Topics on t.TopicId equals tp.Id
                           where t.Id == id
                           select new TeamOutput()
                           {
                               Id = t.Id,
                               SemesterId = t.SemesterId,
                               SemesterName = s.Name,
                               TopicName = tp.Name,
                               TopicId = t.TopicId,
                               TeamSize = t.TeamSize,
                               Students = new List<StudentRegister>(),
                               StudentInfo = new StudentInfo()
                               {
                                   Id = student != null ? student.Id : 0,
                                   Name = student != null ? student.UserName : "Vui lòng đăng nhập",
                                   Identity = student != null ? student.Identity : "Không xác định"
                               }
                           }).FirstOrDefault();

                var studentTeam = _context.RegisterTopics.Where(x => x.TeamId == id);
                if (studentTeam != null && studentTeam.Any())
                {
                    var students = studentTeam.Select(st => new StudentRegister()
                    {
                        Id = _context.Students.FirstOrDefault(x => x.Id == st.StudentId).Id,
                        Identity = _context.Students.FirstOrDefault(x => x.Id == st.StudentId).Identity,
                        Name = _context.Students.FirstOrDefault(x => x.Id == st.StudentId).UserName
                    }).ToList();
                    team.Students.AddRange(students);

                    var isRegisted = students.Count(x => x.Id == studentId);
                    
                    if (isRegisted > 0)
                    {
                        ViewBag.IsRegisted = true;
                    }
                    return View(team);
                }
                
                return View(team);
            }
        }

        [HttpPost("create")]
        public IActionResult Create(RegisterTopic model)
        {
            try
            {
                if (model == null)
                {
                    return View();
                }
                _context.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật thành công thông tin sinh viên";

                return RedirectToAction("team"); // Redirect to the desired view
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var team = _context.RegisterTopics.Find(id);
            var result = _context.RegisterTopics.Remove(team);
            _context.SaveChanges();
            return Json(new { status = 1, message = "Xóa thành công" });
        }
    }
}