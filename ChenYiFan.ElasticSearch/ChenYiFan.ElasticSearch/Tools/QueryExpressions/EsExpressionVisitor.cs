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
            if (node.Member is FieldInfo field)
            {
                var ce = (ConstantExpression)node.Expression;
                var value = field.GetValue(ce.Value).ToString();
                this._leafList.Push(value);
            }
            else if (node.Member is PropertyInfo)
            {
                this._leafList.Push(node.Member.Name);
            }
            return node;
        }

    }
}
