using System;
using System.Linq;
using Our.Umbraco.GoogleMaps.Helpers;
using umbraco.cms.businesslogic.datatype;
using umbraco.editorControls.SettingControls;
using System.Collections.Generic;

namespace Our.Umbraco.GoogleMaps.DataTypes.SingleLocation
{
	/// <summary>
	/// Data Editor for the Google Map (Single Location) data-type.
	/// </summary>
	public class SingleLocationDataType : AbstractDataEditor
	{
		/// <summary>
		/// The Google Map: Single Location control.
		/// </summary>
		private SingleLocationControl m_Control = new SingleLocationControl();

		/// <summary>
		/// Initializes a new instance of the <see cref="SingleLocationDataType"/> class.
		/// </summary>
		public SingleLocationDataType()
		{
			// set the render control as the placeholder
			this.RenderControl = this.m_Control;

			// assign the initialise event for the control
			this.m_Control.Init += new EventHandler(this.m_Control_Init);

			// assign the value to the control
			this.m_Control.PreRender += new EventHandler(this.m_Control_PreRender);

			// assign the save event for the data-type/editor
			this.DataEditorControl.OnSave += new AbstractDataEditorControl.SaveEventHandler(this.DataEditorControl_OnSave);

            this.DataEditorControl.Init += new EventHandler(DataEditorControl_Init);
		}

		/// <summary>
		/// Gets the id of the data-type.
		/// </summary>
		/// <value>The id of the data-type.</value>
		public override Guid Id
		{
			get
			{
				return new Guid(Constants.SingleLocationDataTypeId);
			}
		}

		/// <summary>
		/// Gets the name of the data type.
		/// </summary>
		/// <value>The name of the data type.</value>
		public override string DataTypeName
		{
			get
			{
				return "Google Map"; // "Google Maps: Single Location";
			}
		}

		/// <summary>
		/// Gets or sets the default location.
		/// </summary>
		/// <value>The default location.</value>
		[DataEditorSetting("Default Location", defaultValue = Constants.DefaultCoordinates, type = typeof(TextField))]
		public string DefaultLocation { get; set; }

		/// <summary>
		/// Gets or sets the default zoom.
		/// </summary>
		/// <value>The default zoom.</value>
		[DataEditorSetting("Default Zoom", defaultValue = "12", type = typeof(TextField))]
		public string DefaultZoom { get; set; }

		/// <summary>
		/// Gets or sets the height of the map.
		/// </summary>
		/// <value>The height of the map.</value>
		[DataEditorSetting("Map Height", defaultValue = "375", type = typeof(TextField))]
		public string MapHeight { get; set; }

		/// <summary>
		/// Gets or sets the width of the map.
		/// </summary>
		/// <value>The width of the map.</value>
		[DataEditorSetting("Map Width", defaultValue = "459", type = typeof(TextField))]
		public string MapWidth { get; set; }

		/// <summary>
		/// Gets or sets the search filter.
		/// </summary>
		/// <value>the filter of the search.</value>
		[DataEditorSetting("Search Filter", defaultValue = "", type = typeof(umbraco.editorControls.SettingControls.TextField))]
		public string SearchFilter { get; set; }

		/// <summary>
		/// Gets or sets whether to use only one map point or multiple.
		/// </summary>
		/// <value>A boolean indicating whether or not only one map point or multiple will be used.</value>
		[DataEditorSetting("Use Only One Map Point", defaultValue = "False", type = typeof(umbraco.editorControls.SettingControls.CheckBox))]
		public string UseOnlyOneMapPoint { get; set; }
		//public bool UseOnlyOneMapPoint { get; set; }

		/// <summary>
		/// Handles the Init event of the m_Control control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void m_Control_Init(object sender, EventArgs e)
		{
			// get the options from the Prevalue Editor.
			this.m_Control.DefaultLocation = this.DefaultLocation;
			this.m_Control.DefaultZoom = this.DefaultZoom;
			this.m_Control.MapHeight = this.MapHeight;
			this.m_Control.MapWidth = this.MapWidth;
			this.m_Control.SearchFilter = this.SearchFilter ?? "";
			this.m_Control.UseOnlyOneMapPoint = this.UseOnlyOneMapPoint ?? "false";
		}

		/// <summary>
		/// Handles the PreRender event of the m_Control control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void m_Control_PreRender(object sender, EventArgs e)
		{
			// set the data value of the control
			this.m_Control.Data = this.Data.Value != null ? this.Data.Value.ToString() : string.Empty;
		}

		/// <summary>
		/// Datas the editor control_ on save.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void DataEditorControl_OnSave(EventArgs e)
		{
			this.Data.Value = this.m_Control.Data;
		}

        /// <summary>
        /// Handles the Init method of the DataEditorControl.
        /// </summary>
        private void DataEditorControl_Init(object sender, EventArgs e)
        {
            FixPrevalueSettings();
        }

        /// <summary>
        /// Adds an alias the prevalue settings that are stored in the database.
        /// </summary>
        private void FixPrevalueSettings()
        {
            var storage = new DataEditorSettingsStorage();
            var settings = storage.GetSettings(this.DataTypeDefinitionId);
            var keys = settings.Select(s => s.Key);

            if (keys.Contains(null))
            {
                var s = this.Settings();

                if (settings.Count == s.Count)
                {
                    var updatedSettings = new List<Setting<string, string>>();
                    var enumerator = s.GetEnumerator();

                    foreach (Setting<string, string> setting in settings)
                    {
                        enumerator.MoveNext();
                        updatedSettings.Add(new Setting<string, string>()
                        {
                            Key = enumerator.Current.Key,
                            Value = setting.Value
                        });
                    }

                    storage.UpdateSettings(this.DataTypeDefinitionId, updatedSettings);
                }
            }
        }

        /// <summary>
        /// Gets a the DataEditorSettings.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, DataEditorSetting> Settings()
        {
            Dictionary<string, DataEditorSetting> s = new Dictionary<string, DataEditorSetting>();

            foreach (System.Reflection.PropertyInfo p in this.GetType().GetProperties())
            {

                object[] o = p.GetCustomAttributes(typeof(DataEditorSetting), true);

                if (o.Length > 0)
                    s.Add(p.Name, (DataEditorSetting)o[0]);
            }

            return s;
        }
	}
}