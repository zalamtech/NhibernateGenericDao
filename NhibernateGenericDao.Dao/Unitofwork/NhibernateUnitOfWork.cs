#region copyright
// ------------------------------------------------------------------------
// <copyright file="NhibernateUnitOfWork.cs" company="Zalamtech SARL">
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
using NHibernate;

namespace Com.Googlecode.Genericdao.Dao.Unitofwork
{
    public class NhibernateUnitOfWork: IUnitOfWork
    {
        /// <summary>
        /// Gets current instance of the NhUnitOfWork.
        /// It gets the right instance that is related to current thread.
        /// </summary>
        public static NhibernateUnitOfWork Current
        {
            get { return _current; }
            set { _current = value; }
        }

        [ThreadStatic] 
        private static NhibernateUnitOfWork _current;

        /// <summary>
        /// Gets Nhibernate session object to perform queries.
        /// </summary>
        public ISession Session { get; private set; }

        /// <summary>
        /// Reference to the session factory.
        /// </summary>
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Reference to the currently running transaction.
        /// </summary>
        private ITransaction _transaction;

        /// <summary>
        /// Creates a new instance of NhUnitOfWork.
        /// </summary>
        /// <param name="sessionFactory"></param>
        public NhibernateUnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        /// <summary>
        /// Opens database connection and begins transaction.
        /// </summary>
        public void BeginTransaction()
        {
            Session = _sessionFactory.OpenSession();
            _transaction = Session.BeginTransaction();
        }

        /// <summary>
        /// Commits transaction and closes database connection.
        /// </summary>
        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            finally
            {
                Session.Close();
            }
        }

        /// <summary>
        /// Rollbacks transaction and closes database connection.
        /// </summary>
        public void Rollback()
        {
            try
            {
                _transaction.Rollback();
            }
            finally
            {
                Session.Close();
            }
        }

        public ISessionFactory SessionFactory
        {
            get { return _sessionFactory; }
        }
    }
}