using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CommandDTO
{
    public class QueryViewModel<T>
    {
        public string Language { get; set; } = "ar";
        public SortOrder Order { get; set; }
        public ICollection<Filter> Filter { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public T Model { get; set; }
        public QueryViewModel()
        {
            Filter = new HashSet<Filter>();
        }
    }

    public class SortOrder
    {
        public string FieldName { get; set; }
        public SortTypeEnum SortType { get; set; }
    }

    public enum SortTypeEnum
    {
        ASC = 1,
        DESC = 2
    }

    public class Filter
    {
        public string FieldName { get; set; }
        public string Operation { get; set; }
        public string value { get; set; }
    }
    public static class FilterOperation
    {
        public const string Equal = "=";
        public const string NotEqual = "!=";
        public const string GT = ">";
        public const string GTE = ">=";
        public const string LT = "<";
        public const string LTE = "<=";
        public const string In = "in";
        public const string BW = "BT";
    }
}
