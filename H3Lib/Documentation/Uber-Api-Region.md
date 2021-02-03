# Region Functions

These functions convert H3 indexes to and from polygonal areas.

## Polyfill

```c#
void aPI.PolyFill(GeoPolygon polygon, int r, out List<H3Index> outCells)
```

### Polyfill Summary

polyfill takes a given GeoJSON-like data and fills it with the hexagons that are contained by the GeoJSON-like data structure.

The current implementation is very primitive and slow, but correct, performing a point-in-poly operation on every hexagon in a k-ring defined around the given geofence.

### Polyfill Parameters

| Name | Type | Description |
|------|------|-------------|
|polygon|H3Lib.GeoPolygon|Polygon to be filled|
|r|int|Resolution of cells to fill polygon|
|outCells|**out** List&lt;H3Lib.H3Index&gt;|Cells that fill the polygon|

## MaxPolyfillSize

```c#
int Api.MaxPolyFillSize(GeoPolygon polygon, int r)
```

### MaxPolyfillSize Summary

maxPolyfillSize returns the number of hexagons to allocate space for when performing a polyfill on the given GeoJSON-like data structure.

### MaxPolyfillSize Parameters

| Name | Type | Description |
|------|------|-------------|
|polygon|H3Lib.GeoPolygon|Polygon to be filled|
|r|int|Resolution of cells to fill polygon|

## H3SetToLinkedGeo

```c#
void Api.H3SetToLinkedGeo(List<H3Index> h3Set, out LinkedGeoPolygon outPolygon)
```

### H3SetToLinkedGeo Summary

Create a LinkedGeoPolygon describing the outline(s) of a set of hexagons. Polygon outlines will follow GeoJSON MultiPolygon order:

Each polygon will have one outer loop, which is first in the list, followed by any holes.

It is the responsibility of the caller to call DestroyLinkedPolygon on the populated linked geo structure, or the memory for that structure will not be freed.

It is expected that all hexagons in the set have the same resolution and that the set contains no duplicates. Behavior is undefined if duplicates or multiple resolutions are present, and the algorithm may produce unexpected or invalid output.

### H3SetToLinkedGeo Parameters

| Name | Type | Description |
|------|------|-------------|
|h3Set|List&lt;H3Lib.H3Index&gt;|H3Index cells to create a polygon from|
|outPolygon|**out** H3Lib.LinkedGeoPolygon|The resulting polygon|

## DestroyLinkedPolygon

```c#
void Api.DestroyLinkedPolygon(LinkedGeoPolygon polygon)
```

### DestroyLinkedPolygon Summary

Free all allocated memory for a linked geo structure.
The caller is responsible for freeing memory allocated
to the input polygon struct.

### DestroyLinkedPolygon Parameters

| Name | Type | Description |
|------|------|-------------|
|polygon|LinkedGeoPolygon|Polygon to clear|

<hr>

[Return to Uber API Table of Contents](Uber-Api.md)
