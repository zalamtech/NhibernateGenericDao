#region copyright
// ------------------------------------------------------------------------
// <copyright file="ISearchFacade.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections;

namespace Com.Googlecode.Genericdao.Search
{
    public interface ISearchFacade
    {
        IList Search(ISearch search);

        IList Search(Type searchClass, ISearch search);

        int Count(ISearch search);

        int Count(Type searchClass, ISearch search);

        SearchResult<T> SearchAndCount<T>(ISearch search);

        SearchResult<T> SearchAndCount<T>(Type searchClass, ISearch search);

        object SearchUnique(ISearch search);

        object SearchUnique(Type searchClass, ISearch search);

        Filter GetFilterFromExample(object example);

        Filter GetFilterFromExample(object example, ExampleOptions options);
    }
}