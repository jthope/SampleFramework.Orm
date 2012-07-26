using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;

namespace SampleFramework.Domain {

	public abstract class Entity : IEntity {

		#region Members

		private int _id;
		private int? _oldHashCode;
		private Enumerations.EntityState _entityState;
		private static PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

		#endregion

		#region Constructors

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public Entity() {

			_id = 0;

		}

		#endregion

		#region IEntity Members

		/// <summary>
		/// Primary key field of the entity.
		/// </summary>
		public virtual int Id {
			get { return _id; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is transient.
		/// </summary>
		[Audit(false)]
		public virtual bool IsTransient {
			get { return Equals(Id, default(int)); }
		}

		/// <summary>
		/// Determines if the current record has been modified.
		/// </summary>
		/// <returns>Boolean</returns>
		[Audit(false)]
		public virtual bool IsDirty {
			get { return this.EntityState != Enumerations.EntityState.Unchanged; }
		}

		/// <summary>
		/// Determines the current state of the entity.
		/// </summary>
		[Audit(false)]
		[XmlIgnore()]
		public virtual Enumerations.EntityState EntityState {
			get { return _entityState; }
			private set { _entityState = value; }
		}

		#endregion

		#region INotifyPropertyChanged Members

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public virtual event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region INotifyPropertyChanged Implementation

		/// <summary>
		/// Fires the property changed.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		protected void FirePropertyChanged(string propertyName) {
			if (this.PropertyChanged != null) {
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Sends the property changed.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		protected virtual void SendPropertyChanged<T>(string propertyName, T oldValue, T newValue) {

			try {

				if (oldValue == null && newValue == null) {
					// nothing changed.
				}

				if ((oldValue == null && newValue != null) || !oldValue.Equals((T)newValue)) {

					_entityState = Domain.Enumerations.EntityState.Modified;

					FirePropertyChanged(propertyName);

				}

			} catch (Exception ex) {

				ex.ToString();

			}

		}

		#endregion

		#region Overrides

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
		public override string ToString() {

			if (IsTransient)
				return string.Format("Transient instance of {0}", GetType().Name);

			return string.Format("{0} with Id of {1}", GetType().Name, Id);

		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
		/// <exception cref="T:System.NullReferenceException">
		/// The <paramref name="obj"/> parameter is null.
		///   </exception>
		public override bool Equals(object obj) {

			var other = obj as Entity;

			if (other == null)
				return false;

			if (other.IsTransient && this.IsTransient)
				return ReferenceEquals(other, this);

			return other.Id.Equals(this.Id);

		}

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
		public override int GetHashCode() {

			if (_oldHashCode.HasValue)
				return _oldHashCode.Value;

			if (IsTransient)
				return (_oldHashCode = base.GetHashCode()).Value;

			return Id.GetHashCode();

		}

		#endregion

		/// <summary>
		/// Implements the operator ==.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator ==(Entity x, Entity y) {
			return Equals(x, y);
		}

		/// <summary>
		/// Implements the operator !=.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns>The result of the operator.</returns>
		public static bool operator !=(Entity x, Entity y) {
			return !(x == y);
		}

	}

}