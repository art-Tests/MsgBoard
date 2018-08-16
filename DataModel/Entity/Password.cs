using System;
using System.ComponentModel.DataAnnotations;

namespace DataModel.Entity
{
    public class Password
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string HashPw { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }
    }
}