using System;
using System.Collections.Generic;
using System.Text;
using Application.Filters;

namespace Application.Wrappers
{
    public class FilteredPagedResponse<T> : Response<T>
    {
        public bool NoPaging { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Dictionary<string, string> FilterValue { get; set; }
        public Dictionary<string, string> FilterRange { get; set; }
        public Dictionary<string, List<string>> FilterArray { get; set; }
        public Dictionary<string, string> FilterSearch { get; set; }
        public string SortBy { get; set; }
        public string SortType { get; set; }
        public int Count { get; set; }

        public FilteredPagedResponse(T data, int pageNumber, int pageSize,
            Dictionary<string, string> FilterValue,
            Dictionary<string, string> FilterRange,
            Dictionary<string, List<string>> FilterArray,
            Dictionary<string, string> FilterSearch,
            string SortBy, string SortType, bool NoPaging)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.FilterArray = lowerCaseDict(FilterArray);
            this.FilterRange = lowerCaseDict(FilterRange);
            this.FilterValue = lowerCaseDict(FilterValue);
            this.FilterSearch = lowerCaseDict(FilterSearch);
            this.data = data;
            this.message = null;
            this.succeeded = true;
            this.errors = null;
            this.SortBy = SortBy;
            this.SortType = SortType;
            this.NoPaging = NoPaging;
        }

        public FilteredPagedResponse(T data, FilteredRequestParameter filteredRequestParameter, int Count)
        {
            this.PageNumber = filteredRequestParameter.PageNumber;
            this.PageSize = filteredRequestParameter.PageSize;
            this.FilterArray = lowerCaseDict(filteredRequestParameter.FilterArray);
            this.FilterRange = lowerCaseDict(filteredRequestParameter.FilterRange);
            this.FilterValue = lowerCaseDict(filteredRequestParameter.FilterValue);
            this.FilterSearch = lowerCaseDict(filteredRequestParameter.FilterSearch);
            this.SortBy = filteredRequestParameter.SortBy;
            this.SortType = filteredRequestParameter.SortType;
            this.NoPaging = filteredRequestParameter.NoPaging;
            this.data = data;
            this.message = null;
            this.succeeded = true;
            this.errors = null;
            this.Count = Count;
        }

        public Dictionary<string, string> lowerCaseDict(Dictionary<string,string> dict)
        {
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (string key in dict.Keys)
            {
                newDict.Add(key.ToLower(), dict.GetValueOrDefault(key));
            }
            return newDict;
        }

        public Dictionary<string, List<string>> lowerCaseDict(Dictionary<string, List<string>> dict)
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
