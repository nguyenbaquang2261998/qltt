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
        public IActionResult List([FromQuery] int pageSize, [FromQuery] int pageIndex)
        {
            if (pageSize == 0 || pageIndex == 0)
            {
                pageIndex = 1;
                pageSize = 10;
            }
            var students = _context.Semesters.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return View(students);
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
    }
}
