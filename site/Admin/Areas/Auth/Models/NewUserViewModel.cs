using System.ComponentModel.DataAnnotations;
using Admin.Models;

namespace Admin.Areas.Auth.Models
{
    public class NewUserViewModel : PasswordViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} 必须为{2}到{1}个字符")]
        [RegularExpression(@"^([a-z]|[A-Z]|\d)+$", ErrorMessage = "{0} 必需为字符和数字")]
        public string Username { get; set; }

    }
}