# Extension methods

I've segregated the functionality of each data type to reside in extension methods
for now.  I may roll them back in at a later time, but with the original H3 code
being written in C, with its procedural structure, it felt unusual to find a function
that processed a GeoCoord in the FaceIjk code file mainly since that's where data was
being stored that the GeoCoord code needed.
