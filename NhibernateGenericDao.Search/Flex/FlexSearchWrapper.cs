#region copyright
// ------------------------------------------------------------------------
// <copyright file="FlexSearchWrapper.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;

namespace Com.Googlecode.Genericdao.Search.Flex
{
    public class FlexSearchWrapper : ISearch
    {
        private FlexSearch _search;

        public FlexSearchWrapper(FlexSearch flexSearch)
        {
            _search = flexSearch;
        }


        public int GetFirstResult()
        {
            return _search.FirstResult;
        }

        public int GetMaxResults()
        {
            return _search.MaxResults;
        }

        public int GetPage()
        {
            return _search.Page;
        }

        public Type GetSearchClass()
        {
            return string.IsNullOrEmpty(_search.SearchClassName) ? null : Type.GetType(_search.SearchClassName);
        }

        public IList<Filter> GetFilters()
        {
            return _search.GetFilters();
        }

        public bool IsDisjunction()
        {
            return _search.Disjunction;
        }

        public IList<Sort> GetSorts()
        {
            return _search.GetSorts();
        }

        public IList<Field> GetFields()
        {
            return _search.GetFields();
        }

        public bool IsDistinct()
        {
            return _search.Distinct;
        }

        public IList<string> GetFetches()
        {
            return _search.GetFetches();
        }

        public int GetResultMode()
        {
            return _search.ResultMode;
        }
    }
}