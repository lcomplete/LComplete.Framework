using System;
using System.ComponentModel;

namespace FileServer.Code
{	
	public partial class UploadFile
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
		public string UniqueKey { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
        [DisplayName("")]
		public string OriginalName { get; set; }
		
		/// <summary>
		/// 相对路径
		/// </summary>		
        [DisplayName("相对路径")]
		public string RelativePath { get; set; }
		
		/// <summary>
		/// 文件类型(后缀名)
		/// </summary>		
        [DisplayName("文件类型(后缀名)")]
		public string FileExtension { get; set; }
		
		/// <summary>
		/// http contenttype
		/// </summary>		
        [DisplayName("http contenttype")]
		public string ContentType { get; set; }
		
		/// <summary>
		/// 文件大小(单位byte)
		/// </summary>		
        [DisplayName("文件大小(单位byte)")]
		public long FileSize { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
        [DisplayName("")]
		public int? UploadUserId { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
        [DisplayName("")]
		public int? UploadAdminId { get; set; }
		
		/// <summary>
		/// 创建时间
		/// </summary>		
        [DisplayName("创建时间")]
		public DateTime CreateDate { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
        [DisplayName("")]
		public int VisitCount { get; set; }
		
		/// <summary>
		/// 
		/// </summary>		
        [DisplayName("")]
		public DateTime? LastVisitDate { get; set; }

	    public int Width { get; set; }

	    public int Height { get; set; }
      
    }
}

