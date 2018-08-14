using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MsgBoard.Models.Dto
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Pic { get; set; }
    }
}