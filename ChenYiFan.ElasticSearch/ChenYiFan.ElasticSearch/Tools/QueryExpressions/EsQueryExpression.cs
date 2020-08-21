using ChenYiFan.ElasticSearch.Extensions;
using ChenYiFan.ElasticSearch.Tools.QueryGenerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ChenYiFan.ElasticSearch.Tools.QueryExpressions
{
    public static class EsQueryExpression
    {

        #region 查询子句

        public static QueryNode Range<T>(this QueryNode node, Expression<Func<T, bool>> expression)
        {
            var visitor = new EsExpressionVisitor();
            visitor.Visit(expression);
            var exprString = visitor.GetWhere();
            var exprStr = exprString.Split(" ");

            var propType = typeof(T).GetProperty(exprStr[0])?.PropertyType;
            if (propType?.BaseType == typeof(ValueType))
            {
                if (exprStr[1].ToEsOperator() != exprStr[1])
                {
                    node.AddNodeAndToChild("range")
                        .AddNodeAndToChild(exprStr[0])
                        .AddNode(exprStr[1].ToEsOperator(), exprStr[2]);
                }
                else if (exprStr[1] == "Equal")
                {
                    node.AddNodeAndToChild("range")
                        .AddNodeAndToChild(exprStr[0])
                        .AddNode("gte", exprStr[2])
                        .AddNode("lte", exprStr[2]);
                }
            }
            return node;
        }

        public static QueryNode Match<T>(this QueryNode node, Expression<Func<T, bool>> expression)
        {
            var visitor = new EsExpressionVisitor();
            visitor.Visit(expression);
            var element = visitor.GetWhere().Trim().Split(" ");

            var propType = typeof(T).GetProperty(element[0])?.PropertyType;
            if (propType == typeof(string))
            {
                node.AddNodeAndToChild("match")
                    .AddNodeAndToChild(element[0])
                    .AddNode("query", element[2]);
            }

            return node;
        }

        public static QueryNode RangeOrMatch<T>(this QueryNode node, Expression<Func<T, bool>> expression)
        {
            var visitor = new EsExpressionVisitor();
            visitor.Visit(expression);
            var exprArray = visitor.GetWhere().Split("AndAlso");

            foreach (var expr in exprArray)
            {
                var element = expr.Trim().Split(" ");
                var propType = typeof(T).GetProperty(element[0])?.PropertyType;

                if (propType?.BaseType == typeof(ValueType))
                {
                    if (element[1].ToEsOperator() != element[1])
                    {
                        node.AddNodeAndToChild("range")
                            .AddNodeAndToChild(element[0])
                            .AddNode(element[1].ToEsOperator(), element[2]);
                    }
                    else if (element[1] == "Equal")
                    {
                        node.AddNodeAndToChild("range")
                            .AddNodeAndToChild(element[0])
                            .AddNode("gte", element[2])
                            .AddNode("lte", element[2]);
                    }
                }

                if (propType == typeof(string))
                {
                    node.AddNodeAndToChild("match")
                        .AddNodeAndToChild(element[0])
                        .AddNode("query", element[2]);
                }
            }

            return node;
        }

        public static QueryNode ShouldOrMust<T>(this QueryNode node, Expression<Func<T, bool>> expression)
        {
            var visitor = new QueueExpressionVisitor();
            visitor.Visit(expression);

            while (!visitor.QueueIsEmpty())
            {
                var element = visitor.Dequeue();
                if (element == "AndAlso")
                {
                    if (node.Node != null && node.Node.Any(x => x.Name == ""))
                    {
                        
                    }
                }
                else if (element == "OrElse")
                {
                    node.MultiShould();
                }
            }

            return node;
        }

        #endregion


        #region 查询糖

        /// <summary>
        /// 分页: from
        /// </summary>
        /// <param name="node"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public static QueryNode From(this QueryNode node, int from)
        {
            return node.AddNode("from", from.ToString());
        }

        /// <summary>
        /// 分页: size
        /// </summary>
        /// <param name="node"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static QueryNode Size(this QueryNode node, int size)
        {
            return node.AddNode("size", size.ToString());
        }

        /// <summary>
        /// 使用了bool复合查询，在must中添加多个查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static QueryNode Where<T>(this QueryNode node, Expression<Func<T, bool>> expression)
        {
            return node.Query().Bool().MultiMust().RangeOrMatch<T>(expression).ToRootNode();
        }

        public static QueryNode WhereIf<T>(this QueryNode node, bool flag, Expression<Func<T, bool>> expression)
        {
            if (flag)
            {
                return node.Query().Bool().MultiMust().RangeOrMatch<T>(expression).ToRootNode();
            }
            else
            {
                return node;
            }
        }


        #endregion
    }
}
