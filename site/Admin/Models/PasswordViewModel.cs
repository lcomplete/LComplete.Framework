using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class PasswordViewModel
    {
        [Required]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} 必须为{2}到{1}个字符")]
        public string Password { get; set; }

        [Display(Name = "确认密码")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "两次输入的密码必须一致")]
        public string ConfirmPassword { get; set; }
    }
}