#region copyright
// ------------------------------------------------------------------------
// <copyright file="NhibernateSearchFacade.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections;
using NHibernate;

namespace Com.Googlecode.Genericdao.Search.Nhibernate
{
    public class NhibernateSearchFacade : ISearchFacade
    {
        private ISessionFactory _sessionFactory;
        private NhibernateSearchProcessor _processor;

        public NhibernateSearchFacade(ISessionFactory sessionFactory)
        {
            SetSessionFactory(sessionFactory);
        }

        public void SetSessionFactory(ISessionFactory sessionFactory)
        {
            _processor = NhibernateSearchProcessor.GetInstanceForSessionFactory(sessionFactory);
            _sessionFactory = sessionFactory;
        }

        protected ISession GetSession()
        {
            return _sessionFactory.GetCurrentSession();
        }

        protected NhibernateSearchProcessor GetProcessor()
        {
            return _processor;
        }

        public IList Search(ISearch search)
        {
            return (IList)_processor.Search<object>(GetSession(), search);
        }

        public IList Search(Type searchClass, ISearch search)
        {
            return _processor.Search(GetSession(), searchClass, search);
        }

        public int Count(ISearch search)
        {
            return _processor.Count(GetSession(), search);
        }

        public int Count(Type searchClass, ISearch search)
        {
            return _processor.Count(GetSession(), searchClass, search);
        }

        public SearchResult<T> SearchAndCount<T>(ISearch search)
        {
            return _processor.SearchAndCount<T>(GetSession(), search);
        }

        public SearchResult<T> SearchAndCount<T>(Type searchClass, ISearch search)
        {
            return  _processor.SearchAndCount<T>(GetSession(), searchClass, search);
        }

        public object SearchUnique(ISearch search)
        {
            return _processor.SearchUnique(GetSession(), search);
        }

        public object SearchUnique(Type searchClass, ISearch search)
        {
            return _processor.SearchUnique(GetSession(), searchClass, search);
        }

        public Filter GetFilterFromExample(object example)
        {
            return _processor.GetFilterFromExample(example);
        }

        public Filter GetFilterFromExample(object example, ExampleOptions options)
        {
            return _processor.GetFilterFromExample(example, options);
        }
    }
}