using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleFramework.Domain {

	public interface IArchivable : IPersistent {

		bool IsDeleted { get; set; }

	}

}
