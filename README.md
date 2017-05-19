# GeoNerves
GeoNerves is a (work-in-progress) C# library for bulk-GeoCoding text addresses using the U.S. Census bulk geocoding API documented here: https://geocoding.geo.census.gov/geocoder/Geocoding_Services_API.pdf

### What is GeoCoding
GeoCoding is the process of taking a street address, for example: `667 Massachusetts Avenue, Cambridge, MA, 02139` and turning that into a pair of Latitude / Longitude Coordinates `-71.104225,42.365723`. 

### Why do I need it
Most tools / APIs involved in mapping won't accept text street addresses, and require Latitude and Longitude. To show pins on a Google map canvas requires the coordinates of each address you want to show.

### Why this tool
Major location providers (Google Maps, Bing Maps, SmartyStreets) offer GeoCoding services in rate-limited APIs that are priced for Enterprise. GeoCoding 100,000 addresses will run you upwards of $1,200. For smaller users working with large datasets, these prices are prohibitive. GeoNerves uses U.S. Census TIGER GeoCoding data free of charge. The Census Bulk Geocoding API is not conceptually complex, but the documentation is sparse and the CSV I/O can be cumbersome. GeoNerves adds a more readily consumed interface in the form of a C# class library and the `CensusGeoLocator` object.

### Features
GeoNerves offers GeoCoding of formatted lists of addresses as CSV, XML and JSON input. The Census API does NOT support bulk address parsing / cleanup, so all input addresses must be split into: `UniqueId` `Street` `City` `State` `Zip` ahead of time, in the input.

### Example Input
**XML:**

	<Addresses>
		<Address>
			<UniqueId>1</UniqueId>
			<Street>667 Massachusetts Avenue</Street>
			<City>Cambridge</City>
			<State>MA</State>
			<Zip>02139</Zip>
		</Address>
		<Address>
			<UniqueId>2</UniqueId>
			<Street>675 Massachusetts Avenue</Street>
			<City>Cambridge</City>
			<State>MA</State>
			<Zip>02139</Zip>
		</Address>
	</Addresses>
    
**JSON:**

	{
		"Addresses": [{
			"UniqueId": 1,
			"Street": "667 Massachusetts Avenue",
			"City": "Cambridge",
			"State": "MA",
			"Zip": "02139"
			},
			{
			"UniqueId": 2,
			"Street": "675 Massachusetts Avenue",
			"City": "Cambridge",
			"State": "MA",
			"Zip": "02139"
			}
		]
	}
    
    
**CSV:**

    1,667 Massachusetts Avenue,Cambridge,MA,02139
    2,30 Tyler Street,Boston,MA,02111,
    3,216 Norfolk Street,Cambridge,MA,02139
	
