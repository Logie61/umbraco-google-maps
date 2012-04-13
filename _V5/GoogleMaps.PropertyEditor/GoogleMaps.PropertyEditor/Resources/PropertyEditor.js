/// <reference path="../../Scripts/Umbraco.System/NamespaceManager.js" />
/// <reference path="../../Scripts/Umbraco.Umbraco.System/UrlEncoder.js" />

Umbraco.System.registerNamespace("Umbraco.PropertyEditors");

(function ($) {

    // Singleton GoogleMaps.PropertyEditor class to encapsulate the management of MyPropertyEditors.
    Umbraco.PropertyEditors.GoogleMaps.PropertyEditor = function () {
        
        //You can add your functions here

        return {
            init: function (_opts) {
                //Do stuff with passed in data like:
                //_opts.umbracoPropertyId
            }
        }

    } ();

})(jQuery);

