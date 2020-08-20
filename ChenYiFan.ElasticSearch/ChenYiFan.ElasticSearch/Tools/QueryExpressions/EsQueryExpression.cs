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
        public static QueryNode Query(this QueryNode node)
        {
            if (node.Node.Any(x => x.Name == "query"))
            {
                return node.ToChildNode("query");
            }
            return node.AddNodeAndToChild("query");
        }


        #region Boolean query

        // 用于表示一个bool查询
        public static QueryNode Bool(this QueryNode node)
        {
            if (node.Node.Any(x => x.Name == "bool"))
            {
                return node.ToChildNode("bool");
            }
            return node.AddNodeAndToChild("bool");
        }

        // 子句必须满足，并且会影响score
        public static QueryNode Must(this QueryNode node)
        {
            if (node.Node.Any(x => x.Name == "must"))
            {
                return node.ToChildNode("must");
            }
            return node.AddNodeAndToChild("must");
        }

        // 子句必须满足(多个条件)，并且会影响score
        public static QueryNode MultiMust(this QueryNode node)
        {
            if (node.Node.Any(x => x.Name == "must"))
            {
                return node.ToChildNode("must");
            }
            return node.AddArrayNodeAndToChild("must");
        }



        // 子句必须满足，并且不会影响score
        public static QueryNode Filter(this QueryNode node)
        {
            if (node.Node.Any(x => x.Name == "filter"))
            {
                return node.ToChildNode("filter");
            }
            return node.AddNodeAndToChild("filter");
        }

        // 子句必须不满足，并且不会影响score
        public static QueryNode MustNot(this QueryNode node)
        {
            if (node.Node.Any(x => x.Name == "must_not"))
            {
                return node.ToChildNode("must_not");
            }
            return node.AddNodeAndToChild("must_not");
        }

        // 子句应该满足
        public static QueryNode Should(this QueryNode node)
        {
            if (node.Node.Any(x => x.Name == "should"))
            {
                return node.ToChildNode("should");
            }
            return node.AddNodeAndToChild("should");
        }

        #endregion


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
