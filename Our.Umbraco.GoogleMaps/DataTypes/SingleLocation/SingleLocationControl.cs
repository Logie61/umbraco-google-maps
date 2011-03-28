using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using ClientDependency.Core;
using Our.Umbraco.GoogleMaps.Controls;
using Our.Umbraco.GoogleMaps.Extensions;

[assembly: WebResource("Our.Umbraco.GoogleMaps.DataTypes.SingleLocation.SingleLocation.js", "application/x-javascript")]

namespace Our.Umbraco.GoogleMaps.DataTypes.SingleLocation
{
	public class SingleLocationControl : WebControl
	{
		public string CurrentLocation { get; set; }

		public string CurrentZoom { get; set; }

		/// <summary>
		/// Gets or sets the data.
		/// </summary>
		/// <value>The data.</value>
		public string Data
		{
			get
			{
				if (this.LocationTextBox != null && !string.IsNullOrEmpty(this.LocationTextBox.Value))
				{
					return this.LocationTextBox.Value;
				}
				if (!string.IsNullOrEmpty(this.CurrentLocation) && !string.IsNullOrEmpty(this.CurrentZoom))
				{
					return string.Concat(this.CurrentLocation, ',', this.CurrentZoom);
				}

				return string.Empty;
			}
			set
			{
				var parts = value.Split(',');

				// get the location
				if (parts.Length > 1)
				{
					this.CurrentLocation = string.Concat(parts[0], ',', parts[1]);
				}

				// get the zoom
				if (parts.Length > 2)
				{
					int zoom;
					if (int.TryParse(parts[2], out zoom))
					{
						this.CurrentZoom = zoom.ToString();
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the default location.
		/// </summary>
		/// <value>The default location.</value>
		public string DefaultLocation { get; set; }

		/// <summary>
		/// Gets or sets the default zoom.
		/// </summary>
		/// <value>The default zoom.</value>
		public string DefaultZoom { get; set; }

		public GoogleMap GoogleMap { get; set; }

		public HtmlInputText LocationTextBox { get; set; }

		public HtmlInputButton LocationButton { get; set; }

		public string MapHeight { get; set; }

		public string MapWidth { get; set; }

		public HtmlInputText SearchTextBox { get; set; }

		public HtmlInputButton SearchButton { get; set; }

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			this.EnsureChildControls();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// Adds the client dependencies.
			this.AddResourceToClientDependency("Our.Umbraco.GoogleMaps.DataTypes.SingleLocation.SingleLocation.js", ClientDependency.Core.ClientDependencyType.Javascript);
		}

		/// <summary>
		/// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			// search box
			this.SearchTextBox = new HtmlInputText();
			this.SearchTextBox.Attributes.Add("class", "place");
			this.Controls.Add(this.SearchTextBox);

			// search button
			this.SearchButton = new HtmlInputButton() { Value = "Search" };
			this.SearchButton.Attributes.Add("class", "button");
			this.SearchButton.Attributes.Add("onclick", "javascript:fergusonMoriyamaMapDataType.search(this); return false;");
			this.Controls.Add(this.SearchButton);

			// location box
			this.LocationTextBox = new HtmlInputText();
			this.LocationTextBox.Attributes.Add("class", "value");
			this.LocationTextBox.Value = this.Data;
			this.Controls.Add(this.LocationTextBox);

			// location button
			this.LocationButton = new HtmlInputButton() { Value = "Clear" };
			this.LocationButton.Attributes.Add("class", "button");
			this.LocationButton.Attributes.Add("onclick", "javascript:fergusonMoriyamaMapDataType.clear(this); return false;");
			this.Controls.Add(this.LocationButton);

			this.GoogleMap = new GoogleMap();
			this.GoogleMap.Height = Unit.Parse(this.MapHeight);
			this.GoogleMap.Width = Unit.Parse(this.MapWidth);
			this.Controls.Add(this.GoogleMap);
		}

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Concat("gmapContainer_", this.ClientID));
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "gmapContainer");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
		}

		public override void RenderEndTag(HtmlTextWriter writer)
		{
			writer.RenderEndTag(); // .fmpContainer
		}

		/// <summary>
		/// Renders the contents.
		/// </summary>
		/// <param name="output">The output.</param>
		protected override void RenderContents(HtmlTextWriter writer)
		{
			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			//writer.WriteLine("<input type=\"text\" class=\"place\"/>");
			this.SearchTextBox.RenderControl(writer);
			//writer.Write("<input type=\"button\" value=\"Search\"/ onClick=\"fergusonMoriyamaMapDataType.search(this); return false;\" class=\"button\"/>");
			this.SearchButton.RenderControl(writer);

			writer.RenderEndTag();

			writer.RenderBeginTag(HtmlTextWriterTag.Div);

			// base.Render(writer); // render a TextBox with the default value
			this.LocationTextBox.RenderControl(writer);
			//writer.WriteLine("<input type=\"button\" value=\"Clear\" onClick=\"fergusonMoriyamaMapDataType.clear(this); return false;\" class=\"button\"/>");
			this.LocationButton.RenderControl(writer);

			writer.RenderEndTag();

			this.GoogleMap.RenderControl(writer);

			// writer.Write("<input id=\"defaultloc_" + id + "\" type=\"hidden\" class=\"defaultloc\" value=\"53.430785,-2.960515,12\"/>");
			writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Concat("defaultloc_", this.ClientID));
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "defaultloc");
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
			writer.AddAttribute(HtmlTextWriterAttribute.Value, string.Concat(this.DefaultLocation, ',', this.DefaultZoom));
			writer.RenderBeginTag(HtmlTextWriterTag.Input);
			writer.RenderEndTag(); // .defaultloc

			// add jquery window load event
			//			var javascriptMethod = string.Format("jQuery('#{0}').CharLimit();", this.ClientID);
			//			var javascript = string.Concat("<script type='text/javascript'>jQuery(window).load(function(){", javascriptMethod, "});</script>");
			//			writer.WriteLine(javascript);

			//writer.WriteLine("<script type=\"text/javascript\">");
			//writer.WriteLine("if (fergusonmoriyama == undefined)  { var fergusonmoriyama = {};");
			////writer.WriteLine("fergusonmoriyama.defaultLocation = '" + c.DefaultLocation + "';");
			//writer.WriteLine("fergusonmoriyama.defaultLocation = '53.430785,-2.960515,12';");
			//writer.WriteLine("jQuery.ajax({ type: 'get', dataType: 'script', url: '" + this._scriptPath + "', error: function() { alert('Could not load script'); } }); }</script>");
		}
	}
}
