using System;
using System.Collections.Generic;
using System.IO;

namespace Extrospection {
	class Reporter {

		static string InputDirectory { get; set; } = ".";
		static string ReportFolder { get; set; } = "report";

		static SortedSet<string> Frameworks = new SortedSet<string> (StringComparer.OrdinalIgnoreCase);

		static readonly string [] Platforms = new [] { "iOS", "tvOS", "macOS", "MacCatalyst" };

		public static bool ProcessFramework (string framework)
		{
			bool data = false;
			// merge the shared and specialized ignore data into a single html page
			List<string> ignore = new List<string> ();
			ignore.Add ($"<h1>{framework}</h1>");
			var filename = Path.Combine (InputDirectory, $"common-{framework}.ignore");
			if (File.Exists (filename)) {
				data = true;
				ignore.Add ("<h2>Common (shared) ignored results</h2>");
				ignore.Add ("<xmp>");
				foreach (var line in File.ReadAllLines (filename)) {
					ignore.Add (line);
				}
				ignore.Add ("</xmp>");
			}
			foreach (var platform in Platforms) {
				filename = Path.Combine (InputDirectory, $"{platform}-{framework}.ignore");
				if (File.Exists (filename)) {
					data = true;
					ignore.Add ($"<h2>{platform} specific ignored results</h2>");
					ignore.Add ("<xmp>");
					foreach (var line in File.ReadAllLines (filename)) {
						ignore.Add (line);
					}
					ignore.Add ("</xmp>");
				}
			}
			var output = Path.Combine (ReportFolder, framework) + ".ignore.html";
			File.WriteAllLines (output, ignore);
			return data;
		}

		public static int ProcessFile (string filename)
		{
			if (!File.Exists (filename))
				return 0;
			int count = 0;
			var output = Path.Combine (ReportFolder, Path.GetFileName (filename)) + ".html";
			var name = Path.GetFileNameWithoutExtension (filename);
			List<string> html = new List<string> ();
			html.Add ($"<html><head><title>{name}</title></head>");
			html.Add ($"<body><h1>{name}</h1><xmp>");
			foreach (var line in File.ReadAllLines (filename)) {
				html.Add (line);
				if ((line.Length > 0) && (line [0] == '!'))
					count++;
			}
			html.Add ("</xmp></body></html>");
			File.WriteAllLines (output, html);
			return count;
		}

		static void AddFramework (string file)
		{
			var filename = Path.GetFileNameWithoutExtension (file);
			var fx = filename.Substring (filename.IndexOf ('-') + 1);
			if (!Frameworks.Contains (fx))
				Frameworks.Add (fx);
		}

		public static int Main (string [] args)
		{
			InputDirectory = args.Length == 0 ? "." : args [0];

			// collapse the ignored entries on jenkins bots - focus in on what's needs fixing (for the PR) and the work 'to do'
			bool full = String.IsNullOrEmpty (Environment.GetEnvironmentVariable ("JENKINS_SERVER_COOKIE"));

			int width = 100 / ((full ? 2 : 1) + (full ? 3 : 2) * Platforms.Length);

			var allfiles = new List<string> ();

			ReportFolder = args.Length > 1 ? args [1] : "report";
			Directory.CreateDirectory (ReportFolder);
			var indexPath = Path.Combine (ReportFolder, "index.html");
			var log = new StreamWriter (indexPath);

			#region "Setup HTML Header and styles"
			log.WriteLine ("<html><head><title>Extrospection results</title>"
				+ @"<style>
									:root {
										--bg: #fff;
										--fg: #222;
										--green: #008100;
										--green-light: #90EE90;
										--red: #FF0200;
										--red-light: #FA8072;
										--yellow: #FFA502;
										--yellow-light: #FFDAB9;
										--grey: #e0e0e0;
										--switch-bg: #ccc;
										--switch-knob: #fff;
									}
									body.dark {
										--bg: #181a1b;
										--fg: #f1f1f1;
										--green: #388e3c;
										--green-light: #23422a;
										--red: #e57373;
										--red-light: #3a2323;
										--yellow: #ffd54f;
										--yellow-light: #4a3a1a;
										--grey: #23272a;
										--switch-bg: #555 !important;
										--switch-knob: #f1f1f1 !important;
									}
						body { background: var(--bg); color: var(--fg); transition: background 0.2s, color 0.2s; }
						.switch-container { position: absolute; top: 16px; right: 24px; z-index: 10; }
						.switch-label { cursor: pointer; font-size: 1em; user-select: none; color: var(--fg); }
						.switch-input { display: none; }
						.switch-slider { display: inline-block; width: 44px; height: 24px; background: var(--switch-bg) !important; border-radius: 12px; position: relative; vertical-align: middle; transition: background 0.2s; border: 1px solid #888; }
						.switch-slider:before { content: ''; position: absolute; left: 3px; top: 3px; width: 18px; height: 18px; background: var(--switch-knob) !important; border-radius: 50%; transition: left 0.2s, background 0.2s; box-shadow: 0 1px 3px #0002; border: 1px solid #888; }
						.switch-input:not(:checked) + .switch-slider { background: var(--switch-bg) !important; }
						.switch-input:checked + .switch-slider { background: #444 !important; }
						.switch-input:checked + .switch-slider:before { left: 23px; background: #222 !important; }
						.switch-label span { margin-left: 8px; }
						table { background: var(--bg); color: var(--fg); }
						td, th { color: var(--fg); }
						td[bgcolor='lightgreen'], th[bgcolor='lightgreen'] { background: var(--green-light) !important; color: var(--fg); }
						td[bgcolor='green'], th[bgcolor='green'] { background: var(--green) !important; color: #222; }
						td[bgcolor='red'], th[bgcolor='red'] { background: var(--red) !important; color: #222; }
						body.dark td[bgcolor='green'], body.dark th[bgcolor='green'] { color: #fff; }
						body.dark td[bgcolor='red'], body.dark th[bgcolor='red'] { color: #fff; }
						td[bgcolor='salmon'], th[bgcolor='salmon'] { background: var(--red-light) !important; color: var(--fg); }
						td[bgcolor='orange'], th[bgcolor='orange'] { background: var(--yellow) !important; color: #222; }
						td[bgcolor='peachpuff'], th[bgcolor='peachpuff'] { background: var(--yellow-light) !important; color: var(--fg); }
						td[bgcolor='lightgrey'], th[bgcolor='lightgrey'] { background: var(--grey) !important; color: var(--fg); }
			</style>"
				+ @"<script>
						function setMode(dark, user) {
							document.body.classList.toggle('dark', dark);
							if (user) {
								document.documentElement.classList.add('xtro-user');
								localStorage.setItem('xtro-dark', dark ? '1' : '0');
							}
							updateSwitch();
						}
						function updateSwitch() {
							var input = document.getElementById('xtro-dark-switch');
							if (!input) return;
							// checked state should match body.dark class
							input.checked = document.body.classList.contains('dark');
						}
						window.addEventListener('DOMContentLoaded', function() {
							var userPref = localStorage.getItem('xtro-dark');
							var dark = false;
							if (userPref === '1') dark = true;
							else if (userPref === '0') dark = false;
							else dark = window.matchMedia('(prefers-color-scheme: dark)').matches;
							setMode(dark, false);
							var input = document.getElementById('xtro-dark-switch');
							if (input) input.addEventListener('change', function() { setMode(this.checked, true); });
							window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', function(e) {
								if (!localStorage.getItem('xtro-dark')) {
									setMode(e.matches, false);
								}
							});
						});
			</script>"
				+ "</head>");
			log.WriteLine ("<body>"
				+ "<div class='switch-container'>"
				+ "<label class='switch-label'>"
				+ "<input type='checkbox' class='switch-input' id='xtro-dark-switch'>"
				+ "<span class='switch-slider'></span>"
				+ "<span>Dark Mode</span>"
				+ "</label></div>"
				+ "<h1>Extrospection results</h1>");
			#endregion "Setup HTML Header and styles"

			log.WriteLine ("<table border='0' cellpadding='4' cellspacing='0'>");

			log.WriteLine ("<thead>");
			log.WriteLine ("<tr>");
			log.WriteLine ("<td rowspan='3' bgcolor='lightgrey'>Frameworks</td>");
			if (full)
				log.WriteLine ($"<td align='center' bgcolor='green' colspan='{Platforms.Length + 1}'>REVIEWED (ignored)</td>");
			log.WriteLine ($"<td align='center' bgcolor='red' colspan='{Platforms.Length}'>FIXME (unclassified)</td>");
			log.WriteLine ($"<td align='center' bgcolor='orange' colspan='{Platforms.Length}'>TODO (milestone)</td>");
			log.WriteLine ("</tr>");

			log.WriteLine ("<tr>");
			if (full)
				log.WriteLine ($"<td align='center' bgcolor='green' width='{width}%'>Common</td>");
			foreach (var platform in Platforms) {
				if (full)
					log.WriteLine ($"<td align='center' bgcolor='green' width='{width}%'>{platform}</td>");
				var files = Directory.GetFiles (InputDirectory, $"{platform}-*.ignore");
				foreach (var file in files) {
					AddFramework (file);
				}
			}
			foreach (var platform in Platforms) {
				log.WriteLine ($"<td align='center' bgcolor='red' width='{width}%'>{platform}</td>");
				var files = Directory.GetFiles (InputDirectory, $"{platform}-*.unclassified");
				foreach (var file in files) {
					allfiles.Add (file);
					AddFramework (file);
				}
				var todos = Directory.GetFiles (InputDirectory, $"{platform}-*.todo");
				foreach (var file in todos) {
					AddFramework (file);
				}
			}
			foreach (var platform in Platforms)
				log.WriteLine ($"<td align='center' bgcolor='orange' width='{width}%'>{platform}</td>");
			log.WriteLine ("</tr>");

			var cols = (full ? 3 : 2) * Platforms.Length + (full ? 1 : 0);
			log.WriteLine ("<tr>");
			log.WriteLine ($"<td colspan='{cols + 1}' cellspanning='4'></td>");
			log.WriteLine ("</tr>");

			log.WriteLine ("</thead>");

			var ignored = new int [Platforms.Length + 1];
			var unclassified = new int [Platforms.Length];
			var todo = new int [Platforms.Length];
			int errors = 0;

			foreach (var fx in Frameworks) {
				if (Helpers.Filter (fx))
					continue;
				log.WriteLine ("<tr>");
				log.Write ("<td>");
				if (!full && ProcessFramework (fx))
					log.Write ($"<a href=\"{fx}.ignore.html\">{fx}</a>");
				else
					log.Write (fx);
				log.WriteLine ("</td>");
				if (full) {
					string filename = $"common-{fx}.ignore";
					var count = ProcessFile (Path.Combine (InputDirectory, filename));
					log.Write ("<td align='center' ");
					if (count < 1)
						log.Write ("bgcolor='lightgreen'>-</td>");
					else
						log.Write ($"bgcolor='green'><a href=\"{filename}.html\">{count}</a>");
					log.WriteLine ("</td>");
					ignored [0] += count;
					for (int i = 0; i < Platforms.Length; i++) {
						filename = $"{Platforms [i]}-{fx}.ignore";
						count = ProcessFile (Path.Combine (InputDirectory, filename));
						log.Write ("<td align='center' ");
						if (count < 1)
							log.Write ("bgcolor='lightgreen'>-");
						else
							log.Write ($"bgcolor='green'><a href=\"{filename}.html\">{count}</a>");
						log.WriteLine ("</td>");
						ignored [i + 1] += count;
					}
				}
				for (int i = 0; i < Platforms.Length; i++) {
					string filename = $"{Platforms [i]}-{fx}.unclassified";
					var count = ProcessFile (Path.Combine (InputDirectory, filename));
					log.Write ("<td align='center'");
					if (count < 1)
						log.Write (" bgcolor='salmon'>-");
					else
						log.Write ($"bgcolor='red'><a href=\"{filename}.html\">{count}</a>");
					log.WriteLine ("</td>");
					unclassified [i] += count;
					errors += count;
				}
				for (int i = 0; i < Platforms.Length; i++) {
					string filename = $"{Platforms [i]}-{fx}.todo";
					var count = ProcessFile (Path.Combine (InputDirectory, filename));
					log.Write ("<td align='center' ");
					if (count <= 0)
						log.Write ("bgcolor='peachpuff'>-");
					else
						log.Write ($"bgcolor='orange'><a href=\"{filename}.html\">{count}</a>");
					log.WriteLine ("</td>");
					todo [i] += count;
				}
				log.WriteLine ("</tr>");
			}
			log.WriteLine ("<tfoot>");
			log.WriteLine ("<tr>");
			log.WriteLine ($"<td colspan='{cols}' cellspanning='4'></td>");
			log.WriteLine ("</tr>");
			log.WriteLine ("<tr>");
			log.WriteLine ("<td>Total (per platform)</td>");
			var total_ignored = 0;
			if (full) {
				for (int i = 0; i < Platforms.Length + 1; i++) {
					log.Write ("<td align='center' ");
					var count = ignored [i];
					if (count <= 0)
						log.Write ("bgcolor='lightgreen'>-</td>");
					else
						log.Write ($"bgcolor='green'>{count}</a>");
					log.WriteLine ("</td>");
					total_ignored += count;
				}
			}
			var total_unclassfied = 0;
			for (int i = 0; i < Platforms.Length; i++) {
				log.Write ("<td align='center' ");
				var count = unclassified [i];
				if (count <= 0)
					log.Write ("bgcolor='salmon'>-</td>");
				else
					log.Write ($"bgcolor='red'>{count}</a>");
				log.WriteLine ("</td>");
				total_unclassfied += count;
			}
			var total_todo = 0;
			for (int i = 0; i < Platforms.Length; i++) {
				log.Write ("<td align='center' ");
				var count = todo [i];
				if (count <= 0)
					log.Write ("bgcolor='peachpuff'>-</td>");
				else
					log.Write ($"bgcolor='orange'>{count}</a>");
				log.WriteLine ("</td>");
				total_todo += count;
			}
			log.WriteLine ("</tr>");
			log.WriteLine ("<tr>");
			log.WriteLine ("<td>Total (per state)</td>");
			if (full)
				log.WriteLine ($"<td align='center' bgcolor='green' colspan='{Platforms.Length + 1}'>{total_ignored}</td>");
			log.WriteLine ($"<td align='center' bgcolor='red' colspan='{Platforms.Length}'>{total_unclassfied}</td>");
			log.WriteLine ($"<td align='center' bgcolor='orange' colspan='{Platforms.Length}'>{total_todo}</td>");
			log.WriteLine ("</tr>");

			log.WriteLine ("<tr>");
			log.WriteLine ($"<td colspan='{cols}' cellspanning='4'></td>");
			log.WriteLine ("</tr>");

			log.WriteLine ("<tr>");
			log.WriteLine ($"<td align='center' colspan='{cols + 1}' ");
			if (total_unclassfied == 0)
				log.WriteLine ($"bgcolor='lightgreen'>SUCCESS");
			else
				log.WriteLine ($"bgcolor='red'>FAILURE");
			log.WriteLine ($"</td>");
			log.WriteLine ("</tr>");

			log.WriteLine ("</tfoot>");
			log.WriteLine ("</table>");

			log.WriteLine ("</body>");
			log.WriteLine ("</html>");
			log.Flush ();

			Console.WriteLine ($"@MonkeyWrench: SetSummary: {errors} unclassified found.");
			Console.WriteLine ($"Created {Path.GetFullPath (indexPath)}");

			return errors == 0 ? 0 : 1;
		}
	}
}
