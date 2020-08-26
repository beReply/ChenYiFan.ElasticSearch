using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ChenYiFan.ElasticSearch.Tools.QueryExpressions
{
    class EsExpressionVisitor : ExpressionVisitor
    {
        private readonly Stack<string> _leafList = new Stack<string>();

        public EsExpressionVisitor()
        {

        }

        public string GetWhere()
        {
            return string.Join(" ", _leafList);
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            this.Visit(node.Right);
            this._leafList.Push(node.NodeType.ToString());
            this.Visit(node.Left);
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            this._leafList.Push(node.Value?.ToString());
            return base.VisitConstant(node);
        }


        protected override Expression VisitMember(MemberExpression node)
        {
            var value = ReadMemberExpression(node, new Stack<PropertyInfo>()).ToString();
            _leafList.Push(value);
            return node;
        }

        #region 私有工具

        private object ReadMemberExpression(MemberExpression nodeMember, Stack<PropertyInfo> property)
        {
            if (nodeMember.Member is FieldInfo field)
            {
                var ce = (ConstantExpression)nodeMember.Expression;
                var value = field.GetValue(ce.Value);
                while (property.Count > 0)
                {
                    var propI = property.Pop();
                    value = propI.GetValue(value);
                }
                return value;

            }
            else if (nodeMember.Member is PropertyInfo propertyInfo)
            {
                try
                {
                    var me = (MemberExpression)nodeMember.Expression;
                    property.Push(propertyInfo);
                    var res = ReadMemberExpression(me, property);

                    return res;
                }
                catch (Exception)
                {
                    return nodeMember.Member.Name;
                }
            }

            return "error";
        }

        #endregion

    }
}
