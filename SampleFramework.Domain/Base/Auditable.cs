using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Security.Permissions;

using UnitOfWorkType = SampleFramework.Domain.Enumerations.UnitOfWorkType;

namespace SampleFramework.Domain {

	/// <summary>
	/// The Auditable class contains information needed to provide
	/// an audit trail for audited objects.
	/// </summary>
	/// <remarks></remarks>
	[Serializable()]
	public abstract class Auditable : Archivable, IAuditable {

		#region Private Members

		private string _userName;
		private string _tableAffected;
		private string _auditValue;
		private string _createdBy;
		private DateTime _createdOn;
		private string _modifiedBy;
		private DateTime _modifiedOn;
		private bool _isDeleted = false;

		[field: NonSerialized]
		private Hashtable _originalData;
		private List<AuditTrail> _auditableChanges;

		#endregion

		#region Constructors

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public Auditable() {

			_createdBy = string.Empty;
			_createdOn = DateTime.Now;
			_modifiedBy = string.Empty;
			_modifiedOn = DateTime.Now;
			_isDeleted = false;
			_auditableChanges = new List<AuditTrail>();

		}

		#endregion

		#region IAuditable Members

		/// <summary>
		/// The user who is creating / modifying the current record.
		/// </summary>
		/// <value>The name of the user.</value>
		/// <returns>String</returns>
		[Audit(false)]
		public virtual string UserName {
			get { return _userName; }
			set {
				SendPropertyChanged("UserName", _userName, value);
				_userName = value;
			}
		}

		/// <summary>
		/// The table that is being transacted against.
		/// </summary>
		/// <value>The table affected.</value>
		/// <returns></returns>
		public virtual string TableAffected {
			get { return _tableAffected; }
			set {
				SendPropertyChanged("TableAffected", _tableAffected, value);
				_tableAffected = value;
			}
		}

		/// <summary>
		/// If the audit value is set, this will take precedence over
		/// the primary key on insert or delete of a record.
		/// </summary>
		/// <value>The audit value.</value>
		/// <returns></returns>
		public virtual string AuditValue {
			get { return _auditValue; }
			set {
				SendPropertyChanged("AuditValue", _auditValue, value);
				_auditValue = value;
			}
		}

		/// <summary>
		/// The user who created this record.
		/// </summary>
		/// <value>The created by.</value>
		/// <returns>String</returns>
		public virtual string CreatedBy {
			get { return _createdBy; }
			set {
				SendPropertyChanged("CreatedBy", _createdBy, value);
				_createdBy = value;
			}
		}

		/// <summary>
		/// Date this record was created.
		/// </summary>
		/// <value>The created on.</value>
		/// <returns>DateTime</returns>
		public virtual DateTime CreatedOn {
			get { return _createdOn; }
			set {
				SendPropertyChanged("CreatedOn", _createdOn, value);
				_createdOn = value;
			}
		}

		/// <summary>
		/// The user who modified this record.
		/// </summary>
		/// <value>The modified by.</value>
		/// <returns>String</returns>
		public virtual string ModifiedBy {
			get { return _modifiedBy; }
			set {
				SendPropertyChanged("ModifiedBy", _modifiedBy, value);
				_modifiedBy = value;
			}
		}

		/// <summary>
		/// Date this record was modified on.
		/// </summary>
		/// <value>The modified on.</value>
		/// <returns>DateTime</returns>
		public virtual DateTime ModifiedOn {
			get { return _modifiedOn; }
			set {
				SendPropertyChanged("ModifiedOn", _modifiedOn, value);
				_modifiedOn = value;
			}
		}

		#endregion

		#region Auditing Methods

		/// <summary>
		/// Gets the auditable changes.
		/// </summary>
		/// <returns>returns <see cref="AuditTrail"/> object representing the list of auditable changes</returns>
		public virtual List<AuditTrail> GetAuditableChanges() {
			return _auditableChanges;
		}

		/// <summary>
		/// Saves all the current information stored in the object at the
		/// time of calling this method.
		/// </summary>
		private void TakeSnapshot() {

			TakeSnapshot(false);

		}

		/// <summary>
		/// Saves all the current information stored in the object at the
		/// time of calling this method.
		/// </summary>
		/// <param name="ignoreIsPersistedProperty">Boolean value to ignore the IsPersisted property.
		/// This will need to be called when getting the Id of the record after
		/// it has been persisted into the database.</param>
		private void TakeSnapshot(bool ignoreIsPersistedProperty) {

			_originalData = new Hashtable();

			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this, true);

			foreach (PropertyDescriptor prop in properties) {

				if (prop.Name == "IsPersisted" && ignoreIsPersistedProperty) {

					_originalData.Add(prop, false);

				} else {

					_originalData.Add(prop, prop.GetValue(this));

				}

			}

		}

		/// <summary>
		/// Commits the current changes and audits the transaction.
		/// </summary>
		/// <param name="unitOfWorkType">UnitOfWorkType Enumeration</param>
		public virtual void CommitAudit(UnitOfWorkType unitOfWorkType) {

			switch (unitOfWorkType) {

				case UnitOfWorkType.Save:

					AuditChanges();

					break;

				case UnitOfWorkType.Delete:

					AuditChanges();
					AuditRemove();

					break;

			}

		}

		/// <summary>
		/// Sets created and modified fields for the current entity.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		public virtual void SetAuditFields(string userName) {

			// Only tag created if it is new
			if (!IsPersisted) {
				_createdBy = userName;
				_createdOn = DateTime.Now;
			}

			// Always tag modified information
			_modifiedBy = userName;
			_modifiedOn = DateTime.Now;

		}

		/// <summary>
		/// Audits new and field level changes.
		/// </summary>
		protected internal virtual void AuditChanges() {

			if (_originalData == null)
				return;

			bool isOriginalDataPersisted = Convert.ToBoolean(_originalData[TypeDescriptor.GetProperties(this).Find("IsPersisted", true)]);

			if (isOriginalDataPersisted) {

				PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(this);

				// Retrieve the new information (if this is an update)
				Hashtable currentData = new Hashtable();

				foreach (PropertyDescriptor prop in properties) {

					// Ignore any properties with Audit(false) attribute
					AuditAttribute auditAttribute = default(AuditAttribute);
					auditAttribute = (AuditAttribute)prop.Attributes[typeof(AuditAttribute)];

					if (auditAttribute != null && !auditAttribute.IsFieldAuditable)
						continue;

					currentData.Add(prop, prop.GetValue(this));

				}

				// Compare hashtables
				foreach (PropertyDescriptor prop in properties) {

					// Ignore these values
					if (prop.Name == "CreatedBy" ||
						prop.Name == "CreatedOn" ||
						prop.Name == "ModifiedBy" ||
						prop.Name == "ModifiedOn" ||
						prop.Name == "IsDirty" ||
						(!prop.PropertyType.IsClass && prop.PropertyType.ToString().Contains("System.Collections.Generic.IEnumerable"))
						) {

						continue;

					}

					// Ignore any properties with Audit(false) attribute
					AuditAttribute auditAttribute = default(AuditAttribute);
					auditAttribute = (AuditAttribute)prop.Attributes[typeof(AuditAttribute)];

					if (auditAttribute != null && !auditAttribute.IsFieldAuditable)
						continue;

					string originalValue = string.Empty;
					string currentValue = string.Empty;

					try {

						if (_originalData[prop] != null)
							originalValue = _originalData[prop].ToString();

						if (currentData[prop] != null)
							currentValue = currentData[prop].ToString();

					} catch (Exception ex) {

						ex.ToString();

					}

					try {

						// Compare the values
						if (!originalValue.Equals(currentValue)) {

							AuditTrail trail = new AuditTrail();
							trail.TableAffected = GetTableName();
							trail.RowIdValue = GetEntityPrimaryKey();
							trail.FieldChanged = prop.Name;
							trail.OldValue = originalValue;
							trail.NewValue = currentValue;
							trail.ModifiedBy = ModifiedBy;
							trail.ModifiedDate = ModifiedOn;

							_auditableChanges.Add(trail);

						}

					} catch (Exception ex) {

						throw ex;

					}

				}

			} else {

				AuditCreate();

			}

		}

		/// <summary>
		/// Creates an audit record for new entities going into the database.
		/// The new value will be the primary key of the entity.
		/// </summary>
		protected internal virtual void AuditCreate() {

			try {

				AuditTrail trail = new AuditTrail();
				trail.TableAffected = GetTableName();
				trail.RowIdValue = GetEntityPrimaryKey();
				trail.FieldChanged = Properties.Resources.AuditCreateRecord;
				trail.OldValue = string.Empty;

				if (!string.IsNullOrEmpty(AuditValue)) {

					trail.NewValue = AuditValue;

				} else {

					trail.NewValue = GetEntityPrimaryKey();

				}

				trail.ModifiedBy = ModifiedBy;
				trail.ModifiedDate = ModifiedOn;

				_auditableChanges.Add(trail);

			} catch (Exception ex) {

				throw ex;

			}

			try {

				// create AuditTrail records for inserting when there is a value
				foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this)) {

					// Ignore these values
					if (prop.Name == "CreatedBy" ||
						prop.Name == "CreatedOn" ||
						prop.Name == "ModifiedBy" ||
						prop.Name == "ModifiedOn" ||
						prop.Name == "IsDirty" ||
						(!prop.PropertyType.IsClass && prop.PropertyType.ToString().Contains("System.Collections.Generic.IEnumerable"))) {

						continue;

					}

					// Ignore any properties with Audit(false) attribute
					AuditAttribute auditAttribute = default(AuditAttribute);
					auditAttribute = (AuditAttribute)prop.Attributes[typeof(AuditAttribute)];

					if (auditAttribute != null && !auditAttribute.IsFieldAuditable)
						continue;

					if (prop.GetValue(this) != null && !string.IsNullOrEmpty(prop.GetValue(this).ToString())) {

						AuditTrail trail = new AuditTrail();
						trail.TableAffected = GetTableName();
						trail.RowIdValue = GetEntityPrimaryKey();
						trail.FieldChanged = prop.Name;
						trail.OldValue = string.Empty;
						trail.NewValue = prop.GetValue(this).ToString();
						trail.ModifiedBy = ModifiedBy;
						trail.ModifiedDate = ModifiedOn;

						_auditableChanges.Add(trail);

					}

				}

			} catch (Exception ex) {

				throw ex;

			}

		}

		/// <summary>
		/// Creates an audit record for the removed entity. The old
		/// value will be the primary key, and the new value will be empty.
		/// </summary>
		protected internal virtual void AuditRemove() {

			try {

				AuditTrail trail = new AuditTrail();
				trail.TableAffected = GetTableName();
				trail.RowIdValue = GetEntityPrimaryKey();
				trail.FieldChanged = Properties.Resources.AuditRemoveRecord;
				trail.NewValue = string.Empty;

				if (!string.IsNullOrEmpty(AuditValue)) {

					trail.NewValue = AuditValue;

				} else {

					trail.OldValue = GetEntityPrimaryKey();

				}

				trail.ModifiedBy = ModifiedBy;
				trail.ModifiedDate = ModifiedOn;

				_auditableChanges.Add(trail);

			} catch (Exception ex) {

				throw ex;

			}

		}

		/// <summary>
		/// Gets the primary key for this entity.
		/// </summary>
		/// <returns>returns <see cref="System.String"/> object representing the entity primary key</returns>
		private string GetEntityPrimaryKey() {

			var primaryKey = Convert.ToString(Id);

			return primaryKey;

		}

		/// <summary>
		/// Gets the table name of this entity. If it is not specified,
		/// it will then use the entity name.
		/// </summary>
		/// <returns>returns <see cref="System.String"/> object representing the table name</returns>
		private string GetTableName() {

			if (_tableAffected != null && _tableAffected.Length > 0)
				return _tableAffected;

			return Common.PluralizationService.Pluralize(this.GetType().Name);

		}

		#endregion

		#region INotifyPropertyChanged Implementation

		/// <summary>
		/// Sends the property changed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		protected override void SendPropertyChanged<T>(string propertyName, T oldValue, T newValue) {

			base.SendPropertyChanged<T>(propertyName, oldValue, newValue);

			if (_originalData == null && IsPersisted) {

				TakeSnapshot();

			} else if (_originalData == null && !IsPersisted) {

				TakeSnapshot(true);

			}

		}

		#endregion

	}

}
