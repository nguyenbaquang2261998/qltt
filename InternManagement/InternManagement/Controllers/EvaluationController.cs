using InternManagement.DTOs.Evaluation;
using InternManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using Xceed.Words.NET;

namespace InternManagement.Controllers
{
    [Route("evaluation")]
    public class EvaluationController : Controller
    {
        private readonly ILogger<EvaluationController> _logger;
        private readonly InternManagementContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public EvaluationController(ILogger<EvaluationController> logger, InternManagementContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _environment = environment;
        }


        [HttpGet("list")]
        public IActionResult List([FromQuery] string keyword)
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

            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.ToLower();
                res = res.Where(x => x.TopicName.ToLower().Contains(keysearch) || x.TeacherName.ToLower().Contains(keysearch) || x.SemesterName.ToLower().Contains(keysearch)).ToList();
            }
            return View(res);
        }

        [HttpGet("list-student-evaluation")]
        public IActionResult ListStudentEvaluation([FromQuery] string keyword)
        {
            var registerTopics = (from rt in _context.RegisterTopics
                                  join t in _context.Topics on rt.TopicId equals t.Id
                                  join ts in _context.Teams on rt.TeamId equals ts.Id
                                  join tc in _context.Teachers on t.TeacherId equals tc.Id
                                  join s in _context.Semesters on t.SemesterId equals s.Id
                                  where rt.StudentId > 0
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
            var evaluation = _context.InternshipEvaluations.Where(x => x.StudentId > 0).ToList();

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

            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.ToLower();
                res = res.Where(x => x.TopicName.ToLower().Contains(keysearch) || x.TeacherName.ToLower().Contains(keysearch) || x.SemesterName.ToLower().Contains(keysearch)).ToList();
            }
            return View(res);
        }

        [HttpGet("list-teacher")]
        public IActionResult ListTeacher([FromQuery] string keyword)
        {
            var teacherId = HttpContext.Session.GetInt32("teacherId");
            // lấy đề tài mà gv hướng dẫn
            var topicSupports = _context.Topics.Where(x => x.TeacherId == teacherId).Select(x => x.Id).ToList();

            // danh sách  sinh viên đăng kí đề tài
            var regisTopic = _context.RegisterTopics.Where(x => topicSupports.Contains(x.TopicId)).ToList();

            if (teacherId == null)
            {
                return View();
            }
            var registerTopics = (from rt in regisTopic
                                  join t in _context.Topics on rt.TopicId equals t.Id
                                  join ts in _context.Teams on rt.TeamId equals ts.Id
                                  join tc in _context.Teachers on t.TeacherId equals tc.Id
                                  join s in _context.Semesters on t.SemesterId equals s.Id
                                  join st in _context.Students on rt.StudentId equals st.Id
                                  select new RegisterTopicStudent()
                                  {
                                      StudentId = rt.StudentId,
                                      StudentName = st.UserName,
                                      TopicId = rt.TopicId,
                                      TopicName = t.Name,
                                      TeamId = rt.TeamId,
                                      TeacherId = tc.Id,
                                      TeacherName = tc.Name,
                                      SemesterId = t.SemesterId.Value,
                                      SemesterName = s.Name
                                  }).ToList();
            var evaluation = _context.TeacherEvaluations.Where(x => x.TeacherId == teacherId).ToList();

            var res = registerTopics.Select(x => new RegisterTopicStudent()
            {
                StudentId = x.StudentId,
                StudentName = x.StudentName,
                TopicId = x.TopicId,
                TopicName = x.TopicName,
                TeamId = x.TeamId,
                TeacherId = x.TeacherId,
                TeacherName = x.TeacherName,
                SemesterId = x.SemesterId,
                SemesterName = x.SemesterName,
                Status = (evaluation != null && evaluation.Count(e => e.TopicId == x.TopicId && e.StudentId == x.StudentId) > 0) ? 1 : 0
            }).ToList();

            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.ToLower();
                res = res.Where(x => x.TopicName.ToLower().Contains(keysearch) || x.TeacherName.ToLower().Contains(keysearch) || x.SemesterName.ToLower().Contains(keysearch)).ToList();
            }
            return View(res);
        }

        [HttpGet("teacher")]
        public IActionResult Teacher([FromQuery] int studentId, [FromQuery] int topicId)
        {
            var teacherId = HttpContext.Session.GetInt32("teacherId");

            var res = new InternshipEvaluationInfo();
            if (studentId > 0)
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

                var topicInfo = _context.Topics.FirstOrDefault(x => x.Id == topicId);
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

        [HttpPost("teacher-create")]
        public IActionResult Teacher(TeacherEvaluation model)
        {
            try
            {
                model.CreatedDate = DateTime.Now;
                var teacher = _context.Teachers.FirstOrDefault(x => x.Id == model.Id);
                model.TeacherName = teacher != null ? teacher.Name : "Hội đồng" ;
                _context.TeacherEvaluations.Add(model);
                _context.SaveChanges();
                return RedirectToAction("ListTeacher");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpGet("teacher-view")]
        public IActionResult TeacherView([FromQuery] int studentId, [FromQuery] int topicId)
        {
            try
            {
                var teacherId = HttpContext.Session.GetInt32("teacherId");
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

                    var topicInfo = _context.Topics.FirstOrDefault(x => x.Id == topicId);
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
                var evaluation = _context.TeacherEvaluations.FirstOrDefault(x => x.StudentId == studentId && x.TopicId == topicId);

                return View(evaluation);
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
                var file = SaveToDocx(model);
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
        public IActionResult StudentView([FromQuery] int? studentId, [FromQuery]int id)
        {
            try
            {
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

        public string SaveToDocx(InternshipEvaluation model)
        {
            var now = Guid.NewGuid().ToString();
            string wwwPath = _environment.WebRootPath;
            var fileName = wwwPath + "\\upload\\" + now + ".docx";
            // Create a new document
            var doc = DocX.Create(fileName);

            // Insert your data into the document
            var student = _context.Students.FirstOrDefault(x => x.Id == model.StudentId);
            doc.InsertParagraph("Cộng hòa xã hội chủ nghĩa Việt Nam");
            doc.InsertParagraph("Độc lập tự do hạnh phúc");
            doc.InsertParagraph("------------------------");
            doc.InsertParagraph("ĐÁNH GIÁ KẾT QUẢ THỰC TẬP");
            doc.InsertParagraph("Sinh viên: " + student.UserName);
            doc.InsertParagraph("Mã sinh viên: " + student.UserName);
            doc.InsertParagraph("Đề tài: " + model.TopicName);
            doc.InsertParagraph("GV hướng dẫn: " + model.TeacherName);

            var teacherReview = StripHtml(model.EvaluateTeacher);
            doc.InsertParagraph("1.Đánh giá quá trình thực tập:");
            doc.InsertParagraph(teacherReview);

            var topicReview = StripHtml(model.EvaluateTopic);
            doc.InsertParagraph("2.Đánh giá kết quả:");
            doc.InsertParagraph(topicReview);

            doc.InsertParagraph("Ngày thực hiện: " + DateTime.Now.ToString("dd-MM-yyyy"));

            // Save the document
            doc.Save();

            return fileName;
        }

        //public string TeacherSaveToDocx(TeacherEvaluation model)
        //{
        //    var now = Guid.NewGuid().ToString();
        //    string wwwPath = _environment.WebRootPath;
        //    var fileName = wwwPath + "\\upload\\" + now + ".docx";
        //    // Create a new document
        //    var doc = DocX.Create(fileName);

        //    // Insert your data into the document
        //    var student = _context.Students.FirstOrDefault(x => x.Id == model.StudentId);
        //    doc.InsertParagraph("Cộng hòa xã hội chủ nghĩa Việt Nam");
        //    doc.InsertParagraph("Độc lập tự do hạnh phúc");
        //    doc.InsertParagraph("------------------------");
        //    doc.InsertParagraph("ĐÁNH GIÁ KẾT QUẢ THỰC TẬP");
        //    doc.InsertParagraph("Sinh viên: " + student.UserName);
        //    doc.InsertParagraph("Mã sinh viên: " + student.UserName);
        //    doc.InsertParagraph("Đề tài: " + model.TopicName);
        //    doc.InsertParagraph("GV hướng dẫn: " + model.TeacherName);

        //    var teacherReview = StripHtml(model.EvaluateTeacher);
        //    doc.InsertParagraph("1.Đánh giá quá trình thực tập:");
        //    doc.InsertParagraph(teacherReview);

        //    var topicReview = StripHtml(model.EvaluateTopic);
        //    doc.InsertParagraph("2.Đánh giá kết quả:");
        //    doc.InsertParagraph(topicReview);

        //    doc.InsertParagraph("Ngày thực hiện: " + DateTime.Now.ToString("dd-MM-yyyy"));

        //    // Save the document
        //    doc.Save();

        //    return fileName;
        //}

        public static string StripHtml(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}
