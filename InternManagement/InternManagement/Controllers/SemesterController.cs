using InternManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternManagement.Controllers
{
    [Route("semester")]
    public class SemesterController : Controller
    {
        private readonly ILogger<SemesterController> _logger;
        private readonly InternManagementContext _context;

        public SemesterController(ILogger<SemesterController> logger, InternManagementContext context)
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
            var semesters = _context.Semesters.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.ToLower();
                semesters = semesters.Where(x => x.Name.ToLower().Contains(keysearch));
            }
            semesters.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var res = semesters.ToList();
            return View(res);
        }

        [HttpGet("detail")]
        public IActionResult Detail([FromQuery] int id)
        {
            var student = _context.Semesters.Where(x => x.Id == id).FirstOrDefault();
            return View(student);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(Semester model)
        {
            try
            {
                _context.Semesters.Add(model);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Success";

                return RedirectToAction("list"); // Redirect to the desired view
            }
            catch (Exception)
            {
                return View();
            }
        }
        [HttpGet("update")]
        public IActionResult Update([FromQuery] int id)
        {
            var res = _context.Semesters.Find(id);
            return View(res);
        }

        [HttpPost("update")]
        public IActionResult Update(Semester model)
        {
            try
            {
                if (model == null)
                {
                    return View();
                }

                _context.Semesters.Update(model);
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
            var semester = _context.Semesters.Find(id);
            var result = _context.Semesters.Remove(semester);
            _context.SaveChanges();
            return Json(new { status = 1, message = "Xóa thành công" });
        }
    }
}
