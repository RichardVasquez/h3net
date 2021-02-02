# Index inspection functions

These functions provide metadata about an H3 index, such as its
resolution or base cell, and provide utilities for converting
into and out of the 64-bit representation of an H3 index.

## H3GetResolution
```c#
int Api.H3GetResolution(H3Index h)
```
### Summary
returns the resolution of the provided H3 index

### Parameters
| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index cell to get resolution of |

## H3GetBaseCell
```c#
int Api.H3GetBaseCell(H3Index h)
```
### Summary
returns the base cell "number" (0 to 121) of the provided H3 cell
### Parameters
| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index cell to find the base cell number of |

## StringToH3
```c#
H3Index Api.StringToH3(string s)
```
### Summary
converts the canonical string format to H3Index format

Returns 0 on error.
### Parameters
| Name | Type | Description |
|------|------|-------------|
| s | string | string to parse into an H3Index value |

## H3ToString
```c#
void Api.H3ToString(H3Index h, out string str)
```
### Summary
Converts the H3Index representation of the
index to the string representation.
### Parameters
| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index value to turn into a string |
| str | **out** string | The string representation of the H3Index value | 

## H3IsValid
```c#
int Api.H3IsValid(H3Index h)
```
### Summary
Returns non-zero if this is a valid H3 index.
### Parameters
| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index to inspect |

## H3IsResClassIII
```c#
int Api.H3IsResClassIii(H3Index h)
```
### Summary
Returns non-zero if this index has a resolution with
Class III orientation.
### Parameters
| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index cell under examination |

## H3IsPentagon
```c#
int Api.H3IsPentagon(H3Index h)
```
### Summary
Returns non-zero if this index represents a pentagonal cell.
### Parameters
| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index cell under investigation |

## H3GetFaces
```c#
void Api.H3GetFaces(H3Index h3, out List<int> outFaces)
```
### Summary
Find all icosahedron faces intersected by a given H3 index
and places them in the List<int> outFaces.

Faces are represented as integers from 0-19, inclusive.
The array is sparse, and empty (no intersection) array
values are represented by -1.
maxFaceCount
### Parameters
| Name | Type | Description |
|------|------|-------------|
| h3 | H3Lib.H3Index | H3Index cell under investigation |
| outFaces | **out** List<int> | List of faces overlapped by H3Index cell |

## int MaxFaceCount(H3Index h3);
```c#
int Api.MaxFaceOunt(H3Index h3)
```
### Summary
Returns the maximum number of icosahedron faces the given H3
index may intersect.
### Parameters
| Name | Type | Description |
|------|------|-------------|
| h3 | H3Lib.H3Index | H3Index cell being examined |

<hr>

[Return to Uber API Table of Contents](Uber-Api.md)
