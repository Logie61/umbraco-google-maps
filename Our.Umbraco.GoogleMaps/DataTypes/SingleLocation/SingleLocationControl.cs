﻿using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ClientDependency.Core;
using Our.Umbraco.GoogleMaps.Controls;
using Our.Umbraco.GoogleMaps.Extensions;
using Our.Umbraco.GoogleMaps.Helpers;

[assembly: WebResource(Constants.SingleLocationJavaScript, "application/x-javascript")]

namespace Our.Umbraco.GoogleMaps.DataTypes.SingleLocation
{
	/// <summary>
	/// A control for a Google Map to store a single location.
	/// </summary>
	[ValidationProperty("Data")]
	public class SingleLocationControl : WebControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SingleLocationControl"/> class.
		/// </summary>
		public SingleLocationControl()
			: base(HtmlTextWriterTag.Div)
		{
			this.CssClass = "gmapContainer";
		}

		/// <summary>
		/// Gets or sets the current location.
		/// </summary>
		/// <value>The current location.</value>
		public string CurrentLocation { get; set; }

		/// <summary>
		/// Gets or sets the current zoom.
		/// </summary>
		/// <value>The current zoom.</value>
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
					return string.Concat(this.CurrentLocation, Constants.Comma, this.CurrentZoom);
				}

				return string.Empty;
			}

			set
			{
				var parts = value.Split(Constants.Comma);

				// get the location
				if (parts.Length > 1)
				{
					this.CurrentLocation = string.Concat(parts[0], Constants.Comma, parts[1]);
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

		/// <summary>
		/// Gets or sets search filter.
		/// </summary>
		/// <value>The search filter.</value>
		public string SearchFilter { get; set; }

		/// <summary>
		/// Gets or sets the whether to use only one point or multiple.
		/// </summary>
		/// <value>A boolean which indicates whether one map point or multiple will be used.</value>
		public string UseOnlyOneMapPoint { get; set; }

        /// <summary>
        /// Gets or sets the whether to reverse geocode any searches containing Lat/Long points.
        /// </summary>
        /// <value>True or false depending on whether reverse geocoding used used any searches containing Lat/Long points.</value>
        public string ReverseGeocode { get; set; }

		/// <summary>
		/// Gets or sets the google map.
		/// </summary>
		/// <value>The google map.</value>
		public GoogleMap GoogleMap { get; set; }

		/// <summary>
		/// Gets or sets the hidden location.
		/// </summary>
		/// <value>The hidden location.</value>
		public HtmlInputHidden HiddenLocation { get; set; }

		/// <summary>
		/// Gets or sets the hidden search filter.
		/// </summary>
		/// <value>The hidden search filter.</value>
		public HtmlInputHidden HiddenSearchFilter { get; set; }

		/// <summary>
		/// Gets or sets the whether to use only one point or multiple.
		/// </summary>
		/// <value>True or false depending on whether only one map point or multiple will be used.</value>
		public HtmlInputHidden HiddenUseOnlyOnePoint { get; set; }

        /// <summary>
        /// Gets or sets the whether to reverse geocode any searches containing Lat/Long points.
        /// </summary>
        /// <value>True or false depending on whether reverse geocoding used used any searches containing Lat/Long points.</value>
        public HtmlInputHidden HiddenReverseGeocode { get; set; }

		/// <summary>
		/// Gets or sets the location text box.
		/// </summary>
		/// <value>The location text box.</value>
		public HtmlInputText LocationTextBox { get; set; }

		/// <summary>
		/// Gets or sets the location button.
		/// </summary>
		/// <value>The location button.</value>
		public HtmlInputButton LocationButton { get; set; }

		/// <summary>
		/// Gets or sets the height of the map.
		/// </summary>
		/// <value>The height of the map.</value>
		public string MapHeight { get; set; }

		/// <summary>
		/// Gets or sets the width of the map.
		/// </summary>
		/// <value>The width of the map.</value>
		public string MapWidth { get; set; }

		/// <summary>
		/// Gets or sets the search text box.
		/// </summary>
		/// <value>The search text box.</value>
		public HtmlInputText SearchTextBox { get; set; }

		/// <summary>
		/// Gets or sets the search button.
		/// </summary>
		/// <value>The search button.</value>
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
		/// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			this.LocationTextBox.Value = this.Data;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			// set the ID of the control
			this.ID = string.Concat("gmapContainer_", this.ClientID);

			// Adds the client dependencies.
			this.AddResourceToClientDependency(Constants.SingleLocationJavaScript, ClientDependencyType.Javascript);
		}

		/// <summary>
		/// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			var divSearch = new HtmlGenericControl("div");

			// search box
			this.SearchTextBox = new HtmlInputText();
			this.SearchTextBox.Attributes.Add("class", "place");
			divSearch.Controls.Add(this.SearchTextBox);

			// search button
			this.SearchButton = new HtmlInputButton() { Value = "Search" };
			this.SearchButton.Attributes.Add("class", "button");
			this.SearchButton.Attributes.Add("onclick", "javascript:UmbracoGoogleMapMapDataType.search(this); return false;");
			divSearch.Controls.Add(this.SearchButton);

			this.Controls.Add(divSearch);

			var divLocation = new HtmlGenericControl("div");

			// location box
			this.LocationTextBox = new HtmlInputText();
			this.LocationTextBox.Attributes.Add("class", "value");
			this.LocationTextBox.Value = this.Data;
			divLocation.Controls.Add(this.LocationTextBox);

			// location button
			var locationButtonText = this.UseOnlyOneMapPoint.ToLower() == "true" ? "Reset" : "Clear";
			this.LocationButton = new HtmlInputButton() { Value = locationButtonText };
			this.LocationButton.Attributes.Add("class", "button");
			this.LocationButton.Attributes.Add("onclick", "javascript:UmbracoGoogleMapMapDataType.clear(this); return false;");
			divLocation.Controls.Add(this.LocationButton);

			this.Controls.Add(divLocation);

			this.GoogleMap = new GoogleMap()
			{
				CssClass = "map",
				ID = string.Concat("map_", this.ClientID),
				Height = Unit.Parse(this.MapHeight),
				Width = Unit.Parse(this.MapWidth)
			};
			this.Controls.Add(this.GoogleMap);

			this.HiddenLocation = new HtmlInputHidden()
			{
				ID = string.Concat("defaultloc_", this.ClientID),
				Value = string.Concat(this.DefaultLocation, Constants.Comma, this.DefaultZoom)
			};
			this.HiddenLocation.Attributes.Add("class", "defaultloc");
			this.Controls.Add(this.HiddenLocation);

			this.HiddenSearchFilter = new HtmlInputHidden()
			{
				ID = string.Concat("searchFilter_", this.ClientID),
				Value = this.SearchFilter
			};
			this.HiddenSearchFilter.Attributes.Add("class", "searchFilter");
			this.Controls.Add(this.HiddenSearchFilter);

			this.HiddenUseOnlyOnePoint = new HtmlInputHidden()
			{
				ID = string.Concat("useOnlyOnePoint_", this.ClientID),
				Value = this.UseOnlyOneMapPoint.ToLower()
			};
			this.HiddenUseOnlyOnePoint.Attributes.Add("class", "useOnlyOnePoint");
			this.Controls.Add(this.HiddenUseOnlyOnePoint);

            this.HiddenReverseGeocode = new HtmlInputHidden()
            {
                ID = string.Concat("reverseGeocode_", this.ClientID),
                Value = this.UseOnlyOneMapPoint.ToLower()
            };
            this.HiddenUseOnlyOnePoint.Attributes.Add("class", "reverseGeocode");
            this.Controls.Add(this.HiddenUseOnlyOnePoint);
		}
	}
}