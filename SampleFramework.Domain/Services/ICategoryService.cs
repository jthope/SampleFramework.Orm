using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SampleFramework.Domain;

namespace SampleFramework.Services {

	public interface ICategoryService {

		Category GetById(int id);

		IQueryable<Category> GetList();
		IQueryable<Category> GetList(int pageIndex, int pageSize, ref int totalRecords);

		int GetTotalRecords();

		void Save(Category category);
		void Delete(Category category);

	}

}
