﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using EntityFramework.Toolkit.Utils;

namespace EntityFramework.Toolkit.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        ///     Includes navigation properties.
        /// </summary>
        /// <typeparam name="T">Generic type T.</typeparam>
        /// <param name="queryable">Queryable</param>
        /// <param name="properties">A list of navigation properties to include.</param>
        /// <returns>New queryable which includes the given navigation properties.</returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> queryable, params Expression<Func<T, object>>[] properties)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            foreach (Expression<Func<T, object>> property in properties)
            {
                queryable = queryable.Include(property);
            }

            return queryable;
        }

        /// <summary>
        ///     Includes navigation properties.
        /// </summary>
        /// <typeparam name="T">Generic type T.</typeparam>
        /// <param name="queryable">Queryable</param>
        /// <param name="properties">The navigation property to include.</param>
        /// <returns>New queryable which includes the given navigation properties.</returns>
        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> queryable, Expression<Func<T, TProperty>> pathExpression)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            if (pathExpression == null)
            {
                throw new ArgumentNullException(nameof(pathExpression));
            }

            string path;

            if (!DbHelpers.TryParsePath(pathExpression.Body, out path) || path == null)
            {
                throw new ArgumentException("InvalidIncludePathExpression", nameof(pathExpression));
            }

            return queryable.Include(path);
        }
    }
}