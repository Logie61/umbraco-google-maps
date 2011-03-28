using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using ClientDependency.Core;
using Our.Umbraco.GoogleMaps.Controls;
using Our.Umbraco.GoogleMaps.Extensions;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.editorControls.SettingControls;
using umbraco.interfaces;

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
		/// Gets or sets the default location.
		/// </summary>
		/// <value>The default location.</value>
		[DataEditorSetting("Default Location", defaultValue = "53.430785,-2.960515", type = typeof(TextField))]
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
		/// Initializes a new instance of the <see cref="SingleLocationDataType"/> class.
		/// </summary>
		public SingleLocationDataType()
		{
			// set the render control as the placeholder
			this.RenderControl = this.m_Control;

			// assign the initialise event for the control
			this.m_Control.Init += new EventHandler(this.m_Control_Init);

			// assign the save event for the data-type/editor
			this.DataEditorControl.OnSave += new AbstractDataEditorControl.SaveEventHandler(this.DataEditorControl_OnSave);
		}

		/// <summary>
		/// Gets the id of the data-type.
		/// </summary>
		/// <value>The id of the data-type.</value>
		public override Guid Id
		{
			get
			{
				return new Guid("1B64EAE2-F9A1-4276-A071-F25DDE6913DD");
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

			// set the data value of the control
			if (this.Data.Value != null)
			{
				this.m_Control.Data = this.Data.Value.ToString();
			}
			else
			{
				this.m_Control.Data = string.Empty;
			}
		}

		/// <summary>
		/// Datas the editor control_ on save.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void DataEditorControl_OnSave(EventArgs e)
		{
			this.Data.Value = this.m_Control.Data;
		}
	}
}
