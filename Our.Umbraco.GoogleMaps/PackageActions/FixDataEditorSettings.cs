using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Our.Umbraco.GoogleMaps.DataTypes.SingleLocation;
using Our.Umbraco.GoogleMaps.Helpers;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace Our.Umbraco.GoogleMaps.PackageActions
{
	public class FixDataEditorSettings : IPackageAction
	{
		public string Alias()
		{
			return "GoogleMaps_FixDataEditorSettings";
		}

		public bool Execute(string packageName, XmlNode xmlData)
		{
			// Due to a bug in the Umbraco core, see here for more details: http://issues.umbraco.org/issue/U4-2833
			// The package manifest does not contain the appropriate data to successfully install the `DataEditorSetting` prevalues.
			// Ultimately this need to be fixed in the core.
			// In the meantime, we have included this PackageAction to rectify the issue.
			this.FixPrevalueSettings();

			return true;
		}

		public XmlNode SampleXml()
		{
			string sample = "<Action runat=\"install\" undo=\"true\" alias=\"GoogleMaps_FixDataEditorSettings\" />";
			return helper.parseStringToXmlNode(sample);
		}

		public bool Undo(string packageName, XmlNode xmlData)
		{
			return true;
		}

		/// <summary>
		/// Adds an alias the prevalue settings that are stored in the database.
		/// </summary>
		private void FixPrevalueSettings()
		{
			var dataTypeId = new Guid(Constants.SingleLocationDataTypeId);
			var definitions = DataTypeDefinition.GetAll().Where(x => x.DataType.Id == dataTypeId);

			if (definitions != null)
			{
				var dataEditorSettings = this.Settings();
				var storage = new DataEditorSettingsStorage();

				foreach (var definition in definitions)
				{
					var settings = storage.GetSettings(definition.Id);
					var keys = settings.Select(s => s.Key);

					if (keys.Contains(null) && settings.Count == dataEditorSettings.Count)
					{
						var updatedSettings = new List<Setting<string, string>>();
						var enumerator = dataEditorSettings.GetEnumerator();

						foreach (var setting in settings)
						{
							enumerator.MoveNext();

							updatedSettings.Add(new Setting<string, string>()
							{
								Key = enumerator.Current.Key,
								Value = setting.Value
							});
						}

						storage.UpdateSettings(definition.Id, updatedSettings);
					}
				}
			}
		}

		/// <summary>
		/// Gets the DataEditorSettings.
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, DataEditorSetting> Settings()
		{
			var settings = new Dictionary<string, DataEditorSetting>();

			foreach (var property in typeof(SingleLocationDataType).GetProperties())
			{
				var attribute = property.GetCustomAttributes(typeof(DataEditorSetting), true);

				if (attribute.Length > 0)
					settings.Add(property.Name, (DataEditorSetting)attribute[0]);
			}

			return settings;
		}
	}
}