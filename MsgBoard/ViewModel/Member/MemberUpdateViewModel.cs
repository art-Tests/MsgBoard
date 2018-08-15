using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MsgBoard.ViewModel.Member
{
    /// <summary>
    /// 修改會員表單ViewModel
    /// </summary>
    public class MemberUpdateViewModel
    {
        /// <summary>
        /// 會員Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 使用者大頭照
        /// </summary>

        public HttpPostedFileBase File { get; set; }

        /// <summary>
        /// 會員暱稱
        /// </summary>
        [MaxLength(10, ErrorMessage = "暱稱不可超過10個字")]
        [Required(ErrorMessage = "請填寫暱稱")]
        [DisplayName("暱稱")]
        public string Name { get; set; }

        /// <summary>
        /// 會員密碼
        /// </summary>
        [StringLength(12, ErrorMessage = "{0}長度請設定 {2} 到 {1} 碼之間", MinimumLength = 4)]
        [DisplayName("密碼")]
        public string Password { get; set; }

        /// <summary>
        /// 使用者大頭照圖片虛擬路徑
        /// </summary>
        public string Pic { get; set; }

        public string BackAction { get; set; }
        public string BackController { get; set; }
        public int? BackPage { get; set; }
    }
}