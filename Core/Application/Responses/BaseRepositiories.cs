using HolookorBackend.Infrastructure.Context;
using System.Linq.Expressions;

namespace HolookorBackend.Core.Application.Responses;
    public class BaseRespositories <T> : IBaseRepositoriesResponse<T> where T : class
    {
        public HolookorSystem _context;

        public BaseRespositories(HolookorSystem context)
        {
            _context = context;
        }

        public bool Check(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }

        public async Task<T>? CreateAsync(T entity)
        {

            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public T Update(T entity)
        {
            var property = entity.GetType().GetProperty("DateUpdated");
            if (property != null && property.PropertyType == typeof(DateTime?))
            {
                property.SetValue(entity, DateTime.UtcNow);
            }
            _context.Set<T>().Update(entity);
            return entity;
        }
    }
