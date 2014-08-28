#region copyright
// ------------------------------------------------------------------------
// <copyright file="NhibernateNonEntityMetadata.cs" company="Zalamtech SARL">
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
// <date>2014-8-28 23:21</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Type;

namespace Com.Googlecode.Genericdao.Search.Nhibernate
{
    public class NhibernateNonEntityMetadata : IMetadata
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly IType _type;
        private readonly Type _collectionType;

        public NhibernateNonEntityMetadata(ISessionFactory sessionFactory, IType type, Type collectionType)
        {
            _sessionFactory = sessionFactory;
            _type = type;
            _collectionType = collectionType;
        }

        public bool IsEntity()
        {
            return false;
        }

        public bool IsEmbeddable()
        {
            return _type.IsComponentType;
        }

        public bool IsCollection()
        {
            return _collectionType != null;
        }

        public bool IsString()
        {
            var types = _type.SqlTypes((IMapping) _sessionFactory);
            
            return types.Length == 1 && (types[0].DbType.Equals(DbType.String) || types[0].DbType.Equals(DbType.StringFixedLength));
        }

        public bool IsNumeric()
        {
            return InternalUtil.IsNumeric(GetClass());
        }

        public Type GetClass()
        {
            return _type.ReturnedClass;
        }

        public string GetEntityName()
        {
            throw new NotSupportedException("Cannot get Entity Name of non-entity type.");
        }

        public IEnumerable<string> GetProperties()
        {
            return _type.IsComponentType ? ((ComponentType) _type).PropertyNames : null;
        }

        public object GetPropertyValue(object theObject, string property)
        {
            if (!_type.IsComponentType)
                return null;
            var i = GetPropertyIndex(property);
            return i == -1 ? null : ((ComponentType) _type).GetPropertyValue(theObject, i, EntityMode.Poco);
        }

        public IMetadata GetPropertyType(string property)
        {
            if (!_type.IsComponentType)
                return null;

            var i = GetPropertyIndex(property);
            if (i == -1)
            {
                return null;
            }

            var pType = ((ComponentType) _type).Subtypes[i];
            Type pCollectionType = null;
            if (pType.IsCollectionType)
            {
                pType = ((CollectionType) pType).GetElementType((ISessionFactoryImplementor) _sessionFactory);
                pCollectionType = pType.ReturnedClass;
            }

            if (pType.IsEntityType)
            {
                return new NhibernateEntityMetadata(_sessionFactory,
                    _sessionFactory.GetClassMetadata(((EntityType) pType).Name), pCollectionType);
            }

            return new NhibernateNonEntityMetadata(_sessionFactory, pType, pCollectionType);
        }

        public string GetIdProperty()
        {
            return null;
        }

        public IMetadata GetIdType()
        {
            return null;
        }

        public object GetIdValue(object theObject)
        {
            return null;
        }

        public Type GetCollectionClass()
        {
            return _collectionType;
        }

        private int GetPropertyIndex(string property)
        {
            var properties = GetProperties() as IList<string>;
            
            if (properties == null) return -1;

            for (var i = 0; i < properties.Count; i++)
            {
                if (properties[i].Equals(property))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}