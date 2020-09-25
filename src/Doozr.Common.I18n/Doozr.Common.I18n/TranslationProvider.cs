using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Doozr.Common.I18n
{
	public class TranslationProvider : ITranslationProvider
	{
		private readonly string languageFilesPath;

		private Dictionary<CultureInfo, string> languageFiles;

		public TranslationProvider(string languageFilesPath)
		{
			this.languageFilesPath = languageFilesPath;
		}	

		public CultureInfo[] GetAvailableCultures()
		{
			if (languageFiles == null)
			{
				FillLocalizationFilesDictionary();
			}

			return languageFiles.Keys.ToArray();
		}

		private void FillLocalizationFilesDictionary()
		{
			languageFiles = new Dictionary<CultureInfo, string>();
			var languageFilesDirectoryInfo = new DirectoryInfo(languageFilesPath);
			var languageFileCandidates = languageFilesDirectoryInfo.GetFiles();
			foreach (var languagefileCandidate in languageFileCandidates)
			{
				var regex = new Regex(@"[^\.]*\.([^\.]*)\.json");
				var m = regex.Match(languagefileCandidate.Name);
				if (m.Groups.Count > 1)
				{
					var cultureCode = m.Groups[1].Value;
					var cultureInfo = CultureInfo.GetCultureInfo(cultureCode);

					languageFiles.Add(cultureInfo, languagefileCandidate.FullName);
				}
			}
		}

		public Dictionary<string, Translation> GetTranslations(CultureInfo cultureInfo)
		{
			string languageFile = languageFiles[cultureInfo];
			var languagefileText = File.ReadAllText(languageFile);
			var translations = JsonConvert.DeserializeObject<Translation[]>(languagefileText);
			var translationDict = new Dictionary<string, Translation>();
			foreach (var trans in translations)
			{
				translationDict.Add(trans.Key, trans);
			}
			return translationDict;
		}
	}
}
