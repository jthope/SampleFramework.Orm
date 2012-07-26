using SampleFramework.Domain;

namespace SampleFramework.Domain.Mappings {

	public class CategoryMap : AuditableMap<Category> {

		public CategoryMap() {

			References(x => x.ParentCategory)
				.Nullable();

			Map(x => x.Name).Not.Nullable();
			Map(x => x.Description);

		}

	}

}
