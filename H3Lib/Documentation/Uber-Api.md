# Uber H3 API

This is the direct translation of the Uber H3 API, with some changes,
since it's C# instead of C.  No pointers or pointer addresses, so
functions that use them in the original tend to have an **out** parameter
where the results will be placed.

Additionally, something I discovered is that Microsoft basically took a
shortcut in their trigonometric functions that are still accurate to many
decimal points, but not enough.  As such, this required a retool to use
decimal data types and an external library to maintain that accuracy for
trig operations.

All your non-integer values, as such, will be in the decimal data type.
I'd recommend performing your H3 operations sequentially, then if you have
need of the final result in another type, do your cast then.  Interim casts
then recast back to decimal are not guaranteed to maintain correctness.

## Contents
[Indexing functions](Uber-Api-Indexing.md) - These functions are used for
finding the H3 index containing coordinates, and for finding the center
and boundary of H3 indexes.

[Index Inspection Functions](Uber-Api-Index-Inspection.md) - These
functions provide metadata about an H3 index, such as its resolution or
base cell, and provide utilities for converting into and out of the 64-bit
representation of an H3 index.

[Grid Traversal Functions](Uber-Api-Grid-Traversal.md) - Grid traversal
allows finding cells in the vicinity of an origin cell, and determining
how to traverse the grid from one cell to another.

[Hierarchal Grid Functions](Uber-Api-Hierarchical-Grid.md) - These
functions permit moving between resolutions in the H3 grid system. The
functions produce parent (coarser) or children (finer) cells.

[Region Functions](Uber-Api-Region.md) - These functions convert H3
indexes to and from polygonal areas.

[Unidirectional Edge Functions](Uber-Api-Unidirectional-Edge.md) -
Unidirectional edges allow encoding the directed edge from one cell
to a neighboring cell.

[Miscellaneous H3 Functions](Uber-Api-Miscellaneous-H3.md) - These
functions include descriptions of the H3 grid system.
