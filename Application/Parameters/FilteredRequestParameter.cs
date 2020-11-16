using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Filters
{
    public class FilteredRequestParameter
    {
        public static int MAX_ELEMENTS = 99999;
        public bool NoPaging { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Dictionary<string, string> FilterValue { get; set; }
        public Dictionary<string, string> FilterRange { get; set; }
        public Dictionary<string, List<string>> FilterArray { get; set; }
        public Dictionary<string, string> FilterSearch { get; set; }
        public string SortBy { get; set; }
        public string SortType { get; set; }

        public FilteredRequestParameter()
        {
            this.NoPaging = false;
            this.PageNumber = 1;
            this.PageSize = 10;
            this.FilterValue = new Dictionary<string, string>();
            this.FilterRange = new Dictionary<string, string>();
            this.FilterArray = new Dictionary<string, List<string>>();
            this.FilterSearch = new Dictionary<string, string>();
            this.SortBy = "ID";
            this.SortType = "ASC";
        }
        public FilteredRequestParameter(bool NoPaging, int pageNumber, int pageSize,
            Dictionary<string, string> FilterValue,
            Dictionary<string, string> FilterRange,
            Dictionary<string, List<string>> FilterArray,
            Dictionary<string, string> FilterSearch,
            string SortBy, string SortType)
        {

            this.NoPaging = NoPaging;
            if (this.NoPaging)
            {
               this.PageNumber = 1;
               this.PageSize = MAX_ELEMENTS;
            } else
            {
               this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
               this.PageSize = pageSize < 1 ? 10 : pageSize;
            }
            this.FilterArray = LowerCaseDict(FilterArray);
            this.FilterRange = LowerCaseDict(FilterRange);
            this.FilterValue = LowerCaseDict(FilterValue);
            this.FilterSearch = LowerCaseDict(FilterSearch);
            this.SortBy = SortBy;
            this.SortType = SortType;
        }


        public Dictionary<string, string> LowerCaseDict(Dictionary<string, string> dict)
        {
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (string key in dict.Keys)
            {
                newDict.Add(key.ToLower(), dict.GetValueOrDefault(key));
            }
            return newDict;
        }

        public Dictionary<string, List<string>> LowerCaseDict(Dictionary<string, List<string>> dict)
        {
            Dictionary<string, List<string>> newDict = new Dictionary<string, List<string>>();
            foreach (string key in dict.Keys)
            {
                newDict.Add(key.ToLower(), dict.GetValueOrDefault(key));
            }
            return newDict;
        }

}
}
