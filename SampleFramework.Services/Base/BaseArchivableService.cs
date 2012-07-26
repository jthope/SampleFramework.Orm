using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SampleFramework.Domain;

using Castle.Windsor;

namespace SampleFramework.Services.Base {

	[Serializable()]
	public abstract class BaseArchivableService<TService, TEntity> : BasePersistentService<TService, TEntity>
		where TEntity : class, IArchivable
		where TService : class, new() {

		#region Members

		#endregion

		#region Constructors

		public BaseArchivableService() {

		}

		#endregion

		#region Public Properties

		#endregion

		#region IPersistentService<TEntity> Members

		/// <summary>
		/// Retrieves a <see cref="TEntity"/> object from the repository by it's Id. 
		/// </summary>
		/// <param name="id">Id of the object to retrieve.</param>
		/// <returns><see cref="IQueryable<TEntity>"/>IQueryable<TEntity> object.</returns>
		public override IQueryable<TEntity> GetById(int id) {

			var query = from x in base.GetById(id)
						where
							x.IsDeleted == false

						select x;

			return query;

		}

		/// <summary>
		/// Retrieves a list of <see cref="TEntity"/> objects from the repository. 
		/// </summary>
		/// <returns>IQueryable</returns>
		public override IQueryable<TEntity> GetList() {

			var query = from x in base.GetList()
						where
							x.IsDeleted == false

						select x;

			return query;

		}

		#endregion

		#region Unit of Work Methods

		/// <summary>
		/// Sets archiving information and saves the current 
		/// entity into the datastore.
		/// </summary>
		public override void Save(TEntity entity) {

			try {

				base.Save(entity);

			} catch (Exception ex) {

				throw new Exception(ex.ToString());

			}

		}

		/// <summary>
		/// Sets archiving information and flags the current 
		/// entity as deleted.
		/// </summary>
		public override void Delete(TEntity entity) {

			try {

				entity.IsDeleted = true;

				base.Save(entity);

			} catch (Exception ex) {

				throw new Exception(ex.ToString());

			}

		}

		#endregion

	}

}