using Umbraco.Cms.Web.Model.BackOffice.Editors;
using Umbraco.Cms.Web.Model.BackOffice.PropertyEditors;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DataAnnotationsExtensions;

namespace GoogleMaps.PropertyEditor
{
    public class GoogleMapsPreValueModel : PreValueModel
    {
        [AllowDocumentTypePropertyOverride]
        [Required(ErrorMessage = "Please supply your Google Maps API key https://code.google.com/apis/console")]
        [DisplayName("Google Maps API Key")]
        public string apiKey { get; set; }

        [AllowDocumentTypePropertyOverride]
        [Range(0, 21, ErrorMessage = "Please set a zoom level between 0 and 21")]
        [Required(ErrorMessage = "Please set a zoom level")]
        [DisplayName("Initial Zoom Level")]
        public int initZoomLevel { get; set; }

        [AllowDocumentTypePropertyOverride]
        [Required(ErrorMessage = "Please set the initial latitude")]
        [DisplayName("Initial Latitude")]
        public decimal initLat { get; set; }

        [AllowDocumentTypePropertyOverride]
        [Required(ErrorMessage = "Please set the initial longtitude")]
        [DisplayName("Initial Longtitude")]
        public decimal initLong { get; set; }

        [AllowDocumentTypePropertyOverride]
        [Required(ErrorMessage = "Please set the initial width")]
        [Min(200, ErrorMessage = "The minimum width needs to be greater than 200px")]
        [DisplayName("Initial Width in pixels")]
        public int initWidth { get; set; }

        [AllowDocumentTypePropertyOverride]
        [Required(ErrorMessage = "Please set the initial height")]
        [Min(200, ErrorMessage = "The minimum height needs to be greater than 200px")]
        [DisplayName("Initial Height in pixels")]
        public int initHeight { get; set; }
    }
}
