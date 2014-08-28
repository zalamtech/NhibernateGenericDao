#region copyright
// ------------------------------------------------------------------------
// <copyright file="NhibernateMetadataUtil.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Proxy;

namespace Com.Googlecode.Genericdao.Search.Nhibernate
{
    public class NhibernateMetadataUtil : IMetadataUtil
    {
        private static IDictionary<ISessionFactory, NhibernateMetadataUtil> _map = new Dictionary<ISessionFactory, NhibernateMetadataUtil>();
        private ISessionFactory _sessionFactory;

        public static NhibernateMetadataUtil GetInstanceForSessionFactory(ISessionFactory sessionFactory)
        {
            NhibernateMetadataUtil instance;
            _map.TryGetValue(sessionFactory, out instance);
            if (instance != null) return instance;

            instance = new NhibernateMetadataUtil {_sessionFactory = sessionFactory};
            _map.Add(sessionFactory, instance);
            return instance;
        }

        public object GetId(object entity)
        {
            if (entity == null)
                throw new NullReferenceException("Cannot get ID from null object.");
            return Get(entity.GetType()).GetIdValue(entity);
        }

        public bool IsId(Type rootClass, string propertyPath)
        {
            if (string.IsNullOrEmpty(propertyPath))
                return false;
            // with hibernate, "id" always refers to the id property, no matter what
            // that property is named. just make sure the segment before this "id"
            // refers to an entity since only entities have ids.
            if (propertyPath.Equals("id")
                    || (propertyPath.EndsWith(".id") && Get(rootClass, propertyPath.Substring(0, propertyPath.Length - 3))
                            .IsEntity()))
                return true;

            // see if the property is the identifier property of the entity it
            // belongs to.
            var pos = propertyPath.LastIndexOf(".", StringComparison.InvariantCulture);
            if (pos == -1)
            {
                return propertyPath.Equals(_sessionFactory.GetClassMetadata(rootClass).IdentifierPropertyName);
            }

            var parentType = Get(rootClass, propertyPath.Substring(0, pos));
            return parentType.IsEntity() && propertyPath.Substring(pos + 1).Equals(parentType.GetIdProperty());
        }

        public IMetadata Get(Type klass)
        {
            klass = GetUnproxiedClass(klass);
            var cm = _sessionFactory.GetClassMetadata(klass);
            if (cm == null)
            {
                throw new ArgumentException("Unable to introspect " + klass + ". The class is not a registered Hibernate entity.");
            }

            return new NhibernateEntityMetadata(_sessionFactory, cm, null);
        }

        public IMetadata Get(Type rootEntityClass, string propertyPath)
        {
            try
            {
                var md = Get(rootEntityClass);
                if (string.IsNullOrEmpty(propertyPath))
                    return md;

                var chain = propertyPath.Split('.');

                return chain.Aggregate(md, (current, t) => current.GetPropertyType(t));

            }
            catch (HibernateException)
            {
                throw new ArgumentException("Could not find property '" + propertyPath + "' on class "
                        + rootEntityClass + ".");
            }
        }

        public Type GetUnproxiedClass(Type klass)
        {
            while (_sessionFactory.GetClassMetadata(klass) == null) {
			    klass = klass.BaseType;
			    if (klass == null || klass == typeof(object))
				    return null;
		    }
		
		    return klass;
        }

        public Type GetUnproxiedClass(object entity)
        {
            return NHibernateProxyHelper.GetClassWithoutInitializingProxy(entity);
        }
    }
}