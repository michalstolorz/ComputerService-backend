using ComputerService.Core.Exceptions;
using ComputerService.Core.Extensions;
using ComputerService.Core.Interfaces.Repositories;
using ComputerService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ComputerService.Core.Repositories
{
    public class GenericRepository<TEntity> : BaseRepository, IGenericRepository<TEntity> where TEntity : class
    {
        public GenericRepository(ApplicationDbContext context) : base(context) { }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? take = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;
            query = query.Where(predicate);
            query = take.HasValue ? query.Take(take.Value) : query;

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TResult>, IOrderedQueryable<TResult>> orderBy = null,
            int? take = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new RepositoryException(ErrorCodes.SelectCannotBeNull, $"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            var selectedQuery = query.Where(predicate).Select(select);

            selectedQuery = orderBy?.Invoke(selectedQuery) ?? selectedQuery;
            selectedQuery = take.HasValue ? selectedQuery.Take(take.Value) : selectedQuery;

            return await selectedQuery.ToListAsync(cancellationToken);
        }

        public async Task<TEntity> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            if (id == null || (id is int idInt && idInt == 0)) throw new RepositoryException(ErrorCodes.IdCannotBeNullOrZero, $"Parameter {nameof(id)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;

            var lambda = CreateFindByPrimaryKeyLambda(id);

            var result = await query.SingleOrDefaultAsync(lambda, cancellationToken);

            return result;
        }

        public virtual async Task<TEntity> UpdateAsync(CancellationToken cancellationToken, TEntity entity,
            bool detachAll = false)
        {
            var result = _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            DetachAll(detachAll);

            return result.Entity;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            if (entity == null) throw new RepositoryException(ErrorCodes.EntitiestCannotBeNull, $"Parameter {nameof(entity)} cannot be null.");

            await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entity, CancellationToken cancellationToken)
        {
            if (entity == null) throw new RepositoryException(ErrorCodes.EntitiestCannotBeNull, $"Parameter {nameof(entity)} cannot be null.");

            await _context.Set<TEntity>().AddRangeAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");

            return await _context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
        }

        protected Expression<Func<TEntity, bool>> CreateFindByPrimaryKeyLambda<TId>(TId id)
        {
            var pkPropertyName = _context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();
            var propertyInfo = typeof(TEntity).GetProperty(pkPropertyName);

            if (propertyInfo?.PropertyType != typeof(TId)) throw new RepositoryException(ErrorCodes.InvalidIdType, $"Invalid type of {nameof(id)} argument.");

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var member = Expression.MakeMemberAccess(parameter, propertyInfo);
            var constant = Expression.Constant(id, id.GetType());
            var equation = Expression.Equal(member, constant);

            return Expression.Lambda<Func<TEntity, bool>>(equation, parameter);
        }

        private void DetachAll(bool detachAll)
        {
            if (detachAll)
            {
                EntityEntry[] entityEntries = _context.ChangeTracker.Entries().ToArray();

                foreach (EntityEntry entityEntry in entityEntries)
                {
                    entityEntry.State = EntityState.Detached;
                }
            }
        }
    }
}
