#region copyright
// ------------------------------------------------------------------------
// <copyright file="DependencyInstaller.cs" company="Goswiff">
// 	Goswiff Authentication Server.
// 	Copyright (c) 2014. All rights reserved.
// </copyright>
// <author>Abdoul DIALLO</author>
// <date>2014-08-26 23:33</date>
//  ------------------------------------------------------------------------
#endregion

using System.Linq;
using System.Reflection;
using Castle.Core;
using Castle.Facilities.Logging;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Com.Googlecode.Genericdao.Console.Mappings;
using Com.Googlecode.Genericdao.Console.Repositories.Impl;
using Com.Googlecode.Genericdao.Dao.Unitofwork;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;

namespace Com.Googlecode.Genericdao.Console
{
    public class DependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;

            // Log4net
            log4net.Config.XmlConfigurator.Configure();
            container.AddFacility<LoggingFacility>(f => f.UseLog4Net());

            //Register all controllers
            container.Register(

                //Nhibernate session factory
                Component.For<ISessionFactory>()
                    .UsingFactoryMethod(CreateNhSessionFactory)
                    .LifeStyle.Singleton,

                //Unitofwork interceptor
                Component.For<UnitOfWorkInterceptor>().LifeStyle.Transient,

                //All repoistories
                Classes.FromAssembly(Assembly.GetAssembly(typeof(PersonDao)))
                    .InSameNamespaceAs<PersonDao>()
                    .WithService.DefaultInterfaces()
                    .LifestyleTransient()
                );
        }

        /// <summary>
        /// Creates NHibernate Session Factory.
        /// </summary>
        /// <returns>NHibernate Session Factory</returns>
        private static ISessionFactory CreateNhSessionFactory()
        {
            const string connStr = @"Server=ASDIALLO\SQLEXPRESS;Database=PhoneBook;Trusted_Connection=True;";
            //ConfigurationManager.ConnectionStrings["CastleNhibernate"].ConnectionString;
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connStr))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(PersonMap))))
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
        }

        static void Kernel_ComponentRegistered(string key, IHandler handler)
        {
            //Intercept all methods of all repositories.
            if (UnitOfWorkHelper.IsDaoClass(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(UnitOfWorkInterceptor)));
            }

            //Intercept all methods of classes those have at least one method that has UnitOfWork attribute.
            if (!handler.ComponentModel.Implementation.GetMethods()
                    .Any(UnitOfWorkHelper.HasUnitOfWorkAttribute)) return;

            handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(UnitOfWorkInterceptor)));
        }
    }
}