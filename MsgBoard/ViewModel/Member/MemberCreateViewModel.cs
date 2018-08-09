using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MsgBoard.ViewModel.Member
{
    /// <summary>
    /// 新增會員表單ViewModel
    /// </summary>
    public class MemberCreateViewModel
    {
        /// <summary>
        /// 使用者大頭照
        /// </summary>
        [Required(ErrorMessage = "請上傳jpg圖檔")]
        //[FileExtensions(Extensions = "jpg", ErrorMessage = "圖片僅支援jpg格式")]
        [DisplayName("頭像")]
        public HttpPostedFileBase File { get; set; }

        /// <summary>
        /// 會員暱稱
        /// </summary>
        [MaxLength(10, ErrorMessage = "暱稱不可超過10個字")]
        [Required(ErrorMessage = "請填寫暱稱")]
        [DisplayName("暱稱")]
        public string Name { get; set; }

        /// <summary>
        /// 會員信箱
        /// </summary>
        [RegularExpression(@".*@.*", ErrorMessage = "EMail格式須包含小老鼠符號")]
        [MaxLength(50, ErrorMessage = "Email地址不可超過50個字")]
        [Required(ErrorMessage = "請填寫Email")]
        [DisplayName("Email")]
        public string Mail { get; set; }

        /// <summary>
        /// 會員密碼
        /// </summary>
        [StringLength(12, ErrorMessage = "{0}長度請設定 {2} 到 {1} 碼之間", MinimumLength = 4)]
        [Required(ErrorMessage = "請填寫密碼")]
        [DisplayName("密碼")]
        public string Password { get; set; }
    }
}