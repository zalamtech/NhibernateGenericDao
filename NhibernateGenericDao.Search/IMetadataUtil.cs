#region copyright
// ------------------------------------------------------------------------
// <copyright file="IMetadataUtil.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;

namespace Com.Googlecode.Genericdao.Search
{
    public interface IMetadataUtil
    {
        object GetId(object targetObject);

        bool IsId(Type rootClass, string propertyPath);

        IMetadata Get(Type klass);

        IMetadata Get(Type rootEntityClass, string propertyPath);

        Type GetUnproxiedClass(Type klass);

        Type GetUnproxiedClass(object targetObject);
    }
}