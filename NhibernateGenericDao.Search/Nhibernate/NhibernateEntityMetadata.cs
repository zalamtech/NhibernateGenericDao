#region copyright
// ------------------------------------------------------------------------
// <copyright file="NhibernateEntityMetadata.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Metadata;
using NHibernate.Type;

namespace Com.Googlecode.Genericdao.Search.Nhibernate
{
    public class NhibernateEntityMetadata : IMetadata
    {
         private ISessionFactory _sessionFactory;
	     private IClassMetadata _metadata;
	     private Type _collectionType;

        public NhibernateEntityMetadata(ISessionFactory sessionFactory, IClassMetadata metadata, Type collectionType)
        {
            _sessionFactory = sessionFactory;
            _metadata = metadata;
            _collectionType = collectionType;
        }


        public bool IsEntity()
        {
            return true;
        }

        public bool IsEmbeddable()
        {
            return false;
        }

        public bool IsCollection()
        {
            return _collectionType != null;
        }

        public bool IsString()
        {
            return false;
        }

        public bool IsNumeric()
        {
            return false;
        }

        public Type GetClass()
        {
            return _metadata.GetMappedClass(EntityMode.Poco);
        }

        public string GetEntityName()
        {
            return _metadata.EntityName;
        }

        public IEnumerable<string> GetProperties()
        {
            var pn = _metadata.PropertyNames;
            var result = new string[pn.Length + 1];
            result[0] = _metadata.IdentifierPropertyName;
            for (var i = 0; i < pn.Length; i++)
            {
                result[i + 1] = pn[i];
            }
            return result;
        }

        public object GetPropertyValue(object theObject, string property)
        {
            return GetIdProperty().Equals(property) ? GetIdValue(theObject) : _metadata.GetPropertyValue(theObject, property, EntityMode.Poco);
        }

        public IMetadata GetPropertyType(string property)
        {
            var pType = _metadata.GetPropertyType(property);
		    Type pCollectionType = null;
		    if (pType.IsCollectionType)
		    {
		        pType = ((CollectionType) pType).GetElementType((ISessionFactoryImplementor) _sessionFactory);
			    
			    pCollectionType = pType.ReturnedClass;
		    }
		
		    if (pType.IsEntityType) {
			    return new NhibernateEntityMetadata(_sessionFactory, _sessionFactory.GetClassMetadata(((EntityType)pType).Name), pCollectionType);
		    }

            return new NhibernateNonEntityMetadata(_sessionFactory, pType, pCollectionType);
        }

        public string GetIdProperty()
        {
            return _metadata.IdentifierPropertyName;
        }

        public IMetadata GetIdType()
        {
            return new NhibernateNonEntityMetadata(_sessionFactory, _metadata.IdentifierType, null);
        }

        public object GetIdValue(object theObject)
        {
            return _metadata.GetIdentifier(theObject, EntityMode.Poco);
        }

        public Type GetCollectionClass()
        {
            return _collectionType;
        }
    }
}