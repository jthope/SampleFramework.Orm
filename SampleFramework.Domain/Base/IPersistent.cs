using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleFramework.Domain {

	/// <summary>
	/// Represents the base interface for the entity 
	/// </summary>
	public interface IPersistent : IEntity {

		/// <summary>
		/// Gets or sets a value indicating whether this instance is persisted.
		/// </summary>
		/// <value><c>true</c> if this instance is persisted; otherwise, <c>false</c>.</value>
		bool IsPersisted { get; set; }

	}

}
