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
    
    public partial class ProductPackage
    {
        public int PPK_Id { get; set; }
        public string PPK_Name { get; set; }
        public bool PPK_IsUse { get; set; }
        public string PPK_Logo { get; set; }
        public Nullable<int> PPK_Price { get; set; }
        public Nullable<int> PPK_DiscountPrice { get; set; }
        public Nullable<System.DateTime> PPK_StartTime { get; set; }
        public Nullable<System.DateTime> PPK_EndTime { get; set; }
        public string PPK_Intro { get; set; }
        public int Org_ID { get; set; }
        public string Org_Name { get; set; }
    }
}