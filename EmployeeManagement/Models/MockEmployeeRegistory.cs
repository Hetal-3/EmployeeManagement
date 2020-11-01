using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRegistory: IEmployeeRepository
    {

        private List<Employee> _employeeList;

        public MockEmployeeRegistory()
        {
            _employeeList = new List<Employee>()
            {
                new Employee(){Id=1,Name="Hetal",Department=Dept.IT,Email="hetalchavan@yahoo.com" },
                new Employee(){Id=2,Name="Mahek",Department=Dept.HR,Email="mahekchavan@yahoo.com" },
                new Employee(){Id=3,Name="Rohn",Department=Dept.None,Email="rohn@xyz.com" }
            };
        }

        public Employee AddEmployee(Employee employee)
        {
            employee.Id=_employeeList.Max(e => e.Id)+1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int Id)
        {
            Employee employee=_employeeList.FirstOrDefault(e=>e.Id==Id);
            if (employee!=null) {
                _employeeList.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee!=null) {
                employee.Name = employeeChanges.Name;
                employee.Department = employeeChanges.Department;
                employee.Email = employeeChanges.Email;
            }
            return employee;
        }

        Employee IEmployeeRepository.GetEmployee(int Id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }
    }
}
