using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MsgBoard.Models.ViewModel.Member
{
    /// <summary>
    /// 會員登入ViewModel
    /// </summary>
    public class MemberLoginViewModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [RegularExpression(@".*@.*", ErrorMessage = "EMail格式須包含小老鼠符號")]
        [MaxLength(50, ErrorMessage = "Email地址不可超過50個字")]
        [Required(ErrorMessage = "請填寫Email")]
        [DisplayName("Email")]
        public string Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [Required(ErrorMessage = "請填寫密碼")]
        [DisplayName("密碼")]
        public string Password { get; set; }
    }
}