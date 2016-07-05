﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Transactions;

using EntityFramework.Toolkit.Core;
using EntityFramework.Toolkit.Exceptions;

#if !NET40
using System.Threading.Tasks;
#endif

namespace EntityFramework.Toolkit
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, IContext> contexts;

        private bool disposed;

        public UnitOfWork()
        {
            this.contexts = new Dictionary<Type, IContext>();
        }

        public void RegisterContext<TContext>(TContext contextFactory) where TContext : IContext
        {
            if (contextFactory == null)
            {
                throw new ArgumentNullException(nameof(contextFactory));
            }

            Type contextType = contextFactory.GetType();

            if (!this.contexts.ContainsKey(contextType))
            {
                this.contexts.Add(contextType, contextFactory);
            }
        }

        /// <inheritdoc />
        public ICollection<ChangeSet> Commit()
        {
            var changeSets = new Collection<ChangeSet>();
            Type lastContextType = null;
            try
            {
                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (var context in this.contexts)
                    {
                        lastContextType = context.Key;
                        var changes = context.Value.SaveChanges();
                        changeSets.Add(changes);
                    }

                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new UnitOfWorkException(string.Format("UnitOfWork in context '{0}' failed to commit.", lastContextType?.Name), ex);
            }

            return changeSets;
        }

#if !NET40
        /// <inheritdoc />
        public async Task<int> CommitAsync()
        {
            int numberOfChanges = 0;
            Type lastContextType = null;
            try
            {
                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (var context in this.contexts)
                    {
                        lastContextType = context.Key;
                        numberOfChanges += await context.Value.SaveChangesAsync();
                    }

                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new UnitOfWorkException(string.Format("UnitOfWork in context '{0}' failed to commit.", lastContextType?.Name), ex);
            }

            return numberOfChanges;
        }
#endif

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    foreach (var c in this.contexts.Values)
                    {
                        c.Dispose();
                    }
                    this.contexts.Clear();
                }

                this.disposed = true;
            }
        }

        ~UnitOfWork()
        {
            this.Dispose(false);
        }
    }
}