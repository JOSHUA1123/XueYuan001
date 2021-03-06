﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class examweishaEntities : DbContext
    {
        public examweishaEntities()
            : base("name=examweishaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Accessory> Accessory { get; set; }
        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<AddressList> AddressList { get; set; }
        public virtual DbSet<AddressSort> AddressSort { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Columns> Columns { get; set; }
        public virtual DbSet<CouponAccount> CouponAccount { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CoursePrice> CoursePrice { get; set; }
        public virtual DbSet<DailyLog> DailyLog { get; set; }
        public virtual DbSet<Depart> Depart { get; set; }
        public virtual DbSet<Depart_Subject> Depart_Subject { get; set; }
        public virtual DbSet<Download> Download { get; set; }
        public virtual DbSet<DownloadOS> DownloadOS { get; set; }
        public virtual DbSet<DownloadType> DownloadType { get; set; }
        public virtual DbSet<EmpAcc_Group> EmpAcc_Group { get; set; }
        public virtual DbSet<EmpAccount> EmpAccount { get; set; }
        public virtual DbSet<EmpGroup> EmpGroup { get; set; }
        public virtual DbSet<EmpInfo> EmpInfo { get; set; }
        public virtual DbSet<EmpTitle> EmpTitle { get; set; }
        public virtual DbSet<ExamGroup> ExamGroup { get; set; }
        public virtual DbSet<Examination> Examination { get; set; }
        public virtual DbSet<ExamResults> ExamResults { get; set; }
        public virtual DbSet<ExamResultsTemp> ExamResultsTemp { get; set; }
        public virtual DbSet<Forum> Forum { get; set; }
        public virtual DbSet<FuncPoint> FuncPoint { get; set; }
        public virtual DbSet<Guide> Guide { get; set; }
        public virtual DbSet<GuideColumns> GuideColumns { get; set; }
        public virtual DbSet<InnerEmail> InnerEmail { get; set; }
        public virtual DbSet<InternalLink> InternalLink { get; set; }
        public virtual DbSet<Knowledge> Knowledge { get; set; }
        public virtual DbSet<KnowledgeSort> KnowledgeSort { get; set; }
        public virtual DbSet<LearningCard> LearningCard { get; set; }
        public virtual DbSet<LearningCardSet> LearningCardSet { get; set; }
        public virtual DbSet<LimitDomain> LimitDomain { get; set; }
        public virtual DbSet<Links> Links { get; set; }
        public virtual DbSet<LinksSort> LinksSort { get; set; }
        public virtual DbSet<LogForStudentOnline> LogForStudentOnline { get; set; }
        public virtual DbSet<LogForStudentQuestions> LogForStudentQuestions { get; set; }
        public virtual DbSet<LogForStudentStudy> LogForStudentStudy { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<ManageMenu> ManageMenu { get; set; }
        public virtual DbSet<ManageMenu_Point> ManageMenu_Point { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<MessageBoard> MessageBoard { get; set; }
        public virtual DbSet<MobileUser> MobileUser { get; set; }
        public virtual DbSet<MoneyAccount> MoneyAccount { get; set; }
        public virtual DbSet<Navigation> Navigation { get; set; }
        public virtual DbSet<NewsNote> NewsNote { get; set; }
        public virtual DbSet<Notice> Notice { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<OrganLevel> OrganLevel { get; set; }
        public virtual DbSet<Outline> Outline { get; set; }
        public virtual DbSet<OutlineEvent> OutlineEvent { get; set; }
        public virtual DbSet<PayInterface> PayInterface { get; set; }
        public virtual DbSet<PictMiniature> PictMiniature { get; set; }
        public virtual DbSet<Picture> Picture { get; set; }
        public virtual DbSet<PointAccount> PointAccount { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Product_Package> Product_Package { get; set; }
        public virtual DbSet<ProductFactory> ProductFactory { get; set; }
        public virtual DbSet<ProductMaterial> ProductMaterial { get; set; }
        public virtual DbSet<ProductMessage> ProductMessage { get; set; }
        public virtual DbSet<ProductOrigin> ProductOrigin { get; set; }
        public virtual DbSet<ProductPackage> ProductPackage { get; set; }
        public virtual DbSet<ProfitSharing> ProfitSharing { get; set; }
        public virtual DbSet<Purview> Purview { get; set; }
        public virtual DbSet<QuesAnswer> QuesAnswer { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<QuesTypes> QuesTypes { get; set; }
        public virtual DbSet<RechargeCode> RechargeCode { get; set; }
        public virtual DbSet<RechargeSet> RechargeSet { get; set; }
        public virtual DbSet<ShowPicture> ShowPicture { get; set; }
        public virtual DbSet<SingleSignOn> SingleSignOn { get; set; }
        public virtual DbSet<SmsFault> SmsFault { get; set; }
        public virtual DbSet<SmsMessage> SmsMessage { get; set; }
        public virtual DbSet<Special> Special { get; set; }
        public virtual DbSet<Special_Article> Special_Article { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Student_Collect> Student_Collect { get; set; }
        public virtual DbSet<Student_Course> Student_Course { get; set; }
        public virtual DbSet<Student_Notes> Student_Notes { get; set; }
        public virtual DbSet<Student_Ques> Student_Ques { get; set; }
        public virtual DbSet<StudentSort> StudentSort { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<SystemPara> SystemPara { get; set; }
        public virtual DbSet<Task> Task { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }
        public virtual DbSet<Teacher_Course> Teacher_Course { get; set; }
        public virtual DbSet<TeacherComment> TeacherComment { get; set; }
        public virtual DbSet<TeacherHistory> TeacherHistory { get; set; }
        public virtual DbSet<TeacherSort> TeacherSort { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<Test> Test { get; set; }
        public virtual DbSet<TestPaper> TestPaper { get; set; }
        public virtual DbSet<TestPaperItem> TestPaperItem { get; set; }
        public virtual DbSet<TestPaperQues> TestPaperQues { get; set; }
        public virtual DbSet<TestResults> TestResults { get; set; }
        public virtual DbSet<TrPlan> TrPlan { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<Video> Video { get; set; }
        public virtual DbSet<Vote> Vote { get; set; }
    }
}
