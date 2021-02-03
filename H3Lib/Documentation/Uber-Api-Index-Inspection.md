# Index Inspection Functions

These functions provide metadata about an H3 index, such as its
resolution or base cell, and provide utilities for converting
into and out of the 64-bit representation of an H3 index.

## H3GetResolution

```c#
int Api.H3GetResolution(H3Index h)
```

### H3GetResolution Summary

returns the resolution of the provided H3 index

### H3GetResolution Parameters

| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index cell to get resolution of |

## H3GetBaseCell

```c#
int Api.H3GetBaseCell(H3Index h)
```

### H3GetBaseCell Summary

returns the base cell "number" (0 to 121) of the provided H3 cell

### H3GetBaseCell Parameters

| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index cell to find the base cell number of |

## StringToH3

```c#
H3Index Api.StringToH3(string s)
```

### StringToH3 Summary

converts the canonical string format to H3Index format

Returns 0 on error.

### StringToH3 Parameters

| Name | Type | Description |
|------|------|-------------|
| s | string | string to parse into an H3Index value |

## H3ToString

```c#
void Api.H3ToString(H3Index h, out string str)
```

### H3ToString Summary

Converts the H3Index representation of the
index to the string representation.

### H3ToString Parameters

| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index value to turn into a string |
| str | **out** string | The string representation of the H3Index value |

## H3IsValid

```c#
int Api.H3IsValid(H3Index h)
```

### H3IsValid Summary

Returns non-zero if this is a valid H3 index.

### H3IsValid Parameters

| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index to inspect |

## H3IsResClassIII

```c#
int Api.H3IsResClassIii(H3Index h)
```

### H3IsResClassIII Summary

Returns non-zero if this index has a resolution with
Class III orientation.

### H3IsResClassIII Parameters

| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index cell under examination |

## H3IsPentagon

```c#
int Api.H3IsPentagon(H3Index h)
```

### H3IsPentagon Summary

Returns non-zero if this index represents a pentagonal cell.

### H3IsPentagon Parameters

| Name | Type | Description |
|------|------|-------------|
| h | H3Lib.H3Index | H3Index cell under investigation |

## H3GetFaces

```c#
void Api.H3GetFaces(H3Index h3, out List<int> outFaces)
```

### H3GetFaces Summary

Find all icosahedron faces intersected by a given H3 index
and places them in the List&lt;int&gt; outFaces.

Faces are represented as integers from 0-19, inclusive.
The array is sparse, and empty (no intersection) array
values are represented by -1.
maxFaceCount

### H3GetFaces Parameters

| Name | Type | Description |
|------|------|-------------|
| h3 | H3Lib.H3Index | H3Index cell under investigation |
| outFaces | **out** List&lt;int&gt; | List of faces overlapped by H3Index cell |

## MaxFaceCount

```c#
int Api.MaxFaceOunt(H3Index h3)
```

### MaxFaceCount Summary

Returns the maximum number of icosahedron faces the given H3
index may intersect.

### MaxFaceCount Parameters

| Name | Type | Description |
|------|------|-------------|
| h3 | H3Lib.H3Index | H3Index cell being examined |

<hr>

[Return to Uber API Table of Contents](Uber-Api.md)
