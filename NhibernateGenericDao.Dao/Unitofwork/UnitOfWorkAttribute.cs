#region copyright
// ------------------------------------------------------------------------
// <copyright file="UnitOfWorkAttribute.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;

namespace Com.Googlecode.Genericdao.Dao.Unitofwork
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : Attribute
    {
    }
}