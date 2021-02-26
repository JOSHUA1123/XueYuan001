using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
*┌──────────────────────────────────────────────────────────────┐
*│　描   述：                                                    
*│　作   者：Joshua                                              
*│　版   本：1.0                                                 
*│　创建时间：2020-01-07 9:09:43                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: DingEntities                                   
*│　类   名：MyUserService                                      
*└──────────────────────────────────────────────────────────────┘
*/
namespace DingEntities
{
    public class MyUserService : BaseService<MyUser>
    {
        public override void SetDal()
        {
            this.Dal=new BaseDAL<MyUser>();
        }
    }
}
