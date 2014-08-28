#region copyright
// ------------------------------------------------------------------------
// <copyright file="IMetadata.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;

namespace Com.Googlecode.Genericdao.Search
{
    public interface IMetadata
    {
        /// <summary>
        /// Checks if a type is an entity.
        /// </summary>
        /// <returns>Return true if the type is an entity</returns>
        bool IsEntity();

        /// <summary>
        /// Checks if the type is an embeddable class (a component class in NHibernate)
        /// </summary>
        /// <returns>Returns true if the type is an embeddable</returns>
        bool IsEmbeddable();

        bool IsCollection();

        bool IsString();

        bool IsNumeric();

        Type GetClass();

        string GetEntityName();

        IEnumerable<string> GetProperties();

        object GetPropertyValue(object theObject, string property);

        IMetadata GetPropertyType(string property);

        string GetIdProperty();

        IMetadata GetIdType();

        object GetIdValue(object theObject);

        Type GetCollectionClass();
    }
}
