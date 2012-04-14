using System.Web.UI;
using Umbraco.Cms.Web.Model.BackOffice.PropertyEditors;

[assembly: WebResource("GoogleMaps.PropertyEditor.Resources.PropertyEditor.js", "application/x-javascript")]
[assembly: WebResource("GoogleMaps.PropertyEditor.Resources.GoogleMap.css", "text/css")]

namespace GoogleMaps.PropertyEditor
{
    [PropertyEditor("B8C400DE-6BB5-4041-A4B0-B2BEEE5AE0C5", "GoogleMaps.PropertyEditor", "Google Maps", IsParameterEditor = true)]
    public class GoogleMapsEditor : PropertyEditor<GoogleMapsModel, GoogleMapsPreValueModel>
    {

        public override GoogleMapsModel CreateEditorModel(GoogleMapsPreValueModel preValues)
        {
            return new GoogleMapsModel(preValues);
        }

        public override GoogleMapsPreValueModel CreatePreValueEditorModel()
        {
            return new GoogleMapsPreValueModel();
        }
    }
}
