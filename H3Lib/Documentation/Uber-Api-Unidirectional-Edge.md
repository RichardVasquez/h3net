# Unidirectional edge functions

Unidirectional edges allow encoding the directed edge from one cell
to a neighboring cell.

## H3IndexesAreNeighbors
```c#
int Api.H3IndexesAreNeighbors(H3Index origin, H3Index destination)
```
### Summary
Returns whether or not the provided H3Indexes are neighbors.

Returns 1 if the indexes are neighbors, 0 otherwise.

### Parameters
| Name | Type | Description |
|------|------|-------------|
|origin|H3Lib.H3Index|Origin looking for neighbor|
|destination|H3Lib.H3Index|Cell being tested if neighbor|

## GetH3UnidirectionalEdge
```c#
H3Index Api.GetH3UnidirectionalEdge(H3Index origin, H3Index destination)
```
### Summary
Returns a unidirectional edge H3 index based on the provided origin and
destination.

Returns 0 on error.

### Parameters
| Name | Type | Description |
|------|------|-------------|
|origin|H3Lib.H3Index|Origin cell for edge|
|destination|H3LIb.H3Index|Destination cell for edge|

## H3UnidirectionalEdgeIsValid
```c#
int Api.H3UnidirectionalEdgeIsValid(H3Index edge)
```
### Summary
Determines if the provided H3Index is a valid unidirectional edge index.

Returns 1 if it is a unidirectional edge H3Index, otherwise 0.

### Parameters
| Name | Type | Description |
|------|------|-------------|
|edge|H3Lib.H3Index|Unidirectional edge being tested|

## GetOriginH3IndexFromUnidirectionalEdge
```c#
H3Index Api.GetOriginH3IndexFromUnidirectionalEdge(H3Index edge)
```
### Summary
Returns the origin hexagon from the unidirectional edge H3Index.

### Parameters
| Name | Type | Description |
|------|------|-------------|
|edge|H3Lib.H3Index|Edge to find origin cell|


## GetDestinationH3IndexFromUnidirectionalEdge
```c#
H3Index Api.GetDestinationH3IndexFromUnidirectionalEdge(H3Index edge)
```
### Summary
Returns the destination hexagon from the unidirectional edge H3Index.

### Parameters
| Name | Type | Description |
|------|------|-------------|
|edge|H3Lib.H3Index|Edge to find destination cell|


## GetH3IndexesFromUnidirectionalEdge
```c#
void Api.GetH3IndexesFromUnidirectionalEdge(H3Index edge, out (H3Index, H3Index) originDestination)
```
### Summary
Returns the origin, destination pair of hexagon IDs for the given edge ID,
which are placed at originDestination.Item1 and originDestination.Item2
respectively.

### Parameters
| Name | Type | Description |
|------|------|-------------|
|edge|H3Lib.H3Index|Edge to find origin and destination cells|
|originDestination|**out** Tuple&lt;H3Lib.H3Index, H3Lib.H3Index&gt;|paired origin/destination H3Index cells for edge|



## GetH3UnidirectionalEdgesFromHexagon
```c#
void Api.GetH3UnidirectionalEdgesFromHexagon(H3Index origin, out List<H3Index> edges)
```
### Summary
Provides all of the unidirectional edges from the current H3Index.
The number of undirectional edges placed in edges may be less than 6.

### Parameters
| Name | Type | Description |
|------|------|-------------|
|origin|H3Lib.H3Index|Origin cell to get edges from|
|edges|**out** List&lt;H3Lib.H3Index&gt;|The edges originating from the cell|
## GetH3UnidirectionalEdgeBoundary
```c#
void Api.GetH3UnidirectionalEdgeBoundary(H3Index edge, out GeoBoundary gb)
```
### Summary
Provides the coordinates defining the unidirectional edge.

### Parameters
| Name | Type | Description |
|------|------|-------------|
|edge|H3Lib.H3Index|Edge to find coordinates of|
|gb|H3Lib.GeoBoundary|The geoboundary that defines the edge in terms of vertices|

<hr>

[Return to Uber API Table of Contents](Uber-Api.md)
