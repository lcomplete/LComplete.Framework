using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我一周")]
        public bool IsRememberMe { get; set; }
    }
}