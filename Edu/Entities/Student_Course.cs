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
    
    public partial class Student_Course
    {
        public int Stc_ID { get; set; }
        public int Ac_ID { get; set; }
        public int Cou_ID { get; set; }
        public System.DateTime Stc_CrtTime { get; set; }
        public float Stc_Money { get; set; }
        public System.DateTime Stc_StartTime { get; set; }
        public System.DateTime Stc_EndTime { get; set; }
        public int Org_ID { get; set; }
        public string Rc_Code { get; set; }
        public bool Stc_IsFree { get; set; }
        public bool Stc_IsTry { get; set; }
        public double Stc_QuesScore { get; set; }
        public double Stc_StudyScore { get; set; }
        public double Stc_ExamScore { get; set; }
    }
}