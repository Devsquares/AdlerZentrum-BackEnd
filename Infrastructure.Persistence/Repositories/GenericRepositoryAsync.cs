using Application.Filters;
using Application.Interfaces;
using Domain.Settings;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repository
{
    public class GenericRepositoryAsync<T> : IGenericRepositoryAsync<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepositoryAsync(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetCount(T entity)
        {
            return _dbContext.Set<T>().Count();
        }


        public int GetCount(FilteredRequestParameter filteredRequestParameter)
        {
            return _dbContext
                .Set<T>()
                .Where(IsMatchedExpression(filteredRequestParameter))
                .Count();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _dbContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetPagedReponseAsync(FilteredRequestParameter filteredRequestParameter)
        {
            bool noPaging = filteredRequestParameter.NoPaging;
            if (noPaging)
            {
                filteredRequestParameter.PageNumber = 1;
                filteredRequestParameter.PageSize = FilteredRequestParameter.MAX_ELEMENTS;
            }
            int pageNumber = filteredRequestParameter.PageNumber;
            int pageSize = filteredRequestParameter.PageSize;

            string sortBy = filteredRequestParameter.SortBy;
            if (sortBy == null)
            {
                sortBy = "ID";
            }
            string sortType = filteredRequestParameter.SortType;
            bool sortASC = true;

            if (sortType.ToUpper().Equals("DESC"))
            {
                sortASC = false;
            }
            return await _dbContext
                    .Set<T>()
                    .Where(IsMatchedExpression(filteredRequestParameter))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .OrderBy(sortBy, sortASC)
                    .AsNoTracking()
                    .ToListAsync();
            
        }

        public Expression<Func<T, bool>> IsMatchedExpression(FilteredRequestParameter filteredRequestParameter)
        {
            Dictionary<string, string> filterValue = LowerCaseDict(filteredRequestParameter.FilterValue);
            Dictionary<string, string> filterRange = LowerCaseDict(filteredRequestParameter.FilterRange);
            Dictionary<string, List<string>> filterArray = LowerCaseDict(filteredRequestParameter.FilterArray);
            Dictionary<string, string> filterSearch = LowerCaseDict(filteredRequestParameter.FilterSearch);

            var parameterExpression = Expression.Parameter(typeof(T));
            var binaryExpression = Expression.Equal(Expression.Constant(1), Expression.Constant(1));
            foreach (string key in filterValue.Keys)
            {
                string value;
                filterValue.TryGetValue(key, out value);
                var propertyOrField = Expression.PropertyOrField(parameterExpression, key);

                var propertyInfo = (PropertyInfo)propertyOrField.Member;
                var propertyType = propertyInfo.PropertyType;
                var propertyName = propertyInfo.Name;
                if (propertyType == typeof(string))
                {
                    binaryExpression = Expression.AndAlso(binaryExpression, Expression.Equal(propertyOrField, Expression.Constant(value)));
                } else if (propertyType == typeof(int))
                {
                    binaryExpression = Expression.AndAlso(binaryExpression, Expression.Equal(propertyOrField, Expression.Constant(int.Parse(value))));
                } else if (propertyType  == typeof(DateTime))
                {
                    //TODO: convert to date
                }
                //TODO: other data type?   
            }
            return Expression.Lambda<Func<T, bool>>(binaryExpression, parameterExpression);
        }


        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext
                 .Set<T>()
                 .ToListAsync();
        }

        public string BuildOrderStatement(string sortBy, string sortType)
        {
            sortBy ??= "ID";
            sortType ??= "ASC";
            return " ORDER BY " + sortBy + " " + sortType;
        }

        public string BuildWhereStatement(FilteredRequestParameter filteredRequestParameter, Dictionary<string, Types> types)
        {
            //TODO: this code doesn't handle sql injection
            //TODO: this should be fixed using sql parameters
            // Not a priority now
            string where = "";
            Dictionary<string, string> FilterValue = LowerCaseDict(filteredRequestParameter.FilterValue);
            Dictionary<string, string> FilterRange = LowerCaseDict(filteredRequestParameter.FilterRange);
            Dictionary<string, List<string>> FilterArray = LowerCaseDict(filteredRequestParameter.FilterArray);
            Dictionary<string, string> FilterSearch = LowerCaseDict(filteredRequestParameter.FilterSearch);

            foreach (string filteredProperty in FilterValue.Keys)
            {
                string value;
                FilterValue.TryGetValue(filteredProperty, out value);
                where = AddToWhereValue(filteredProperty.ToUpper(), value, where, types);
            }
            foreach (string filteredProperty in FilterRange.Keys)
            {
                string range;
                FilterRange.TryGetValue(filteredProperty, out range);
                where = AddToWhereRange(filteredProperty.ToUpper(), range, where, types);
            }
            foreach (string filteredProperty in FilterArray.Keys)
            {
                List<string> array;
                FilterArray.TryGetValue(filteredProperty, out array);
                where = AddToWhereArray(filteredProperty.ToUpper(), array, where, types);
            }
            foreach (string filteredProperty in FilterSearch.Keys)
            {
                string searchValue;
                FilterSearch.TryGetValue(filteredProperty, out searchValue);
                where = AddToWhereSearch(filteredProperty.ToUpper(), searchValue, where, types);
            }
            return where;
        }

        public string AddToWhereValue(string key, string value, string where, Dictionary<string, Types> types)
        {

            string newWhere = null;
            Types keyType;
            types.TryGetValue(key, out keyType);
            if (keyType.Equals(Types.Number))
            {
                newWhere = key + " = " + value;
            }
            else if (keyType.Equals(Types.Date))
            {
                //TODO not implemented yet
            }
            else
            {
                newWhere = key + " = " + "'" + value + "'";
            }

            newWhere = AddToWhere(where, newWhere);

            return newWhere;
        }

        public string AddToWhereRange(string key, string range, string where, Dictionary<string, Types> types)
        {

            string newWhere = null;
            Types keyType;
            types.TryGetValue(key, out keyType);
            string value1, value2;

            if (range != null && !range.Equals("") && range.Contains('-'))
            {
                value1 = range.Split('-')[0];
                value2 = range.Split('-')[1];
            }
            else
            {
                return where;
            }
            if (keyType.Equals(Types.Number))
            {
                newWhere = key + " between " + value1 + " and " + value2;
            }
            else if (keyType.Equals(Types.Date))
            {
                //TODO not implemented yet
            }
            else
            {
                newWhere = key + " between '" + value1 + "' and '" + value2 + "'";
            }

            newWhere = AddToWhere(where, newWhere);

            return newWhere;
        }

        public string AddToWhereArray(string key, List<string> array, string where, Dictionary<string, Types> types)
        {
            //Hotfix
            //TODO: replace list with normal string
            string[] actualArray = array[0].Substring(1, array[0].Length - 2).Split(',');
            array = actualArray.ToList<string>();
            string newWhere = null;
            Types keyType;
            types.TryGetValue(key, out keyType);
            if (keyType.Equals(Types.Number))
            {
                newWhere = key + " in (" + string.Join(",", array.ToArray()) + ")";
            }
            else if (keyType.Equals(Types.Date))
            {
                //TODO not implemented yet
            }
            else
            {
                List<string> newArray = new List<string>();
                foreach (string value in array)
                {
                    newArray.Add("'" + value + "'");
                }
                newWhere = key + " in (" +  string.Join(",", newArray.ToArray()) + ")";
            }

            newWhere = AddToWhere(where, newWhere);

            return newWhere;
        }

        public string AddToWhereSearch(string key, string value, string where, Dictionary<string, Types> types)
        {
            string newWhere = null;
            Types keyType;
            types.TryGetValue(key, out keyType);
            if (key.ToLower().Equals("global"))
            {
                //TODO not implemented yet
            }
            else if (keyType.Equals(Types.Number))
            {
                newWhere = key + " = " + value;
            }
            else if (keyType.Equals(Types.Date))
            {
                //TODO not implemented yet
            }
            else
            {
                newWhere = key + " LIKE '%" + value + "%'";
            }

            newWhere = AddToWhere(where, newWhere);

            return newWhere;
        }

        public string AddToWhere(string where, string newWhere)
        {
            if (newWhere == null || newWhere.Equals(""))
            {
                return where;
            }
            string returnWhere = where;
            if (returnWhere.Equals(""))
            {
                returnWhere = " WHERE ";
                returnWhere += "( " + newWhere + " )";
            }
            else
            {
                returnWhere += " AND ";
                returnWhere += "( " + newWhere + " )";
            }
            return returnWhere;
        }

        public string BuildSQLCount(string sqlCols, string sqlFrom, string sqlJoin)
        {
            string sql = "SELECT " + sqlCols + " FROM " + sqlFrom + sqlJoin;
            string outerSQL = "SELECT count(*) FROM (" + sql + ") t ";
            return outerSQL;
        }

        public string BuildSQL(string sqlCols, string sqlFrom, string sqlJoin)
        {
            string sql = "SELECT " + sqlCols + " FROM " + sqlFrom + sqlJoin;
            string outerSQL = "SELECT * FROM (" + sql + ") t ";
            return outerSQL;
        }

        public string BuildSQL(string sqlCols, string sqlFrom, string sqlJoin, string sqlWhere)
        {
            string sql = BuildSQL(sqlCols, sqlFrom, sqlJoin);
            sql += sqlWhere;
            return sql;
        }

        public string BuildSQL(string sqlCols, string sqlFrom, string sqlJoin, string sqlWhere, string sqlOrder)
        {
            string sql = BuildSQL(sqlCols, sqlFrom, sqlJoin, sqlWhere);
            sql += sqlOrder;
            return sql;
        }

        public Dictionary<string, string> LowerCaseDict(Dictionary<string, string> dict)
        {
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (string key in dict.Keys)
            {
                newDict.Add(key.ToLower(), dict.GetValueOrDefault(key));
            }
            return newDict;
        }

        public Dictionary<string, List<string>> LowerCaseDict(Dictionary<string, List<string>> dict)
        {
            Dictionary<string, List<string>> newDict = new Dictionary<string, List<string>>();
            foreach (string key in dict.Keys)
            {
                newDict.Add(key.ToLower(), dict.GetValueOrDefault(key));
            }
            return newDict;
        }

    }
}
