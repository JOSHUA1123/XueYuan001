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
    
    public partial class ProductMessage
    {
        public int Pm_Id { get; set; }
        public string Pm_Title { get; set; }
        public string Pm_Context { get; set; }
        public string Pm_Answer { get; set; }
        public Nullable<System.DateTime> Pm_CrtTime { get; set; }
        public Nullable<System.DateTime> Pm_AnsTime { get; set; }
        public bool Pm_IsAns { get; set; }
        public bool Pm_IsShow { get; set; }
        public string Pm_IP { get; set; }
        public string Pm_Phone { get; set; }
        public string Pm_Email { get; set; }
        public string Pm_QQ { get; set; }
        public string Pm_Address { get; set; }
        public Nullable<int> Pd_Id { get; set; }
        public string Pd_Name { get; set; }
        public int Org_ID { get; set; }
        public string Org_Name { get; set; }
    }
}
