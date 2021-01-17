# H3NET: A Hexagonal Hierarchical Geospatial Indexing System In C# #

H3Net is a geospatial indexing system using hexagonal grid that can be (approximately) subdivided into finer and finer hexagonal grids, combining the benefits of a hexagonal grid with [S2](https://code.google.com/archive/p/s2-geometry-library/)'s hierarchical subdivisions, mostly translated from the original C code from [Uber](https://github.com/uber)'s [H3](https://github.com/uber/h3) project.

## Why? There's already a version in C!

1. Because I can.
    * I also wanted to refresh and expand my knowledge of C.
    * I am building a mobile game, and it's using Unity, and
      when I first started it, building C extensions wasn't
      working for me when going from desktop to Android.
2. Because I want to.
    * I've wanted to go beyond my Asp.Net/MVC web experience.
      While I also do [Project Euler](https://projecteuler.net)
      and [Advent of Code](https://adventodcode.com) mostly in
      C#, those are "one offs", as it were.
    * Besides the game I'm working on, I like 
      [hexagons](https://www.youtube.com/watch?v=thOifuHs6eY),
      I like tilings, and while this isn't tiling, but partitioning,
      on a local scale, it's pretty close.

## History
* Current - Sometime soon.  It'll have the capabilities of
  3.7.1, and I'm going to try to make the stock H3 API work.
  I'm also going to start deprecating my old API, **and**
  implement my new API based on fluid operations.
* Version 2 - A horrible attempt to implement H3 v3.2.1, and
  I've removed the branch for it.
* Version 1 - According to my Git repository, I pushed it on
  November 16, 2018.  It has the capability of Uber H3 3.1.1,
  but it has a [horrible API](OldApi.md).

## Caveat Emptor
This doesn't work *exactly* like H3, especially under the hood,
but it's close enough for most work, I feel.

Most data types used and returned are readonly structs, though
I have provided functions that will provide a mutated *copy*
of the original type.

The exceptions are the polygon functions, since they have to be
modifiable to some degree, though I've done some tinkering with
the internals to use actual the .Net LinkedList, but since most
of that's internal, it shouldn't really affect end users.

Starting from 3.7.1, I'll be throwing methods, fields, properties,
etc. into the appropriate scopes of private, internal, and public
so that entry points make sense, and people can't blow things up
easily.

In other words, I'm turning this from a proof of concept to a
designed tool.


## Testing
For the most part, I've converted the unit tests from the original H3
project to work in h3net.  They were extremely helpful with the
architecture change going from 3.1.1 to 3.7.1.

Some corner cases weren't convertible as they tested for null objects
while h3net uses primarily extension methods on readonly structs.
Where this has come up, I've documented it in the appropriate unit test
file.

## Roadmap
For the 3.* version, it will likely just be bug fixes and code cleaning.

The H3 library is moving to Version 4, and I'll likely try to keep up
with them, but I'll always be lagging in that regards.

## Version
I will be keeping the version number the same as the functionality of
H3 that I'm matching.

Currently: 3.7.1
