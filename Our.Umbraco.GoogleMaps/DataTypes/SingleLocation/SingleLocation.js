if (fergusonmoriyama == undefined) var fergusonmoriyama = {};
if (!fergusonmoriyama.defaultLocation) { fergusonmoriyama.defaultLocation = ''; }

var fergusonMoriyamaMapDataType = null;

fergusonmoriyama.mapDatatype = function () {
	this._maps = new Object();
	this._apiLoaded = false;
	this._editControlId = 0;
	this._id = 0;
	this._container = null;
	this._renderedTabs = new Object();
};

fergusonmoriyama.map = function (id, container) {
	this._id = id;
	this._container = container;
	this._map = null;
	this._markers = new Array();
	this._val = '';
};

fergusonmoriyama.map.prototype = {
	draw: function (data) {

		var coords = new google.maps.LatLng(0, 0);
		var options = {
			zoom: 8,
			center: coords,
			mapTypeId: google.maps.MapTypeId.ROADMAP
		}

		this._markers = new Array();
		this._map = new google.maps.Map(document.getElementById(this._id), options);

		var markers = this._markers;
		var context = this;

		var infowindow = new google.maps.InfoWindow({
			content: 'Loading...'
		});


		// BUG WITH MULTIPLE MARKERS!!! use "bayswater" as example [LK]


		for (var i = 0; i < data.length; i++) {

			var result = data[i];

			var name = result.formatted_address;
			name = name.replace(/'/, "\\'");

			var marker = new google.maps.Marker({
				map: this._map,
				position: result.geometry.location,
				draggable: true,
				html: '<span class="fmText">' + name + '<br/><a href="#" onClick="fergusonMoriyamaMapDataType.markerClick(\'' + context._id + '\', ' + context._markers.length + '); return false;">Use this location</a></span>'
			});

			google.maps.event.addListener(marker, 'click', function () {
				infowindow.setContent(this.html);
				infowindow.open(context._map, this);
			});

			google.maps.event.addListener(marker, 'drag', function () {
				infowindow.close();
			});

			google.maps.event.addListener(marker, 'dragend', function (e) {
				//this.title = e.latLng.lat() + ', ' + e.latLng.lng();
				//infowindow.content = '<span class="fmText">' + this.title + '<br/><a href="#" onClick="fergusonMoriyamaMapDataType.markerClick(\'' + context._id + '\', 0); return false;">Use this location</a></span>'
			});

			context._markers[context._markers.length] = marker;
		}

		//  Create a new viewpoint bound
		var bounds = new google.maps.LatLngBounds();

		//  Go through each...
		$.each(this._markers, function (index, marker) {
			bounds.extend(marker.position);
		});

		//  Fit these bounds to the map
		this._map.fitBounds(bounds);

		if (this._map.getZoom() > 13) {
			this._map.setZoom(13);
		}
	},

	render: function () {
		var v = jQuery('input.value', this._container).attr('value')
		var mapId = jQuery('div.map', this._container).attr('id');
		fergusonmoriyama.defaultLocation = jQuery('input.defaultloc', this._container).attr('value');

		var coords = new google.maps.LatLng(0, 0);
		var zoom = 13;

		this._map = new google.maps.Map(document.getElementById(this._id));

		if (v.length == 0) {

			if (fergusonmoriyama.defaultLocation.match(/^\-*[\d\.]+,\-*[\d\.]+,\d+/)) {
				var loc = fergusonmoriyama.defaultLocation.split(',');
				loc[0] = parseFloat(loc[0]);
				loc[1] = parseFloat(loc[1]);
				loc[2] = parseInt(loc[2]);

				coords = new google.maps.LatLng(loc[0], loc[1]);
				zoom = loc[2];

			} else if (fergusonmoriyama.defaultLocation.match(/^\-*[\d\.]+,\-*[\d\.]+$/)) {
				var loc = fergusonmoriyama.defaultLocation.split(',');
				loc[0] = parseFloat(loc[0]);
				loc[1] = parseFloat(loc[1]);

				coords = new google.maps.LatLng(loc[0], loc[1]);
			} else {
				coords = new google.maps.LatLng(37.4419, -122.1419);
			}

		} else {

			var pointData = v.split(',');

			pointData[0] = parseFloat(pointData[0]);
			pointData[1] = parseFloat(pointData[1]);
			pointData[2] = parseInt(pointData[2]);

			coords = new google.maps.LatLng(pointData[0], pointData[1]);

			var marker = new google.maps.Marker({
				map: this._map,
				position: coords,
				draggable: true,
				html: '<span class="fmText">' + pointData[0] + ', ' + pointData[1] + '<br/><a href="#" onClick="fergusonMoriyamaMapDataType.markerClick(\'' + this._id + '\', 0); return false;">Use this location</a></span>'
			});

			var infowindow = new google.maps.InfoWindow({
				content: 'Loading...'
			});

			this._markers[0] = marker;

			var map2 = this._map;
			google.maps.event.addListener(marker, 'click', function () {
				infowindow.setContent(this.html);
				infowindow.open(map2, this);
			});

			google.maps.event.addListener(marker, 'drag', function () {
				infowindow.close();
			});

			google.maps.event.addListener(marker, 'dragend', function (e) {
				//marker.title = e.latLng.lat() + ', ' + e.latLng.lng();
				//infowindow.content = '<span class="fmText">' + marker.title + '<br/><a href="#" onClick="fergusonMoriyamaMapDataType.markerClick(\'' + this._id + '\', 0); return false;">Use this location</a></span>'
			});
		}

		var options = {
			zoom: zoom,
			center: coords,
			mapTypeId: google.maps.MapTypeId.ROADMAP
		}

		this._map.setOptions(options);
	}
}

fergusonmoriyama.mapDatatype.prototype = {
	markerClick: function (mapId, markerId) {
		var map = this._maps[mapId];
		var marker = map._markers[markerId];

		var z = map._map.getZoom();
		var l = marker.getPosition();
		var lat = l.lat();
		var lon = l.lng();
		var val = lat + ',' + lon + ',' + z;
		map._val = val;

		jQuery("input.value", map._container).attr('value', val);
		map._val = val;
	},

	edit: function () {
		this._apiLoaded = true;
		this._maps[this._id] = new fergusonmoriyama.map(this._id, this._container);

		this._maps[this._id].render();
	},

	preEdit: function (id, container) {

		this._id = id;
		this._container = container;

		if (this._apiLoaded) {
			this.edit();
		} else {
			fergusonmoriyama.loadMapsApi('fergusonMoriyamaMapDataType.edit');
		}

	},

	guiMap: function () {
		var context = this;
		jQuery('div.gmapContainer').each(function () {
			var id = jQuery('div.map', this).attr('id');
			context._maps[id] = new fergusonmoriyama.map(id, this);
			context._maps[id].render();
		});
	},

	clear: function (button) {
		var container = button.parentNode;
		jQuery('input.value', container).val('');
	},

	search: function (button) {
		var container = button.parentNode.parentNode;
		var id = container.id;
		var searchTerm = jQuery('input.place', container).val();
		var mapId = jQuery('div.map', container).attr('id');
		var map = this._maps[mapId];

		var geocoder = new google.maps.Geocoder();
		geocoder.geocode({ 'address': searchTerm }, function (data, status) {
			if (status == google.maps.GeocoderStatus.OK) {
				map.draw(data);
			} else {
				alert('Your search didn\'t return any results');
			}
		});
	}
};

fergusonmoriyama.loadMapsApi = function (cb) {
	jQuery.ajax({
		type: "get",
		dataType: "script",
		url: 'http://maps.google.com/maps/api/js',
		data: {
			sensor: false,
			callback: cb
		},
		error: function () { alert('Could not load Google Maps API'); }
	});
}

fergusonMoriyamaMapDataType = new fergusonmoriyama.mapDatatype();

if (typeof ItemEditing != 'undefined') {
	ItemEditing.add_startEdit(function (item) {
		var itemId = item._activeItem.itemId;
		var container = jQuery('umbraco\\:iteminfo[itemid=' + itemId + ']');
		if (jQuery.browser.msie) {
			container = jQuery('*[itemid=' + itemId + ']');
		}
		if (jQuery("div.map", container).size() > 0) {
			jQuery('input.value').focus(function () {
				$(this).blur();
			});
			var mapId = jQuery('div.map', container).attr('id');
			fergusonMoriyamaMapDataType.preEdit(mapId, container);
		}
	});
} else {

	jQuery(document).ready(function () {

		fergusonmoriyama.loadMapsApi('fergusonMoriyamaMapDataType.guiMap');

		jQuery('input.value').focus(function () {
			$(this).blur();
		});

		$('a').click(function () {
			if ($(this).attr('id').indexOf('TabView') > -1) {

				var id = $(this).attr('id');
				id = id.replace(/^(.*\_tab\d+).*$/, "$1");
				id += 'layer';

				if (!fergusonMoriyamaMapDataType._renderedTabs[id]) {
					jQuery('#' + id).each(function () {
						jQuery('div.map', this).each(function () {
							var id = jQuery(this).attr('id');
							if (fergusonMoriyamaMapDataType._maps[id]) {
								fergusonMoriyamaMapDataType._maps[id].render();
							}
						});
					});

					fergusonMoriyamaMapDataType._renderedTabs[id] = true;
				}
			}
		});

	});
}