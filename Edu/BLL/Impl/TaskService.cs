using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/**
*┌──────────────────────────────────────────────────────────────┐
*│　描   述：                                                    
*│　作   者：Joshua                                              
*│　版   本：1.0                                                 
*│　创建时间：2019-12-28 13:06:41                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: BLL.Impl                                   
*│　类   名：TaskService                                      
*└──────────────────────────────────────────────────────────────┘
*/
namespace BLL.Impl
{
    public class TaskService : BaseService<Task>
    {
        public override void SetDal()
        {
            this.Dal=new DAL.BaseDAL<Task>();
        }
    }
}
