using SampleFramework.Domain;
using FluentNHibernate.Mapping;

namespace SampleFramework.Domain.Mappings {

	public abstract class EntityMap<T> : ClassMap<T> where T : Entity {

		protected EntityMap() {

			LazyLoad();

			Table(Common.PluralizationService.Pluralize(typeof(T).Name));
			
			Id(x => x.Id)
				.GeneratedBy.Identity()
				.Access.ReadOnlyPropertyThroughCamelCaseField(Prefix.Underscore);
		
		}

	}

}
