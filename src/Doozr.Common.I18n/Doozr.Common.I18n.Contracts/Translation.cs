﻿using System;

namespace Doozr.Common.I18n
{
	[Serializable]
	public class Translation
	{
		public string CultureName { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }
	}
}
