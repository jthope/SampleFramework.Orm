using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleFramework.Domain {

	public interface IAuditable : IPersistent {

		string UserName { get; set; }
		string TableAffected { get; set; }
		string AuditValue { get; set; }
		string CreatedBy { get; set; }
		DateTime CreatedOn { get; set; }
		string ModifiedBy { get; set; }
		DateTime ModifiedOn { get; set; }

		List<AuditTrail> GetAuditableChanges();

		void CommitAudit(Domain.Enumerations.UnitOfWorkType unitOfWorkType);
		void SetAuditFields(string userName);

	}

}
