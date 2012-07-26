using System;
using System.Reflection;

using NCommon.Util;

using FluentNHibernate;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Conventions.Inspections;

namespace SampleFramework.Domain.Mappings {

	public class TableNameConvention : IClassConvention {

		public void Apply(FluentNHibernate.Conventions.Instances.IClassInstance instance) {

			string typeName = instance.EntityType.Name;
			instance.Table(Inflector.Pluralize(typeName));

		}

	}

	public class PrimaryKeyConvention : IIdConvention {

		public void Apply(IIdentityInstance instance) {

			var colName = "Id";
			var sqName = Inflector.Pluralize(instance.EntityType.Name);

			if (sqName.Length > 27) {
				sqName = sqName.Substring(0, 26);
			}

			sqName = sqName.Insert(0, "SQ_");

			instance.Column(colName);
			instance.Access.CamelCaseField(CamelCasePrefix.Underscore);
			instance.UnsavedValue("0");
			instance.GeneratedBy.Sequence(sqName);

		}

	}

	public class CustomForeignKeyConvention : ForeignKeyConvention {

		protected override string GetKeyName(Member property, Type type) {

			if (property == null)
				return type.Name + "Id";

			return property.Name + "Id";

		}
	}

	public class ColumnNameConvention : IPropertyConvention {

		#region IConvention<IPropertyInspector,IPropertyInstance> Members

		public void Apply(IPropertyInstance instance) {

			instance.Column(instance.Property.Name);
			instance.Access.CamelCaseField(CamelCasePrefix.Underscore);

		}

		#endregion

	}

	public class CollectionConvention : ICollectionConvention {

		#region IConvention<ICollectionInspector,ICollectionInstance> Members

		public void Apply(ICollectionInstance instance) {

			instance.Access.CamelCaseField(CamelCasePrefix.Underscore);

		}

		#endregion

	}


	//public class CustomForeignKeyConvention : ForeignKeyConvention {

	//    protected override string GetKeyName(FluentNHibernate.Member property, Type type) {

	//        if (property == null)
	//            return type.Name + "Id";

	//        return property.Name + "Id";

	//    }

	//}

	//public class ForeignKeyConstraintNameConvention : IHasManyConvention, IReferenceConvention {

	//    #region IConvention<IOneToManyCollectionInspector,IOneToManyCollectionInstance> Members

	//    public void Apply(IOneToManyCollectionInstance instance) {
	//        var keyName = String.Format("FK_{0}_{1}", instance.Member.Name, Inflector.Pluralize(instance.EntityType.Name));
	//        instance.Key.ForeignKey(keyName);
	//    }

	//    #endregion

	//    #region IConvention<IManyToOneInspector,IManyToOneInstance> Members

	//    public void Apply(IManyToOneInstance instance) {
	//        var keyName = String.Format("FK_{0}_{1}", Inflector.Pluralize(instance.EntityType.Name), instance.Property.PropertyType.Name);
	//        instance.ForeignKey(keyName);
	//    }

	//    #endregion

	//}

}
