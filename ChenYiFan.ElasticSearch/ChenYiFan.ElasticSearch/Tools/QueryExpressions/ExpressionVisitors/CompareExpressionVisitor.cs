using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ChenYiFan.ElasticSearch.Extensions;

namespace ChenYiFan.ElasticSearch.Tools.QueryExpressions.ExpressionVisitors
{
    class CompareExpressionVisitor : ExpressionVisitor
    {
        private readonly Stack<string> _leafList = new Stack<string>();

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
            var value = node.ReadExpression(new Stack<PropertyInfo>()).ToString();
            _leafList.Push(value);
            return node;
        }

    }
}
