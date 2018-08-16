using System;
using MsgBoard.DataModel.Dto;
using MsgBoard.DataModel.Extension;

namespace MsgBoard.DataModel.ViewModel.Post
{
    public class PostIndexViewModel
    {
        public int RowId { get; set; }
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string ChineseTime => UpdateTime.ConvertToChinese();
        public Author CreateAuthor { get; set; }
        public Author UpdateAuthor { get; set; }

        public int ReplyCount { get; set; }
    }
}