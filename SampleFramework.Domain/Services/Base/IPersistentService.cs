using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleFramework.Services.Base {

	/// <summary>
	/// An interface to define the methods that will be required when
	/// using the PersistentService object.
	/// </summary>
	public interface IPersistentService<TEntity> {

		IQueryable<TEntity> GetById(int id);
		IQueryable<TEntity> GetList();
		IQueryable<TEntity> GetList(int pageIndex, int pageSize, ref int totalRecords);

		void SetBatchSize(int batchSize);
		void Save(TEntity entity);
		void Delete(TEntity entity);

	}

}
