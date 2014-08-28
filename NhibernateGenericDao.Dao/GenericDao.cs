#region copyright
// ------------------------------------------------------------------------
// <copyright file="GenericDao.cs" company="Zalamtech SARL">
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
// <date>2014-8-28 23:21</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Googlecode.Genericdao.Dao.Entity;
using Com.Googlecode.Genericdao.Search;
using NHibernate;

namespace Com.Googlecode.Genericdao.Dao
{
    public class GenericDao<TEntity, TId> : NhibernateBaseDao, IGenericDao<TEntity, TId> 
        where TEntity : Entity<TId>
    {

        protected readonly Type PersistentClass = typeof(TEntity);

        public GenericDao(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        public TEntity Find(TId id)
        {
            return _get<TEntity, TId>(id);
        }

        public async Task<TEntity> FindAsync(TId id)
        {
            return await Task.Run(() => Find(id));
        }

        public TEntity[] Find(params TId[] ids)
        {
            return _getArray<TEntity, TId>(ids);
        }

        public async Task<TEntity[]> FindAsync(params TId[] ids)
        {
            return await Task.Run(() => Find(ids));
        }

        public TEntity GetReference(TId id)
        {
            return _load<TEntity, TId>(id);
        }

        public TEntity[] GetReferences(params TId[] ids)
        {
            return _loadArray<TEntity, TId>(ids);
        }

        public bool Save(TEntity entity)
        {
            return _saveOrUpdateIsNew(entity);
        }

        public async Task<bool> SaveAsync(TEntity entity)
        {
            return await Task.Run(() => Save(entity)) ;
        }

        public bool[] Save(params TEntity[] entities)
        {
            if (entities == null) throw new ArgumentNullException("entities");
        // ReSharper disable once CoVariantArrayConversion
            return _saveOrUpdateIsNew(entities);
        }

        public async Task<bool[]> SaveAsync(params TEntity[] entities)
        {
            return await Task.Run(() => Save(entities));
        }

        public bool Remove(TEntity entity)
        {
            return _deleteEntity(entity);
        }

        public async Task<bool> RemoveAsync(TEntity entity)
        {
            return await Task.Run(() => Remove(entity));
        }

        public void Remove(params TEntity[] entities)
        {
            _deleteEntity(entities);
        }

        public bool RemoveById(TId id)
        {
            return _deleteById(PersistentClass, id);
        }

        public async Task<bool> RemoveByIdAsync(TId id)
        {
            return await Task.Run(() => RemoveById(id));
        }

        public void RemoveByIds(params TId[] ids)
        {
            _deleteByIds(PersistentClass, ids);
        }

        public IQueryable<TEntity> FindAll()
        {
            return _all<TEntity, TId>();
        }

        public async Task<IQueryable<TEntity>> FindAllAsync()
        {
            return await Task.Run(() => FindAll());
        }

        public IEnumerable<TREntity> Search<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>
        {
            return search == null ? _all<TREntity, TRId>().AsEnumerable() : _search<TREntity, TRId>(search);
        }

        public async Task<IEnumerable<TREntity>> SearchAsync<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>
        {
            return await Task.Run(() => SearchAsync<TREntity, TRId>(search));
        }

        public TREntity SearchUnique<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>
        {
            return _searchUnique<TREntity, TRId>(search);
        }

        public async Task<TREntity> SearchUniqueAsync<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>
        {
            return await Task.Run(() => SearchUniqueAsync<TREntity, TRId>(search));
        }

        public int Count(ISearch search)
        {
            return _count<TEntity, TId>(search);
        }

        public async Task<int> CountAsync(ISearch search)
        {
            return await Task.Run(() => Count(search));
        }

        public SearchResult<TREntity> SearchAndCount<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>
        {
            return _searchAndCount<TREntity, TRId>(search);
        }

        public async Task<SearchResult<TREntity>> SearchAndCountAsync<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>
        {
            return await Task.Run(() => SearchAndCount<TREntity, TRId>(search));
        }

        public bool IsAttached(TEntity entity)
        {
            return _sessionContains(entity);
        }

        public async Task<bool> IsAttachedAsync(TEntity entity)
        {
            return await Task.Run(() => IsAttached(entity));
        }

        public void Refresh(params TEntity[] entities)
        {
            // ReSharper disable once CoVariantArrayConversion
            _refresh(entities);
        }

        public void Flush()
        {
            _flush();
        }

        public Filter GetFilterFromExample(TEntity example)
        {
            return _getFilterFromExample(example);
        }

        public async Task<Filter> GetFilterFromExampleAsync(TEntity example)
        {
            return await Task.Run(() => GetFilterFromExample(example));
        }

        public Filter GetFilterFromExample(TEntity example, ExampleOptions options)
        {
            return _getFilterFromExample(example, options);
        }

        public async Task<Filter> GetFilterFromExampleAsync(TEntity example, ExampleOptions options)
        {
            return await Task.Run(() => GetFilterFromExample(example, options));
        }
    }
}