using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class ChangePasswordViewModel:PasswordViewModel
    {
        [Required]
        [Display(Name = "原密码")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} 必须为{2}到{1}个字符")]
        public string RawPassword { get; set; }
    }
}