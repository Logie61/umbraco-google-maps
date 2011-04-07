using System;
using System.Text;
using System.Xml;
using System.Xml.XPath;

using umbraco;

namespace Our.Umbraco.GoogleMaps.Helpers
{
	[XsltExtension("google.maps")]
	public class Library
	{
		public static XPathNodeIterator ParseCsv(string csv)
		{
			var array = csv.Split(',');
			var tag = "<GoogleMap lon='{0}' lat='{1}' zoom='{2}' />";
			var xml = new XmlDocument();
			xml.LoadXml(string.Format(tag, array));
			return xml.CreateNavigator().Select("/GoogleMap");
		}

		/// <summary>
		/// Gets the static map.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string GetStaticMap(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				var array = value.Split(',');
				var lon = (array.Length > 0) ? array[0] : "53.430785";
				var lat = (array.Length > 1) ? array[1] : "-2.960515";
				var zoom = (array.Length > 2) ? int.Parse(array[2]) : 13;

				return GetStaticMap(lon, lat, zoom);
			}

			return string.Empty;
		}

		/// <summary>
		/// Gets the static map.
		/// </summary>
		/// <param name="lon">The lon.</param>
		/// <param name="lat">The lat.</param>
		/// <returns></returns>
		public static string GetStaticMap(string lon, string lat)
		{
			return GetStaticMap(lon, lat, 13, 250, 250);
		}

		/// <summary>
		/// Gets the static map.
		/// </summary>
		/// <param name="lon">The lon.</param>
		/// <param name="lat">The lat.</param>
		/// <param name="zoom">The zoom.</param>
		/// <returns></returns>
		public static string GetStaticMap(string lon, string lat, int zoom)
		{
			return GetStaticMap(lon, lat, zoom, 250, 250);
		}

		/// <summary>
		/// Gets the static map.
		/// </summary>
		/// <param name="lon">The lon.</param>
		/// <param name="lat">The lat.</param>
		/// <param name="zoom">The zoom.</param>
		/// <param name="height">The height.</param>
		/// <param name="width">The width.</param>
		/// <returns></returns>
		public static string GetStaticMap(string lon, string lat, int zoom, int height, int width)
		{
			return GetStaticMap(lon, lat, zoom, height, width, "roadmap");
		}

		/// <summary>
		/// Gets the static map.
		/// </summary>
		/// <param name="lon">The lon.</param>
		/// <param name="lat">The lat.</param>
		/// <param name="zoom">The zoom.</param>
		/// <param name="height">The height.</param>
		/// <param name="width">The width.</param>
		/// <param name="mapType">Type of the map.</param>
		/// <returns></returns>
		public static string GetStaticMap(string lon, string lat, int zoom, int height, int width, string mapType)
		{
			switch (mapType.ToUpper())
			{
				case "ROADMAP":
				case "SATELLITE":
				case "TERRAIN":
				case "HYBRID":
					break;

				default:
					mapType = "roadmap";
					break;
			}

			string staticMapUrl = "http://maps.google.com/maps/api/staticmap?markers={0},{1}&zoom={2}&size={4}x{3}&maptype={5}&sensor=false";
			return string.Format(staticMapUrl, lon, lat, zoom, height, width, mapType.ToLower());
		}
	}
}
