#region copyright
// ------------------------------------------------------------------------
// <copyright file="PersonDao.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 22:50</date>
//  ------------------------------------------------------------------------
#endregion

using Com.Googlecode.Genericdao.Console.Entities;
using Com.Googlecode.Genericdao.Dao;
using NHibernate;

namespace Com.Googlecode.Genericdao.Console.Repositories.Impl
{
    public class PersonDao : GenericDao<Person, int>, IPersonDao
    {
        public PersonDao(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }
    }
}