# Miscellaneous H3 functions
These functions include descriptions of the H3 grid system.

## DegsToRads
```c#
decimal Api.DegsToRads(decimal degrees)
```
### Summary
Converts degrees to radians.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|degrees|decimal|Degree to change to radians|

## RadsToDegs
```c#
decimal Api.RadsToDegs(decimal radians)
```
### Summary
Converts radians to degrees.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|radians|decimal|Radians to change to degrees|

## HexAreaKm2
```c#
decimal Api.HexAreaKm2(int r)
```
### Summary
Average hexagon area in square kilometers at the given resolution.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|r|int|Resolution to find average of|

## HexAreaM2
```c#
decimal Api.HexAreaM2(int r)
```
### Summary
Average hexagon area in square meters at the given resolution.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|r|int|Resolution to find average of|

## CellAreaKm2
```c#
decimal Api.CellAreaKm2(H3Index h)
```
### Summary
Exact area of specific cell in square kilometers.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|h|H3Lib.H3Index|Cell to find area of|


## CellAreaM2
```c#
decimal Api.CellAreaM2(H3Index h)
```
### Summary
Exact area of specific cell in square meters.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|h|H3Lib.H3Index|Cell to find area of|

## CellAreaRads2
```c#
decimal Api.CellAreaRads2(H3Index h)
```
### Summary
Exact area of specific cell in square radians.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|h|H3Lib.H3Index|Cell to find area of|

## EdgeLengthKm
```c#
decimal Api.EdgeLengthKm(int r)
```
### Summary
Average hexagon edge length in kilometers at the given resolution.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|r|int|Resolution|

## EdgeLengthM
```c#
decimal Api.EdgeLengthM(int r)
```
### Summary
Average hexagon edge length in meters at the given resolution.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|r|int|Resolution|


## ExactEdgeLengthKm
```c#
decimal Api.ExactEdgeLengthKm(H3Index edge)
```
### Summary
Exact edge length of specific unidirectional edge in kilometers.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|edge|H3Lib.H3Index|Unidirecitonal edge to measure|
## ExactEdgeLengthM
```c#
decimal Api.ExactEdgeLengthM(H3Index edge)
```
### Summary
Exact edge length of specific unidirectional edge in meters.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|edge|H3Lib.H3Index|Unidirecitonal edge to measure|

## ExactEdgeLengthRads
```c#
decimal Api.ExactEdgeLengthRads(H3Index edge)
```
### Summary
Exact edge length of specific unidirectional edge in radians.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|edge|H3Lib.H3Index|Unidirecitonal edge to measure|

## NumHexagons
```c#
long Api.NumHexagons(int r)
```
### Summary
Number of unique H3 indexes at the given resolution.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|r|int|Resolution to count cells of|
## GetRes0Indexes
```c#
void Api.GetRes0Indexes(out List<H3Index> outCells)
```
### Summary
All the resolution 0 H3 indexes. out must be an array of at least size res0IndexCount().
### Parameters
| Name | Type | Description |
|------|------|-------------|
|outCells|**out** List&lt;H3Lib.H3Index&gt;|Cells of resolution 0|

## Res0IndexCount
```c#
int Api.Res0IndexCount()
```
### Summary
Number of resolution 0 H3 indexes.

## GetPentagonIndexes
```c#
void Api.GetPentagonIndexes(int r, out List<H3Index> outCells)
```
### Summary
All the pentagon H3 indexes at the specified resolution.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|r|int|Resolution of pentagons|
|outCells|**out** List&lt;H3Lib.H3Index&gt;|List of pentagons|

## PentagonIndexCount
```c#
int Api.PentagonIndexCount()
```
### Summary
Number of pentagon H3 indexes per resolution. This is always 12, but provided as a convenience.

## PointDistKm
```c#
decimal Api.PointDistKm(GeoCoord a, GeoCoord b)
```
### Summary
Gives the "great circle" or "haversine" distance between pairs of GeoCoord points (lat/lng pairs) in kilometers.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|a|H3Lib.GeoCoord|Point A|
|b|H3Lib.GeoCoord|Point B|

## PointDistM
```c#
decimal Api.PointDistM(GeoCoord a, GeoCoord b)
```
### Summary
Gives the "great circle" or "haversine" distance between pairs of GeoCoord points (lat/lng pairs) in meters.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|a|H3Lib.GeoCoord|Point A|
|b|H3Lib.GeoCoord|Point B|

## PointDistRads
```c#
decimal Api.PointDistRads(GeoCoord a, GeoCoord b)
```
### Summary
Gives the "great circle" or "haversine" distance between pairs of GeoCoord points (lat/lng pairs) in radians.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|a|H3Lib.GeoCoord|Point A|
|b|H3Lib.GeoCoord|Point B|

<hr>

[Return to Uber API Table of Contents](Uber-Api.md)
