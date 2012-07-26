using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SampleFramework.Domain;
using SampleFramework.Services;

using NCommon.Data;

namespace SampleFramework.Web {

	public partial class Default : System.Web.UI.Page {

		protected void Page_Load(object sender, EventArgs e) {

			if (!IsPostBack) {

				PopulateCategories();
				BindGridView();
				BindAuditTrail();

			}

		}

		public void BindGridView() {

			using (var uow = new UnitOfWorkScope()) {

				var totalCategories = 0;

				uxCategoriesGrid.DataSource =
					CategoryService.Instance.GetList(uxCategoriesGrid.CurrentPageIndex,
													 uxCategoriesGrid.PageSize,
													 ref totalCategories).ToList();

				uxCategoriesGrid.VirtualItemCount = totalCategories;
				uxCategoriesGrid.DataBind();

				// commit the transaction
				uow.Commit();

			}

		}

		public void BindAuditTrail() {

			using (var uow = new UnitOfWorkScope()) {

				var totalAudits = 0;

				uxAuditTrailGrid.DataSource =
					AuditTrailService.Instance.GetList(uxAuditTrailGrid.CurrentPageIndex,
													 uxAuditTrailGrid.PageSize,
													 ref totalAudits).ToList();

				uxAuditTrailGrid.VirtualItemCount = totalAudits;
				uxAuditTrailGrid.DataBind();

				// commit the transaction
				uow.Commit();

			}

		}

		protected void uxCategoriesGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {

			uxCategoriesGrid.CurrentPageIndex = e.NewPageIndex;
			BindGridView();

		}

		protected void uxAuditTrailGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e) {

			uxAuditTrailGrid.CurrentPageIndex = e.NewPageIndex;
			BindAuditTrail();

		}

		public void PopulateCategories() {
		
			// this will initially populate the Categories table if there is no data in it.
			// also showing the auto commit parameter (UnitOfWorkScopeTransactionOptions.AutoComplete)
			using (var uow = new UnitOfWorkScope(UnitOfWorkScopeTransactionOptions.AutoComplete)) {

				if (CategoryService.Instance.GetTotalRecords() == 0) {

					var category1 = new Category { Name = "Computers", Description = "Computers Category." };
					var category2 = new Category { Name = "Apple", ParentCategory = category1, Description = "Apple Sub-category of Computers." };

					var category3 = new Category { Name = "Entertainment", Description = "Entertainment Category." };
					var category4 = new Category { Name = "TV", ParentCategory = category3, Description = "TV Sub-category of Entertainment." };

					var category5 = new Category { Name = "Music", Description = "Music Category." };
					var category6 = new Category { Name = "Rock", ParentCategory = category5, Description = "Rock Sub-cateogry of Music." };
					var category7 = new Category { Name = "Progressive", ParentCategory = category6, Description = "Progressive Rock Sub-category of Rock." };

					// categories
					var categories = new List<Category>();
					categories.Add(category1);
					categories.Add(category2);
					categories.Add(category3);
					categories.Add(category4);
					categories.Add(category5);
					categories.Add(category6);
					categories.Add(category7);

					categories.ForEach(category => CategoryService.Instance.Save(category));

				}

			}

		}

		protected void uxEditRecord_Click(object sender, EventArgs e) {

			// lets edit a record to show the changes in the audit trail
			// start a new unit of work
			using (var uow = new UnitOfWorkScope(UnitOfWorkScopeTransactionOptions.AutoComplete)) {

				var category = CategoryService.Instance.GetById(7);
				category.Name = "Prog Rock";

				CategoryService.Instance.Save(category);

			}

			BindAuditTrail();
			
		}

	}

}