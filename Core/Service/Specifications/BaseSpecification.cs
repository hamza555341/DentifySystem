using Domain.Entites;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public abstract class BaseSpecification<TEntity, Tkey> : ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {

        #region Criteria
        protected BaseSpecification(Expression<Func<TEntity, bool>> criteriaExp)
        {
            Criteria = criteriaExp;

        }
        public Expression<Func<TEntity, bool>> Criteria { get; }

        #endregion

        #region Includes
        public ICollection<Expression<Func<TEntity, object>>> IncludeExepressions { get; } = [];

        protected void AddInclude(Expression<Func<TEntity, object>> IncludeExp)
        {
            IncludeExepressions.Add(IncludeExp);

        }
        #endregion

        #region Order by (Sorting)

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }


        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExp)
        {
            OrderBy = orderByExp;
        }

        protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExp)
        {
            OrderByDescending = orderByDescExp;
        }


        #endregion

    }
}
