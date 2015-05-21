using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Domain.Model
{
    [DataContract]
    public partial class Auth_Group
    {
        public Auth_Group()
        {
            Auth_Group_Permissions=new List<Auth_Group_Permissions>();
            Auth_User_Groups=new List<Auth_User_Groups>();
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("Id")]
        [DataMember]
        [Key]
        public Int32 Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("分组名")]
        [DataMember]
        [Required]
        [StringLength(20,MinimumLength =2,ErrorMessage = "{0} 必须为{2}到{1}个字符")]
        public String Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("创建时间")]
        [DataMember]
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Auth_Group_Permissions> Auth_Group_Permissions { get; set; }
        public virtual ICollection<Auth_User_Groups> Auth_User_Groups { get; set; }
    }
}
