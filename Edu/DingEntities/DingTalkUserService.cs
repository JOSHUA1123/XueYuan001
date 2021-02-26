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
*│　创建时间：2020-03-09 9:33:55                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: DingEntities                                   
*│　类   名：DingTalkUserService                                      
*└──────────────────────────────────────────────────────────────┘
*/
namespace DingEntities
{
    class DingTalkUserService : BaseService<DingTalkUser>
    {
        public override void SetDal()
        {
            this.Dal=new BaseDAL<DingTalkUser>();
        }
    }
}
