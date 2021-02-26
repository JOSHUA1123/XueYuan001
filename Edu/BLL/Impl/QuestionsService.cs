using Entities;
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
*│　创建时间：2019-12-28 12:19:56                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: BLL.Impl                                   
*│　类   名：QuestionsService                                      
*└──────────────────────────────────────────────────────────────┘
*/
namespace BLL.Impl
{
    public class QuestionsService : BaseService<Questions>
    {
        public override void SetDal()
        {
            this.Dal=new DAL.BaseDAL<Questions>();
        }
    }
}
