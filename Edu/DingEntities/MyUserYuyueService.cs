using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DingEntities
{
    public class MyUserYuyueService : BaseService<MyUser_Yuyue>
    {
        public override void SetDal()
        {
            this.Dal=new BaseDAL<MyUser_Yuyue>();
        }
    }
}
