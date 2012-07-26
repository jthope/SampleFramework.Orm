using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleFramework.Domain {

	/// <summary>
	/// A POCO domain class that represents the way in which
	/// we use the AuditTrail object.
	/// </summary>
	[Serializable()]
	public partial class AuditTrail : Persistent {

		#region Members

		private string _tableAffected;
		private string _rowIdValue;
		private string _fieldChanged;
		private string _oldValue;
		private string _newValue;
		private string _modifiedBy;
		private DateTime _modifiedDate;

		#endregion

		#region Constructors

		public AuditTrail() {

		}

		#endregion

		#region Properties

		public virtual string TableAffected {
			get { return _tableAffected; }
			set {

				if (!String.IsNullOrEmpty(value) && value.Length > 256)
					value = value.Substring(0, 256);

				SendPropertyChanged<string>("TableAffected", _tableAffected, value);
				_tableAffected = value;
			}
		}

		public virtual string RowIdValue {
			get { return _rowIdValue; }
			set {

				if (!String.IsNullOrEmpty(value) && value.Length > 256)
					value = value.Substring(0, 256);

				SendPropertyChanged<string>("RowIdValue", _rowIdValue, value);
				_rowIdValue = value;
			}
		}

		public virtual string FieldChanged {
			get { return _fieldChanged; }
			set {

				if (!String.IsNullOrEmpty(value) && value.Length > 256)
					value = value.Substring(0, 256);

				SendPropertyChanged<string>("FieldChanged", _fieldChanged, value);
				_fieldChanged = value;
			}
		}

		public virtual string OldValue {
			get { return _oldValue; }
			set {

				if (!String.IsNullOrEmpty(value) && value.Length > 4000)
					value = value.Substring(0, 4000);

				SendPropertyChanged<string>("OldValue", _oldValue, value);
				_oldValue = value;
			}
		}

		public virtual string NewValue {
			get { return _newValue; }
			set {

				if (!String.IsNullOrEmpty(value) && value.Length > 4000)
					value = value.Substring(0, 4000);

				SendPropertyChanged<string>("NewValue", _newValue, value);
				_newValue = value;
			}
		}

		public virtual string ModifiedBy {
			get { return _modifiedBy; }
			set {

				if (!String.IsNullOrEmpty(value) && value.Length > 50)
					value = value.Substring(0, 50);

				SendPropertyChanged<string>("ModifiedBy", _modifiedBy, value);
				_modifiedBy = value;
			}
		}

		public virtual DateTime ModifiedDate {
			get { return _modifiedDate; }
			set {

				SendPropertyChanged<DateTime>("ModifiedDate", _modifiedDate, value);
				_modifiedDate = value;
			}
		}

		#endregion

	}

}
