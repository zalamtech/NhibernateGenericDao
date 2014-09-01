#region copyright
// ------------------------------------------------------------------------
// <copyright file="NhibernateEntityMetadata.cs" company="Zalamtech SARL">
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
// <date>2014-9-1 18:18</date>
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