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
	/// 
	/// </summary>
	[ToolboxData("<{0}:GoogleMap runat=server></{0}:GoogleMap>")]
	public class GoogleMap : WebControl
	{
		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Adds the client dependencies.
			this.AddResourceToClientDependency("Our.Umbraco.GoogleMaps.Controls.GoogleMap.css", ClientDependencyType.Css);
		}

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			// base.RenderBeginTag(writer);
		}

		public override void RenderEndTag(HtmlTextWriter writer)
		{
			// base.RenderEndTag(writer);
		}

		/// <summary>
		/// Renders the contents.
		/// </summary>
		/// <param name="output">The output.</param>
		protected override void RenderContents(HtmlTextWriter writer)
		{
			var clientId = this.Parent != null ? this.Parent.ClientID : this.ClientID;
			writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Concat("map_", clientId));
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "map");
			writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Format("height:{0}px;width:{1}px;", this.Height.Value, this.Width.Value));
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.RenderEndTag(); // .map
		}
	}
}
