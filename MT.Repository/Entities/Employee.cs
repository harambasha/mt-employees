using System.ComponentModel.DataAnnotations;


namespace MT.Repository.Entities
{
    public class Employee
    {
        public int ? Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Range(0, 2)]
        public Department Department { get; set; }
    }
}
