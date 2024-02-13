using InternManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternManagement.Controllers
{
    [Route("teacher")]
    public class TeacherController : Controller
    {
        private readonly ILogger<TeacherController> _logger;
        private readonly InternManagementContext _context;

        public TeacherController(ILogger<TeacherController> logger, InternManagementContext context)
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
            var students = _context.Teachers.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return View(students);
        }

        [HttpGet("detail")]
        public IActionResult Detail([FromQuery] int id)
        {
            var student = _context.Teachers.Where(x => x.Id == id).FirstOrDefault();
            return View(student);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(Teacher model)
        {
            try
            {
                model.CreatedDate = DateTime.Now;
                _context.Teachers.Add(model);
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
            var teacher = _context.Teachers.Find(id);
            return View(teacher);
        }

        [HttpPost("update")]
        public IActionResult Update(Teacher model)
        {
            try
            {
                if (model == null)
                {
                    return View();
                }

                _context.Teachers.Update(model);
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
            var teacher = _context.Teachers.Find(id);
            var result = _context.Teachers.Remove(teacher);
            _context.SaveChanges();
            return Json(new { status = 1, message = "Xóa thành công" });
        }
    }
}
