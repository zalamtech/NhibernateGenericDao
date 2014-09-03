#region copyright
// ------------------------------------------------------------------------
// <copyright file="NhibernateSearchProcessor.cs" company="Zalamtech SARL">
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
using NHibernate;
using NHibernate.Transform;

namespace Com.Googlecode.Genericdao.Search.Nhibernate
{
    public class NhibernateSearchProcessor : BaseSearchProcessor
    {
        private static IDictionary<ISessionFactory, NhibernateSearchProcessor> _map = new Dictionary<ISessionFactory, NhibernateSearchProcessor>();

        private NhibernateSearchProcessor(IMetadataUtil mdu)
            : base(QLTYPE_HQL, mdu)
        {
        }

        public static NhibernateSearchProcessor GetInstanceForSessionFactory(ISessionFactory sessionFactory)
        {
            NhibernateSearchProcessor instance;
            _map.TryGetValue(sessionFactory, out instance);

            if (instance != null) return instance;

            instance = new NhibernateSearchProcessor(NhibernateMetadataUtil.GetInstanceForSessionFactory(sessionFactory));
            _map.Add(sessionFactory, instance);
            return instance;
        }

        // --- Public Methods ---

        /**
         * Search for objects based on the search parameters in the specified
         * <code>ISearch</code> object.
         * 
         * @see ISearch
         */

        public IList<T> Search<T>(ISession session, ISearch search)
        {
            IList<object> paramList = new List<object>();
            var hql = GenerateQL(typeof(T), search, paramList);
            var query = session.CreateQuery(hql);
            AddParams(query, paramList);
            AddPaging(query, search);
            AddResultMode(query, search);

            return query.List<T>();
        }

        /**
	     * Search for objects based on the search parameters in the specified
	     * <code>ISearch</code> object. Uses the specified searchClass, ignoring the
	     * searchClass specified on the search itself.
	     * 
	     * @see ISearch
	     */
        public IList Search(ISession session, Type searchClass, ISearch search)
        {
            IList<object> paramList = new List<object>();
            var hql = GenerateQL(searchClass, search, paramList);
            var query = session.CreateQuery(hql);
            AddParams(query, paramList);
            AddPaging(query, search);
            AddResultMode(query, search);

            return query.List();
        }

        /**
         * Returns the total number of results that would be returned using the
         * given <code>ISearch</code> if there were no paging or maxResult limits.
         * 
         * @see ISearch
         */
        public int Count(ISession session, ISearch search)
        {
            return search == null ? 0 : Count(session, search.GetSearchClass(), search);
        }

        /**
	     * Returns the total number of results that would be returned using the
	     * given <code>ISearch</code> if there were no paging or maxResult limits.
	     * Uses the specified searchClass, ignoring the searchClass specified on the
	     * search itself.
	     * 
	     * @see ISearch
	     */
        public int Count(ISession session, Type searchClass, ISearch search)
        {
            if (searchClass == null || search == null)
                return 0;

            IList<object> paramList = new List<object>();
            var hql = GenerateRowCountQL(searchClass, search, paramList);
            if (hql == null)
            { // special case where the query uses column operators
                return 1;
            }
            var query = session.CreateQuery(hql);
            AddParams(query, paramList);

            return Convert.ToInt32(query.UniqueResult<long>());
        }

        /**
         * Returns a <code>SearchResult</code> object that includes the list of
         * results like <code>search()</code> and the total length like
         * <code>searchLength</code>.
         * 
         * @see ISearch
         */
        public SearchResult<T> SearchAndCount<T>(ISession session, ISearch search)
        {
            return search == null ? null : SearchAndCount<T>(session, typeof(T), search);
        }

        /**
         * Returns a <code>SearchResult</code> object that includes the list of
         * results like <code>search()</code> and the total length like
         * <code>searchLength</code>. Uses the specified searchClass, ignoring the
         * searchClass specified on the search itself.
         * 
         * @see ISearch
         */
        public SearchResult<T> SearchAndCount<T>(ISession session, Type searchClass, ISearch search)
        {
            if (searchClass == null || search == null)
                return null;

            var result = new SearchResult<T> { Result = Search<T>(session, search) };

            if (search.GetMaxResults() > 0)
            {
                result.TotalCount = Count(session, searchClass, search);
            }
            else
            {
                result.TotalCount = result.Result.Count + SearchUtil.CalcFirstResult(search);
            }

            return result;
        }

        /**
         * Search for a single result using the given parameters.
         */
        public object SearchUnique(ISession session, ISearch search)
        {
            return search == null ? null : SearchUnique(session, search.GetSearchClass(), search);
        }

        /**
         * Search for a single result using the given parameters. Uses the specified
         * searchClass, ignoring the searchClass specified on the search itself.
         */
        public object SearchUnique(ISession session, Type entityClass, ISearch search)
        {
            if (search == null)
                return null;

            var paramList = new List<object>();
            var hql = GenerateQL(entityClass, search, paramList);
            var query = session.CreateQuery(hql);
            AddParams(query, paramList);
            AddPaging(query, search);
            AddResultMode(query, search);

            return query.UniqueResult();
        }

        // ---- SEARCH HELPERS ---- //
        private static void AddParams(IQuery query, IEnumerable<object> paramObjects)
        {
            //StringBuilder debug = null;

            var i = 1;
            foreach (var o in paramObjects)
            {
                //if (logger.isDebugEnabled()) {
                //    if (debug == null)
                //        debug = new StringBuilder();
                //    else
                //        debug.append("\n\t");
                //    debug.append("p");
                //    debug.append(i);
                //    debug.append(": ");
                //    debug.append(InternalUtil.paramDisplayString(o));
                //}
                if (o is ICollection)
                {
                    query.SetParameterList("p" + (i++), (IList)o);
                }
                else
                {
                    query.SetParameter("p" + (i++), o);
                }
            }
            
            //if (debug != null && debug.length() != 0) {
            //    logger.debug(debug.toString());
            //}
        }

        private static void AddPaging(IQuery query, ISearch search)
        {
            var firstResult = SearchUtil.CalcFirstResult(search);
            if (firstResult > 0)
            {
                query.SetFirstResult(firstResult);
            }
            if (search.GetMaxResults() > 0)
            {
                query.SetMaxResults(search.GetMaxResults());
            }
        }

        private static void AddResultMode(IQuery query, ISearch search)
        {
            var resultMode = search.GetResultMode();
            if (resultMode == SearchConstants.RESULT_AUTO)
            {
                var count = 0;
                
                foreach (var field in search.GetFields())
                {
                    if (field.Key != null && !field.Key.Equals(""))
                    {
                        resultMode = SearchConstants.RESULT_MAP;
                        break;
                    }
                    count++;
                }

                if (resultMode == SearchConstants.RESULT_AUTO)
                {
                    resultMode = count > 1 ? SearchConstants.RESULT_ARRAY : SearchConstants.RESULT_SINGLE;
                }
            }

            switch (resultMode)
            {
                case SearchConstants.RESULT_ARRAY:
                    query.SetResultTransformer(ARRAY_RESULT_TRANSFORMER);
                    break;
                case SearchConstants.RESULT_LIST:
                    query.SetResultTransformer(Transformers.ToList);
                    break;
                case SearchConstants.RESULT_MAP:
                    var keyList = new List<string>();
                    var fieldItr = search.GetFields();
                    foreach (var field in fieldItr)
                    {
                        if (field.Key != null && !field.Key.Equals(""))
                        {
                            keyList.Add(field.Key);
                        }
                        else
                        {
                            keyList.Add(field.Property);
                        }
                    }
                    query.SetResultTransformer(new MapResultTransformer(keyList.ToArray()));
                    break;
            }
        }

        // ReSharper disable once InconsistentNaming
        private static readonly IResultTransformer ARRAY_RESULT_TRANSFORMER = new ArrayResultTransformer();


        private sealed class ArrayResultTransformer : IResultTransformer
        {

            public IList TransformList(IList collection)
            {
                return collection;
            }

            public object TransformTuple(object[] tuple, string[] aliases)
            {
                return tuple;
            }
        }

        private sealed class MapResultTransformer : IResultTransformer
        {
            private readonly string[] _keys;

            public MapResultTransformer(string[] keys)
            {
                _keys = keys;
            }

            public IList TransformList(IList collection)
            {
                return collection;
            }

            public object TransformTuple(object[] tuple, string[] aliases)
            {
                IDictionary map = new Dictionary<string, Object>();
                for (var i = 0; i < _keys.Length; i++)
                {
                    var key = _keys[i];
                    if (key != null)
                    {
                        map.Add(key, tuple[i]);
                    }
                }

                return map;
            }
        }
    }
}