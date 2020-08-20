using System;
using System.Collections.Generic;
using System.Text;

namespace ChenYiFan.ElasticSearch.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrWhiteSpace(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }

            return false;
        }

        public static string ToEsOperator(this string binaryExprStr)
        {
            if (binaryExprStr == "GreaterThanOrEqual")
            {
                return "gte";
            }
            else if (binaryExprStr == "GreaterThan")
            {
                return "gt";
            }
            else if (binaryExprStr == "LessThanOrEqual")
            {
                return "lte";
            }
            else if (binaryExprStr == "LessThan")
            {
                return "lt";
            }
            else
            {
                return binaryExprStr;
            }
        }
    }
}
