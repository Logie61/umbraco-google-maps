using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Umbraco.Cms.Web.EmbeddedViewEngine;
using Umbraco.Cms.Web.Model.BackOffice.PropertyEditors;
using System.Web.Mvc;
using DataAnnotationsExtensions;

namespace GoogleMaps.PropertyEditor
{
    [EmbeddedView("GoogleMaps.PropertyEditor.Views.Editor.cshtml", "GoogleMaps.PropertyEditor")]
    public class GoogleMapsModel : EditorModel<GoogleMapsPreValueModel>
    {
        public GoogleMapsModel(GoogleMapsPreValueModel preValues)
            : base(preValues)
        {
            //Use prevalues as the default values until it's saved
            //Tip from Shannon's CG11 Plugin talk...
            Lat         = preValues.initLat;
            Long        = preValues.initLong;
            ZoomLevel   = preValues.initZoomLevel;
        }

        public string Postcode { get; set; }

        [Required(ErrorMessage = "Please set the location latitude")]
        [RegularExpression("[-]?[0-9]*[.]{0,1}[0-9]*", ErrorMessage = "Please provide the longtitude as a decimal")]
        public string Lat { get; set; }

        [Required(ErrorMessage = "Please set the location longtitude")]
        [RegularExpression("[-]?[0-9]*[.]{0,1}[0-9]*", ErrorMessage = "Please provide the longtitude as a decimal")]
        public string Long { get; set; }

        [Range(1, 20, ErrorMessage = "Please set a zoom level between 1 and 20")]
        [Required(ErrorMessage = "Please set a zoom level")]
        public int ZoomLevel { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string LatLongCombined { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string LatLongZoomCombined { get; set; }

        public override IDictionary<string, object> GetSerializedValue()
        {
            //Lat,Long
            LatLongCombined = string.Concat(Lat, ", ", Long);

            //Lat,Long,ZoomLevel
            if (ZoomLevel >= 0)
            {
                LatLongZoomCombined = string.Concat(Lat, ",", Long, ",", ZoomLevel);
            }

            return base.GetSerializedValue();
        }

    }
}
