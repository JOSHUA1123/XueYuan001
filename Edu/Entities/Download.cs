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
    
    public partial class Download
    {
        public int Dl_Id { get; set; }
        public string Dl_Name { get; set; }
        public string Dl_Intro { get; set; }
        public string Dl_Details { get; set; }
        public string Dl_Version { get; set; }
        public string Dl_FilePath { get; set; }
        public string Dl_Logo { get; set; }
        public string Dl_LogoSmall { get; set; }
        public bool Dl_IsShow { get; set; }
        public string Dl_Keywords { get; set; }
        public string Dl_Descr { get; set; }
        public bool Dl_IsRec { get; set; }
        public bool Dl_IsTop { get; set; }
        public bool Dl_IsHot { get; set; }
        public bool Dl_IsDel { get; set; }
        public Nullable<System.DateTime> Dl_CrtTime { get; set; }
        public Nullable<System.DateTime> Dl_UpdateTime { get; set; }
        public Nullable<int> Dl_Size { get; set; }
        public string Dl_OS { get; set; }
        public string Dl_Author { get; set; }
        public Nullable<int> Dl_LookNumber { get; set; }
        public Nullable<int> Dl_DownNumber { get; set; }
        public Nullable<int> Col_Id { get; set; }
        public string Col_Name { get; set; }
        public Nullable<int> Acc_Id { get; set; }
        public string Acc_Name { get; set; }
        public string Dl_QrCode { get; set; }
        public bool Dl_IsStatic { get; set; }
        public Nullable<System.DateTime> Dl_PushTime { get; set; }
        public string Dl_Uid { get; set; }
        public Nullable<int> Dty_Id { get; set; }
        public string Dty_Type { get; set; }
        public string Dl_Label { get; set; }
        public string OtherData { get; set; }
        public int Org_ID { get; set; }
        public string Org_Name { get; set; }
    }
}
