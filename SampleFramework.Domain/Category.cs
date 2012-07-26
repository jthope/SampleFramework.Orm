using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleFramework.Domain {

	public class Category : Auditable {

		#region Members

		private Category _parentCategory;
		private string _name;
		private string _description;

		#endregion

		#region Constructors

		public Category() {

			_parentCategory = null;

		}

		#endregion

		#region Public Properties

		public virtual Category ParentCategory {
			get { return _parentCategory; }
			set {
				SendPropertyChanged<Category>("ParentCategory", _parentCategory, value);
				_parentCategory = value;			
			}
		}
		
		public virtual string Name {
			get { return _name; }
			set {
				SendPropertyChanged("Name", _name, value);
				_name = value;
			}
		}

		public virtual string Description {
			get { return _description; }
			set {
				SendPropertyChanged("Description", _description, value);
				_description = value;
			}
		}

		#endregion

	}

}
