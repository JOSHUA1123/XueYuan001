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
    
    public partial class InnerEmail
    {
        public int Ine_Id { get; set; }
        public string Ine_Title { get; set; }
        public string Ine_Context { get; set; }
        public Nullable<int> Acc_Id { get; set; }
        public string Acc_Name { get; set; }
        public Nullable<System.DateTime> Ine_CrtTime { get; set; }
        public Nullable<System.DateTime> Ine_ToTime { get; set; }
        public Nullable<System.DateTime> Ine_ReadTime { get; set; }
        public Nullable<int> Ine_State { get; set; }
        public string Ine_ToName { get; set; }
        public Nullable<int> Ine_ToId { get; set; }
        public Nullable<int> Ine_OfBox { get; set; }
        public string Ine_ToAllId { get; set; }
        public string Ine_ToAllName { get; set; }
        public Nullable<int> Ine_OwnerId { get; set; }
        public bool Ine_Del { get; set; }
        public string Ine_UniqueId { get; set; }
        public int Org_ID { get; set; }
        public string Org_Name { get; set; }
    }
}
