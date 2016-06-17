﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Extensions.Utils;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Extensions.Extensions
{
    public static class DbContextExtensions
    {
        public static void Seed(this DbContext context, IEnumerable<IDataSeed> dataSeeds)
        {
            foreach (var dataSeed in dataSeeds)
            {
                var predicate = dataSeed.GetAddOrUpdateExpression();

                ReflectionHelper.InvokeGenericMethod(
                    null,
                    () => DbContextExtensions.AddOrUpdate<object>(null, null, null),
                    dataSeed.EntityType,
                    new object[] { context, predicate, dataSeed.GetAllObjects() });
            }
        }

        public static void AddOrUpdate<TEntity>(this DbContext context, Expression<Func<object, object>> propertyExpression, params object[] entities) where TEntity : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            var set = context.Set<TEntity>();
            var parameter = Expression.Parameter(typeof(TEntity));
            foreach (var entity in entities)
            {
                string propertyName = propertyExpression.GetPropertyInfo().Name;
                object propertyValue = entity.GetPropertyValue(propertyName);

                var equalExpression = Expression.Equal(
                 Expression.Property(parameter, propertyName),
                 Expression.Constant(propertyValue));

                var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(equalExpression, parameter);
                TEntity existingEntity = set.SingleOrDefault(lambdaExpression);
                if (existingEntity != null)
                {
                    context.Entry(existingEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    context.Entry(entity).State = EntityState.Added;
                }
            }
        }

        /// <summary>
        ///     Adds an entity (if newly created) or update (if has non-default Id).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The db context.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <remarks>
        ///     Will not work for HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).
        ///     Will not work for composite keys.
        /// </remarks>
        public static T AddOrUpdate<T>(this DbContext context, T entity) where T : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (IsTransient(context, entity))
            {
                context.Set<T>().Add(entity);
            }
            else
            {
                context.Set<T>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
            return entity;
        }

        /// <summary>
        ///     Determines whether the specified entity is newly created (Id not specified).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///     <c>true</c> if the specified entity is transient; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        ///     Will not work for HasDatabaseGeneratedOption(DatabaseGeneratedOption.None).
        ///     Will not work for composite keys.
        /// </remarks>
        public static bool IsTransient<T>(this DbContext context, T entity) where T : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var propertyInfo = GetPrimaryKeyFor<T>(context);
            var propertyType = propertyInfo.PropertyType;
            //what's the default value for the type?
            var transientValue = propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
            //is the pk the same as the default value (int == 0, string == null ...)
            return Equals(propertyInfo.GetValue(entity, null), transientValue);
        }

        /// <summary>
        ///     Loads a stub entity (or actual entity if already loaded).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <remarks>
        ///     Will not work for composite keys.
        /// </remarks>
        public static T Load<T>(this DbContext context, object id) where T : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var property = GetPrimaryKeyFor<T>(context);
            //check to see if it's already loaded (slow if large numbers loaded)
            var entity = context.Set<T>().Local.FirstOrDefault(x => id.Equals(property.GetValue(x, null)));
            if (entity == null)
            {
                //it's not loaded, just create a stub with only primary key set
                entity = CreateEntity<T>(id, property);

                context.Set<T>().Attach(entity);
            }
            return entity;
        }

        /// <summary>
        ///     Determines whether the specified entity is loaded from the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="id">The id.</param>
        /// <returns>
        ///     <c>true</c> if the specified entity is loaded; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        ///     Will not work for composite keys.
        /// </remarks>
        public static bool IsLoaded<T>(this DbContext context, object id) where T : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var property = GetPrimaryKeyFor<T>(context);
            //check to see if it's already loaded (slow if large numbers loaded)
            var entity = context.Set<T>().Local.FirstOrDefault(x => id.Equals(property.GetValue(x, null)));
            return entity != null;
        }

        /// <summary>
        ///     Marks the reference navigation properties unchanged.
        ///     Use when adding a new entity whose references are known to be unchanged.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="entity">The entity.</param>
        public static void MarkReferencesUnchanged<T>(DbContext context, T entity) where T : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var objectSet = objectContext.CreateObjectSet<T>();
            var elementType = objectSet.EntitySet.ElementType;
            var navigationProperties = elementType.NavigationProperties;
            //the references
            var references = from navigationProperty in navigationProperties
                             let end = navigationProperty.ToEndMember
                             where end.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne || end.RelationshipMultiplicity == RelationshipMultiplicity.One
                             select navigationProperty.Name;
            //NB: We don't check Collections. EF wants to handle the object graph.

            var parentEntityState = context.Entry(entity).State;
            foreach (var navigationProperty in references)
            {
                //if it's modified but not loaded, don't need to touch it
                if (parentEntityState == EntityState.Modified && !context.Entry(entity).Reference(navigationProperty).IsLoaded) continue;
                var propertyInfo = typeof(T).GetProperty(navigationProperty);
                var value = propertyInfo.GetValue(entity, null);
                context.Entry(value).State = EntityState.Unchanged;
            }
        }

        /// <summary>
        ///     Merges a DTO into a new or existing entity attached/added to context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <param name="dataTransferObject">
        ///     The data transfer object. It must have a primary key property of the same name and
        ///     type as the actual entity.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        ///     Will not work for composite keys.
        /// </remarks>
        public static T Merge<T>(this DbContext context, T dataTransferObject) where T : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (dataTransferObject == null)
            {
                throw new ArgumentNullException(nameof(dataTransferObject));
            }

            var property = GetPrimaryKeyFor<T>(context);
            //find the id property of the dto
            var idProperty = dataTransferObject.GetType().GetProperty(property.Name);
            if (idProperty == null)
            {
                throw new InvalidOperationException("Cannot find an id on the dataTransferObject");
            }
            var id = idProperty.GetValue(dataTransferObject, null);
            //has the id been set (existing item) or not (transient)?
            var propertyType = property.PropertyType;
            var transientValue = propertyType.IsValueType ? Activator.CreateInstance(propertyType) : null;
            var isTransient = Equals(id, transientValue);
            T entity;
            if (isTransient)
            {
                //it's transient, just create a dummy
                entity = CreateEntity<T>(id, property);
                //if DatabaseGeneratedOption(DatabaseGeneratedOption.None) and no id, this errors
                context.Set<T>().Attach(entity);
            }
            else
            {
                //try to load from identity map or database
                entity = context.Set<T>().Find(id);
                if (entity == null)
                {
                    //could not find entity, assume assigned primary key
                    entity = CreateEntity<T>(id, property);
                    context.Set<T>().Add(entity);
                }
            }
            //copy the values from DTO onto the entry
            context.Entry(entity).CurrentValues.SetValues(dataTransferObject);
            return entity;
        }

        /// <summary>
        /// Returns the primary key <see cref="PropertyInfo"/> for a given type <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="context">The context in which the given entity type lives.</param>
        /// <returns></returns>
        public static PropertyInfo GetPrimaryKeyFor<TEntity>(this DbContext context) where TEntity : class
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //find the primary key
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            //this will error if it's not a mapped entity
            var objectSet = objectContext.CreateObjectSet<TEntity>();
            var elementType = objectSet.EntitySet.ElementType;
            var primaryKey = elementType.KeyMembers.First();
            //look it up on the entity
            var propertyInfo = typeof(TEntity).GetProperty(primaryKey.Name);
            return propertyInfo;
        }

        private static T CreateEntity<T>(object id, PropertyInfo property) where T : class
        {
            // consider IoC here
            var entity = (T)Activator.CreateInstance(typeof(T));
            //set the value of the primary key (may error if wrong type)
            property.SetValue(entity, id, null);
            return entity;
        }

        /// <summary>
        /// Scans the assembly in which the given <paramref name="context"/> lives for all
        /// implementations of <see cref="EntityTypeConfiguration{TEntityType}"/> and 
        /// configures them using the given <paramref name="modelBuilder"/>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="modelBuilder">The model builder used to configure the EntityTypeConfigurations.</param>
        public static void AutoConfigure(this DbContext context, DbModelBuilder modelBuilder)
        {
            var assembly = Assembly.GetAssembly(context.GetType());
            var entityConfigurationTypes = assembly.GetTypes().Where(type => 
                type.BaseType != null &&
                type.BaseType.IsGenericType && 
                type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            foreach (var entityConfigurationType in entityConfigurationTypes)
            {
                dynamic entityConfiguration = Activator.CreateInstance(entityConfigurationType);
                modelBuilder.Configurations.Add(entityConfiguration);
            }
        }
    }
}