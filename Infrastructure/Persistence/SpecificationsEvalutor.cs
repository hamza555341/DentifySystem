using Domain.Entites;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class SpecificationsEvalutor
    {
        // Create Query or (build Query)

        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> EntryPoint,
            ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var Query = EntryPoint; // _dbContext.Products

            if (specification is not null)
            {
                if (specification.Criteria is not null)
                {
                    Query = Query.Where(specification.Criteria);
                }


                if (specification.IncludeExepressions is not null && specification.IncludeExepressions.Any())
                {
                    ///foreach (var IncludeExp in specification.IncludeExepressions)
                    ///{
                    ///    Query = Query.Include(IncludeExp);

                    ///}

                    Query = specification.IncludeExepressions.Aggregate(Query,
                    (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));


                }


                if (specification.OrderBy is not null)
                {
                    Query = Query.OrderBy(specification.OrderBy);
                }

                if (specification.OrderByDescending is not null)
                {
                    Query = Query.OrderByDescending(specification.OrderByDescending);
                }
            }


            return Query;
        }
       }
    }