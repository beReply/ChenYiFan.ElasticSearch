using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ChenYiFan.ElasticSearch.Extensions
{
    public static class ExpressionExtension
    {
        public static object ReadExpression(this MemberExpression nodeMember, Stack<PropertyInfo> property)
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
                    var res = me.ReadExpression(property);

                    return res;
                }
                catch (Exception)
                {
                    return nodeMember.Member.Name;
                }
            }

            return "error";
        }


        public static bool IsLogicExpression(this Expression node)
        {
            var binaryExpressionType = new List<ExpressionType>
            {
                ExpressionType.AndAlso,
                ExpressionType.OrElse
            };

            return binaryExpressionType.Contains(node.NodeType);
        }
    }
}
