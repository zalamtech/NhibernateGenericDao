#region copyright
// ------------------------------------------------------------------------
// <copyright file="IPersonDao.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 20:24</date>
//  ------------------------------------------------------------------------
#endregion

using Com.Googlecode.Genericdao.Console.Entities;
using Com.Googlecode.Genericdao.Dao;

namespace Com.Googlecode.Genericdao.Console.Repositories
{
    public interface IPersonDao : IGenericDao<Person, int>
    {
         
    }
}