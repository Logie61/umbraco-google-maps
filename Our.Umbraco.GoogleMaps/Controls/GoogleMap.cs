using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using ClientDependency.Core;
using Our.Umbraco.GoogleMaps.Extensions;

[assembly: WebResource("Our.Umbraco.GoogleMaps.Controls.GoogleMap.css", "text/css")]

namespace Our.Umbraco.GoogleMaps.Controls
{
	/// <summary>
	/// Google Map control.
	/// </summary>
	[ToolboxData("<{0}:GoogleMap runat=server></{0}:GoogleMap>")]
	public class GoogleMap : WebControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GoogleMap"/> class.
		/// </summary>
		public GoogleMap()
			: base(HtmlTextWriterTag.Div)
		{
			this.CssClass = "map";
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			////// set the id for the output/markup
			//// var clientId = this.Parent != null ? this.Parent.ClientID : this.ClientID;
			//// this.ID = string.Concat("map_", clientId);

			// Adds the client dependencies.
			this.AddResourceToClientDependency("Our.Umbraco.GoogleMaps.Controls.GoogleMap.css", ClientDependencyType.Css);
		}
	}
}
