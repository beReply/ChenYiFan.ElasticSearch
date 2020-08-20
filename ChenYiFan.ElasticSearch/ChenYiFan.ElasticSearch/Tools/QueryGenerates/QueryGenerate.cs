using System;
using System.Collections.Generic;
using System.Text;
using static ChenYiFan.ElasticSearch.Enums.ElasticSearchEnum;

namespace ChenYiFan.ElasticSearch.Tools.QueryGenerates
{
    public static class QueryGenerate
    {
        #region 添加节点

        /// <summary>
        /// 增加一个 子节点 并指向该 子节点 (普通节点或叶子节点)
        /// </summary>
        /// <param name="fatherNode"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static QueryNode AddNodeAndToChild(this QueryNode fatherNode, string name, string value = null)
        {
            var sonNode = new QueryNode(name, value) { FatherNode = fatherNode };

            fatherNode.Node.Add(sonNode);
            return sonNode;
        }

        /// <summary>
        /// 增加一个子节点(普通节点或叶子节点)
        /// </summary>
        /// <param name="fatherNode"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static QueryNode AddNode(this QueryNode fatherNode, string name, string value = null)
        {
            var sonNode = new QueryNode(name, value) { FatherNode = fatherNode };
            fatherNode.Node.Add(sonNode);
            return fatherNode;
        }

        /// <summary>
        /// 增加一个子数组节点 并指向该子数组节点
        /// </summary>
        /// <param name="fatherNode"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static QueryNode AddArrayNodeAndToChild(this QueryNode fatherNode, string name, string value = null)
        {
            var sonNode = new QueryNode(name, value, NodeType.数组节点) { FatherNode = fatherNode };
            fatherNode.Node.Add(sonNode);
            return sonNode;
        }

        #endregion


        #region 指针移动

        /// <summary>
        /// 获取父节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static QueryNode ToFatherNode(this QueryNode node)
        {
            return node.FatherNode;
        }

        /// <summary>
        /// 获取对应名字的子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static QueryNode ToChildNode(this QueryNode node, string name)
        {
            return node.Node.Find(x => x.Name == name);
        }

        /// <summary>
        /// 获取根节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static QueryNode ToRootNode(this QueryNode node)
        {
            var fatherNode = node.FatherNode;

            // 一直向上直到找到根节点
            while (!(fatherNode.Value == null && fatherNode.Name == null && fatherNode.NodeType == NodeType.普通节点))
            {
                fatherNode = fatherNode.FatherNode;
            }

            return fatherNode;
        }

        #endregion


        #region 构建查询Body

        public static string GenerateQueryString(this QueryNode queryNode)
        {
            // 为叶子节点，则返回节点值
            if (queryNode.NodeType == NodeType.叶子节点)
            {
                if (queryNode.FatherNode.NodeType == NodeType.数组节点)
                {
                    return $"\"{queryNode.Value}\"";
                }

                return $"\"{queryNode.Name}\":\"{queryNode.Value}\"";
            }


            if (queryNode.Node.Count != 0)
            {
                string sonStr = "";
                foreach (var node in queryNode.Node)
                {
                    sonStr += node.GenerateQueryString() + ",";
                }
                sonStr = sonStr.Substring(0, sonStr.Length - 1);

                if (queryNode.NodeType == NodeType.数组节点)
                {
                    return $"\"{queryNode.Name}\"" + ":[" + sonStr + "]";
                }

                var queryStr = queryNode.Name == null ? "{" + sonStr + "}" : $"\"{queryNode.Name}\"" + ":{" + sonStr + "}";

                if (queryNode.FatherNode != null && queryNode.FatherNode.NodeType == NodeType.数组节点 && queryNode.Node != null)
                {
                    queryStr = "{" + queryStr + "}";
                }

                return queryStr;
            }
            else
            {
                return $"\"{queryNode.Name}\"" + ":{}";
            }
        }

        #endregion
    }
}
