using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TwitchAPI.TagHelpers
{
	// You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
	[HtmlTargetElement("signalr")]
	public class SignalRTagHelper : TagHelper
	{
		public SignalRTagHelper(IHostingEnvironment env)
		{
			this.HostingEnvironment = env;
		}

		public readonly IHostingEnvironment HostingEnvironment;

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "script";
			output.TagMode = TagMode.StartTagAndEndTag;

			var filename = IdentifyClientLibrary(new FileSystem(), HostingEnvironment.WebRootPath);

			output.Attributes.Add("src", filename);
		}

		public string IdentifyClientLibrary(IFileSystem fileSystem, string webRootPath)
		{
			var folderName = fileSystem.Path.Combine(webRootPath, "lib/signalr");
			var folder = fileSystem.DirectoryInfo.FromDirectoryName(folderName);

			var fileInfo = folder.GetFiles("signalr.min.*")
				.OrderByDescending(f => f.Name).First();

			return $"~/lib/signar/{fileInfo.Name}"; ;
		}
	}
}
