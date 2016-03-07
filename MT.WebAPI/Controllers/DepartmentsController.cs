using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MT.Repository.Entities;
using MT.Repository.Repositories;
using MT.WebAPI.HttpPipeline;

namespace MT.WebAPI.Controllers
{
    [RoutePrefix("api/departments")]
    public class DepartmentsController : ApiController
    {
        private readonly IEmployeeRepository _employeeRepository;
        public DepartmentsController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [Route("")]
        [HttpGet]
        public List<object> GetAlltDepartements()
        {
            var departments = new List<object>();
            foreach (var item in Enum.GetValues(typeof(Department)))
            {
                departments.Add(new
                {
                    id = (int)item,
                    name = item.ToString()
                });
            }
            return departments;
        }

        [Route("{departmentId:range(0,2)}/employees/{isActive:bool=true}")]
        [HttpGet]
        public IEnumerable<Employee> GetEmployeesByDepartmentAndActivity(int departmentId, bool isActive)
        {
            IEnumerable<Employee> employees = _employeeRepository.GetByIsActive(e => e.IsActive.Equals(isActive) && e.Department == (Department)departmentId);
            var employeesByActivity = employees as Employee[] ?? employees.ToArray();
            if (employeesByActivity.Any())
            {
                return employeesByActivity;
            }
            throw new ApiException(HttpStatusCode.NotFound, "There are no employees with supplied , isActive or departmentId ");
        }
    }
}
