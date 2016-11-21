using System;
using Microsoft.Owin.Hosting;
using Serilog;
using Mono.Options;

namespace Reston.Pinata.Runner
{
	class Host
	{
		public static void Main (string[] args)
		{
			Console.Title = "Owin Self Host";

			Log.Logger = new LoggerConfiguration ()
			.WriteTo
			.LiterateConsole (outputTemplate: "{Timestamp:HH:MM} [{Level}] ({Name:l}){NewLine} {Message}{NewLine}{Exception}")
			.CreateLogger ();

			var proto = "http";
			var host = "*";
			var	port = 8080;
			var path = "";
			bool showHelp = false;
			var startupClass = "";

			var opts = new OptionSet ()
			.Add ("?|help|h", "Show this help message", opt => showHelp = opt != null)
			.Add ("p=|proto=", "HTTP protocol [http (default), https]", opt => proto = opt.Trim ().ToLower ())
			.Add ("host=", "Bind to address (default is '*' for all address)", opt => host = opt.Trim ().ToLower ())
			.Add ("port=", "Bind port [default is 8080]", opt => port = Int32.Parse (opt))
			.Add ("path=", "Root context path [none (default)]", opt => path = (opt.StartsWith ("/") ? opt.Substring (1) : opt))
			.Add ("startup=", "Owin Startup configuration type name, may be followed by assembly name", opt => startupClass = opt);

			try {
				opts.Parse (args);
			} catch (OptionException e) {
				Host.help (opts, e.Message);
				Environment.Exit (-1);
			}

			if (showHelp) {
				Host.help (opts, "");
				Environment.Exit (0);
			}

			if (startupClass == null || startupClass.Trim ().Equals ("")) {
				Host.help (opts, "Required parameter missing: config");
				Environment.Exit (-1);
			}

			//----

			var url = String.Format ("{0}://{1}:{2}/{3}"
			, proto, host, port, path);

			var startOpts = new StartOptions (url);
			startOpts.AppStartup = startupClass;

			using (WebApp.Start (startOpts)) {
				Console.WriteLine ("Listening on {0}", url);
				Console.WriteLine ("Press any key to quit");
				Console.ReadLine ();
			}

		}

		public static void help (OptionSet opts, string msg)
		{
			Console.WriteLine (msg);
			opts.WriteOptionDescriptions (Console.Out);
		}
	}
}
