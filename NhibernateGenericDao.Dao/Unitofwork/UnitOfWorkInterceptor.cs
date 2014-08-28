#region copyright
// ------------------------------------------------------------------------
// <copyright file="UnitOfWorkInterceptor.cs" company="Zalamtech SARL">
//	NHibernate Generic Dao.
//	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul</author>
// <date>2014-8-27 18:25</date>
// ------------------------------------------------------------------------
#endregion
using System;
using System.Reflection;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using NHibernate;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Com.Googlecode.Genericdao.Dao.Unitofwork
{
    public class UnitOfWorkInterceptor : IInterceptor
    {
        public ILogger Logger { get; set; }

        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Creates a new NhUnitOfWorkInterceptor object.
        /// </summary>
        /// <param name="sessionFactory">Nhibernate session factory.</param>
        public UnitOfWorkInterceptor(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        /// <summary>
        /// Intercepts a method.
        /// </summary>
        /// <param name="invocation">Method invocation arguments</param>
        public void Intercept(IInvocation invocation)
        {
            //If there is a running transaction, just run the method
            if (NhibernateUnitOfWork.Current != null || !RequiresDbConnection(invocation.MethodInvocationTarget))
            {
                invocation.Proceed();
                return;
            }

            try
            {
                NhibernateUnitOfWork.Current = new NhibernateUnitOfWork(_sessionFactory);
                NhibernateUnitOfWork.Current.BeginTransaction();

                try
                {
                    invocation.Proceed();
                    NhibernateUnitOfWork.Current.Commit();
                }
                catch (Exception exception)
                {
                    Logger.Error("Could not proceed action", exception);
                    try
                    {
                        Logger.Info("Rolling back action");
                        NhibernateUnitOfWork.Current.Rollback();
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch (Exception re)
                    {
                        Logger.Error("Could not rollback action", re);
                    }

                    throw;
                }
            }
            finally
            {
                NhibernateUnitOfWork.Current = null;
            }
        }

        private static bool RequiresDbConnection(MethodInfo methodInfo)
        {
            return UnitOfWorkHelper.HasUnitOfWorkAttribute(methodInfo) || UnitOfWorkHelper.IsDaoMethod(methodInfo);
        } 
    }
}