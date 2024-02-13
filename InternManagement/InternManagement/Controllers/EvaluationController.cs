using InternManagement.DTOs.Evaluation;
using InternManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Xceed.Words.NET;

namespace InternManagement.Controllers
{
    [Route("evaluation")]
    public class EvaluationController : Controller
    {
        private readonly ILogger<EvaluationController> _logger;
        private readonly InternManagementContext _context;

        public EvaluationController(ILogger<EvaluationController> logger, InternManagementContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet("list")]
        public IActionResult List()
        {
            var studentId = HttpContext.Session.GetInt32("studentId");
            if (studentId == null)
            {
                return View();
            }
            var registerTopics = (from rt in _context.RegisterTopics
                                 join t in _context.Topics on rt.TopicId equals t.Id
                                 join ts in _context.Teams on rt.TeamId equals ts.Id
                                 join tc in _context.Teachers on t.TeacherId equals tc.Id
                                 join s in _context.Semesters on t.SemesterId equals s.Id
                                 where rt.StudentId == studentId
                                 select new RegisterTopicStudent()
                                 {
                                     StudentId = rt.StudentId,
                                     TopicId = rt.TopicId,
                                     TopicName = t.Name,
                                     TeamId = rt.TeamId,
                                     TeacherId = tc.Id,
                                     TeacherName = tc.Name,
                                     SemesterId = t.SemesterId.Value,
                                     SemesterName = s.Name
                                 }).ToList();
            var evaluation = _context.InternshipEvaluations.Where(x => x.StudentId == studentId).ToList();

            var res = registerTopics.Select(x => new RegisterTopicStudent()
            {
                StudentId = x.StudentId,
                TopicId = x.TopicId,
                TopicName = x.TopicName,
                TeamId = x.TeamId,
                TeacherId = x.TeacherId,
                TeacherName = x.TeacherName,
                SemesterId = x.SemesterId,
                SemesterName = x.SemesterName,
                Status = (evaluation != null && evaluation.Count(e => e.TopicId == x.TopicId) > 0) ? 1 : 0
            }).ToList();
            return View(res);
        }

        [HttpGet]
        public IActionResult Teacher()
        {
            return View();
        }

        [HttpPost("teacher")]
        public IActionResult Teacher(TeacherEvaluation model)
        {
            try
            {
                _context.TeacherEvaluations.Add(model);
                _context.SaveChanges();
                return RedirectToAction("list");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet("student")]
        public IActionResult Student(int id)
        {
            var res = new InternshipEvaluationInfo();
            var studentId = HttpContext.Session.GetInt32("studentId");
            if (studentId != null)
            {
                var studentInfo = _context.Students.FirstOrDefault(x => x.Id == studentId);
                if (studentInfo != null)
                {
                    res.StudentInfo = new StudentInfo
                    {
                        Id = studentInfo.Id,
                        Identity = studentInfo.Identity,
                        Name = studentInfo.UserName
                    };
                }

                var topicInfo = _context.Topics.FirstOrDefault(x => x.Id == id);
                res.TopicInfo = new TopicInfo()
                {
                    Id = topicInfo.Id,
                    Name = topicInfo.Name,
                    Content = topicInfo.Content,
                    TeacherId = topicInfo.TeacherId.Value
                };
                ViewBag.DataInfo = res;
                return View();
            }
            return View();
        }

        [HttpPost("student-create")]
        public IActionResult Student(InternshipEvaluation model)
        {
            try
            {
                var teacher = _context.Teachers.Where(x => x.Id == model.TeacherId).FirstOrDefault();
                model.TeacherName = teacher.Name;

                // tạo file
                var file = SaveToDocx();
                model.Docs = file;

                _context.InternshipEvaluations.Add(model);
                _context.SaveChanges();
                return RedirectToAction("list");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet("student-view")]
        public IActionResult StudentView([FromQuery]int id)
        {
            try
            {
                var studentId = HttpContext.Session.GetInt32("studentId");
                if (studentId != null)
                {
                    var res = new InternshipEvaluationInfo();
                    var studentInfo = _context.Students.FirstOrDefault(x => x.Id == studentId);
                    if (studentInfo != null)
                    {
                        res.StudentInfo = new StudentInfo
                        {
                            Id = studentInfo.Id,
                            Identity = studentInfo.Identity,
                            Name = studentInfo.UserName
                        };
                    }

                    var topicInfo = _context.Topics.FirstOrDefault(x => x.Id == id);
                    res.TopicInfo = new TopicInfo()
                    {
                        Id = topicInfo.Id,
                        Name = topicInfo.Name,
                        Content = topicInfo.Content,
                        TeacherId = topicInfo.TeacherId.Value,
                        Deadline = topicInfo.Deadline
                    };
                    ViewBag.DataInfo = res;
                }
                var evaluation = _context.InternshipEvaluations.FirstOrDefault(x => x.StudentId == studentId && x.TopicId == id);

                return View(evaluation);
            }
            catch (Exception)
            {
                return View();
            }
        }

        public string SaveToDocx()
        {
            var now = Guid.NewGuid().ToString();
            var fileName = now + ".docx";
            // Create a new document
            var doc = DocX.Create(fileName);

            // Insert your data into the document
            doc.InsertParagraph("This is a sample text.");

            // Save the document
            doc.Save();

            return fileName;
        }
    }
}
