#region copyright
// ------------------------------------------------------------------------
// <copyright file="ISearch.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;

namespace Com.Googlecode.Genericdao.Search
{
    public interface ISearch
    {
        int GetFirstResult();

        int GetMaxResults();

        int GetPage();

        Type GetSearchClass();

        IList<Filter> GetFilters();

        bool IsDisjunction();

        IList<Sort> GetSorts();

        IList<Field> GetFields();

        bool IsDistinct();

        IList<String> GetFetches();

        int GetResultMode();
    }
}