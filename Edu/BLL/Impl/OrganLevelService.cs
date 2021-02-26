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
*│　创建时间：2019-12-28 12:04:00                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: BLL.Impl                                   
*│　类   名：OrganLevelService                                      
*└──────────────────────────────────────────────────────────────┘
*/
namespace BLL.Impl
{
    public class OrganLevelService : BaseService<OrganLevel>
    {
        public override void SetDal()
        {
            this.Dal=new DAL.BaseDAL<OrganLevel>();
        }
    }
}
