#region copyright
// ------------------------------------------------------------------------
// <copyright file="SearchUtil.cs" company="Zalamtech SARL">
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Googlecode.Genericdao.Search
{
    public static class SearchUtil
    {
        public static void AddFetch(IMutableSearch search, string property)
        {
            if (string.IsNullOrEmpty(property))
                return; // null properties do nothing, don't bother to add them.

            var fetches = search.GetFetches();
            if (fetches == null)
            {
                fetches = new List<string>();
                search.SetFetches(fetches);
            }
            fetches.Add(property);
        }

        public static void AddFetches(IMutableSearch search, params string[] properties)
        {
            if (properties == null) return;
            foreach (var property in properties)
            {
                AddFetch(search, property);
            }
        }

        public static void AddField(IMutableSearch search, Field field)
        {
            var fields = search.GetFields();
            if (fields == null)
            {
                fields = new List<Field>();
                search.SetFields(fields);
            }
            fields.Add(field);
        }

        public static void AddFields(IMutableSearch search, params Field[] fields)
        {
            if (fields == null) return;

            foreach (var field in fields)
            {
                AddField(search, field);
            }
        }

        public static void AddField(IMutableSearch search, string property)
        {
            if (property == null)
            {
                return; // null properties do nothing, don't bother to add them.
            }

            AddField(search, new Field(property));
        }

        public static void AddField(IMutableSearch search, string property, int theOperator)
        {
            if (property == null)
                return; // null properties do nothing, don't bother to add them.
            AddField(search, new Field(property, theOperator));
        }

        public static void AddField(IMutableSearch search, string property, int theOperator, string key)
        {
            if (property == null || key == null)
                return; // null properties do nothing, don't bother to add them.
            AddField(search, new Field(property, theOperator, key));
        }

        public static void AddField(IMutableSearch search, string property, string key)
        {
            if (property == null || key == null)
                return; // null properties do nothing, don't bother to add them.
            AddField(search, new Field(property, key));
        }

        public static void AddFilter(IMutableSearch search, Filter filter)
        {
            var filters = search.GetFilters();
            if (filters == null)
            {
                filters = new List<Filter>();
                search.SetFilters(filters);
            }
            filters.Add(filter);
        }

        public static void AddFilters(IMutableSearch search, params Filter[] filters)
        {
            if (filters == null) return;
            foreach (var filter in filters)
            {
                AddFilter(search, filter);
            }
        }

        public static void AddFilterAll(IMutableSearch search, string property, Filter filter)
        {
            AddFilter(search, Filter.All(property, filter));
        }

        public static void AddFilterAnd(IMutableSearch search, params Filter[] filters)
        {
            AddFilter(search, Filter.And(filters));
        }

        /**
         * Add a filter that uses the IS EMPTY operator.
         */
        public static void AddFilterEmpty(IMutableSearch search, string property)
        {
            AddFilter(search, Filter.IsEmpty(property));
        }

        /**
         * Add a filter that uses the == operator.
         */
        public static void AddFilterEqual(IMutableSearch search, string property, object value)
        {
            AddFilter(search, Filter.Equal(property, value));
        }

        /**
         * Add a filter that uses the >= operator.
         */
        public static void AddFilterGreaterOrEqual(IMutableSearch search, string property, object value)
        {
            AddFilter(search, Filter.GreaterOrEqual(property, value));
        }

        /**
         * Add a filter that uses the > operator.
         */
        public static void AddFilterGreaterThan(IMutableSearch search, string property, object value)
        {
            AddFilter(search, Filter.GreaterThan(property, value));
        }

        /**
         * Add a filter that uses the ILIKE operator.
         */
        public static void AddFilterILike(IMutableSearch search, string property, string value)
        {
            AddFilter(search, Filter.Ilike(property, value));
        }

        /**
         * Add a filter that uses the IN operator.
         */
        public static void AddFilterIn(IMutableSearch search, string property, IList value)
        {
            AddFilter(search, Filter.In(property, value));
        }

        /**
         * Add a filter that uses the IN operator.
         * 
         * <p>
         * This takes a variable number of parameters. Any number of values can be
         * specified.
         */
        public static void AddFilterIn(IMutableSearch search, string property, params object[] value)
        {
            AddFilter(search, Filter.In(property, value));
        }

        /**
	     * Add a filter that uses the <= operator.
	     */
	    public static void AddFilterLessOrEqual(IMutableSearch search, string property, object value) {
		    AddFilter(search, Filter.LessOrEqual(property, value));
	    }

	    /**
	     * Add a filter that uses the < operator.
	     */
	    public static void AddFilterLessThan(IMutableSearch search, string property, object value) {
		    AddFilter(search, Filter.LessThan(property, value));
	    }

	    /**
	     * Add a filter that uses the LIKE operator.
	     */
	    public static void AddFilterLike(IMutableSearch search, string property, string value) {
		    AddFilter(search, Filter.Like(property, value));
	    }

	    /**
	     * Add a filter that uses the NONE operator.
	     */
	    public static void AddFilterNone(IMutableSearch search, string property, Filter filter) {
		    AddFilter(search, Filter.None(property, filter));
	    }

	    /**
	     * Add a filter that uses the NOT operator.
	     */
	    public static void AddFilterNot(IMutableSearch search, Filter filter) {
		    AddFilter(search, Filter.Not(filter));
	    }

	    /**
	     * Add a filter that uses the != operator.
	     */
	    public static void AddFilterNotEqual(IMutableSearch search, string property, object value) {
		    AddFilter(search, Filter.NotEqual(property, value));
	    }

	    /**
	     * Add a filter that uses the NOT IN operator.
	     */
	    public static void AddFilterNotIn(IMutableSearch search, string property, IList value) {
		    AddFilter(search, Filter.NotIn(property, value));
	    }

	    /**
	     * Add a filter that uses the NOT IN operator.
	     * 
	     * <p>
	     * This takes a variable number of parameters. Any number of values can be
	     * specified.
	     */
	    public static void AddFilterNotIn(IMutableSearch search, string property, params object[] value) {
		    AddFilter(search, Filter.NotIn(property, value));
	    }

	    /**
	     * Add a filter that uses the IS NOT EMPTY operator.
	     */
	    public static void AddFilterNotEmpty(IMutableSearch search, string property) {
		    AddFilter(search, Filter.IsNotEmpty(property));
	    }

	    /**
	     * Add a filter that uses the IS NOT NULL operator.
	     */
	    public static void AddFilterNotNull(IMutableSearch search, string property) {
		    AddFilter(search, Filter.IsNotNull(property));
	    }

	    /**
	     * Add a filter that uses the IS NULL operator.
	     */
	    public static void AddFilterNull(IMutableSearch search, string property) {
		    AddFilter(search, Filter.IsNull(property));
	    }

	    /**
	     * Add a filter that uses the OR operator.
	     * 
	     * <p>
	     * This takes a variable number of parameters. Any number of <code>Filter
	     * </code>s can be
	     * specified.
	     */
	    public static void AddFilterOr(IMutableSearch search, params Filter[] filters) {
		    AddFilter(search, Filter.Or(filters));
	    }

	    /**
	     * Add a filter that uses the SOME operator.
	     */
	    public static void AddFilterSome(IMutableSearch search, string property, Filter filter) {
		    AddFilter(search, Filter.Some(property, filter));
	    }
	
	    /**
	     * Add a filter that uses a custom expression.
	     * 
	     * @see {@link Filter#custom(string)}
	     */
	    public static void AddFilterCustom(IMutableSearch search, string expression) {
		    AddFilter(search, Filter.Custom(expression));
	    }

	    /**
	     * Add a filter that uses a custom expression.
	     * 
	     * @see {@link Filter#custom(string, object...)}
	     */
	    public static void AddFilterCustom(IMutableSearch search, string expression, params object[] values) {
		    AddFilter(search, Filter.Custom(expression, values));
	    }
	
	    /**
	     * Add a filter that uses a custom expression.
	     * 
	     * @see {@link Filter#custom(string, Collection)}
	     */
	    public static void AddFilterCustom(IMutableSearch search, string expression, IList values) {
		    AddFilter(search, Filter.Custom(expression, values));
	    }

	    // Sorts
	    public static void AddSort(IMutableSearch search, Sort sort) {
		    if (sort == null)
			    return;

		    var sorts = search.GetSorts();
		    if (sorts == null) {
			    sorts = new List<Sort>();
			    search.SetSorts(sorts);
		    }
		    sorts.Add(sort);
	    }
	
	    public static void AddSorts(IMutableSearch search, params Sort[] sorts)
	    {
	        if (sorts == null) return;

	        foreach (var sort in sorts) {
	            AddSort(search, sort);
	        }
	    }

        /**
	     * Add sort by property. Ascending if <code>desc == false</code>, descending
	     * if <code>desc == true</code>.
	     */
	    public static void AddSort(IMutableSearch search, string property, bool desc) {
		    AddSort(search, property, desc, false);
	    }

	    /**
	     * Add sort by property. Ascending if <code>desc == false</code>, descending
	     * if <code>desc == true</code>.
	     */
	    public static void AddSort(IMutableSearch search, string property, bool desc, bool ignoreCase) {
		    if (property == null) return; // null properties do nothing, don't bother to add them.

		    AddSort(search, new Sort(property, desc, ignoreCase));
	    }

	    /**
	     * Add ascending sort by property
	     */
	    public static void AddSortAsc(IMutableSearch search, string property) {
		    AddSort(search, property, false, false);
	    }

	    /**
	     * Add ascending sort by property
	     */
	    public static void AddSortAsc(IMutableSearch search, string property, bool ignoreCase) {
		    AddSort(search, property, false, ignoreCase);
	    }

	    /**
	     * Add descending sort by property
	     */
	    public static void AddSortDesc(IMutableSearch search, string property) {
		    AddSort(search, property, true, false);
	    }

	    /**
	     * Add descending sort by property
	     */
	    public static void AddSortDesc(IMutableSearch search, string property, bool ignoreCase) {
		    AddSort(search, property, true, ignoreCase);
	    }

	    // ---------- Remove ----------

	    public static void RemoveFetch(IMutableSearch search, string property) {
		    if (search.GetFetches() != null)
			    search.GetFetches().Remove(property);
	    }

	    public static void RemoveField(IMutableSearch search, Field field) {
		    if (search.GetFields() != null)
			    search.GetFields().Remove(field);
	    }

	    public static void RemoveField(IMutableSearch search, string property) {
		    if (search.GetFields() == null)
			    return;

	        var fields = search.GetFields().Where(field => !field.Property.Equals(property)).ToList();
            
	        search.SetFields(fields);
	    }

	    public static void RemoveField(IMutableSearch search, string property, string key) {
		    if (search.GetFields() == null)
			    return;

            var fields = search.GetFields().Where(field => !field.Property.Equals(property) || !field.Key.Equals(key)).ToList();

	        search.SetFields(fields);
	    }

	    public static void RemoveFilter(IMutableSearch search, Filter filter) {
		    var filters = search.GetFilters();
		    if (filters != null)
            {
			    filters.Remove(filter);
            }
	    }

	    /**
	     * Remove all filters on the given property.
	     */
	    public static void RemoveFiltersOnProperty(IMutableSearch search, string property) {
		    if (property == null || search.GetFilters() == null)
			    return;

	        var filters = search.GetFilters().Where(filter => !property.Equals(filter.Property)).ToList();

	        search.SetFilters(filters);
	    }

	    public static void RemoveSort(IMutableSearch search, Sort sort) {
		    if (search.GetSorts() != null)
			    search.GetSorts().Remove(sort);
	    }

	    public static void RemoveSort(IMutableSearch search, string property) {
		    if (property == null || search.GetSorts() == null)
			    return;

            var sorts = search.GetSorts().Where(sort => !property.Equals(sort.Property)).ToList();

            search.SetSorts(sorts);
	    }

        // ---------- Clear ----------

        public static void Clear(IMutableSearch search)
        {
            ClearFilters(search);
            ClearSorts(search);
            ClearFields(search);
            ClearPaging(search);
            ClearFetches(search);
            search.SetResultMode(SearchConstants.RESULT_AUTO);
            search.SetDisjunction(false);
        }

        public static void ClearFetches(IMutableSearch search)
        {
            if (search.GetFetches() != null)
                search.GetFetches().Clear();
        }

        public static void ClearFields(IMutableSearch search)
        {
            if (search.GetFields() != null)
                search.GetFields().Clear();
        }

        public static void ClearFilters(IMutableSearch search)
        {
            if (search.GetFilters() != null)
                search.GetFilters().Clear();
        }

        public static void ClearPaging(IMutableSearch search)
        {
            search.SetFirstResult(-1);
            search.SetPage(-1);
            search.SetMaxResults(-1);
        }

        public static void ClearSorts(IMutableSearch search)
        {
            if (search.GetSorts() != null)
                search.GetSorts().Clear();
        }

        // ---------- Merge ----------
	    /**
	     * Modify the search by adding the given sorts before the current sorts in
	     * the search.
	     */
	    public static void MergeSortsBefore(IMutableSearch search, IList<Sort> sorts) {
		    var list = search.GetSorts();
		    if (list == null) {
			    list = new List<Sort>();
			    search.SetSorts(list);
		    }

	        var newList = new List<Sort>();

		    if (list.Any()) {
		        newList.AddRange(
                    from sort in list 
                    where sort.Property != null 
                    let found = sorts.Any(sort1 => sort.Property.Equals(sort1.Property)) 
                    where !found select sort
               );
		    }

	        newList.AddRange(sorts);

	        search.SetSorts(newList);
	    }

	    /**
	     * Modify the search by adding the given sorts before the current sorts in
	     * the search.
	     */
	    public static void MergeSortsBefore(IMutableSearch search, params Sort[] sorts) {
		    MergeSortsBefore(search, sorts.ToList());
	    }

	    /**
	     * Modify the search by adding the given sorts after the current sorts in
	     * the search.
	     */
	    public static void MergeSortsAfter(IMutableSearch search, IList<Sort> sorts) {
		    var list = search.GetSorts();
		    if (list == null) {
			    list = new List<Sort>();
			    search.SetSorts(list);
		    }

	        var newList = new List<Sort>();

		    if (list.Any()) {
		        newList.AddRange(
                    from sort in list 
                    where sort.Property != null 
                    let found = sorts.Any(sort1 => sort.Property.Equals(sort1.Property)) 
                    where !found select sort
               );
		    }

	        newList.AddRange(sorts);

	        search.SetSorts(newList);
	    }

	    /**
	     * Modify the search by adding the given sorts after the current sorts in
	     * the search.
	     */
	    public static void MergeSortsAfter(IMutableSearch search, params Sort[] sorts)
        {
		    MergeSortsAfter(search, sorts.ToList());
	    }

        /**
	     * Modify the search by adding the given fetches to the current fetches in
	     * the search.
	     */
	    public static void MergeFetches(IMutableSearch search, IList<string> fetches) {
		    var list = search.GetFetches();
		    if (list == null) {
			    list = new List<string>();
			    search.SetFetches(list);
		    }

		    foreach (var fetch in fetches.Where(fetch => !list.Contains(fetch)))
		    {
		        list.Add(fetch);
		    }
	    }

	    /**
	     * Modify the search by adding the given fetches to the current fetches in
	     * the search.
	     */
	    public static void MergeFetches(IMutableSearch search, params string[] fetches) {
		    MergeFetches(search, fetches.ToList());
	    }

	    /**
	     * Modify the search by adding the given filters using AND semantics
	     */
	    public static void MergeFiltersAnd(IMutableSearch search, IList<Filter> filters) {
		    var list = search.GetFilters();
		    if (list == null) {
			    list = new List<Filter>();
			    search.SetFilters(list);
		    }

		    if (list.Count == 0 || !search.IsDisjunction()) {
			    search.SetDisjunction(false);
		        foreach (var filter in filters)
		        {
		            list.Add(filter);
		        }
		    } else {
			    search.SetFilters(new List<Filter>());

			    // add the previous filters with an OR
			    var orFilter = Filter.Or();
			    orFilter.Value = list;
			    AddFilter(search, orFilter);

			    // add the new filters with AND
			    search.SetDisjunction(false);
		        foreach (var f in filters)
		        {
		            search.GetFilters().Add(f);
		        }
		    }
	    }

	    /**
	     * Modify the search by adding the given filters using AND semantics
	     */
	    public static void MergeFiltersAnd(IMutableSearch search, params Filter[] filters) {
		    MergeFiltersAnd(search, filters.ToList());
	    }

	    /**
	     * Modify the search by adding the given filters using OR semantics
	     */
	    public static void MergeFiltersOr(IMutableSearch search, IList<Filter> filters) {
		    var list = search.GetFilters();
		    if (list == null) {
			    list = new List<Filter>();
			    search.SetFilters(list);
		    }

		    if (list.Count == 0 || search.IsDisjunction()) {
			    search.SetDisjunction(true);
			    foreach (var filter in filters)
		        {
		            list.Add(filter);
		        }
		    } else {
			    search.SetFilters(new List<Filter>());

			    // add the previous filters with an AND
			    var orFilter = Filter.And();
			    orFilter.Value = list;
			    AddFilter(search, orFilter);

			    // add the new filters with or
			    search.SetDisjunction(true);
			    foreach (var f in filters)
		        {
		            search.GetFilters().Add(f);
		        }
		    }
	    }

	    /**
	     * Modify the search by adding the given filters using OR semantics
	     */
	    public static void MergeFiltersOr(IMutableSearch search, params Filter[] filters) {
		    MergeFiltersOr(search, filters.ToList());
	    }

	    /**
	     * Modify the search by adding the given fields before the current fields in
	     * the search.
	     */
	    public static void MergeFieldsBefore(IMutableSearch search, IList<Field> fields) {
		    var list = search.GetFields();
		    if (list == null) {
			    list = new List<Field>();
			    search.SetFields(list);
		    }

            foreach (var f in fields)
		    {
		        list.Add(f);
		    }
	    }

	    /**
	     * Modify the search by adding the given fields before the current fields in
	     * the search.
	     */
	    public static void MergeFieldsBefore(IMutableSearch search, params Field[] fields) {
		    MergeFieldsBefore(search, fields.ToList());
	    }

	    /**
	     * Modify the search by adding the given fields after the current fields in
	     * the search.
	     */
	    public static void MergeFieldsAfter(IMutableSearch search, IList<Field> fields) {
		    var list = search.GetFields();
		    if (list == null) {
			    list = new List<Field>();
			    search.SetFields(list);
		    }

		    foreach (var f in fields)
		    {
		        list.Add(f);
		    }
	    }

	    /**
	     * Modify the search by adding the given fields after the current fields in
	     * the search.
	     */
	    public static void MergeFieldsAfter(IMutableSearch search, params Field[] fields) {
		    MergeFieldsAfter(search, fields.ToList());
	    }

	    // ---------- Other Methods ----------

	    /**
	     * Calculate the first result to use given the <code>firstResult</code>,
	     * <code>page</code> and <code>maxResults</code> values of the search
	     * object.
	     * 
	     * <p>
	     * The calculation is as follows:
	     * <ul>
	     * <li>If <code>firstResult</code> is defined (i.e. > 0), use it.
	     * <li>Otherwise if <code>page</code> and <code>maxResults</code> are
	     * defined (i.e. > 0), use <code>page * maxResults</code>.
	     * <li>Otherwise, just use 0.
	     * </ul>
	     */
	    public static int CalcFirstResult(ISearch search) {
		    return (search.GetFirstResult() > 0) ? search.GetFirstResult() : (search.GetPage() > 0 && search
				    .GetMaxResults() > 0) ? search.GetPage() * search.GetMaxResults() : 0;
	    }

	    /**
	     * Copy the contents of the source search object to the destination search
	     * object, overriding any contents previously found in the destination. All
	     * destination properties reference the same objects from the source
	     * properties.
	     */
	    public static IMutableSearch ShallowCopy(ISearch source, IMutableSearch destination) {
		    destination.SetSearchClass(source.GetSearchClass());
		    destination.SetDistinct(source.IsDistinct());
		    destination.SetDisjunction(source.IsDisjunction());
		    destination.SetResultMode(source.GetResultMode());
		    destination.SetFirstResult(source.GetFirstResult());
		    destination.SetPage(source.GetPage());
		    destination.SetMaxResults(source.GetMaxResults());
		    destination.SetFetches(source.GetFetches());
		    destination.SetFields(source.GetFields());
		    destination.SetFilters(source.GetFilters());
		    destination.SetSorts(source.GetSorts());

		    return destination;
	    }

	    /**
	     * Copy the contents of the source search object to the destination search
	     * object, overriding any contents previously found in the destination. All
	     * destination properties reference the same objects from the source
	     * properties.
	     */
	    public static IMutableSearch ShallowCopy(ISearch source) {
		    return ShallowCopy(source, new Search());
	    }

	    /**
	     * Copy the contents of the source search object to the destination search
	     * object, overriding any contents previously found in the destination. All
	     * collections are copied into new collections, but the items in those
	     * collections are not duplicated; they still point to the same objects.
	     */
	    public static T Copy<T>(ISearch source, T destination) where T : IMutableSearch
        {
		    ShallowCopy(source, destination);

		    var fetches = new List<string>();
		    fetches.AddRange(source.GetFetches());
		    destination.SetFetches(fetches);

		    var fields = new List<Field>();
		    fields.AddRange(source.GetFields());
		    destination.SetFields(fields);

		    var filters = new List<Filter>();
		    filters.AddRange(source.GetFilters());
		    destination.SetFilters(filters);

		    var sorts = new List<Sort>();
		    sorts.AddRange(source.GetSorts());
		    destination.SetSorts(sorts);

		    return destination;
	    }

	    /**
	     * Copy the contents of the source search object into a new search object.
	     * All collections are copied into new collections, but the items in those
	     * collections are not duplicated; they still point to the same objects.
	     */
	    public static IMutableSearch Copy(ISearch source) {
		    return Copy(source, new Search());
	    }

        /**
	     * Return true if the search objects have equivalent contents.
	     */
	    public static bool Equals(ISearch search, object obj) {
		    if (search == obj)
			    return true;
		    if (!(obj is ISearch))
			    return false;
		    var s = (ISearch) obj;

		    if (search.GetSearchClass() == null ? s.GetSearchClass() != null : !(search.GetSearchClass() == s.GetSearchClass()))
			    return false;
		    if (search.IsDisjunction() != s.IsDisjunction() || search.GetResultMode() != s.GetResultMode()
				    || search.GetFirstResult() != s.GetFirstResult() || search.GetPage() != s.GetPage()
				    || search.GetMaxResults() != s.GetMaxResults())
			    return false;

		    if (search.GetFetches() == null ? s.GetFetches() != null : !search.GetFetches().Equals(s.GetFetches()))
			    return false;
		    
            if (search.GetFields() == null ? s.GetFields() != null : !search.GetFields().Equals(s.GetFields()))
			    return false;
		    
            if (search.GetFilters() == null ? s.GetFilters() != null : !search.GetFilters().Equals(s.GetFilters()))
			    return false;
		    
            return search.GetSorts() == null ? s.GetSorts() == null : search.GetSorts().Equals(s.GetSorts());
	    }

	    /**
	     * Return a hash code value for the given search.
	     */
	    public static int GetHashCode(ISearch search) {
		    var hash = 1;
		    hash = hash * 31 + (search.GetSearchClass() == null ? 0 : search.GetSearchClass().GetHashCode());
		    hash = hash * 31 + (search.GetFields() == null ? 0 : search.GetFields().GetHashCode());
		    hash = hash * 31 + (search.GetFilters() == null ? 0 : search.GetFilters().GetHashCode());
		    hash = hash * 31 + (search.GetSorts() == null ? 0 : search.GetSorts().GetHashCode());
		    hash = hash * 31 + (search.IsDisjunction() ? 1 : 0);
		    hash = hash * 31 + search.GetResultMode().GetHashCode();
		    hash = hash * 31 + search.GetFirstResult().GetHashCode();
		    hash = hash * 31 + search.GetPage().GetHashCode();
		    hash = hash * 31 + search.GetMaxResults().GetHashCode();

		    return hash;
	    }

	    /**
	     * Return a human-readable string describing the contents of the given
	     * search.
	     */
	    public static string ToString(ISearch search) {
		    var sb = new StringBuilder("Search(");   
		    sb.Append(search.GetSearchClass());
		    sb.Append(")[first: ").Append(search.GetFirstResult());
		    sb.Append(", page: ").Append(search.GetPage());
		    sb.Append(", max: ").Append(search.GetMaxResults());
		    sb.Append("] {\n resultMode: ");

		    switch (search.GetResultMode()) {
		    case SearchConstants.RESULT_AUTO:
			    sb.Append("AUTO");
			    break;
		    case SearchConstants.RESULT_ARRAY:
			    sb.Append("ARRAY");
			    break;
		    case SearchConstants.RESULT_LIST:
			    sb.Append("LIST");
			    break;
		    case SearchConstants.RESULT_MAP:
			    sb.Append("MAP");
			    break;
		    case SearchConstants.RESULT_SINGLE:
			    sb.Append("SINGLE");
			    break;
		    default:
			    sb.Append("**INVALID RESULT MODE: (" + search.GetResultMode() + ")**");
			    break;
		    }

		    sb.Append(",\n disjunction: ").Append(search.IsDisjunction());
		    sb.Append(",\n fields: { ");
		    AppendList(sb, search.GetFields(), ", ");
		    sb.Append(" },\n filters: {\n  ");
		    AppendList(sb, search.GetFilters(), ",\n  ");
		    sb.Append("\n },\n sorts: { ");
		    AppendList(sb, search.GetSorts(), ", ");
		    sb.Append(" }\n}");

		    return sb.ToString();
	    }

	    private static void AppendList(StringBuilder sb, IEnumerable list, string separator) {
		    if (list == null) {
			    sb.Append("null");
			    return;
		    }

		    var first = true;
		    foreach (var o in list) {
			    if (first) {
				    first = false;
			    } else {
				    sb.Append(separator);
			    }
			    sb.Append(o);
		    }
	    }

        /**
	     * Visit each non-null item is a list. Each item may be replaced by the
	     * visitor. The modified list is returned. If removeNulls is true, any null
	     * elements will be removed from the final list.
	     * 
	     * <p>
	     * If there are any modifications to be made to the list a new list is made
	     * with the changes so that the original list remains unchanged. If no
	     * changes are made, the original list is returned.
	     */
	    public static IList<T> WalkList<T>(IList<T> list, ItemVisitor<T> visitor, bool removeNulls) {
		    if (list == null)
			    return null;

		    List<T> copy = null;

		    var i = 0;
		    foreach (var item in list) {
			    var result = visitor.Visit(item);
                if(result == null)
                {
                    if (copy != null) 
                        copy.Insert(i, result);
                }
                else if (!result.Equals(item) || (removeNulls && result == null)) 
                {
				    if (copy == null) {
					    copy = new List<T>(list.Count);
					    copy.AddRange(list);
				    }
				    copy.Insert(i, result);
				    //item = result;
			    }
			    i++;
		    }

	        if (copy == null) return list;

	        if (!removeNulls) return copy;

	        for (var j = copy.Count - 1; j >= 0; j--) 
            {
	            if (copy.ElementAt(j) == null)
	                copy.RemoveAt(j);
	        }

	        return copy;
	    }

	    

	    /**
	     * Walk through a list of filters and all the sub filters, visiting each
	     * filter in the tree. A FilterVisitor is used to visit each filter. The
	     * FilterVisitor may replace the Filter that is is visiting. If it does, a
	     * new tree and list of Filters will be created for every part of the tree
	     * that is affected, thus preserving the original tree.
	     * 
	     * @return if any changes have been made, the new list of Filters; if not,
	     *         the original list.
	     */
	    public static IList<Filter> WalkFilters(IList<Filter> filters, FilterVisitor visitor, bool removeNulls) {
		    return WalkList(filters, new FilterListVisitor(visitor, removeNulls), removeNulls);
	    }

	    /**
	     * Walk a filter and all its sub filters, visiting each filter in the tree.
	     * A FilterVisitor is used to visit each filter. The FilterVisitor may
	     * replace the Filter that is is visiting. If it does, a new tree and will
	     * be created for every part of the tree that is affected, thus preserving
	     * the original tree.
	     * 
	     * @return if any changes have been made, the new Filter; if not, the
	     *         original Filter.
	     */
	    public static Filter WalkFilter(Filter filter, FilterVisitor visitor, bool removeNulls) {
		    filter = visitor.VisitBefore(filter);

		    if (filter != null) {
			    if (filter.IsTakesSingleSubFilter()) {
				    if (filter.Value is Filter) {
					    var result = WalkFilter((Filter) filter.Value, visitor, removeNulls);
					    if (!result.Equals(filter.Value)) {
						    filter = new Filter(filter.Property, result, filter.Operator);
					    }
				    }
			    } else if (filter.IsTakesListOfSubFilters()) {
				    if (filter.Value is ICollection)
				    {
				        var filts = filter.Value as IList;
                        IList<Filter> filters = new List<Filter>();
                        if(filts != null)
				            foreach (var filt in filts)
				            {
				                filters.Add((Filter) filt);
				            }

					    var result = WalkFilters(filters, visitor, removeNulls);
					    if (!result.Equals(filter.Value)) {
						    filter = new Filter(filter.Property, result, filter.Operator);
					    }
				    }
			    }
		    }

		    filter = visitor.VisitAfter(filter);

		    return filter;
	    }

	    
    }

    /**
	* Visitor for use with walkList()
	*/
    public class ItemVisitor<T> {
	    public virtual T Visit(T item) {
		    return item;
	    }
    }

    /**
	 * Visitor for use with walkFilter and walkFilters
	 */
    public class FilterVisitor {
	    public virtual Filter VisitBefore(Filter filter) {
		    return filter;
	    }

	    public virtual Filter VisitAfter(Filter filter) {
		    return filter;
	    }
    }

    /**
	 * Used in walkFilters
	 */
	public class FilterListVisitor : ItemVisitor<Filter> {
		private readonly FilterVisitor _visitor;
		private readonly bool _removeNulls;

		public FilterListVisitor(FilterVisitor visitor, bool removeNulls) {
			_visitor = visitor;
			_removeNulls = removeNulls;
		}

		public override Filter Visit(Filter filter) {
			return SearchUtil.WalkFilter(filter, _visitor, _removeNulls);
		}
	}
}