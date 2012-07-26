using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using NCommon.Util;

namespace SampleFramework.Common {

	// Current inflector is not so great, will come back to this later.
	public class PluralizationService {

		public static string Pluralize(string word) {

			return Inflector.Pluralize(word);

		}

		public static string Singularize(string word) {

			return Inflector.Singularize(word);

		}

	}

}
