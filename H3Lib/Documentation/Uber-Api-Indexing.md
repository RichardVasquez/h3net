# Indexing functions 

These functions are used for finding the H3 index containing coordinates,
and for finding the center and boundary of H3 indexes.

## GeoToH3
```c#
H3Index Api.GeoToH3(GeoCoord g, int r)
```
### Summary

find the H3 index of the resolution res cell containing the lat/lng

### Parameters
| Name | Type | Description |
|------|------|-------------|
| g | H3Lib.GeoCoord | Geographical coordinate (in radians) data type |
| r | int | Resolution (0-15 inclusive) of resulting H3Index |

## H3ToGeo
```c#
void Api.H3ToGeo(H3Index h3, out GeoCoord g) 
```
### Summary
find the lat/lon center point g of the cell h3

_Note:_ This is provided to map the same general syntax
of the C library.  You will probably want to use the ToGeo
method for H3Index data types.

### Parameters
| Name | Type | Description |
|------|------|-------------|
| h3 | H3Lib.H3Index | The H3Index cell to find the centroid of |
| g | **out** H3Lib.GeoCoord | The geographical coordinate that is the center of the cell |


## H3ToGeoBoundary
```c#
void Api.H3ToGeoBoundary(H3Index h3, out GeoBoundary gb)
```
give the cell boundary in lat/lon coordinates for the cell h3

_Note:_ Similar to H3ToGeo in regards to C API mapping, and an
assigned variable.  The preferred method would be ToGeoBoundary().

| Name | Tyoe | Description |
|------|------|-------------|
| h3 | H3Lib.H3Index | The H3Index to find the GeoBoundary of |
| g  | **out** H3Lib.GeoBoundary | The GeoBoundary defining the vertices of the H3Index cell | 

<hr>

[Return to Uber API Table of Contents](Uber-Api.md)
