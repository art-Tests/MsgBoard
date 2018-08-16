using System.ComponentModel.DataAnnotations;

namespace DataModel.Entity
{
    public class User
    {
        public int RowId { get; set; }

        [Required]
        public int Id { get; set; }

        [MaxLength(10)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string Mail { get; set; }

        [MaxLength(200)]
        [Required]
        public string Pic { get; set; }

        [MaxLength(36)]
        [Required]
        public string Guid { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        public bool IsDel { get; set; }
    }
}