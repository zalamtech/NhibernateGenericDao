#region copyright
// ------------------------------------------------------------------------
// <copyright file="IMutableSearch.cs" company="Zalamtech SARL">
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
    public interface IMutableSearch : ISearch
    {
        IMutableSearch SetFirstResult(int firstResult);

        IMutableSearch SetMaxResults(int maxResults);

        IMutableSearch SetPage(int page);

        IMutableSearch SetSearchClass(Type searchClass);

        IMutableSearch SetFilters(IList<Filter> filters);

        IMutableSearch SetDisjunction(bool disjunction);

        IMutableSearch SetSorts(IList<Sort> sorts);

        IMutableSearch SetFields(IList<Field> fields);

        IMutableSearch SetDistinct(bool distinct);

        IMutableSearch SetFetches(IList<String> fetches);

        IMutableSearch SetResultMode(int resultMode);
    }
}