# NUnit Tests

The base test suites converted to NUnit functionality.

At this writing, there are 222 tests, and 10 of them fail.  My hunch, based on mapping
many points on the globe and mappping out hexagons and pentagons on h3explorer lead me
to considering that somewhere, in _some_ cases, shapes are slightly smaller than they're
supposed to be, which is causing discrepancies that lead to the test failures.

The _logic_ seems to work, i.e., Hex A has children A0-A6, and Pentagon P has children
P0-P5, but the actual geographic locations of the vertices are sometimes "off".

See [Issues](https://github.com/RichardVasquez/h3net/issues) for failing tests.
