#region copyright
// ------------------------------------------------------------------------
// <copyright file="FlexSearch.cs" company="Zalamtech SARL">
//	Copyright 2014.
// </copyright>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// <author>Abdoul DIALLO</author>
// <date>2014-8-28 23:21</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.Googlecode.Genericdao.Search.Flex
{
    [Serializable]
    public class FlexSearch
    {
        protected int _firstResult = -1; // -1 stands for unspecified

        protected int _maxResults = -1; // -1 stands for unspecified

        protected int _page = -1; // -1 stands for unspecified

        protected string _searchClassName;

        protected IList<Filter> _filters = new List<Filter>();

        protected bool _disjunction;

        protected IList<Sort> _sorts = new List<Sort>();

        protected IList<Field> _fields = new List<Field>();

        protected bool _distinct;

        protected IList<String> _fetches = new List<String>();

        protected int _resultMode = SearchConstants.RESULT_AUTO;

        public string SearchClassName
        {
            get { return _searchClassName; }
            set { _searchClassName = value; }
        }

        public Filter[] GetFilters()
        {
            return _filters as Filter[];
        }

        public void SetFilters(Filter[] filters)
        {
            _filters.Clear();
            if (filters == null) return;
            foreach (var t in filters.Where(t => t != null))
            {
                _filters.Add(t);
            }
        }

        public Sort[] GetSorts()
        {
            return _sorts as Sort[];
        }

        public void SetSorts(Sort[] sorts)
        {
            _sorts.Clear();

            if (sorts == null) return;

            foreach (var t in sorts.Where(t => t != null))
            {
                _sorts.Add(t);
            }
        }

        public Field[] GetFields()
        {
            return _fields as Field[];
        }

        public void SetFields(Field[] fields)
        {
            _fields.Clear();
            if (fields == null) return;

            foreach (var f in fields.Where(f => f != null && f.Property != null && f.Property.Length > 0))
            {
                if (f.Key == null)
                    f.Key = f.Property;
                _fields.Add(f);
            }
        }

        public string[] GetFetches()
        {
            return _fetches as string[];
        }

        public void SetFetches(string[] fetches)
        {
            _fetches.Clear();
            if (fetches == null) return;
            foreach (var t in fetches.Where(t => !string.IsNullOrEmpty(t)))
            {
                _fetches.Add(t);
            }
        }

        public int FirstResult
        {
            get { return _firstResult; }
            set { _firstResult = value; }
        }

        public int MaxResults
        {
            get { return _maxResults; }
            set { _maxResults = value; }
        }

        public int Page
        {
            get { return _page; }
            set { _page = value; }
        }

        public bool Disjunction
        {
            get { return _disjunction; }
            set { _disjunction = value; }
        }

        public bool Distinct
        {
            get { return _distinct; }
            set { _distinct = value; }
        }

        public int ResultMode
        {
            get { return _resultMode; }
            set { _resultMode = value; }
        }
    }
}