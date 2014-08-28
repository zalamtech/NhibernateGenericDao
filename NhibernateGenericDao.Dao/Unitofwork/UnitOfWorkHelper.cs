#region copyright
// ------------------------------------------------------------------------
// <copyright file="UnitOfWorkHelper.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Reflection;

namespace Com.Googlecode.Genericdao.Dao.Unitofwork
{
    public class UnitOfWorkHelper
    {
        public static bool IsDaoMethod(MethodInfo methodInfo)
        {
            return IsDaoClass(methodInfo.DeclaringType);
        }

        public static bool IsDaoClass(Type type)
        {
            return typeof(IGenericDao).IsAssignableFrom(type);
        }

        public static bool HasUnitOfWorkAttribute(MethodInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        } 
    }
}