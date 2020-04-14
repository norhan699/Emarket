//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Emarket.Models
{
    using System;
    using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel;
	using System.Web;
	[MetadataType(typeof(Product))]
	public partial class Product
    {
        public int id { get; set; }
		[Display(Name = "Product Name")]
		public string name { get; set;}
		[Display(Name = "Product Price")]
		[DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
		public Nullable<double> price { get; set; }
		[DisplayName("Upload File")]
        public string image { get; set; }
        public string description { get; set; }
        public Nullable<int> category_id { get; set; }
    
        public virtual Category Category { get; set; }
		public HttpPostedFileBase ImageFile { get; set; }
	}
}
