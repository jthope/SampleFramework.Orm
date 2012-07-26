using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SampleFramework.Domain;

namespace SampleFramework.Services {

	public class CategoryService : Base.BaseAuditableService<CategoryService, Category>, ICategoryService {
		
		#region ICategoryService Members

		public new Category GetById(int id) {

			var query = from x in base.GetById(id)
						select x;

			return query.SingleOrDefault();

		}

		public new IQueryable<Category> GetList() {

			var query = from x in base.GetList()
						select x;

			return query;

		}

		public new IQueryable<Category> GetList(int pageIndex, int pageSize, ref int totalRecords) {

			var query = from x in base.GetList()

						orderby
							x.Id

						select x;

			totalRecords = query.Count();

			return query.Skip(pageIndex * pageSize).Take(pageSize);

		}

		public int GetTotalRecords() {

			var query = from x in Repository
						select x;

			return query.Count();

		}

		public override void Save(Category category) {

			base.Save(category);

		}

		public override void Delete(Category category) {

			base.Delete(category);

		}

		#endregion

	}

}
