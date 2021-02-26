using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

/**
*┌──────────────────────────────────────────────────────────────┐
*│　描   述：                                                    
*│　作   者：Joshua                                              
*│　版   本：1.0                                                 
*│　创建时间：2020-01-07 8:41:24                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: DingEntities                                   
*│　类   名：DingDbContextFactory                                      
*└──────────────────────────────────────────────────────────────┘
*/
namespace DingEntities
{
    public class DingDbContextFactory
    {/// <summary>
     /// 创建EF上下文对象,已存在就直接取,不存在就创建,保证线程内是唯一。
     /// </summary>
        public static DbContext Create()
        {
            DbContext dbContext = CallContext.GetData("DbContext") as DbContext;
            if (dbContext == null)
            {

                dbContext = new DingUserEntities();
                CallContext.SetData("DbContext", dbContext);
            }
            return dbContext;
        }
    }
}
