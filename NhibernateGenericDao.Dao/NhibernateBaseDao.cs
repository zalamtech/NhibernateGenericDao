#region copyright
// ------------------------------------------------------------------------
// <copyright file="NhibernateBaseDao.cs" company="Zalamtech SARL">
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
// <date>2014-9-1 18:17</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Logging;
using Com.Googlecode.Genericdao.Dao.Entity;
using Com.Googlecode.Genericdao.Dao.Unitofwork;
using Com.Googlecode.Genericdao.Search;
using Com.Googlecode.Genericdao.Search.Nhibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace Com.Googlecode.Genericdao.Dao
{
    public class NhibernateBaseDao
    {
        private readonly NhibernateSearchProcessor _searchProcessor;
        private readonly ISessionFactory _sessionFactory;
        private readonly NhibernateMetadataUtil _metadataUtil;

        public ILogger Logger { get; set; }

        public NhibernateBaseDao(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
            _searchProcessor = NhibernateSearchProcessor.GetInstanceForSessionFactory(sessionFactory);
            _metadataUtil = NhibernateMetadataUtil.GetInstanceForSessionFactory(sessionFactory);
        }

        protected ISessionFactory GetSessionFactory()
        {
            return _sessionFactory;
        }

        /**
         * Get the current Hibernate session
         */
        protected ISession GetSession()
        {
            return NhibernateUnitOfWork.Current.Session;
        }

        /**
         * Get the instance of HibernateMetadataUtil associated with the session
         * factory
         */
        protected NhibernateMetadataUtil GetMetadataUtil()
        {
            return _metadataUtil;
        }

        /**
         * Get the instance of EJBSearchProcessor associated with the session
         * factory
         */
        protected NhibernateSearchProcessor GetSearchProcessor()
        {
            return _searchProcessor;
        }

        protected object _save(object entity)
        {
            return GetSession().Save(entity);
        }

        protected void _save(params object[] entities)
        {
            foreach (var entity in entities)
            {
                _save(entity);
            }
        }

        protected void _saveOrUpdate(object entity)
        {
            GetSession().SaveOrUpdate(entity);
        }

        protected bool _saveOrUpdateIsNew(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var id = GetMetadataUtil().GetId(entity);
            if (GetSession().Contains(entity))
                return false;

            if (id == null || (0L).Equals(id) || !_exists(entity))
            {
                _save(entity);
                return true;
            }

            _update(entity);

            return false;
        }

        protected bool[] _saveOrUpdateIsNew(params object[] entities)
        {
            var exists = new bool?[entities.Length];

            for (var i = 0; i < entities.Length; i++)
            {
                if (entities[i] == null)
                {
                    throw new ArgumentNullException("entities");
                }
                if (GetSession().Contains(entities[i]))
                {
                    exists[i] = true;
                }
                else
                {
                    var id = GetMetadataUtil().GetId(entities[i]);
                    if (id == null || (0L).Equals(id))
                    {
                        exists[i] = false;
                    }
                }
            }
            // if it has an id and is not contained in the session, it may exist
            IDictionary<Type, IList<Int32>> mayExist = new Dictionary<Type, IList<Int32>>();
            for (var i = 0; i < entities.Length; i++)
            {
                if (exists[i] != null) continue;

                var entityClass = _metadataUtil.GetUnproxiedClass(entities[i]); //Get the real entity class
                IList<Int32> l;
                mayExist.TryGetValue(entityClass, out l);
                if (l == null)
                {
                    l = new List<Int32>();
                    mayExist.Add(entityClass, l);
                }
                l.Add(i);
            }

            // for each type of entity, do a batch call to the datastore to see
            // which of the entities of that class exist
            foreach (var entry in mayExist)
            {
                var ids = new object[entry.Value.Count];
                for (var i = 0; i < ids.Length; i++)
                {
                    ids[i] = GetMetadataUtil().GetId(entities[entry.Value[i]]);
                }
                var exists2 = _exists(entry.Key, ids);
                for (var i = 0; i < ids.Length; i++)
                {
                    exists[entry.Value[i]] = exists2[i];
                }
            }

            var isNew = new bool[entities.Length];
            // now that we know which ones exist, save or update each.
            for (var i = 0; i < entities.Length; i++)
            {
                if (entities[i] == null) continue;

                if (exists[i] != null && (bool)exists[i])
                {
                    _update(entities[i]);
                    isNew[i] = false;
                }
                else
                {
                    _save(entities[i]);
                    isNew[i] = true;
                }
            }

            return isNew;
        }

        protected void _persist(params object[] entities)
        {
            foreach (var entity in entities)
            {
                GetSession().Persist(entity);
            }
        }

        protected bool _deleteById(Type type, object id)
        {
            if (id == null) return false;

            type = _metadataUtil.GetUnproxiedClass(type); //Get the real entity class
            var entity = GetSession().Get(type, id);

            if (entity == null) return false;
            GetSession().Delete(entity);

            return true;
        }

        protected void _deleteByIds(Type type, params object[] ids)
        {
            type = _metadataUtil.GetUnproxiedClass(type); //Get the real entity class
            var c = GetSession().CreateCriteria(type);

            c.Add(Restrictions.In("id", ids));
            foreach (var entity in c.List())
            {
                GetSession().Delete(entity);
            }
        }

        protected bool _deleteEntity(object entity)
        {
            if (entity == null) return false;

            var id = GetMetadataUtil().GetId(entity);
            if (id == null) return false;

            entity = GetSession().Get(GetMetadataUtil().GetUnproxiedClass(entity), id);

            if (entity == null) return false;

            GetSession().Delete(entity);

            return true;
        }

        protected void _deleteEntities(params object[] entities)
        {
            foreach (var entity in entities.Where(entity => entity != null))
            {
                GetSession().Delete(entity);
            }
        }

        protected T _get<T, TId>(TId id) where T : Entity<TId>
        {
            return GetSession().Get<T>(id);
        }

        protected T[] _getArray<T, TId>(params TId[] ids) where T : Entity<TId>
        {
            var c = GetSession().CreateCriteria(typeof(T));
            c.Add(Restrictions.In("id", ids));
            var retVal = new T[ids.Length];

            foreach (var entity in c.List<T>())
            {
                var id = GetMetadataUtil().GetId(entity);
                for (var i = 0; i < ids.Length; i++)
                {
                    if (!id.Equals(ids[i])) continue;

                    retVal[i] = entity;
                    break;
                }
            }

            return retVal;
        }

        protected T _load<T, TId>(TId id) where T : Entity<TId>
        {
            return GetSession().Load<T>(id);
        }

        protected T[] _loadArray<T, TId>(params TId[] ids) where T : Entity<TId>
        {
            var retVal = new T[ids.Length];
            for (var i = 0; i < ids.Length; i++)
            {
                retVal[i] = _load<T, TId>(ids[i]);
            }
            return retVal;
        }

        protected void _load(object transientEntity, object id)
        {
            GetSession().Load(transientEntity, id);
        }

        protected IQueryable<T> _all<T, TId>() where T : Entity<TId>
        {
            return GetSession().Query<T>();
        }

        protected void _update(params object[] transientEntities)
        {
            foreach (var entity in transientEntities)
            {
                GetSession().Update(entity);
            }
        }

        protected T _merge<T, TId>(T entity) where T : Entity<TId>
        {
            return GetSession().Merge(entity);
        }

        protected IEnumerable<object> _search(ISearch search)
        {
            if (search == null)
                throw new NullReferenceException("Search is null.");
            if (search.GetSearchClass() == null)
                throw new NullReferenceException("Search class is null.");

            return GetSearchProcessor().Search<object>(GetSession(), search);
        }

        protected IEnumerable<T> _search<T, TId>(ISearch search) where T : Entity<TId>
        {
            if (search == null)
                throw new NullReferenceException("Search is null.");

            if (search.GetSearchClass() != null && search.GetSearchClass() != typeof(T))
                throw new ArgumentException("Search class does not match expected type: " + typeof(T).Name);

            return GetSearchProcessor().Search<T>(GetSession(), search);
        }

        protected int _count(ISearch search)
        {
            if (search == null)
                throw new NullReferenceException("Search is null.");
            if (search.GetSearchClass() == null)
                throw new NullReferenceException("Search class is null.");

            return GetSearchProcessor().Count(GetSession(), search);
        }

        protected int _count<T, TId>(ISearch search) where T : Entity<TId>
        {
            if (search == null)
                throw new NullReferenceException("Search is null.");

            if (search.GetSearchClass() != null && search.GetSearchClass() != typeof(T))
                throw new ArgumentException("Search class does not match expected type: " + typeof(T).Name);

            return GetSearchProcessor().Count(GetSession(), typeof(T), search);
        }

        protected int _count<T, TId>() where T : Entity<TId>
        {
            var counts = GetSession().CreateQuery("select count(_it_) from " + GetMetadataUtil().Get(typeof(T)).GetEntityName() + " _it_").List();
            return counts.Cast<object>().Sum(count => Convert.ToInt32((long)count));
        }

        protected SearchResult<object> _searchAndCount(ISearch search)
        {
            if (search == null)
                throw new NullReferenceException("Search is null.");
            if (search.GetSearchClass() == null)
                throw new NullReferenceException("Search class is null.");

            return GetSearchProcessor().SearchAndCount<object>(GetSession(), search);
        }

        protected SearchResult<T> _searchAndCount<T, TId>(ISearch search) where T : Entity<TId>
        {
            if (search == null)
                throw new NullReferenceException("Search is null.");

            if (search.GetSearchClass() != null && search.GetSearchClass() != typeof(T))
                throw new ArgumentException("Search class does not match expected type: " + typeof(T).Name);

            return GetSearchProcessor().SearchAndCount<T>(GetSession(), search);
        }

        protected object _searchUnique(ISearch search)
        {
            if (search == null)
                throw new NullReferenceException("Search is null.");
            if (search.GetSearchClass() == null)
                throw new NullReferenceException("Search class is null.");

            return GetSearchProcessor().SearchUnique(GetSession(), search);
        }

        protected T _searchUnique<T, TId>(ISearch search) where T : Entity<TId>
        {
            if (search == null)
                throw new NullReferenceException("Search is null.");

            if (search == null)
                throw new NullReferenceException("Search is null.");

            if (search.GetSearchClass() != null && search.GetSearchClass() != typeof(T))
                throw new ArgumentException("Search class does not match expected type: " + typeof(T).Name);

            return (T)GetSearchProcessor().SearchUnique(GetSession(), typeof(T), search);
        }

        /**
	     * Returns true if the object is connected to the current hibernate session.
	     */
        protected bool _sessionContains(object o)
        {
            return GetSession().Contains(o);
        }

        /**
         * Flushes changes in the hibernate cache to the datastore.
         */
        protected void _flush()
        {
            GetSession().Flush();
        }

        /**
         * Refresh the content of the given entity from the current datastore state.
         */
        protected void _refresh(params object[] entities)
        {
            foreach (var entity in entities)
                GetSession().Refresh(entity);
        }

        protected bool _exists(object entity)
        {
            return GetSession().Contains(entity) || _exists(entity.GetType(), GetMetadataUtil().GetId(entity));
        }

        protected bool _exists(Type type, object id)
        {
            if (type == null)
                throw new NullReferenceException("Type is null.");
            if (id == null)
                return false;
            type = _metadataUtil.GetUnproxiedClass(type); //Get the real entity class

            var query = GetSession().CreateQuery("select id from " + GetMetadataUtil().Get(type).GetEntityName() + " where id = :id");
            query.SetParameter("id", id);
            return query.List().Count == 1;
        }

        protected bool[] _exists(Type type, params object[] ids)
        {
            if (type == null)
                throw new NullReferenceException("Type is null.");
            type = _metadataUtil.GetUnproxiedClass(type); //Get the real entity class

            var ret = new bool[ids.Length];

            // we can't use "id in (:ids)" because some databases do not support
            // this for compound ids.
            var sb = new StringBuilder("select id from " + GetMetadataUtil().Get(type).GetEntityName() + " where");
            var first = true;
            for (var i = 0; i < ids.Length; i++)
            {
                if (first)
                {
                    first = false;
                    sb.Append(" id = :id");
                }
                else
                {
                    sb.Append(" or id = :id");
                }
                sb.Append(i);
            }

            var query = GetSession().CreateQuery(sb.ToString());
            for (var i = 0; i < ids.Length; i++)
            {
                query.SetParameter("id" + i, ids[i]);
            }

            foreach (var id in (List<object>)query.List())
            {
                for (var i = 0; i < ids.Length; i++)
                {
                    if (id.Equals(ids[i]))
                    {
                        ret[i] = true;
                        // don't break. the same id could be in the list twice.
                    }
                }
            }

            return ret;
        }

        protected Filter _getFilterFromExample(object example)
        {
            return _searchProcessor.GetFilterFromExample(example);
        }

        protected Filter _getFilterFromExample(object example, ExampleOptions options)
        {
            return _searchProcessor.GetFilterFromExample(example, options);
        }
    }
}