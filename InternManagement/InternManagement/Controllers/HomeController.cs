﻿using InternManagement.DTOs.Access;
using InternManagement.Models;
using Microsoft.AspNetCore.Mvc;
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
            return View();
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}