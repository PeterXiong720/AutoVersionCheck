using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomaticVersionCheckAPI.Models
{
	public class Data
	{
		public string AppName { get; set; }
		public string AccessKey { get; set; }
		public AppVersion Version { get; set; }
	}
}

