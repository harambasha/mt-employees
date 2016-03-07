using System.Collections.Generic;
using MT.Repository.Entities;
using MT.Repository.Infrastructure;

namespace MT.Repository.Repositories
{
    public sealed class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        private static readonly List<Employee> Employees = new List<Employee>()
        {
            new Employee { Id = 1, Name = "Ismir Harambasic", Department = Department.Development, IsActive = true },
            new Employee { Id = 2, Name = "Irhad Babic", Department = Department.Development, IsActive = true },
            new Employee { Id = 3, Name = "Salih Hajlakovic", Department = Department.HR, IsActive = true },
            new Employee { Id = 4, Name = "Kljuco", Department = Department.HR, IsActive = true },
            new Employee { Id = 5, Name = "Mehmed pasa", Department = Department.Management, IsActive = false },
            new Employee { Id = 6, Name = "Amila Avdukic", Department = Department.HR, IsActive = true },
            new Employee { Id = 7, Name = "Edvin Lovic", Department = Department.Management, IsActive = true },
            new Employee { Id = 8, Name = "Random user", Department = Department.Development, IsActive = false },
            new Employee { Id = 9, Name = "Random user 2", Department = Department.Development, IsActive = false }

        };

        public EmployeeRepository()
            : base(Employees)
        {
        }
    }
    public interface IEmployeeRepository : IRepository<Employee>
    {
    }
}
