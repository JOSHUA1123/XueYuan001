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
    
    public partial class EmpGroup
    {
        public int EGrp_Id { get; set; }
        public string EGrp_Name { get; set; }
        public bool EGrp_IsUse { get; set; }
        public int EGrp_Tax { get; set; }
        public string EGrp_Intro { get; set; }
        public bool EGrp_IsSystem { get; set; }
        public int Org_ID { get; set; }
        public string Org_Name { get; set; }
    }
}
