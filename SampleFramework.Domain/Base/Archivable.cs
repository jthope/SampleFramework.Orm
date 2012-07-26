using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleFramework.Domain {

	/// <summary>
	/// The Archivable class ensures that the entity will
	/// not be physically deleted, only flagged as deleted.
	/// </summary>
	/// <remarks></remarks>
	[Serializable()]
	public abstract class Archivable : Persistent, IArchivable {

		#region Members

		private bool _isDeleted = false;

		#endregion

		#region Constructors

		/// <summary>
		/// Default Constructor.
		/// </summary>
		/// <remarks></remarks>
		public Archivable() {

			_isDeleted = false;

		}

		#endregion

		#region IArchivable Members

		/// <summary>
		/// Determines if the current record has been flagged as
		/// deleted in the datastore.
		/// </summary>
		public virtual bool IsDeleted {
			get { return _isDeleted; }
			set {
				SendPropertyChanged("IsDeleted", _isDeleted, value);
				_isDeleted = value;
			}
		}

		#endregion

	}

}
