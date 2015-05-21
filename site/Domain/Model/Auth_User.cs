using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Domain.Model
{
    [DataContract]
    public partial class Auth_User
    {

        public Auth_User()
        {
            Auth_User_Groups=new List<Auth_User_Groups>();
        }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        [Key]
        public Int32 Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public String Username { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public String Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public String PasswordSalt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public String RealName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public String Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public Boolean IsEnabled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public Boolean IsSuperUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public DateTime LastLoginDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 权限分组
        /// </summary>
        public virtual ICollection<Auth_User_Groups> Auth_User_Groups { get; set; }

        
    }
}
