using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SampleFramework.Domain {

	public interface IEntity : INotifyPropertyChanged {

		int Id { get; }
		bool IsTransient { get; }
		Enumerations.EntityState EntityState { get; }

	}

}
