using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StyleCopSettingParser
{
	class Program
	{
		static void Main(string[] args)
		{
			var fileName = "StyleCopSettings.csv";
			var outputFileName = args.FirstOrDefault() ?? @"..\..\..\Settings.StyleCop";

			var str = File.ReadAllText(fileName);
			var root = new XElement("StyleCopSettings");
			root.Add(new XAttribute("Version", "105"));
			var analyzers = new XElement("Analyzers");
			root.Add(analyzers);

			var settings = Lavn.TextFile.CsvUtility.GetCsv(str);

			foreach (var setting in settings)
			{
				var enabled = bool.Parse(setting[0]);
				var analyzerId = setting[1];
				//var ruleId = setting[2];
				var name = setting[3];

				var analyzer = analyzers.Nodes().OfType<XElement>().FirstOrDefault(x => (string)x.Attribute("AnalyzerId") == analyzerId);
				if (analyzer == null)
				{
					analyzer = new XElement("Analyzer");
					analyzer.Add(new XAttribute("AnalyzerId", analyzerId));
					analyzer.Add(new XElement("Rules"));
					analyzers.Add(analyzer);
				}

				var rules = analyzer.Nodes().Cast<XElement>().First(x => x.Name == "Rules");
				var rule = new XElement("Rule");
				rule.Add(new XAttribute("Name", name));
				var ruleSettings = new XElement("RuleSettings");
				var booleanProperty = new XElement("BooleanProperty");

				booleanProperty.Add(new XAttribute("Name", "Enabled"));
				booleanProperty.Add(enabled.ToString());	// True / False

				ruleSettings.Add(booleanProperty);
				rule.Add(ruleSettings);
				rules.Add(rule);
				analyzers.Add(analyzer);
			}

			var xDoc = new XDocument();
			xDoc.Add(root);
			xDoc.Save(outputFileName, SaveOptions.None);
		}
	}
}
