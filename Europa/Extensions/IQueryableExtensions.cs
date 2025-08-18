using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Europa.Extensions
{

    // ReSharper disable once InconsistentNaming
    public static class IQueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        /// <summary>
        /// Filtra a propriedade passada como parâmetro (DateTime - Lambda) de acordo com as datas indicadas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="exp"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereByDateRange<T>(this IQueryable<T> source, Expression<Func<T, DateTime>> exp, DateTime? dateFrom, DateTime? dateTo)
        {
            return source.Where(FilterByDateRange(exp, dateFrom, dateTo));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var path = propertyName.Split('.');

            ParameterExpression param = Expression.Parameter(typeof(T));
            var expressions = new List<MemberExpression>();
            for (int idx = 0; idx < path.Length; idx++)
            {
                Expression objectNativate = idx == 0 ? (Expression) param : expressions[idx - 1];
                expressions.Insert(idx, Expression.Property(objectNativate, path[idx]));
            }

            UnaryExpression unaryExpression = Expression.Convert(expressions[expressions.Count-1], typeof(object));
            return Expression.Lambda<Func<T, object>>(unaryExpression, param);
        }

        private static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                       Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        private static Expression<Func<T, bool>> OrExpression<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        private static Expression<Func<T, bool>> FilterByDateRange<T>(Expression<Func<T, DateTime>> exp, DateTime? beginDate, DateTime? finalDate)
        {
            var resultadoFinalMaiorQueDataDe = HigherOrEquals(exp, beginDate);
            var resultadoFinalMenorQueDataAte = LowerOrEquals(exp, finalDate);

            Expression<Func<T, bool>> expression;
            FinalFilter(resultadoFinalMaiorQueDataDe, resultadoFinalMenorQueDataAte, out expression);
            return expression;
        }

        private static void FinalFilter<T>(Expression<Func<T, bool>> resultadoFinalMaiorQueDataDe, Expression<Func<T, bool>> resultadoFinalMenorQueDataAte,
            out Expression<Func<T, bool>> expression)
        {
            if (resultadoFinalMaiorQueDataDe != null && resultadoFinalMenorQueDataAte != null)
            {
                expression = And(resultadoFinalMaiorQueDataDe, resultadoFinalMenorQueDataAte);
            }else if (resultadoFinalMaiorQueDataDe != null)
            {
                expression = resultadoFinalMaiorQueDataDe;
            }else if (resultadoFinalMenorQueDataAte != null)
            {
                expression = resultadoFinalMenorQueDataAte;
            }
            else
            {
                expression = x => true;
            }
        }

        private static Expression<Func<T,bool>> LowerOrEquals<T>(Expression<Func<T, DateTime>> lambdaThatRepresentsDateTime, DateTime? finalDate)
        {
            if (!finalDate.HasValue) return null;
            var max = Expression.Constant(finalDate.Value.Date.AddDays(1).AddTicks(-1), typeof(DateTime));
            var menorOuIgualQueDataAte = Expression.LessThanOrEqual(lambdaThatRepresentsDateTime.Body, max);
            return Expression.Lambda<Func<T, bool>>(menorOuIgualQueDataAte, lambdaThatRepresentsDateTime.Parameters);
        }

        private static Expression<Func<T,bool>> HigherOrEquals<T>(Expression<Func<T, DateTime>> exp, DateTime? beginDate)
        {
            if (!beginDate.HasValue) return null;
            var from = Expression.Constant(beginDate.Value.Date, typeof(DateTime));
            var maiorOuIgualQueDataDe = Expression.GreaterThanOrEqual(exp.Body, from);
            return Expression.Lambda<Func<T, bool>>(maiorOuIgualQueDataDe, exp.Parameters);
        }
    }

    /// <summary>
    /// Classe que força a ordenação de strings também em ordem numérica
    /// </summary>
    /// <example> Se a pessoa tem várias strings como (Casa1, Casa2, Casa13, Casa23) a ordenação natural de strings Seria - Casa 1, Casa 11, Casa 2, Casa23. Com 
    /// a classe em escopo, temos a organização como Casa1, Casa2, Casa11, Casa23 </example>
    public class SemiNumericComparer : IComparer<string>
    {
        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string x, string y);

        public int Compare(string x, string y)
        {
            return StrCmpLogicalW(x, y);
        }
    }
}
