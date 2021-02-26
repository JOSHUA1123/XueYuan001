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
*│　创建时间：2019-12-28 11:51:26                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: BLL.Impl                                   
*│　类   名：ManageMenu_PointService                                      
*└──────────────────────────────────────────────────────────────┘
*/
namespace BLL.Impl
{
   public  class ManageMenu_PointService : BaseService<ManageMenu_Point>
    {
        public override void SetDal()
        {
            this.Dal=new DAL.BaseDAL<ManageMenu_Point>();
        }
    }
}
