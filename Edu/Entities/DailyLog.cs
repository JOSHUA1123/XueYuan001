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
    
    public partial class DailyLog
    {
        public int Dlog_Id { get; set; }
        public string Dlog_Type { get; set; }
        public string Dlog_Note { get; set; }
        public string Dlog_Plan { get; set; }
        public Nullable<System.DateTime> Dlog_CrtTime { get; set; }
        public Nullable<System.DateTime> Dlog_WrtTime { get; set; }
        public Nullable<int> Acc_Id { get; set; }
        public string Acc_Name { get; set; }
        public int Org_ID { get; set; }
        public string Org_Name { get; set; }
    }
}