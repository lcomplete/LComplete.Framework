using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Domain.Model
{
    [DataContract]
    public partial class Auth_User_Groups
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
        public Int32 UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DisplayName("")]
        [DataMember]
        public Int32 GroupId { get; set; }

        public virtual Auth_User Auth_User { get; set; }

        public virtual Auth_Group Auth_Group { get; set; }
    }
}
