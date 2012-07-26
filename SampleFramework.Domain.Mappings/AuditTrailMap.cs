using SampleFramework.Domain;

namespace SampleFramework.Domain.Mappings {
	
	public class AuditTrailMap : EntityMap<AuditTrail> {

		public AuditTrailMap() {

			Map(x => x.TableAffected);
			Map(x => x.RowIdValue);
			Map(x => x.FieldChanged);
			Map(x => x.OldValue);
			Map(x => x.NewValue);
			Map(x => x.ModifiedBy).Not.Nullable();
			Map(x => x.ModifiedDate).Not.Nullable();

		}

	}

}
