using InternManagement.DTOs.Topic;
using InternManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InternManagement.Controllers
{
    [Route("topic")]
    public class TopicController : Controller
    {
        private readonly ILogger<TopicController> _logger;
        private readonly InternManagementContext _context;

        public TopicController(ILogger<TopicController> logger, InternManagementContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet("list")]
        public IActionResult List([FromQuery] int pageSize, [FromQuery] int pageIndex, [FromQuery] string keyword)
        {
            if (pageSize == 0 || pageIndex == 0)
            {
                pageIndex = 1;
                pageSize = 10;
            }
            var topics = from topic in _context.Topics
                         join semester in _context.Semesters on topic.SemesterId equals semester.Id
                         join teacher in _context.Teachers on topic.TeacherId equals teacher.Id
                         select new TopicOuput
                         {
                             Id = topic.Id,
                             Name = topic.Name,
                             Content = topic.Content,
                             Reference = topic.Reference,
                             Semester = semester.Name,
                             Teacher = teacher.Name,
                             CreatedDate = topic.CreatedDate
                         };
            if (!string.IsNullOrEmpty(keyword))
            {
                var keyserch = keyword.ToLower();
                topics = topics.Where(x => x.Name.ToLower().Contains(keyserch) || x.Teacher.ToLower().Contains(keyserch) || x.Semester.ToLower().Contains(keyserch));
            }
            return View(topics.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        [HttpGet("detail")]
        public IActionResult Detail([FromQuery] int id)
        {
            var res = from topic in _context.Topics
                                     join semester in _context.Semesters on topic.SemesterId equals semester.Id
                                     join teacher in _context.Teachers on topic.TeacherId equals teacher.Id
                                     where topic.Id == id
                                     select new TopicOuput
                                     {
                                         Id = topic.Id,
                                         Name = topic.Name,
                                         Content = topic.Content,
                                         Reference = topic.Reference,
                                         Semester = semester.Name,
                                         Teacher = teacher.Name,
                                         CreatedDate = topic.CreatedDate
                                     };
            return View(res.FirstOrDefault());
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var teachers = _context.Teachers.ToList();
            var semesters = _context.Semesters.ToList();
            ViewBag.DataTeachers = new SelectList(teachers, "Id", "Name");
            ViewBag.DataSemesters = new SelectList(semesters, "Id", "Name");
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(Topic model)
        {
            try
            {
                if (model == null)
                {
                    return View();
                }

                model.CreatedDate = DateTime.Now;
                _context.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Thành công";

                return RedirectToAction("list"); // Redirect to the desired view
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpGet("update")]
        public IActionResult Update([FromQuery] int id)
        {
            var teachers = _context.Teachers.ToList();
            var semesters = _context.Semesters.ToList();
            ViewBag.DataTeachers = new SelectList(teachers, "Id", "Name");
            ViewBag.DataSemesters = new SelectList(semesters, "Id", "Name");

            var res = _context.Topics.Find(id);
            return View(res);
        }

        [HttpPost("update")]
        public IActionResult Update(Topic model)
        {
            try
            {
                if (model == null)
                {
                    return View();
                }

                _context.Topics.Update(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Thành công";

                return RedirectToAction("list"); // Redirect to the desired view
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var topic = _context.Topics.Find(id);
            var result = _context.Topics.Remove(topic);
            _context.SaveChanges();
            return Json(new { status = 1, message = "Xóa thành công" });
        }
    }
}
