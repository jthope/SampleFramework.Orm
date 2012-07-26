using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SampleFramework.Domain;

using NCommon.Storage;
using Castle.Windsor;

namespace SampleFramework.Services.Base {

	[Serializable()]
	public class BaseService<TService, TEntity>
		where TEntity : class, IEntity
		where TService : class, new() {

		#region Members

		private static TService _instance;
		private WindsorContainer _container;

		#endregion

		#region Constructors

		public BaseService() {
			_container = (WindsorContainer)Store.Application.Get<WindsorContainer>("ApplicationContainer");
		}

		#endregion

		#region Public Properties

		public static TService Instance {
			get {
				if (_instance == null) {
					_instance = new TService();
				}
				return _instance;
			}
		}

		public WindsorContainer Container {
			get { return _container; }
		}

		#endregion

	}

}