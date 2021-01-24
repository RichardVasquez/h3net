# H3Lib Data Structures

The majority of these items are readonly struct value types.
This provides the ability for a fluid architecture, and a
simpler approach to the API rather than having a large amount
of ref based parameters in function calls.  It also allows
for removing some overhead on detecting duplicates, as there's
some internal functionality that uses HashSet<T> for dealing
with what could be overlapping data during recursions.

A quick example going from the original C to the 3.1.1 based C#
version to the current method follows:

**Original C**
```c++
_ijkScale(&transVec, unitScaleByCIIres[adjRes] * 3);
_ijkAdd(ijk, &transVec, ijk);
_ijkNormalize(ijk);
```
**C# v3.1.1**
```c#
CoordIJK transVec = fijkOrient.translate;
CoordIJK._ijkScale(ref transVec, unitScaleByCIIres[adjRes] * 3);
CoordIJK._ijkAdd(ijk, transVec, ref ijk);
CoordIJK_ijkNormalize(ref ijk);
```
**C# v.3.7.1**
```c#
ijk =
    (ijk + fijkOrient.Translate * Constants.FaceIjk.UnitScaleByCiiRes[adjRes] * 3)
   .Normalized();
```
There's tradeoffs, such as you can no longer do something like:
```C#
H3Index h3 = new H3Index();
h3.SetResolution(12);
```
You now have to reassign the result of the action like so:
```C#
H3Index h3 = new H3Index();
h3 = h3.SetResolution(12);
```
But it _does_ let you do something like so:
```C#
h3Index h3 = new h3Index().SetResolution(12);
```
Again, like I said, tradeoffs.  I'm fine with it even if it led to many hours finding all the
assignment replacements in the code.

The exception to this design are the linked geo data structures.  Those are regular
classes with references to each other, though I've added the .Net LinkedList to handle
the work for maintaining loops and such.  More on those, later.
