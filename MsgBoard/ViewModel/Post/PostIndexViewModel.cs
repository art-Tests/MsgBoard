using System;
using MsgBoard.Extension;
using MsgBoard.Models.Entity;

namespace MsgBoard.ViewModel.Post
{
    public class PostIndexViewModel
    {
        public int RowId { get; set; }
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string ChineseTime => UpdateTime.ConvertToChinese();
        public User Author { get; set; }

        public int ReplyCount { get; set; }
    }
}