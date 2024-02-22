using InternManagement.DTOs.Student;
using InternManagement.Extensions;
using InternManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternManagement.Controllers
{
    [Route("student")]
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly InternManagementContext _context;

        public StudentController(ILogger<StudentController> logger, InternManagementContext context)
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

            var classOutput = ApplicationConfig.GetClass();
            var students = from s in _context.Students.ToList()
                           join c in classOutput on s.ClassCode equals c.ClassCode
                           select new Student()
                           {
                               Id = s.Id,
                               UserName = s.UserName,
                               Address = s.Address,
                               Birthday = s.Birthday,
                               ClassCode = s.ClassCode,
                               ClassName = c.Name,
                               Email = s.Email,
                               Identity = s.Identity,
                               Phone = s.Phone,
                               SubPhone = s.SubPhone,
                               Password = s.Password
                           };
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.ToLower();
                students = students.Where(x => x.UserName.ToLower().Contains(keysearch) || x.Identity.ToLower().Contains(keysearch));
            }
            var res = students.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return View(res);
        }

        [HttpGet("detail")]
        public IActionResult Detail([FromQuery] int id)
        {
            var student = _context.Students.Where(x => x.Id == id).FirstOrDefault();
            return View(student);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            var classOutput = ApplicationConfig.GetClass();
            ViewBag.DataClasses = new SelectList(classOutput, "ClassCode", "Name");
            return View();
        }

        [HttpPost("create")]
        public IActionResult Create(Student model)
        {
            try
            {
                if (model == null)
                {
                    return View();
                }

                var classOutput = ApplicationConfig.GetClass();
                var student = new Student()
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Address = model.Address,
                    Email = model.Email,
                    Phone = model.Phone,
                    SubPhone = model.SubPhone,
                    ClassCode = model.ClassCode,
                    ClassName = classOutput.FirstOrDefault(x => x.ClassCode == model.ClassCode).Name,
                    Identity = model.Identity,
                    Birthday = model.Birthday,
                    CreatedDate = DateTime.Now
                };

                _context.Add(student);
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
            var classOutput = ApplicationConfig.GetClass();
            ViewBag.DataClasses = new SelectList(classOutput, "ClassCode", "Name");
            var student = _context.Students.Find(id);
            return View(student);
        }

        [HttpPost("update")]
        public IActionResult Update(Student model)
        {
            try
            {
                if (model == null)
                {
                    return View();
                }

                var classOutput = ApplicationConfig.GetClass();

                model.ClassCode = model.ClassCode;
                model.ClassName = classOutput.FirstOrDefault(x => x.ClassCode == model.ClassCode).Name;

                _context.Students.Update(model);
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
            var student = _context.Students.Find(id);
            var result = _context.Students.Remove(student);
            _context.SaveChanges();
            return Json(new { status = 1, message = "Xóa thành công" });
        }
    }
}
