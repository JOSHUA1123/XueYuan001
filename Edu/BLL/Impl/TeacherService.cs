﻿using Entities;
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
*│　创建时间：2019-12-28 13:08:10                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: BLL.Impl                                   
*│　类   名：TeacherService                                      
*└──────────────────────────────────────────────────────────────┘
*/
namespace BLL.Impl
{
    public class TeacherService : BaseService<Teacher>
    {
        public override void SetDal()
        {
            this.Dal=new DAL.BaseDAL<Teacher>();
        }
    }
}
