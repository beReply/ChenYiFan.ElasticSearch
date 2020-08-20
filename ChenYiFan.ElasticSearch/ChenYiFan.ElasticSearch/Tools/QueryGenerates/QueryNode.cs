using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static ChenYiFan.ElasticSearch.Enums.ElasticSearchEnum;

namespace ChenYiFan.ElasticSearch.Tools.QueryGenerates
{
    public class QueryNode
    {
        /// <summary>
        /// 如果未指定节点类型，节点类型将通过节点的内容进行判断；
        /// 当name不为null时，如果value不为null，则判定节点为叶子节点，为null则为普通节点；
        /// 当name为null且value为null时，该节点为根节点；
        /// 叶子节点不再拥有添加子节点能力；
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="nodeType"></param>
        public QueryNode(string name = null, string value = null, NodeType nodeType = NodeType.普通节点)
        {
            NodeType = nodeType == NodeType.普通节点 ? (value == null ? NodeType.普通节点 : NodeType.叶子节点) : nodeType;
            Name = name;
            Value = value;

            if (NodeType != NodeType.叶子节点) { Node = new List<QueryNode>(); }
        }

        #region 树

        [DisplayName("父节点")]
        public QueryNode FatherNode { get; set; }

        [DisplayName("子节点")]
        public List<QueryNode> Node { get; set; }

        #endregion


        #region 值

        [DisplayName("节点名")]
        public string Name { get; set; }

        [DisplayName("节点值")]
        public string Value { get; set; }

        [DisplayName("节点类型")]
        public NodeType NodeType { get; set; }

        #endregion
    }
}
