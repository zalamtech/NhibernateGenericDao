#region copyright
// ------------------------------------------------------------------------
// <copyright file="IGenericDao.cs" company="Zalamtech SARL">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Googlecode.Genericdao.Search;
using Com.Googlecode.Genericdao.Dao.Entity;

namespace Com.Googlecode.Genericdao.Dao
{
    public interface IGenericDao
    {
    }

    public interface IGenericDao<TEntity, in TId> : IGenericDao where TEntity : Entity<TId>
    {
        /**
        * <p>
        * Get the entity with the specified type and id from the datastore.
        * 
        * <p>
        * If none is found, return null.
        */
        TEntity Find(TId id);

        Task<TEntity> FindAsync(TId id);

        /**
         * Get all entities of the specified type from the datastore that have one
         * of these ids.
         */
        TEntity[] Find(params TId[] ids);

        Task<TEntity[]> FindAsync(params TId[] ids);

        /**
         * <p>
         * Get a reference to the entity with the specified type and id from the
         * datastore.
         * 
         * <p>
         * TEntityhis does not require a call to the datastore and does not populate any
         * of the entity's values. Values may be fetched lazily at a later time.
         * TEntityhis increases performance if a another entity is being saved that should
         * reference this entity but the values of this entity are not needed.
         * 
         * @throws a
         *             HibernateException if no matching entity is found
         */
        TEntity GetReference(TId id);

        /**
         * <p>
         * Get a reference to the entities of the specified type with the given ids
         * from the datastore.
         * 
         * <p>
         * TEntityhis does not require a call to the datastore and does not populate any
         * of the entities' values. Values may be fetched lazily at a later time.
         * TEntityhis increases performance if a another entity is being saved that should
         * reference these entities but the values of these entities are not needed.
         * 
         * @throws a
         *             HibernateException if any of the matching entities are not
         *             found.
         */
        TEntity[] GetReferences(params TId[] ids);

        /// <summary>
        /// <p>
        /// If the id of the entity is null or zero, add it to the datastore and
        /// assign it an id; otherwise, update the corresponding entity in the
        /// datastore with the properties of this entity. In either case the entity
        /// passed to this method will be attached to the session.
        /// </p>
        /// <p>
        /// If an entity to update is already attached to the session, this method
        /// will have no effect. If an entity to update has the same id as another
        /// instance already attached to the session, an error will be thrown.
        /// </p>
        /// <param name="entity">the entity</param>
        /// <returns>
        ///     <code>true</code> if create; <code>false</code> if update.
        /// </returns>
        /// </summary>
        bool Save(TEntity entity);

        Task<bool> SaveAsync(TEntity entity);

        /// <summary>
        /// <p>
        /// For each entity, if the id of the entity is null or zero, add it to the
        /// datastore and assign it an id; otherwise, update the corresponding entity
        /// in the datastore with the properties of this entity. In either case the
        /// entity passed to this method will be attached to the session.
        /// </p>
        /// <p>
        /// If an entity to update is already attached to the session, this method
        /// will have no effect. If an entity to update has the same id as another
        /// instance already attached to the session, an error will be thrown.
        /// </p>
        /// </summary>
        bool[] Save(params TEntity[] entities);

        Task<bool[]> SaveAsync(params TEntity[] entities);
        /**
         * Remove the specified entity from the datastore.
         * 
         * @return <code>true</code> if the entity is found in the datastore and
         *         removed, <code>false</code> if it is not found.
         */
        bool Remove(TEntity entity);

        Task<bool> RemoveAsync(TEntity entity);
        /**
         * Remove all of the specified entities from the datastore.
         */
        void Remove(params TEntity[] entities);

        /**
         * Remove the entity with the specified type and id from the datastore.
         * 
         * @return <code>true</code> if the entity is found in the datastore and
         *         removed, <code>false</code> if it is not found.
         */
        bool RemoveById(TId id);

        Task<bool> RemoveByIdAsync(TId id);

        /**
         * Remove all the entities of the given type from the datastore that have
         * one of these ids.
         */
        void RemoveByIds(params TId[] ids);

        /**
         * Get a list of all the objects of the specified type.
         */
        IQueryable<TEntity> FindAll();

        Task<IQueryable<TEntity>> FindAllAsync();
        /**
         * Search for entities given the search parameters in the specified
         * <code>ISearch</code> object.
         * 
         * @param RTEntity TEntityhe result type is automatically determined by the context in which the method is called.
         */
        IEnumerable<TREntity> Search<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>;

        Task<IEnumerable<TREntity>> SearchAsync<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>;

        /**
         * Search for a single entity using the given parameters.
         * 
         * @param RTEntity TEntityhe result type is automatically determined by the context in which the method is called.
         */
        // ReSharper disable once InconsistentNaming
        TREntity SearchUnique<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>;

        Task<TREntity> SearchUniqueAsync<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>;
        /**
         * Returns the total number of results that would be returned using the
         * given <code>ISearch</code> if there were no paging or maxResults limits.
         */
        int Count(ISearch search);

        Task<int> CountAsync(ISearch search);

        /**
         * Returns a <code>SearchResult</code> object that includes both the list of
         * results like <code>search()</code> and the total length like
         * <code>count()</code>.
         * 
         * @param RTEntity TEntityhe result type is automatically determined by the context in which the method is called.
         */
        // ReSharper disable once InconsistentNaming
        SearchResult<TREntity> SearchAndCount<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>;

        Task<SearchResult<TREntity>> SearchAndCountAsync<TREntity, TRId>(ISearch search) where TREntity : Entity<TRId>;

        /**
         * Returns <code>true</code> if the object is connected to the current
         * Hibernate session.
         */
        bool IsAttached(TEntity entity);

        Task<bool> IsAttachedAsync(TEntity entity);
        /**
         * Refresh the content of the given entity from the current datastore state.
         */
        void Refresh(params TEntity[] entities);

        /**
         * Flushes changes in the Hibernate session to the datastore.
         */
        void Flush();

        /**
         * Generates a search filter from the given example using default options. 
         */
        Filter GetFilterFromExample(TEntity example);

        Task<Filter> GetFilterFromExampleAsync(TEntity example);

        /**
         * Generates a search filter from the given example using the specified options. 
         */
        Filter GetFilterFromExample(TEntity example, ExampleOptions options);

        Task<Filter> GetFilterFromExampleAsync(TEntity example, ExampleOptions options);
    }
}