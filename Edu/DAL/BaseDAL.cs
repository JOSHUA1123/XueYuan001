using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;


namespace DAL
{
    public partial class BaseDAL<T> where T : class, new()
    {
       public DbContext dbContext = DbContextFactory.Create();
       
        ///// <summary>
        ///// 开启事务
        ///// </summary>
        ///// <returns></returns>
        //public DbContextTransaction StratTran()
        //{
        //    return dbContext.Database.BeginTransaction();
        //}
        public void Add(T t)
        {
            dbContext.Set<T>().Add(t);
        }
        public void AddRange(IEnumerable<T> t)
        {
            dbContext.Set<T>().AddRange(t);
        }
        public void Delete(T t)
        {
            dbContext.Set<T>().Remove(t);
        }

        public void Update(T t)
        {
            dbContext.Set<T>().AddOrUpdate(t);
           
        }

        public IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
         
            return dbContext.Set<T>().Where(whereLambda);
        }

        public IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc,
            Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda)
        {
            //是否升序
            if (isAsc)
            {
                return dbContext.Set<T>().Where(WhereLambda).OrderBy(OrderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                return dbContext.Set<T>().Where(WhereLambda).OrderByDescending(OrderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
        }

        /// <summary>
        /// 求和
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <param name="where"></param>
        /// <returns></returns>
       public decimal? GetModelsSum(Expression<Func<T, decimal?>> whereLambda, Expression<Func<T, bool>>where)
        {
            return dbContext.Set<T>().Where(where).Sum(whereLambda);
        }

        public bool SaveChanges()
        {
          
            return dbContext.SaveChanges() > 0;
        }
    }
}