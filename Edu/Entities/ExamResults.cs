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
    
    public partial class ExamResults
    {
        public int Exr_ID { get; set; }
        public float Exr_Score { get; set; }
        public float Exr_ScoreFinal { get; set; }
        public float Exr_Draw { get; set; }
        public float Exr_Colligate { get; set; }
        public string Exr_Results { get; set; }
        public System.DateTime Exr_CrtTime { get; set; }
        public string Exr_IP { get; set; }
        public string Exr_Mac { get; set; }
        public bool Exr_IsSubmit { get; set; }
        public int Exam_ID { get; set; }
        public string Exam_UID { get; set; }
        public string Exam_Name { get; set; }
        public int Sbj_ID { get; set; }
        public string Sbj_Name { get; set; }
        public int Tp_Id { get; set; }
        public int Ac_ID { get; set; }
        public string Ac_Name { get; set; }
        public int Dep_Id { get; set; }
        public int Team_ID { get; set; }
        public string Exam_Title { get; set; }
        public System.DateTime Exr_SubmitTime { get; set; }
        public int Org_ID { get; set; }
        public string Org_Name { get; set; }
        public int Sts_ID { get; set; }
        public int Ac_Sex { get; set; }
        public string Ac_IDCardNumber { get; set; }
        public bool Exr_IsCalc { get; set; }
        public System.DateTime Exr_OverTime { get; set; }
        public System.DateTime Exr_CalcTime { get; set; }
        public System.DateTime Exr_LastTime { get; set; }
    }
}
