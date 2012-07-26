using SampleFramework.Domain;
using FluentNHibernate.Mapping;

namespace SampleFramework.Domain.Mappings {

	public abstract class AuditableMap<T> : ArchivableMap<T> where T : Auditable {

		protected AuditableMap() {

			Map(x => x.CreatedBy)
				.Access.CamelCaseField(Prefix.Underscore)
				.Length(50)
				.Not.Nullable();

			Map(x => x.CreatedOn)
				.Access.CamelCaseField(Prefix.Underscore)
				.Not.Nullable();

			Map(x => x.ModifiedBy)
				.Access.CamelCaseField(Prefix.Underscore)
				.Length(50)
				.Not.Nullable();

			Map(x => x.ModifiedOn)
				.Access.CamelCaseField(Prefix.Underscore)
				.Not.Nullable();

		}

	}

}
