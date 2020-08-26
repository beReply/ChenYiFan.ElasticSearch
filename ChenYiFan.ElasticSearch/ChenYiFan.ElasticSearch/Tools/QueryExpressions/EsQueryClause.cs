using ChenYiFan.ElasticSearch.Tools.QueryGenerates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChenYiFan.ElasticSearch.Tools.QueryExpressions
{
    public static class EsQueryClause
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

        public static QueryNode MultiShould(this QueryNode node)
        {
            if (node.Node.Any(x => x.Name == "should"))
            {
                return node.ToChildNode("should");
            }
            return node.AddArrayNodeAndToChild("should");
        }

        #endregion


        #region 判断子节点是否有

        public static bool HasShould(this QueryNode node)
        {
            return node.Node != null && node.Node.Any(x => x.Name == "should");
        }

        public static bool HasMust(this QueryNode node)
        {
            return node.Node != null && node.Node.Any(x => x.Name == "must");
        }

        #endregion

    }
}
