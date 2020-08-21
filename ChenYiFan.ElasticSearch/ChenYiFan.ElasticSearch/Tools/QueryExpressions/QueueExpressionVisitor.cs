using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ChenYiFan.ElasticSearch.Tools.QueryExpressions
{
    public class QueueExpressionVisitor : ExpressionVisitor
    {
        private readonly Queue<string> _queue = new Queue<string>();

        // 出队列
        public string Dequeue()
        {
            return _queue.Dequeue();
        }

        // 队列中元素数量
        public int QueueCount()
        {
            return _queue.Count;
        }

        // 队列是否为空
        public bool QueueIsEmpty()
        {
            return _queue.Count == 0;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }


        protected override Expression VisitBinary(BinaryExpression node)
        {
            _queue.Enqueue(node.NodeType.ToString());
            return base.VisitBinary(node);
        }


        protected override Expression VisitConstant(ConstantExpression node)
        {
            _queue.Enqueue(node.Value?.ToString());
            return base.VisitConstant(node);
        }


        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member is FieldInfo field)
            {
                var ce = (ConstantExpression)node.Expression;
                var value = field.GetValue(ce.Value).ToString();
                _queue.Enqueue(value);
            }
            else if (node.Member is PropertyInfo)
            {
                _queue.Enqueue(node.Member.Name);
            }
            return node;
        }
    }
}
