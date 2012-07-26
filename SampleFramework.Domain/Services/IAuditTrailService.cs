using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SampleFramework.Domain;

namespace SampleFramework.Services {

	public interface IAuditTrailService {

		AuditTrail GetById(int id);

		IQueryable<AuditTrail> GetList();
		IQueryable<AuditTrail> GetList(int pageIndex, int pageSize, ref int totalRecords);
		IQueryable<AuditTrail> GetListByCriteria(string tableAffected, string rowIDValue, string oldValue, string newValue, string fieldChanged, string modifiedBy,
												 DateTime? modifiedStartDate, DateTime? modifiedEndDate, int pageIndex, int pageSize, ref int totalRecords);
		
		void Save(AuditTrail auditTrail);
		void Delete(AuditTrail auditTrail);

	}

}
