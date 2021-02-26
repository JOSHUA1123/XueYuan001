using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingEntities
{
    public class DingUserInfo
    {

        public static string appId = "dingoapjhmhgsjkxfcxpzq";
        public static string appSecret = "15aEFVfLMrbCZViJI-e6xW5FmOiAVT7vLUc-wjvQc19WKzQ642HVNzY51w_UQVuu";
        private static DingTalkUserService talkUserService = new DingTalkUserService();
        private static MyUserYuyueService myUserYuyueService = new MyUserYuyueService();
        /// <summary>
        /// 两表联查
        /// </summary>
        /// <returns></returns>
        public static List<DingUsers> biao()
        {
            using (DingUserEntities db = new DingUserEntities())
            {
                var sql = from s in db.MyUser
                          join c in db.User_dingding on s.ID equals c.UserID
                          select new
                          {
                              s.YourName,
                              s.UserName,
                              s.Mobile,
                              s.Sex,
                              s.Age,
                              s.classID,
                              c.DingID,
                              c.DingOpenID
                          };
                List<DingUsers> li = new List<DingUsers>();
                foreach (var item in sql.ToList())
                {
                    DingUsers d = new DingUsers();
                    d.YourName = item.YourName;
                    d.UserName = item.UserName;
                    d.Mobile = item.Mobile;
                    d.Sex = item.Sex == "男" ? 1 : 2;
                    d.Age = int.Parse(item.Age.ToString());
                    d.classID = int.Parse(item.classID.ToString());
                    d.DingID = item.DingID;
                    d.DingOpenID = item.DingOpenID;
                    li.Add(d);
                }
                return li;
            }
        }
        public static List<DingTalkUser> GetDingTalkUser01()
        {
            var list=talkUserService.GetModels(n=>true).ToList();
            return list;
        }

        public static List<MyUser_Yuyue> GetDingTalkUser()
        {
            var list = myUserYuyueService.GetModels(n => true).ToList();
            return list;
        }

    }


    public class JsonData{
        public int errcode { set; get; }
        public string errmsg { set; get; }
        public userinfoDing user_info { set; get; }

}

public class userinfoDing
{
      public string nick { get; set; }
      public string  unionid                {get;set;}
      public string  dingId                 {get;set;}
      public string  openid                 {get;set;}
      public bool  main_org_auth_high_level {get;set;}
}

    public class DingUsers
    {
        public string YourName   {set;get;}
        public string UserName   {set;get;}
        public string Mobile     {set;get;}
        public int Sex        {set;get;}
        public int Age        {set;get;}
        public int classID    {set;get;}
        public string DingID     {set;get;}
        public string DingOpenID { set; get; }
    }
}
