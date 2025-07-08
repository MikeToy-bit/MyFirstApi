using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MyFirstApi.Extensions
{
    public static class DynamicQueryExtensions
    {
        /// <summary>
        /// 基于DTO属性自动构建查询条件
        /// </summary>
        public static IQueryable<TEntity> ApplyFiltersFromDTO<TEntity, TDTO>(this IQueryable<TEntity> query, TDTO dto)
            where TEntity : class
            where TDTO : class
        {
            if (dto == null) return query;

            var entityType = typeof(TEntity);
            var dtoType = typeof(TDTO);
            var parameter = Expression.Parameter(entityType, "x");

            foreach (var dtoProperty in dtoType.GetProperties())
            {
                var dtoValue = dtoProperty.GetValue(dto);
                if (dtoValue == null) continue;

                // 跳过分页相关属性和日期范围属性（这些由专门的方法处理）
                if (IsSkipProperty(dtoProperty.Name)) continue;

                var entityProperty = entityType.GetProperty(dtoProperty.Name);
                if (entityProperty == null) continue;

                var propertyAccess = Expression.Property(parameter, entityProperty);

                // 构建查询条件
                Expression condition = null;

                // 字符串包含查询
                if (dtoProperty.PropertyType == typeof(string) && dtoValue is string stringValue && !string.IsNullOrEmpty(stringValue))
                {
                    condition = BuildStringContainsExpression(propertyAccess, stringValue);
                }
                // 可空布尔类型精确匹配
                else if (dtoProperty.PropertyType == typeof(bool?) && dtoValue is bool boolValue)
                {
                    condition = Expression.Equal(propertyAccess, Expression.Constant(boolValue, typeof(bool?)));
                }
                // 其他类型的精确匹配
                else if (dtoProperty.PropertyType == entityProperty.PropertyType)
                {
                    condition = Expression.Equal(propertyAccess, Expression.Constant(dtoValue, dtoProperty.PropertyType));
                }

                // 应用条件
                if (condition != null)
                {
                    var lambda = Expression.Lambda<Func<TEntity, bool>>(condition, parameter);
                    query = query.Where(lambda);
                }
            }

            return query;
        }

        /// <summary>
        /// 判断是否跳过该属性
        /// </summary>
        private static bool IsSkipProperty(string propertyName)
        {
            return propertyName.Contains("Page") || 
                   propertyName.Contains("Size") || 
                   propertyName.StartsWith("Start") || 
                   propertyName.StartsWith("End");
        }

        /// <summary>
        /// 构建字符串包含查询表达式
        /// </summary>
        private static Expression BuildStringContainsExpression(Expression propertyAccess, string value)
        {
            var nullCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null, typeof(string)));
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var containsCall = Expression.Call(propertyAccess, containsMethod, Expression.Constant(value));
            return Expression.AndAlso(nullCheck, containsCall);
        }
    }
} 