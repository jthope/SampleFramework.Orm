using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SampleFramework.Domain;

using NCommon;
using NCommon.Data;

using Castle.Windsor;

namespace SampleFramework.Services.Base {

	[Serializable()]
	public abstract class BasePersistentService<TService, TEntity> : BaseService<TService, TEntity>, IPersistentService<TEntity>
		where TEntity : class, IPersistent
		where TService : class, new() {

		#region Members

		private IRepository<TEntity> _repository;

		#endregion

		#region Constructors

		public BasePersistentService() {

		}

		#endregion

		#region Public Properties

		public IRepository<TEntity> Repository {
			get {
				_repository = Container.Resolve<IRepository<TEntity>>();
				return _repository;
			}
		}

		#endregion

		#region IPersistentService<TEntity> Members

		/// <summary>
		/// Retrieves a <see cref="TEntity"/> object from the repository by it's Id. 
		/// </summary>
		/// <param name="id">Id of the object to retrieve.</param>
		/// <returns><see cref="IQueryable<TEntity>"/>IQueryable<TEntity> object.</returns>
		public virtual IQueryable<TEntity> GetById(int id) {

			var query = from x in Repository
						where
							x.Id == id

						select x;

			return query;

		}

		/// <summary>
		/// Retrieves a list of <see cref="TEntity"/> objects from the repository. 
		/// </summary>
		/// <returns>IQueryable</returns>
		public virtual IQueryable<TEntity> GetList() {

			var query = from x in Repository
						select x;

			return query;

		}

		/// <summary>
		/// Retrieves a paged list of <see cref="TEntity"/> objects from the repository. 
		/// </summary>
		/// <param name="pageIndex">Current page to query.</param>
		/// <param name="pageSize">Number of records to bring back.</param>
		/// <param name="totalRecords">Total amount of records.</param>
		/// <returns>IQueryable</returns>
		public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, ref int totalRecords) {

			var query = from x in Repository
						select x;

			return query.Skip(pageIndex * pageSize).Take(pageSize);

		}

		#endregion

		#region Unit of Work Methods

		/// <summary>
		/// Sets the batch size for the current entity.
		/// </summary>
		/// <param name="size"></param>
		public void SetBatchSize(int batchSize) {

			Repository.SetBatchSize(batchSize);

		}

		/// <summary>
		/// Saves the entity to the datastore.
		/// </summary>
		public virtual void Save(TEntity entity) {

			try {

				Repository.Add(entity);

			} catch (Exception ex) {

				throw new Exception(ex.ToString());

			}

		}

		/// <summary>
		/// Physically deletes the entity from the datastore.
		/// </summary>
		public virtual void Delete(TEntity entity) {

			try {

				ValidateDelete(entity);

				Repository.Delete(entity);

			} catch (Exception ex) {

				throw new Exception(ex.ToString());

			}

		}

		#endregion

		#region Validation

		/// <summary>
		/// Checks to make sure the entity flagged for deletion 
		/// can be deleted.
		/// </summary>
		private void ValidateDelete(TEntity entity) {

			if (entity.IsTransient)
				throw new Exception("Cannot delete a persisted entity.");

		}

		#endregion

	}

}
