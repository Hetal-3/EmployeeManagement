using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeCreateViewModel
    {
        
        [Required]
        [MaxLength(50, ErrorMessage = "Name can not exceed 50 characters")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        //[RegularExpression(@"^([a - zA - Z0 - 9_\-\.] +)@([a - zA - Z0 - 9_\-\.] +)\.([a - zA - Z]{2, 5})$",ErrorMessage ="Invalid Email format. Please enter valid email eg:xyz@gmail.com")]
        [Display(Name = "Office email")]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }
        
        [BindProperty]
        public IFormFile Photo { get; set; }
    }
}
