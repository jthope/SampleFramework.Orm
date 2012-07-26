using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SampleFramework.Domain;
using NCommon.Data;

namespace SampleFramework.Services {

	public class AuditTrailService : Base.BasePersistentService<AuditTrailService, AuditTrail>, IAuditTrailService {

		#region IAuditTrailService Members

		public new AuditTrail GetById(int id) {

			var query = from x in base.GetById(id)
						select x;

			return query.SingleOrDefault();

		}

		public new IQueryable<AuditTrail> GetList() {

			var query = from x in base.GetList()
						select x;

			return query;

		}

		public new IQueryable<AuditTrail> GetList(int pageIndex, int pageSize, ref int totalRecords) {

			var query = from x in base.GetList()

						orderby
							x.Id descending

						select x;

			totalRecords = query.Count();

			return query.Skip(pageIndex * pageSize).Take(pageSize);

		}

		public IQueryable<AuditTrail> GetListByCriteria(string tableAffected, string rowIDValue, string oldValue, string newValue, string fieldChanged, string modifiedBy,
														DateTime? modifiedStartDate, DateTime? modifiedEndDate, int pageIndex, int pageSize, ref int totalRecords) {

			var query = from x in base.GetList()

						where
							(string.IsNullOrEmpty(tableAffected) || x.TableAffected == tableAffected) &&
							(string.IsNullOrEmpty(rowIDValue) || x.RowIdValue == rowIDValue) &&
							(string.IsNullOrEmpty(oldValue) || x.OldValue.Contains(oldValue)) &&
							(string.IsNullOrEmpty(newValue) || x.NewValue.Contains(newValue)) &&
							(string.IsNullOrEmpty(fieldChanged) || x.FieldChanged.Contains(fieldChanged)) &&
							(string.IsNullOrEmpty(modifiedBy) || x.ModifiedBy.Contains(modifiedBy)) &&
							(!modifiedStartDate.HasValue || x.ModifiedDate >= modifiedStartDate) &&
							(!modifiedEndDate.HasValue || x.ModifiedDate <= modifiedEndDate)

						select x;

			totalRecords = query.Count();

			return query.Skip(pageIndex * pageSize).Take(pageSize);

		}

		public override void Save(AuditTrail auditTrail) {

			base.Save(auditTrail);

		}

		public override void Delete(AuditTrail auditTrail) {

			base.Delete(auditTrail);

		}

		#endregion

	}

}
