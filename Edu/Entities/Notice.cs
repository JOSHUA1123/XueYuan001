//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Notice
    {
        public int No_Id { get; set; }
        public string No_Ttl { get; set; }
        public string No_Context { get; set; }
        public bool No_IsShow { get; set; }
        public bool No_IsOpen { get; set; }
        public bool No_IsTop { get; set; }
        public Nullable<System.DateTime> No_CrtTime { get; set; }
        public Nullable<System.DateTime> No_StartTime { get; set; }
        public Nullable<System.DateTime> No_EndTime { get; set; }
        public Nullable<int> Acc_Id { get; set; }
        public string Acc_Name { get; set; }
        public string No_Organ { get; set; }
        public int Org_ID { get; set; }
        public string Org_Name { get; set; }
    }
}
