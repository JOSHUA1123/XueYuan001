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
    
    public partial class Guide
    {
        public int Gu_Id { get; set; }
        public string Gc_Title { get; set; }
        public Nullable<int> Gc_ID { get; set; }
        public int Cou_ID { get; set; }
        public string Cou_Name { get; set; }
        public string Gu_Title { get; set; }
        public string Gu_TitleAbbr { get; set; }
        public string Gu_TitleFull { get; set; }
        public string Gu_TitleSub { get; set; }
        public string Gu_Color { get; set; }
        public string Gu_Font { get; set; }
        public bool Gu_IsError { get; set; }
        public string Gu_ErrInfo { get; set; }
        public bool Gu_IsUse { get; set; }
        public bool Gu_IsShow { get; set; }
        public bool Gu_IsImg { get; set; }
        public bool Gu_IsHot { get; set; }
        public bool Gu_IsTop { get; set; }
        public bool Gu_IsRec { get; set; }
        public bool Gu_IsDel { get; set; }
        public bool Gu_IsVerify { get; set; }
        public string Gu_VerifyMan { get; set; }
        public bool Gu_IsOut { get; set; }
        public string Gu_OutUrl { get; set; }
        public string Gu_Keywords { get; set; }
        public string Gu_Descr { get; set; }
        public string Gu_Author { get; set; }
        public Nullable<int> Acc_Id { get; set; }
        public string Acc_Name { get; set; }
        public string Gu_Source { get; set; }
        public string Gu_Intro { get; set; }
        public string Gu_Details { get; set; }
        public string Gu_Endnote { get; set; }
        public Nullable<System.DateTime> Gu_CrtTime { get; set; }
        public Nullable<System.DateTime> Gu_LastTime { get; set; }
        public Nullable<System.DateTime> Gu_VerifyTime { get; set; }
        public int Gu_Number { get; set; }
        public bool Gu_IsNote { get; set; }
        public string Gu_Logo { get; set; }
        public bool Gu_IsStatic { get; set; }
        public Nullable<System.DateTime> Gu_PushTime { get; set; }
        public string Gu_Label { get; set; }
        public string Gu_Uid { get; set; }
        public string OtherData { get; set; }
        public int Org_ID { get; set; }
        public string Org_Name { get; set; }
    }
}
