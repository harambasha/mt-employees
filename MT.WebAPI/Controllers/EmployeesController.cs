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
    [RoutePrefix("api/employees")]
    public class EmployeesController : ApiController
    {

        private  readonly IEmployeeRepository _employeeRepository;
        public EmployeesController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [Route("")]
        public IEnumerable<Employee> GetAll()
        {
            return _employeeRepository.GetAll();
        }

        [Route("{isActive:bool}")]
        public IEnumerable<Employee> GetEmployeesByActivity(bool isActive)
        {
            IEnumerable<Employee> employees = _employeeRepository.GetByIsActive(e=>e.IsActive.Equals(isActive));
            var employeesByActivity = employees as Employee[] ?? employees.ToArray();
            if (employeesByActivity.Any())
            {
                return employeesByActivity;
            }
            throw new ApiException(HttpStatusCode.NotFound, "There are no employees with supplied , isActive  ");
        }

        [Route("{id}")]
        public Employee GetEmployeeById(int id)
        {
            Employee employee = _employeeRepository.GetById(e=>e.Id.Equals(id));
            if (employee == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "There are no employees with supplied , id ");
            }
            return employee;
        }
        [Route("")]
        [HttpPost]
        public Employee AddEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "The request is not properly formatted , employee");
            }
            // part of mock data
            Random random = new Random();
            employee.Id = random.Next(10, 100);

            return _employeeRepository.Add(employee);
        }


        [Route("{id}")]
        [HttpPut]
        public Employee UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "The request is not properly formatted, employee");
            }
            Employee retrievedEmployee = _employeeRepository.GetById(e => e.Id.Equals(id));
            if (retrievedEmployee == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "There are no entries for supplied id");
            }
            return _employeeRepository.Update(employee, e=>e.Id.Equals(id));
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(int id)
        {
            Employee item = _employeeRepository.GetById(e => e.Id.Equals(id));
            if (item == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Employee not found so it couldn't be deleted");
            }
            _employeeRepository.Delete(e=>e.Id.Equals(id));
        }
    }
}