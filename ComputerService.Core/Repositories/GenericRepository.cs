using ComputerService.Common.Helpers;
using ComputerService.Core.Exceptions;
using ComputerService.Core.Extensions;
using ComputerService.Core.Services;
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

        public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (entities == null) throw new RepositoryException(ErrorCodes.EntitiestCannotBeNull, $"Parameter {nameof(entities)} cannot be null.");

            _context.Set<TEntity>().UpdateRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
            DetachAll(detachAll);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<TEntity, dynamic> distinctBy = null)
        {
            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;

            var result = await query.ToListAsync(cancellationToken);

            return distinctBy != null ? result.DistinctBy(distinctBy) : result;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsyncWithPredicate(CancellationToken cancellationToken,
         Expression<Func<TEntity, bool>> predicate,
         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
         Func<TEntity, dynamic> distinctBy = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;

            var result = await query.Where(predicate).ToListAsync(cancellationToken);

            return distinctBy != null ? result.DistinctBy(distinctBy) : result;
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

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");

            return await _context.Set<TEntity>().CountAsync(predicate, cancellationToken);
        }

        public virtual IEnumerable<TEntity> GetPage(IEnumerable<TEntity> list, byte pageSize = 16, int pageNumber = 0)
        {
            var start = pageSize * pageNumber > list.Count() ? 0 : pageSize * pageNumber;
            int numberOfElements = start + pageSize > list.Count() ? list.Count() - start : pageSize;

            var result = list?.Skip(start).Take(numberOfElements).ToList() ?? null;

            return result;
        }

        public virtual async Task<TEntity> GetFirstAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;

            var result = await query.FirstOrDefaultAsync(predicate, cancellationToken);
            if (result == null) throw new RepositoryException(ErrorCodes.ObjectOfTypeNotFound, $"Object of type {typeof(TEntity).Name} not found.");

            return result;
        }

        public virtual async Task<TResult> GetFirstAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new RepositoryException(ErrorCodes.SelectCannotBeNull, $"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;

            var result = query.Where(predicate);

            if (!await result.AnyAsync()) throw new RepositoryException(ErrorCodes.ObjectOfTypeNotFound, $"Object of type {typeof(TEntity).Name} not found.");

            return await result.Select(select).FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetFirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;

            return await query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<TResult> GetFirstOrDefaultAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new RepositoryException(ErrorCodes.SelectCannotBeNull, $"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;

            return await query.Where(predicate).Select(select).FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetSingleAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;

            var result = await query.SingleOrDefaultAsync(predicate, cancellationToken);
            if (result == null) throw new RepositoryException(ErrorCodes.ObjectOfTypeNotFound, $"Object of type {typeof(TEntity).Name} not found.");

            return result;
        }

        public virtual async Task<TResult> GetSingleAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new RepositoryException(ErrorCodes.SelectCannotBeNull, $"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;

            var result = query.Where(predicate);

            if (!await result.AnyAsync()) throw new RepositoryException(ErrorCodes.ObjectOfTypeNotFound, $"Object of type {typeof(TEntity).Name} not found.");

            return await result.Select(select).SingleOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<TEntity> GetSingleOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;

            return await query.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<TResult> GetSingleOrDefaultAsync<TResult>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TResult>> select,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");
            if (select == null) throw new RepositoryException(ErrorCodes.SelectCannotBeNull, $"Parameter {nameof(select)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;

            return await query.Where(predicate).Select(select).SingleOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<int> GetNumberOfPagesAsync(CancellationToken cancellationToken, byte pageSize = 16,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<TEntity, dynamic> distinctBy = null)
        {
            var result = await this.GetAllAsync(cancellationToken, include, orderBy, distinctBy);

            return result.Count() % pageSize == 0 ? result.Count() / pageSize : result.Count() / pageSize + 1;
        }

        public virtual async Task DeletePermanentlyAsync(TEntity entity, CancellationToken cancellationToken, bool detachAll = false)
        {

            if (entity == null) throw new RepositoryException(ErrorCodes.EntitiestCannotBeNull, $"Parameter {nameof(entity)} cannot be null.");

            _context.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            DetachAll(detachAll);
        }

        public virtual async Task DeletePermanentlyByIdAsync<TId>(TId id, CancellationToken cancellationToken, bool detachAll = false)
        {
            if (id == null || (id is int idInt && idInt == 0)) throw new RepositoryException(ErrorCodes.IdCannotBeNullOrZero, $"Parameter {nameof(id)} cannot be null or zero.");

            var entity = await GetByIdAsync(id, cancellationToken);

            await DeletePermanentlyAsync(entity, cancellationToken);
            DetachAll(detachAll);
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

        public async Task<PaginatedList<TEntity>> GetAsync(Expression<Func<TEntity,
            bool>> predicate, int pageSize, int pageIndex,
            CancellationToken cancellationToken,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>,
            IIncludableQueryable<TEntity, object>> include = null)
        {
            if (predicate == null) throw new RepositoryException(ErrorCodes.PredicateCannotBeNull, $"Parameter {nameof(predicate)} cannot be null.");

            var query = _context.Set<TEntity>().AsNoTracking();
            query = include?.Invoke(query) ?? query;
            query = orderBy?.Invoke(query) ?? query;
            query = query.Where(predicate);

            return await PaginatedList<TEntity>.CreateAsync(query, pageIndex, pageSize);
        }

        Task<PaginatedList<TEntity>> IGenericRepository<TEntity>.GetAsync(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageNumber, CancellationToken cancellationToken, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include)
        {
            throw new NotImplementedException();
        }
    }
}
