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
    
    public partial class NewsNote
    {
        public int Nn_Id { get; set; }
        public Nullable<int> Art_Id { get; set; }
        public string Nn_Name { get; set; }
        public string Nn_Title { get; set; }
        public string Nn_Details { get; set; }
        public Nullable<System.DateTime> Nn_CrtTime { get; set; }
        public string Nn_IP { get; set; }
        public bool Nn_IsShow { get; set; }
        public string Nn_Email { get; set; }
        public string Nn_Province { get; set; }
        public string Nn_City { get; set; }
        public int Org_ID { get; set; }
        public string Org_Name { get; set; }
    }
}
