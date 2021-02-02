# Hierarchical grid functions

These functions permit moving between resolutions in
the H3 grid system. The functions produce parent
(coarser) or children (finer) cells.

## H3ToParent
```c#
H3Lib.H3Index Api.H3ToParent(H3Index h, int parentRes)
```
### Summary
Returns the parent (coarser) index containing h.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|h|H3Lib.3Index|Cell to find the parent of|
|parentRes|int|coarser resolution|

## H3ToChildren
```c#
void Api.H3ToChildren(H3Index h, int childRes, out List<H3Index> outChildren)
```
### Summary
Populates children with the indexes contained by h at resolution
childRes. children must be an array of at least size
maxH3ToChildrenSize(h, childRes).
### Parameters
| Name | Type | Description |
|------|------|-------------|
|h|H3Lib.H3Index|Cell to find children of|
|childRes|int|finer resolution|
|outChildren|**out** List&lt;H3Lib.H3Index&gt;|child cells|

## MaxH3ToChildrenSize
```c#
long Api.MaxH3ToChildrenSize(H3Index h, int childRes)
```
### Summary
Returns the size of the array needed by h3ToChildren for these inputs.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|h|H3Lib.H3Index|Cell to find children of|
|childRes|int|Resolution of children|

## H3ToCenterChild
```c#
H3Lib.H3Index Api.H3ToCenterChild(H3Index h, int childRes)
```
### Summary
Returns the center child (finer) index contained by h at resolution
childRes.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|h|H3Lib.H3Index|Cell to find center child of|
|childRes|int|finer resolution|

## Compact
```c#
int Api.Compact(List<H3Index> h3Set, out List<H3Index> outCompacted)
```
### Summary
Compacts the set h3Set of indexes as best as possible, into the List
outCompacted. 

Returns 0 on success.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|h3Set|List&lt;H3Lib.H3Index&gt;|Cells to compact|
|outCompacted|**out** List&lt;H3Lib.H3Index&gt;|Compacted cells|

## Uncompact
```c#
int Api.Uncompact(List<H3Index> compactedSet, out List<H3Index> outCells, int res)
```
### Summary
Uncompacts the set compactedSet of indexes to the resolution res.

Returns 0 on success.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|compactedSet|List&lt;H3Lib.H3Index&gt;|Compacted cells|
|outCells|**out** List&lt;H3Lib.H3Index&gt;|Uncompacted cells|
|res|int|Resolution to uncompact to|

## MaxUncompactSize
```c#
long Api.MaxUncompactSize(H3Index compacted, int r)
```
### Summary
Returns the size of the array needed by uncompact.
### Parameters
| Name | Type | Description |
|------|------|-------------|
|compacted|H3Lib.H3Index|Cell to uncompact|
|r|int|Resolution to uncompact to|

<hr>

[Return to Uber API Table of Contents](Uber-Api.md)
