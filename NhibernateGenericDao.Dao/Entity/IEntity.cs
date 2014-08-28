#region copyright
// ------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
namespace Com.Googlecode.Genericdao.Dao.Entity
{
    public interface IEntity<TId>
    {
        TId Id { get; set; } 
    }
}