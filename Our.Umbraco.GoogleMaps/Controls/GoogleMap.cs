using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClientDependency.Core;
using Our.Umbraco.GoogleMaps.Extensions;
using Our.Umbraco.GoogleMaps.Helpers;

[assembly: WebResource(Constants.GoogleMapCss, "text/css")]

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

			// Adds the client dependencies.
			this.AddResourceToClientDependency(Constants.GoogleMapCss, ClientDependencyType.Css);
		}
	}
}