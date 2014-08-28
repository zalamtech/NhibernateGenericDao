#region copyright
// ------------------------------------------------------------------------
// <copyright file="CityDao.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 22:48</date>
//  ------------------------------------------------------------------------
#endregion

using Com.Googlecode.Genericdao.Console.Entities;
using Com.Googlecode.Genericdao.Dao;
using NHibernate;

namespace Com.Googlecode.Genericdao.Console.Repositories.Impl
{
    public class CityDao : GenericDao<City, int>, ICityDao
    {
        public CityDao(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }
    }
}