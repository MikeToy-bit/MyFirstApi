using System;
using System.Linq;
using System.Linq.Expressions;

namespace MyFirstApi.Extensions
{
    public static class QueryExtensions
    {
        /// <summary>
        /// 日期范围查询的简化方法
        /// query: 查询对象
        /// startDate: 开始日期
        /// endDate: 结束日期
        /// selector: 日期属性选择器
        /// 返回值: 查询对象
        /// 示例：
        /// var query = db.T_SYS_User.WhereDateRange(startDate, endDate, u => u.CreateTime);
        /// </summary>
        public static IQueryable<T> WhereDateRange<T>(this IQueryable<T> query, DateTime? startDate, DateTime? endDate, Expression<Func<T, DateTime?>> selector)
        {
            if (startDate.HasValue)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Invoke(selector, parameter);
                var condition = Expression.GreaterThanOrEqual(property, Expression.Constant(startDate.Value, typeof(DateTime?)));
                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                query = query.Where(lambda);
            }

            if (endDate.HasValue)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Invoke(selector, parameter);
                var condition = Expression.LessThanOrEqual(property, Expression.Constant(endDate.Value, typeof(DateTime?)));
                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);
                query = query.Where(lambda);
            }

            return query;
        }
    }
} 