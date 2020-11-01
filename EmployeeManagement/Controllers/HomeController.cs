using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    
    public class HomeController:Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(IEmployeeRepository employeeRepository,
                                IWebHostEnvironment webHostEnvironment)
        {
            this._employeeRepository = employeeRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        
        
        
        public ViewResult Index() {
            //return this.Json(new {id=1,name="Hetal" });
            //return _employeeRepository.GetEmployee(1).Name;   
            var model = _employeeRepository.GetAllEmployees();
            return View("~/Views/Home/Index.cshtml",model);
        }

        public ViewResult Details(int? id,string name)
        {

            Employee model = _employeeRepository.GetEmployee(1);
            ViewBag.Employee = model;

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id ?? 1)
            };

            //return "id:"+id.Value.ToString() + "    name:"+name;
            return View(homeDetailsViewModel);
            //return View(model);
            // ViewData["Employee"] = model;
            //return View();
            //return View("../Test/Update");
            //return View("MyViews/Test.cshtml");
            //return View("Test");
            //return this.Json(new {id=1,name="Hetal" });

            //return _employeeRepository.GetEmployee(1).Name;
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo!=null) {
                    string uploadsFolder=Path.Combine(webHostEnvironment.WebRootPath, "images");
                    uniqueFileName=Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder,uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath,FileMode.Create));

                }
                

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                } ;

                _employeeRepository.AddEmployee(newEmployee);

                return RedirectToAction("Details", new { id = newEmployee.Id });
        }
            return View();
    }
    }
}
