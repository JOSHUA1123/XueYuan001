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
    
    public partial class Questions
    {
        public int Qus_ID { get; set; }
        public string Qus_Title { get; set; }
        public string Qus_Answer { get; set; }
        public int Qus_Diff { get; set; }
        public int Qus_Type { get; set; }
        public string Qus_Explain { get; set; }
        public bool Qus_IsUse { get; set; }
        public bool Qus_IsError { get; set; }
        public string Qus_UID { get; set; }
        public float Qus_Number { get; set; }
        public System.DateTime Qus_CrtTime { get; set; }
        public System.DateTime Qus_LastTime { get; set; }
        public bool Qus_IsCorrect { get; set; }
        public int Kn_ID { get; set; }
        public int Sbj_ID { get; set; }
        public string Qus_ErrorInfo { get; set; }
        public int Org_ID { get; set; }
        public int Cou_ID { get; set; }
        public int Ol_ID { get; set; }
        public bool Qus_IsWrong { get; set; }
        public int Qt_ID { get; set; }
        public string Qus_WrongInfo { get; set; }
        public string Qus_Items { get; set; }
        public bool Qus_IsTitle { get; set; }
        public string Ol_Name { get; set; }
        public string Sbj_Name { get; set; }
        public int Qus_Tax { get; set; }
        public int Qus_Errornum { get; set; }
    }
}
