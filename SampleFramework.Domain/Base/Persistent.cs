using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleFramework.Domain {

	[Serializable()]
	public abstract class Persistent : Entity, IPersistent {

		#region Members

		private bool _isPersisted;

		#endregion

		#region Constructors

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public Persistent() {

			_isPersisted = false;

		}

		#endregion

		#region IPersistent Members

		/// <summary>
		/// Determines if the current record is persisted into the datastore.
		/// </summary>
		/// <value><c>true</c> if this instance is persisted; otherwise, <c>false</c>.</value>
		/// <returns>Boolean</returns>
		[Audit(false)]
		public virtual bool IsPersisted {

			get {

				if (this.Id > 0)
					_isPersisted = true;

				return _isPersisted;

			}
			set {

				SendPropertyChanged("IsPersisted", _isPersisted, value);
				_isPersisted = value;

			}

		}

		#endregion

	}

}
