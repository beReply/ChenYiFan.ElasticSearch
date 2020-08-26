using ChenYiFan.ElasticSearch.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ChenYiFan.ElasticSearch.Tools.QueryExpressions.ExpressionVisitors
{
    public class LogicExpressionVisitor : ExpressionVisitor
    {
        private readonly Queue<Expression> _queue = new Queue<Expression>();

        // 出队列
        public Expression Dequeue()
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

        // 入口函数
        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }


        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.IsLogicExpression())
            {
                _queue.Enqueue(node);
                if (!node.Right.IsLogicExpression()) {_queue.Enqueue(node.Right);}
                if (!node.Left.IsLogicExpression()) {_queue.Enqueue(node.Left); }
            }

            return base.VisitBinary(node);
        }

    }
}
