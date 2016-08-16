﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EntityFramework.Toolkit.Core.Extensions
{
    public static class GenericRepositoryExtensions
    {
        /// <summary>
        ///     Indicates whether an entity with the given primary key value exists.
        /// </summary>
        /// <param name="ids">The primary keys of the entity to be found.</param>
        /// <returns>true, if an entity with given primary key exists; otherwise, false.</returns>
        public static bool Any<T>(this IGenericRepository<T> repository, params object[] ids)
        {
            return repository.FindById(ids) != null;
        }

        /// <summary>
        ///     Marks the the entity with the given primary key as Deleted such that it will be deleted from the database when
        ///     SaveChanges
        ///     is called. Note that the entity must exist in the context in some other state before this method
        ///     is called.
        /// </summary>
        /// <returns> The entity that has been removed.</returns>
        /// <remarks>
        ///     Note that if the entity exists in the context in the Added state, then this method
        ///     will cause it to be detached from the context.  This is because an Added entity is assumed not to
        ///     exist in the database such that trying to delete it does not make sense.
        /// </remarks>
        public static T RemoveById<T>(this IGenericRepository<T> repository, params object[] ids)
        {
            var entity = repository.FindById(ids);
            if (entity == null)
            {
                throw new ArgumentOutOfRangeException(nameof(ids));
            }

            return repository.Remove(entity);
        }

        /// <summary>Removes all entities that match the conditions defined by the given predicate.</summary>
        /// <returns>The removed entities.</returns>
        /// <param name="predicate">The expression that defines the conditions of the elements to remove.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="predicate" /> is null.
        /// </exception>
        public static IEnumerable<T> RemoveAll<T>(this IGenericRepository<T> repository, Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = repository.Get();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return repository.RemoveRange(query);
        }
    }
}