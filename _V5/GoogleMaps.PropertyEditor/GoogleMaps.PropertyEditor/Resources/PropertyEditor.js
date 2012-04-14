/*
===========================
Google Map Property Editor
===========================
*/

(function ($) {

    //Plugin name
    $.fn.umbracoGoogleMap = function (settings) {

        //For each matched element $('.googleMap')
        this.each(function (index, element) {

            //Common variables used for each map
            var map = null;
            var marker = null;
            var point = null;

            //Setup ref to jQuery
            var $this = $(this);

            //ID's of DOM elements
            //Using .find() for child elements
            var postcode    = $this.find('.googlePostcode');
            var searchBtn   = $this.find('.googleSearch');
            var lat         = $this.find('.googleLat');
            var long        = $this.find('.googleLong');
            var zoom        = $this.find('.googleZoom');

            //Setting passed in from view into the plugin
            var googleMap = $(settings.googleMap).get(0); //Gets the actual DOM element to pass to Google Maps API

            //Setup the geocoder...
            geocoder = new google.maps.Geocoder();


            //Load the map with the options the user has passed in
            LoadGoogleMap();


            //When clicking the find/search button
            $(searchBtn).click(function (e) {

                //Grab the value in the postcode/search box
                var address = $(postcode).val();

                geocoder.geocode({ 'address': address }, function (results, status) {
                    //If got an OK back from GeoCoder
                    if (status == google.maps.GeocoderStatus.OK) {
                        //Update map with location
                        updateMap(results[0].geometry.location);
                    }
                    else {
                        //Notify user of issue with geocoder
                        alert("Geocode was not successful for the following reason: " + status);
                    }
                });

                //Prevent default click...
                e.preventDefault();
            });

            //Autocomplete for search results...
            $(postcode).autocomplete({
                source: function (request, response) {
                    geocoder.geocode({ 'address': request.term }, function (results, status) {
                        response($.map(results, function (item) {
                            return {
                                //Map the values back to autocomplete
                                label: item.formatted_address,
                                value: item.formatted_address,
                                data: results[0].geometry.location
                            };
                        }));
                    });
                },
                minLength: 1,
                select: function (event, ui) {

                    //When selecting an item from automcomplete list
                    displaySelectedItem(ui.item.data);
                }
            });


            //Load Google Map
            function LoadGoogleMap() {

                //Grab the values from the input fields
                var latValue = $(lat).val();
                var longValue = $(long).val();
                var zoomValue = parseInt($(zoom).val()); //Have to parseInt() as .val() gets the number as a string

                //Location of pin
                var location = new google.maps.LatLng(latValue, longValue);

                //Map options
                var mapOptions = {
                    zoom: zoomValue,
                    center: location,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                //Bind against DOM element #starterGoogleMap
                map = new google.maps.Map(googleMap, mapOptions);

                //Add the marker to the map
                marker = new google.maps.Marker({
                    map: map,
                    position: location,
                    draggable: true
                });

                /// === EVENT ====
                //Add listener to map when finishes dragging pin around
                google.maps.event.addListener(marker, "dragend", function (event) {
                    var point = marker.getPosition();
                    map.panTo(point);

                    //Update the input boxes with lat & long
                    $(lat).val(marker.getPosition().lat());
                    $(long).val(marker.getPosition().lng());
                });

                /// === EVENT ====
                //Add listener to map when user changes zoom level
                google.maps.event.addListener(map, 'zoom_changed', function () {
                    //Update the input box with zoom level
                    $(zoom).val(map.getZoom());
                });
            }


            //Used to updateMap when an autocomplete item selected/clicked
            function displaySelectedItem(data) {
                //Update map...
                updateMap(data);
            }


            //Update the map with new location
            function updateMap(location) {

                //Set the marker on the map with our found location
                marker.setPosition(location);

                //Set the map center to where we found
                map.setCenter(location);

                //Update the input boxes with lat & long
                $(lat).val(marker.getPosition().lat());
                $(long).val(marker.getPosition().lng());
            }

        }); //Close each

        //Returns the jQuery object to allow for chainability
        return this;
    }


})(jQuery);
