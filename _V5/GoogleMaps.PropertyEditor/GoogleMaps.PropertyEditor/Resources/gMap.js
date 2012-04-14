var geocoder;
var map;
var marker;

function initialize() {
    //MAP

    //Grab the values from the input fields
    var latValue    = $(latID).val();
    var longValue   = $(longID).val();
    var zoomValue   = parseInt($(zoomID).val()); //Have to parseInt() as .val() gets the number as a string


    //Set defaults if empty 
    if (latValue == '') {
        latValue    = initLat;
        longValue   = initLong;        
        zoomValue   = initZoomLevel;

        //Update the input boxes with our defaults...
        //If we have values saved then they will appear instead
        $(latID).val(latValue);
        $(longID).val(longValue);
        $(zoomID).val(zoomValue);
        
    }

    var location = new google.maps.LatLng(latValue, longValue);

    //Map options
    var options = {
        zoom: zoomValue,
        center: location,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    //Bind against DOM element #starterGoogleMap
    map = new google.maps.Map(document.getElementById("starterGoogleMap"), options);

    //Add the marker to the map
    marker = new google.maps.Marker({
        map:map,
        position: location,
        draggable: true
    });

    //Setup the geocoder...
    geocoder = new google.maps.Geocoder();

    
    google.maps.event.addListener(marker, "dragend", function (event) {
        var point = marker.getPosition();
        map.panTo(point);

        //Update the input boxes with lat & long
        $(latID).val(marker.getPosition().lat());
        $(longID).val(marker.getPosition().lng());       
    });
    
    
    
};

$(document).ready(function () {

    //When DOM ready initialize the google map
    initialize();

    $(function () {
        $(postCodeID).autocomplete({
            //This bit uses the geocoder to fetch address values
            source: function (request, response) {
                geocoder.geocode({ 'address': request.term }, function (results, status) {
                    response($.map(results, function (item) {
                        return {
                            label: item.formatted_address,
                            value: item.formatted_address
                        };
                    }));
                });
            }
        });
    });


    $('#findbutton').click(function (e) {
        var address = $(postCodeID).val();
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                //Set the marker on the map with our found location
                marker.setPosition(results[0].geometry.location);

                //Set the map center to where we found
                map.setCenter(results[0].geometry.location);

                //Update the input boxes with lat & long
                $(latID).val(marker.getPosition().lat());
                $(longID).val(marker.getPosition().lng());
            } else {
                alert("Geocode was not successful for the following reason: " + status);
            }
        });
        e.preventDefault();
    });


    //Add listener to map when user changes zoom level
    google.maps.event.addListener(map, 'zoom_changed', function () {
        //Update the input box with zoom level
        $(zoomID).val(map.getZoom());
    });


});