using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MsgBoard.Models.Entity
{
    public class Post
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(2000)]
        [Required]
        [DisplayName("內容")]
        [AllowHtml]
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