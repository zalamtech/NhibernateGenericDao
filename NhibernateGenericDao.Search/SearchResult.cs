#region copyright
// ------------------------------------------------------------------------
// <copyright file="SearchResult.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:26</date>
// ------------------------------------------------------------------------
#endregion
using System.Collections.Generic;

namespace Com.Googlecode.Genericdao.Search
{
    public class SearchResult<T>
    {
        // ReSharper disable InconsistentNaming
        protected IList<T> _result;
        protected int _totalCount = -1;
        // ReSharper restore InconsistentNaming

        public IList<T> Result
        {
            get { return _result; }
            set { _result = value; }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; }
        }
    }
}