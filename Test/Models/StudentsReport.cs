//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Test.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class StudentsReport
    {
        public int id { get; set; }
        public Nullable<int> StudentId { get; set; }
        public Nullable<int> CirruculumId { get; set; }
        public Nullable<int> FirstExam { get; set; }
        public Nullable<int> SecondExam { get; set; }
        public Nullable<bool> Ready { get; set; }
        public Nullable<int> Absent { get; set; }
    
        public virtual Curriculum Curriculum { get; set; }
        public virtual Student Student { get; set; }
    }
}
