#region copyright
// ------------------------------------------------------------------------
// <copyright file="Search.cs" company="Zalamtech SARL">
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
// <date>2014-9-1 18:18</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections;
using System.Collections.Generic;

namespace Com.Googlecode.Genericdao.Search
{
    [Serializable]
    public class Search : IMutableSearch
    {
        // ReSharper disable InconsistentNaming
        protected int _firstResult = -1; // -1 stands for unspecified
        protected int _maxResults = -1; // -1 stands for unspecified
        protected int _page = -1; // -1 stands for unspecified
        protected Type _searchClass;
        protected IList<Filter> _filters = new List<Filter>();
        protected bool _disjunction;
        protected IList<Sort> _sorts = new List<Sort>();
        protected IList<Field> _fields = new List<Field>();
        protected bool _distinct;
        protected IList<string> _fetches = new List<string>();
        protected int _resultMode = SearchConstants.RESULT_AUTO;
        // ReSharper enable InconsistentNaming

        #region constructors
        public Search()
        {

        }

        public Search(Type searchClass)
        {
            _searchClass = searchClass;
        }
        #endregion

        #region accessors
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

        public Type SearchClass
        {
            get { return _searchClass; }
            set { _searchClass = value; }
        }

        public IList<Filter> Filters
        {
            get { return _filters; }
            set { _filters = value; }
        }

        public bool Disjunction
        {
            get { return _disjunction; }
            set { _disjunction = value; }
        }

        public IList<Sort> Sorts
        {
            get { return _sorts; }
            set { _sorts = value; }
        }

        public IList<Field> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        public bool Distinct
        {
            get { return _distinct; }
            set { _distinct = value; }
        }

        public IList<string> Fetches
        {
            get { return _fetches; }
            set { _fetches = value; }
        }

        public int ResultMode
        {
            get { return _resultMode; }
            set { _resultMode = value; }
        }
        #endregion

        #region interface implementation
        public Search AddFilter(Filter filter)
        {
            SearchUtil.AddFilter(this, filter);
            return this;
        }

        public Search AddFilters(params Filter[] filters)
        {
            SearchUtil.AddFilters(this, filters);
            return this;
        }

        public Search AddFilterEqual(string property, object value)
        {
            SearchUtil.AddFilterEqual(this, property, value);
            return this;
        }

        public Search AddFilterGreaterOrEqual(string property, object value)
        {
            SearchUtil.AddFilterGreaterOrEqual(this, property, value);
            return this;
        }

        public Search AddFilterGreaterThan(string property, object value)
        {
            SearchUtil.AddFilterGreaterThan(this, property, value);
            return this;
        }

        public Search AddFilterIn(string property, IList value)
        {
            SearchUtil.AddFilterIn(this, property, value);
            return this;
        }

        public Search AddFilterIn(string property, params object[] value)
        {
            SearchUtil.AddFilterIn(this, property, value);
            return this;
        }

        public Search AddFilterNotIn(string property, IList value)
        {
            SearchUtil.AddFilterNotIn(this, property, value);
            return this;
        }

        public Search AddFilterNotIn(string property, params object[] value)
        {
            SearchUtil.AddFilterNotIn(this, property, value);
            return this;
        }

        public Search AddFilterLessOrEqual(string property, object value)
        {
            SearchUtil.AddFilterLessOrEqual(this, property, value);
            return this;
        }

        public Search AddFilterLessThan(string property, object value)
        {
            SearchUtil.AddFilterLessThan(this, property, value);
            return this;
        }

        public Search AddFilterLike(string property, string value)
        {
            SearchUtil.AddFilterLike(this, property, value);
            return this;
        }

        public Search AddFilterILike(string property, string value)
        {
            SearchUtil.AddFilterILike(this, property, value);
            return this;
        }

        public Search AddFilterNotEqual(string property, object value)
        {
            SearchUtil.AddFilterNotEqual(this, property, value);
            return this;
        }

        public Search AddFilterNull(string property)
        {
            SearchUtil.AddFilterNull(this, property);
            return this;
        }

        public Search AddFilterNotNull(string property)
        {
            SearchUtil.AddFilterNotNull(this, property);
            return this;
        }

        public Search AddFilterEmpty(string property)
        {
            SearchUtil.AddFilterEmpty(this, property);
            return this;
        }

        public Search AddFilterNotEmpty(string property)
        {
            SearchUtil.AddFilterNotEmpty(this, property);
            return this;
        }

        public Search AddFilterAnd(params Filter[] filters)
        {
            SearchUtil.AddFilterAnd(this, filters);
            return this;
        }

        public Search AddFilterOr(params Filter[] filters)
        {
            SearchUtil.AddFilterOr(this, filters);
            return this;
        }

        public Search AddFilterNot(Filter filter)
        {
            SearchUtil.AddFilterNot(this, filter);
            return this;
        }

        public Search AddFilterSome(string property, Filter filter)
        {
            SearchUtil.AddFilterSome(this, property, filter);
            return this;
        }

        public Search AddFilterAll(string property, Filter filter)
        {
            SearchUtil.AddFilterAll(this, property, filter);
            return this;
        }

        public Search AddFilterNone(string property, Filter filter)
        {
            SearchUtil.AddFilterNone(this, property, filter);
            return this;
        }

        public Search AddFilterCustom(string expression)
        {
            SearchUtil.AddFilterCustom(this, expression);
            return this;
        }

        public Search AddFilterCustom(string expression, params object[] values)
        {
            SearchUtil.AddFilterCustom(this, expression, values);
            return this;
        }

        public Search AddFilterCustom(string expression, IList values)
        {
            SearchUtil.AddFilterCustom(this, expression, values);
            return this;
        }

        public void RemoveFilter(Filter filter)
        {
            SearchUtil.RemoveFilter(this, filter);
        }

        public void RemoveFiltersOnProperty(string property)
        {
            SearchUtil.RemoveFiltersOnProperty(this, property);
        }

        public void ClearFilters()
        {
            SearchUtil.ClearFilters(this);
        }

        public Search AddSort(Sort sort)
        {
            SearchUtil.AddSort(this, sort);
            return this;
        }

        public Search AddSorts(params Sort[] sorts)
        {
            SearchUtil.AddSorts(this, sorts);
            return this;
        }

        /**
         * Add ascending sort by property
         */
        public Search AddSortAsc(string property)
        {
            SearchUtil.AddSortAsc(this, property);
            return this;
        }

        /**
         * Add ascending sort by property
         */
        public Search AddSortAsc(string property, bool ignoreCase)
        {
            SearchUtil.AddSortAsc(this, property, ignoreCase);
            return this;
        }

        /**
         * Add descending sort by property
         */
        public Search AddSortDesc(string property)
        {
            SearchUtil.AddSortDesc(this, property);
            return this;
        }

        /**
         * Add descending sort by property
         */
        public Search AddSortDesc(string property, bool ignoreCase)
        {
            SearchUtil.AddSortDesc(this, property, ignoreCase);
            return this;
        }

        /**
         * Add sort by property. Ascending if <code>desc == false</code>, descending
         * if <code>desc == true</code>.
         */
        public Search AddSort(string property, bool desc)
        {
            SearchUtil.AddSort(this, property, desc);
            return this;
        }

        /**
         * Add sort by property. Ascending if <code>desc == false</code>, descending
         * if <code>desc == true</code>.
         */
        public Search AddSort(string property, bool desc, bool ignoreCase)
        {
            SearchUtil.AddSort(this, property, desc, ignoreCase);
            return this;
        }

        public void RemoveSort(Sort sort)
        {
            SearchUtil.RemoveSort(this, sort);
        }

        public void RemoveSort(string property)
        {
            SearchUtil.RemoveSort(this, property);
        }

        public void ClearSorts()
        {
            SearchUtil.ClearSorts(this);
        }

        // Fields
        public Search AddField(Field field)
        {
            SearchUtil.AddField(this, field);
            return this;
        }

        public Search AddFields(params Field[] fields)
        {
            SearchUtil.AddFields(this, fields);
            return this;
        }

        /**
         * If this field is used with <code>resultMode == RESULT_MAP</code>, the
         * <code>property</code> will also be used as the key for this value in the
         * map.
         */
        public Search AddField(string property)
        {
            SearchUtil.AddField(this, property);
            return this;
        }

        /**
         * If this field is used with <code>resultMode == RESULT_MAP</code>, the
         * <code>key</code> will be used as the key for this value in the map.
         */
        public Search AddField(string property, string key)
        {
            SearchUtil.AddField(this, property, key);
            return this;
        }

        /**
         * If this field is used with <code>resultMode == RESULT_MAP</code>, the
         * <code>property</code> will also be used as the key for this value in the
         * map.
         */
        public Search AddField(string property, int theOperator)
        {
            SearchUtil.AddField(this, property, theOperator);
            return this;
        }

        /**
         * If this field is used with <code>resultMode == RESULT_MAP</code>, the
         * <code>key</code> will be used as the key for this value in the map.
         */
        public Search AddField(string property, int theOperator, string key)
        {
            SearchUtil.AddField(this, property, theOperator, key);
            return this;
        }

        public void RemoveField(Field field)
        {
            SearchUtil.RemoveField(this, field);
        }

        public void RemoveField(string property)
        {
            SearchUtil.RemoveField(this, property);
        }

        public void RemoveField(string property, string key)
        {
            SearchUtil.RemoveField(this, property, key);
        }

        public void ClearFields()
        {
            SearchUtil.ClearFields(this);
        }

        // Fetches
        public Search AddFetch(string property)
        {
            SearchUtil.AddFetch(this, property);
            return this;
        }

        public Search AddFetches(params string[] properties)
        {
            SearchUtil.AddFetches(this, properties);
            return this;
        }

        public void RemoveFetch(String property)
        {
            SearchUtil.RemoveFetch(this, property);
        }

        public void ClearFetches()
        {
            SearchUtil.ClearFetches(this);
        }

        public void Clear()
        {
            SearchUtil.Clear(this);
        }

        public void ClearPaging()
        {
            SearchUtil.ClearPaging(this);
        }

        public Search Copy()
        {
            var dest = new Search();
            SearchUtil.Copy(this, dest);
            return dest;
        }

        public override bool Equals(Object obj)
        {
            return SearchUtil.Equals(this, obj);
        }

        public override int GetHashCode()
        {
            return SearchUtil.GetHashCode(this);
        }

        public override string ToString()
        {
            return SearchUtil.ToString(this);
        }

        public int GetFirstResult()
        {
            return _firstResult;
        }

        public int GetMaxResults()
        {
            return _maxResults;
        }

        public int GetPage()
        {
            return _page;
        }

        public Type GetSearchClass()
        {
            return _searchClass;
        }

        public IList<Filter> GetFilters()
        {
            return _filters;
        }

        public bool IsDisjunction()
        {
            return _disjunction;
        }

        public IList<Sort> GetSorts()
        {
            return _sorts;
        }

        public IList<Field> GetFields()
        {
            return _fields;
        }

        public bool IsDistinct()
        {
            return _distinct;
        }

        public IList<string> GetFetches()
        {
            return _fetches;
        }

        public int GetResultMode()
        {
            return _resultMode;
        }

        public IMutableSearch SetFirstResult(int firstResult)
        {
            _firstResult = firstResult;
            return this;
        }

        public IMutableSearch SetMaxResults(int maxResults)
        {
            _maxResults = maxResults;
            return this;
        }

        public IMutableSearch SetPage(int page)
        {
            _page = page;
            return this;
        }

        public IMutableSearch SetSearchClass(Type searchClass)
        {
            _searchClass = searchClass;
            return this;
        }

        public IMutableSearch SetFilters(IList<Filter> filters)
        {
            _filters = filters;
            return this;
        }

        public IMutableSearch SetDisjunction(bool disjunction)
        {
            _disjunction = disjunction;
            return this;
        }

        public IMutableSearch SetSorts(IList<Sort> sorts)
        {
            _sorts = sorts;
            return this;
        }

        public IMutableSearch SetFields(IList<Field> fields)
        {
            _fields = fields;
            return this;
        }

        public IMutableSearch SetDistinct(bool distinct)
        {
            _distinct = distinct;
            return this;
        }

        public IMutableSearch SetFetches(IList<string> fetches)
        {
            _fetches = fetches;
            return this;
        }

        public IMutableSearch SetResultMode(int resultMode)
        {
            if (resultMode < 0 || resultMode > 4)
                throw new ArgumentException("Result Mode ( " + resultMode + " ) is not a valid option.");
            _resultMode = resultMode;
            return this;
        }
        #endregion
    }
}