#region copyright
// ------------------------------------------------------------------------
// <copyright file="NhibernateSearchFacade.cs" company="Zalamtech SARL">
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