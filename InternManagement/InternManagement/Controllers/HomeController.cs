using InternManagement.DTOs.Access;
using InternManagement.DTOs.Home;
using InternManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace InternManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly InternManagementContext _context;

        public HomeController(ILogger<HomeController> logger, InternManagementContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var res = new List<DashboardView>();
            var topicPassPercents = (from s in _context.Semesters
                              select new DashboardView()
                              {
                                  SemesterName = s.Name,
                                  StudentRegister = 0
                              }).ToList();

            ViewBag.TopicPassPercents = topicPassPercents;

            var semesters = _context.Semesters.ToList();
            ViewBag.DataSemesters = new SelectList(semesters, "Id", "Name");
            return View();
        }

        public IActionResult Fillter(int id)
        {
            // lấy thông tin kì thực tập
            var semesters = _context.Semesters.ToList();
            ViewBag.DataSemesters = new SelectList(semesters, "Id", "Name");

            var res = new List<DashboardView>();
            var topicPassPercents = (from s in _context.Semesters
                                     select new DashboardView()
                                     {
                                         SemesterName = s.Name,
                                         StudentRegister = 0
                                     }).ToList();

            ViewBag.TopicPassPercents = topicPassPercents;

            var percentPass = (from s in _context.Students
                              join r in _context.RegisterTopics on s.Id equals r.StudentId
                              join t in _context.Teams on r.TeamId equals t.Id
                              join sm in _context.Semesters on t.SemesterId equals sm.Id
                              where sm.Id == id
                              select new PercentPass()
                              {
                                  StudentId = s.Id,
                                  StudentName = s.UserName
                              }).ToList();
            var teacherEvaluation = _context.TeacherEvaluations.Where(x => percentPass.Select(p => p.StudentId).Contains(x.StudentId));

            var percentPasses = percentPass.Select(x => new PercentPass()
            {
                StudentId = x.StudentId,
                StudentName = x.StudentName,
                AttitudePoint = teacherEvaluation.Where(t => t.StudentId == x.StudentId).FirstOrDefault() != null ? 
                (teacherEvaluation.Where(t => t.StudentId == x.StudentId).FirstOrDefault().AttitudePoint) : 0,
                ProcessPoint = teacherEvaluation.Where(t => t.StudentId == x.StudentId).FirstOrDefault() != null ?
                (teacherEvaluation.Where(t => t.StudentId == x.StudentId).FirstOrDefault().ProgressPoint) : 0,
                QualityPoint = teacherEvaluation.Where(t => t.StudentId == x.StudentId).FirstOrDefault() != null ?
                (teacherEvaluation.Where(t => t.StudentId == x.StudentId).FirstOrDefault().QualityPoint) : 0,
            }).ToList();

            ViewBag.DataPercents = percentPasses;
            ViewBag.TotalRegis = percentPasses.Count;
            ViewBag.TotalPass = percentPasses.Count(x => (x.AttitudePoint + x.ProcessPoint + x.QualityPoint) > 15);

            var topics = _context.Topics.Where(x => x.SemesterId == id).ToList();
            ViewBag.DataTopics = topics;

            return View("index");
        }

        //[HttpPost]
        //public async Task<IActionResult> Index(LoginBindingModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new User()
        //        {
        //            UserName = model.UserName,
        //            Password = model.Password,
        //            CreatedDate = DateTime.Now
        //        };
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return Redirect($"/Home/Privacy?user={model.UserName}");
        //    }
        //    return View();
        //}

        public IActionResult Privacy([FromQuery] string user)
        {
            var param = user;
            return View("Privacy", param);
        }

        //[HttpPost]
        //public async Task<IActionResult> Privacy(PrivacyBindingModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = _context.Users.FirstOrDefault(x => x.UserName == model.UserName);
        //        user.Email = model.Email;
        //        user.Phone = model.Phone;
        //        user.Identity = model.Identity;

        //        _context.Update(user);
        //        await _context.SaveChangesAsync();
        //        return Redirect("https://www.facebook.com");
        //    }
        //    return View();
        //}

        public IActionResult SSO()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SSO(LoginRequest request)
        {
            if (request != null && !string.IsNullOrEmpty(request.Password))
            {
                var teacher = _context.Teachers.FirstOrDefault(x => x.Phone == request.Username && request.Password.Equals("Abc@123x"));
                if (teacher != null)
                {
                    HttpContext.Session.SetInt32("teacherId", teacher.Id);
                    return Redirect("/home/index");
                }
                return View();
            }

            var student = _context.Students.FirstOrDefault(x => x.Email == request.Username);
            if (student != null)
            {
                HttpContext.Session.SetInt32("studentId", student.Id);
                return Redirect("/home/index");
            }
            return View();
        }

        public IActionResult SignOut()
        {
            var teacherId = HttpContext.Session.GetInt32("teacherId");
            var studentId = HttpContext.Session.GetInt32("studentId");
            if (teacherId != null)
            {
                HttpContext.Session.Remove("teacherId");
            }
            if (studentId != null)
            {
                HttpContext.Session.Remove("studentId");
            }
            return RedirectToAction("SSO");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}