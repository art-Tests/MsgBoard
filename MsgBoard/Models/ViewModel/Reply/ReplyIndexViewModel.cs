using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MsgBoard.Models.Dto;

namespace MsgBoard.Models.ViewModel.Reply
{
    public class ReplyIndexViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [MaxLength(2000)]
        [Required]
        [DisplayName("內容")]
        [AllowHtml]
        public string Content { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        public DateTime UpdateTime { get; set; }

        public string CreateTimeText => CreateTime.ToString("yyyy-MM-dd HH:mm");
        public string UpdateTimeText => UpdateTime.ToString("yyyy-MM-dd HH:mm");
        public Author CreateAuthor { get; set; }
        public Author UpdateAuthor { get; set; }
        public bool IsAdmin { get; set; }
        public int UserId { get; set; }
    }
}