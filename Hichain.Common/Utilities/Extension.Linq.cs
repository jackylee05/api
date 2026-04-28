///*********************************************************************
///Copyright (C) 2016-2019 56-cloud.com 
///Project Name          : 56SCM 
///Create By             : hkwong.wang@qq.com
///Create Date           : 2018-11-11
///Last Updated By       :
///Last Updated Date     :
///Version               :2018-11-11
///*********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Hichain.Common.Utilities
{
    /// <summary>
    /// Defines the <see cref="LinqExtensions" />.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        /// The Property.
        /// </summary>
        /// <param name="expression">The expression<see cref="Expression"/>.</param>
        /// <param name="propertyName">The propertyName<see cref="string"/>.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        public static Expression Property(this Expression expression, string propertyName)
        {
            return Expression.Property(expression, propertyName);
        }

        /// <summary>
        /// The AndAlso.
        /// </summary>
        /// <param name="left">The left<see cref="Expression"/>.</param>
        /// <param name="right">The right<see cref="Expression"/>.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        public static Expression AndAlso(this Expression left, Expression right)
        {
            return Expression.AndAlso(left, right);
        }

        /// <summary>
        /// The Call.
        /// </summary>
        /// <param name="instance">The instance<see cref="Expression"/>.</param>
        /// <param name="methodName">The methodName<see cref="string"/>.</param>
        /// <param name="arguments">The arguments<see cref="Expression[]"/>.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        public static Expression Call(this Expression instance, string methodName, params Expression[] arguments)
        {
            return Expression.Call(instance, instance.Type.GetMethod(methodName), arguments);
        }

        /// <summary>
        /// The GreaterThan.
        /// </summary>
        /// <param name="left">The left<see cref="Expression"/>.</param>
        /// <param name="right">The right<see cref="Expression"/>.</param>
        /// <returns>The <see cref="Expression"/>.</returns>
        public static Expression GreaterThan(this Expression left, Expression right)
        {
            return Expression.GreaterThan(left, right);
        }

        /// <summary>
        /// The ToLambda.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="body">The body<see cref="Expression"/>.</param>
        /// <param name="parameters">The parameters<see cref="ParameterExpression[]"/>.</param>
        /// <returns>The <see cref="Expression{T}"/>.</returns>
        public static Expression<T> ToLambda<T>(this Expression body, params ParameterExpression[] parameters)
        {
            return Expression.Lambda<T>(body, parameters);
        }

        /// <summary>
        /// The True.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="Expression{Func{T, bool}}"/>.</returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return param => true;
        }

        /// <summary>
        /// The False.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <returns>The <see cref="Expression{Func{T, bool}}"/>.</returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return param => false;
        }

        /// <summary>
        /// 组合And.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="first">The first<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="second">The second<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        /// <summary>
        /// 组合Or.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="first">The first<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <param name="second">The second<see cref="Expression{Func{T, bool}}"/>.</param>
        /// <returns>.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="first">The first<see cref="Expression{T}"/>.</param>
        /// <param name="second">The second<see cref="Expression{T}"/>.</param>
        /// <param name="merge">The merge<see cref="Func{Expression, Expression, Expression}"/>.</param>
        /// <returns>The <see cref="Expression{T}"/>.</returns>
        internal static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// ParameterRebinder.
        /// </summary>
        private class ParameterRebinder : ExpressionVisitor
        {
            /// <summary>
            /// The ParameterExpression map.
            /// </summary>
            internal readonly Dictionary<ParameterExpression, ParameterExpression> map;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
            /// </summary>
            /// <param name="map">The map.</param>
            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            /// <summary>
            /// Replaces the parameters.
            /// </summary>
            /// <param name="map">The map.</param>
            /// <param name="exp">The exp.</param>
            /// <returns>Expression.</returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            /// <summary>
            /// Visits the parameter.
            /// </summary>
            /// <param name="p">The p.</param>
            /// <returns>Expression.</returns>
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;
                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }
                return base.VisitParameter(p);
            }
        }
    }
}
