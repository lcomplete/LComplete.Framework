using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Domain.Model
{
    [DataContract]
    public partial class Auth_Group_Permissions
    {

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
        public Int32 GroupId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public String PermissionKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public DateTime CreateDate { get; set; }

        public virtual Auth_Group Auth_Group { get; set; }
    }
}
