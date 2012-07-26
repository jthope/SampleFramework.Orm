using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using SampleFramework;

using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Practices.ServiceLocation;

using NCommon;
using NCommon.Data;
using NCommon.Storage;
using NCommon.Data.NHibernate;

using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Tool.hbm2ddl;

namespace SampleFramework.Web {

	public class Global : System.Web.HttpApplication {

		static bool _configured;
		static ISessionFactory _sessionFactory;
		static object _configureLock = new object();

		protected void Application_Start(object sender, EventArgs e) {

			log4net.Config.XmlConfigurator.Configure();

			Configure();

		}

		void Begin_Request() {
			Configure();
		}

		protected void Session_Start(object sender, EventArgs e) {

		}

		protected void Application_BeginRequest(object sender, EventArgs e) {

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e) {

		}

		protected void Application_Error(object sender, EventArgs e) {

		}

		protected void Session_End(object sender, EventArgs e) {

		}

		protected void Application_End(object sender, EventArgs e) {

		}

		void Configure() {

			if (_configured)
				return;

			lock (_configureLock) {

				if (_configured)
					return;

				ConfigureContainer();
				ConfigureNHibernate();

				_configured = true;

			}

		}

		static void ConfigureContainer() {

			var container = new WindsorContainer();
			container.Register(Component.For<IUnitOfWorkFactory>().ImplementedBy<NHUnitOfWorkFactory>().LifeStyle.Transient);
			container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(NHRepository<>)).LifeStyle.Transient);

			Store.Application.Set("ApplicationContainer", container);

			Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(
				() => new CommonServiceLocator.WindsorAdapter.WindsorServiceLocator(Store.Application.Get<IWindsorContainer>("ApplicationContainer"))
			);

		}

		static void ConfigureNHibernate() {

			var schemaMode = ConfigurationManager.AppSettings["SchemaExportMode"];

			var configuration = Fluently.Configure()
				.Database(MsSqlConfiguration.MsSql2005.ConnectionString(x => x.FromConnectionStringWithKey("SampleFrameworkDb"))
				.ProxyFactoryFactory<ProxyFactoryFactory>())
				.Mappings(mappings => mappings.FluentMappings.AddFromAssemblyOf<Domain.Mappings.AuditTrailMap>()
				.Conventions.Setup(c => {
					c.Add<Domain.Mappings.ColumnNameConvention>();
					c.Add<Domain.Mappings.CustomForeignKeyConvention>();
					c.Add<Domain.Mappings.CollectionConvention>();
				}))
				.BuildConfiguration();

			_sessionFactory = configuration.BuildSessionFactory();

			Store.Application.Set("NHibernateSessionFactory", _sessionFactory);
			NHUnitOfWorkFactory.SetSessionProvider(GetSession);

			switch (schemaMode) {

				case ("CREATE"):
					var export = new SchemaExport(configuration);
					export.Drop(false, true);
					export.Create(false, true);
					break;

				case ("UPDATE"):
					new SchemaUpdate(configuration).Execute(false, true);
					break;

			}

		}

		public static ISession GetSession() {
			return Store.Application.Get<ISessionFactory>("NHibernateSessionFactory").OpenSession();
		}

	}

}