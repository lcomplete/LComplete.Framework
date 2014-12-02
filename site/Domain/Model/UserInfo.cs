using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LComplete.Framework.Site.Domain.Model
{	

	public partial class UserInfo
    {
		
		/// <summary>
		/// 
		/// </summary>		
        [DisplayName("")]
		public int Id { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
        [DisplayName("")]
		public string Username { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
        [DisplayName("")]
		public string Password { get; set; }
		 
      
    }
}

