//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace mpf.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NewsTbl
    {
        public int id { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public bool status { get; set; }
        public Nullable<System.DateTime> rts { get; set; }
        public List<NewsTbl> NewsList { get; internal set; }
    }
}