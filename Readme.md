# H3NET: A Hexagonal Hierarchical Geospatial Indexing System In C# #
| | |
|---|---|
| H3Net is a geospatial indexing system using hexagonal grid that can be (approximately) subdivided into finer and finer hexagonal grids, combining the benefits of a hexagonal grid with [S2](https://code.google.com/archive/p/s2-geometry-library/) hierarchical subdivisions, mostly translated from the original C code from [Uber's](https://github.com/uber) [H3](https://github.com/uber/h3) project.| ![h3net logo](./h3net.300.png)|



## Why? There's already a version in C!

The short version:

I'm working on a project that needs H3 capabilities in C#. When I first started this, I could
make the bindings work on desktop, but not mobile, so I wrote the 3.1.1 version.  Working with
it in my project over the past couple of years, along with some of the new things in H3, I
came back to it to work with 3.7.1 capabilities.

That's what's here now.  I'm still working on it, but it's now at a usable, albeit
[lightly documented](H3Lib/Documentation) state.  You really should check out the
H3 link I placed before, if you want to know what's involved here.

## History
* Current (3rd round) - Currently has the same capability as
  3.7.1, if not exactly the same syntax.
* Version 2 - A horrible attempt to implement H3 v3.2.1, and
  I've removed the branch for it.
* Version 1 - According to my Git repository, I pushed it on
  November 16, 2018.  It has the capability of Uber H3 3.1.1,
  but it has a [horrible API](OldApi.md).

## Caveat Emptor
This doesn't work *exactly* like H3, especially under the hood,
but it's close enough for most work, I feel.

Right now, you can probably work your way through the syntax in
Api.cs in the lib directory.  For actual use, I've got a fluid
API to chain commands together, though most of that is being
used internally.

I'm also going to be closing off access to the internals shortly
so that functionality reflecting the API will be the only direct
access to the code.

At this point, the only comprehensive documentation is the auto-generated
file at [H3Lib/Documentation](H3Lib/Documentation).  Keep an eye on that
directory as I'll be cleaning that up next.

## Input
I'm going to be doing the following:
  * Cleaning up the code, including some of the brute force translations
  * Extracting/writing documentation for the library
  * Probably adding a few unit tests for the modifications I've made.

You can help too.  Fork and make a PR, and we'll go from there.

## Caveat Emptor II
I wanted to get this done in a month.  I have.  It's **nowhere** near
the polished state I want it to be, but it works, and it passes 200+
unit tests.  It's now ready to be played with.

Don't go crazy with it just yet, as I'm going to clean it up some more, 
work on documentation, deal with TODO's in the code, and so forth.  At
some point after that, I'll have an actual release.

## Testing
For the most part, I've converted the unit tests from the original H3
project to work in h3net.  They were extremely helpful with the
architecture change going from 3.1.1 to 3.7.1.

Some corner cases weren't convertible as they tested for null objects
while h3net uses primarily extension methods on readonly structs.
Where this has come up, I've documented it in the appropriate unit test
file.

## Version
I will be keeping the version number the same as the functionality of
H3 that I'm matching.

Currently: **3.7.1**

Previous: **3.1.1**

## Badges
![.NET](https://github.com/RichardVasquez/h3net/workflows/.NET/badge.svg)
[![Maintainability](https://api.codeclimate.com/v1/badges/ed65501f16bda4b50200/maintainability)](https://codeclimate.com/github/RichardVasquez/h3net/maintainability)
![JetBrains Rider](https://img.shields.io/badge/-Rider-blue?style=flat&logo=JetBrains)
![H3 3.7.1](https://img.shields.io/badge/H3-3.7.1-brightgreen)
![Burma Shave](https://img.shields.io/badge/Burma-Shave-brightgreen)
## Fin
[Hexagons are the bestagons](https://www.youtube.com/watch?v=thOifuHs6eY)
