﻿#region copyright
// ------------------------------------------------------------------------
// <copyright file="IPhoneDao.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 20:26</date>
//  ------------------------------------------------------------------------
#endregion

using Com.Googlecode.Genericdao.Console.Entities;
using Com.Googlecode.Genericdao.Dao;

namespace Com.Googlecode.Genericdao.Console.Repositories
{
    public interface IPhoneDao : IGenericDao<Phone, int>
    {
         
    }
}