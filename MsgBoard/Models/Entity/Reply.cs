using System;
using System.ComponentModel.DataAnnotations;

namespace MsgBoard.Models.Entity
{
    public class Reply
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [MaxLength(2000)]
        [Required]
        public string Content { get; set; }

        [Required]
        public bool IsDel { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public int CreateUserId { get; set; }

        [Required]
        public DateTime UpdateTime { get; set; }

        [Required]
        public int UpdateUserId { get; set; }
    }
}