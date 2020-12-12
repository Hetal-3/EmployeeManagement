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
            throw new Exception("Error in details view");
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee == null) {
                Response.StatusCode = 404;
                return View("EmployeeNotFound",id.Value);
            
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
                PageTitle="Employee Details"
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

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                Email=employee.Email,
                Department = employee.Department,
                ExistingPhotoPath=employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;

                string uniqueFileName = ProcessUploadedFile(model);
                if (uniqueFileName != null) {
                    employee.PhotoPath = uniqueFileName;
                    if (model.ExistingPhotoPath != null) {
                        string filePath=Path.Combine(webHostEnvironment.WebRootPath,"images",model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                
                }

                _employeeRepository.Update(employee);

                return RedirectToAction("Index");
            }
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream =new FileStream(filePath, FileMode.Create)) {
                    model.Photo.CopyTo(fileStream);
                }

            }

            return uniqueFileName;
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                

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
