using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Model;
using LComplete.Framework.Verify;

namespace Admin.Areas.Auth.Models
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Auth_Groups=new List<Auth_Group>();
            SelectGroupIds=new List<int>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "用户名")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} 必须为{2}到{1}个字符")]
        [RegularExpression(@"^([a-z]|[A-Z]|\d)+$", ErrorMessage = "{0} 必需为字符和数字")]
        public String Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "{0} 必须为{2}到{1}个字符")]
        [RegularExpression(@"^\S+", ErrorMessage = "{0} 输入有误")]
        public String RealName { get; set; }

        [Required]
        [Display(Name = "E-mail 地址")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "{0} 必须为{2}到{1}个字符")]
        [RegularExpression(VerifyUtils.EmailRegex,ErrorMessage = "{0} 输入格式错误")]
        public String Email { get; set; }

        [Required]
        public Boolean IsEnabled { get; set; }

        [Required]
        public Boolean IsSuperUser { get; set; }

        public DateTime LastLoginDate { get; set; }

        public DateTime CreateDate { get; set; }

        public IList<Auth_Group> Auth_Groups { get; set; }

        public IList<int> SelectGroupIds { get; set; }
    }
}