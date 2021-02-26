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
*│　创建时间：2019-12-28 12:13:48                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: BLL.Impl                                   
*│　类   名：Product_PackageService                                      
*└──────────────────────────────────────────────────────────────┘
*/
namespace BLL.Impl
{
    class Product_PackageService : BaseService<Product_Package>
    {
        public override void SetDal()
        {
            this.Dal=new DAL.BaseDAL<Product_Package>();
        }
    }
}
