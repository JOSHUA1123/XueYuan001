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
*│　创建时间：2019-12-28 12:27:10                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: BLL.Impl                                   
*│　类   名：Special_ArticleService                                      
*└──────────────────────────────────────────────────────────────┘
*/
namespace BLL.Impl
{
    class Special_ArticleService : BaseService<Special_Article>
    {
        public override void SetDal()
        {
           this.Dal=new DAL.BaseDAL<Special_Article>();
        }
    }
}
