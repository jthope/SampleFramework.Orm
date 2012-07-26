using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SampleFramework.Domain;
using SampleFramework.Domain.Enumerations;

using Castle.Windsor;

namespace SampleFramework.Services.Base {

	[Serializable()]
	public abstract class BaseAuditableService<TService, TEntity> : BaseArchivableService<TService, TEntity>
		where TEntity : class, IAuditable, IArchivable
		where TService : class, new() {

		#region Members

		#endregion

		#region Constructors

		public BaseAuditableService() {

		}

		#endregion

		#region Public Properties

		#endregion

		#region Unit of Work Methods

		internal string GetAuditUser() {

			var auditUser = "System";

			try {

				//if (!string.IsNullOrEmpty(Common.StateManager.Instance.UserName)) {
				//    auditUser = Common.StateManager.Instance.UserName;
				//}

			} catch (Exception ex) {

				// unable to find audit user.
				ex.ToString();

			}

			return auditUser;

		}

		/// <summary>
		/// Sets auditing information and saves the current 
		/// entity into the datastore.
		/// </summary>
		public override void Save(TEntity entity) {

			try {

				//if (ConfigurationService.EnableAuditTrail) {

					entity.SetAuditFields(GetAuditUser());

				//}

				base.Save(entity);

				//if (ConfigurationService.EnableAuditTrail) {

					try {

						entity.CommitAudit(UnitOfWorkType.Save);

						var auditableChanges = entity.GetAuditableChanges();
						auditableChanges.ForEach(auditTrail => {

							var auditTrailService = new AuditTrailService();
							auditTrailService.Save(auditTrail);

						});

					} catch (Exception ex) {

						ex.ToString();

					}

				//}

			} catch (Exception ex) {

				throw new Exception(ex.ToString());

			}

		}

		/// <summary>
		/// Sets auditing information and flags the current 
		/// entity as deleted.
		/// </summary>
		public override void Delete(TEntity entity) {

			try {

				//if (ConfigurationService.EnableAuditTrail) {

					entity.SetAuditFields(GetAuditUser());

				//}

				base.Delete(entity);

				//if (ConfigurationService.EnableAuditTrail) {

					try {

						entity.CommitAudit(UnitOfWorkType.Delete);

						var auditableChanges = entity.GetAuditableChanges();
						auditableChanges.ForEach(auditTrail => {

							var auditTrailService = new AuditTrailService();
							auditTrailService.Save(auditTrail);

						});

					} catch (Exception ex) {

						ex.ToString();
						// failed to audit.

					}

				//}

			} catch (Exception ex) {

				throw new Exception(ex.ToString());

			}

		}

		#endregion

	}

}
