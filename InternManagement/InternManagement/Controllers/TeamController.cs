using InternManagement.DTOs.Team;
using InternManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternManagement.Controllers
{
    [Route("team")]
    public class TeamController : Controller
    {
        private readonly ILogger<TeamController> _logger;
        private readonly InternManagementContext _context;

        public TeamController(ILogger<TeamController> logger, InternManagementContext context)
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

            var teams = from t in _context.Teams
                           join s in _context.Semesters on t.SemesterId equals s.Id
                           join tp in _context.Topics on t.TopicId equals tp.Id
                           select new TeamOutput()
                           {
                               Id = t.Id,
                               SemesterId = t.SemesterId,
                               SemesterName = s.Name,
                               TopicName = tp.Name,
                               TopicId = t.TopicId,
                               TeamSize = t.TeamSize,
                           };
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.ToLower();
                teams = teams.Where(x => x.SemesterName.ToLower().Contains(keysearch) || x.TopicName.ToLower().Contains(keysearch));
            }
            var res = teams.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return View(res);
        }

        [HttpGet("detail")]
        public IActionResult Detail([FromQuery] int id)
        {
            var student = _context.Teams.Where(x => x.Id == id).FirstOrDefault();
            return View(student);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var topic = _context.Topics.ToList();
            ViewBag.DataTopics = new SelectList(topic, "Id", "Name");
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(Team model)
        {
            try
            {
                if (model == null)
                {
                    return View();
                }
                var topic = _context.Topics.FirstOrDefault(x => x.Id == model.TopicId);
                model.SemesterId = _context.Semesters.FirstOrDefault(x => x.Id == topic.SemesterId).Id;
                _context.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật thành công thông tin sinh viên";

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
            var topic = _context.Topics.ToList();
            ViewBag.DataTopics = new SelectList(topic, "Id", "Name");
            var team = _context.Teams.FirstOrDefault(x => x.Id == id);
            return View(team);
        }

        [HttpPost("update")]
        public IActionResult Update(Team model)
        {
            try
            {
                if (model == null)
                {
                    return View();
                }

                var topic = _context.Topics.FirstOrDefault(x => x.Id == model.TopicId);
                model.SemesterId = _context.Semesters.FirstOrDefault(x => x.Id == topic.SemesterId).Id;

                _context.Teams.Update(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật thành công thông tin sinh viên";

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
            var team = _context.Teams.Find(id);
            var result = _context.Teams.Remove(team);
            _context.SaveChanges();
            return Json(new { status = 1, message = "Xóa thành công" });
        }
    }
}
