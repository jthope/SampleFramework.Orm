using SampleFramework.Domain;
using FluentNHibernate.Mapping;

namespace SampleFramework.Domain.Mappings {

	public abstract class ArchivableMap<T> : EntityMap<T> where T : Archivable {

		protected ArchivableMap() {

			Map(x => x.IsDeleted)
				.Access.CamelCaseField(Prefix.Underscore)
				.Not.Nullable();

		}

	}

}
