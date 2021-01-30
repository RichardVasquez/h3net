# Extension methods

In the previous C# version, to provide the same functionality
the C code did, I used a lot of ref parameters in function
calls.  This led to a messy code base, especially since I'd
have to copy a property from a class, perform an operation on
that property, then reassign the original class with it.

There's also sections in the original C code that use fairly
standard hashing functions with buckets and linked lists, and
C# has a Hashset&lt;T&gt; that works just fine so long as you
have good immutable data items.

That led to a majority of the data structures from the original
C code to being turned into C# readonly struct types.  Which
in turn can create other issues.  In the end, I decided it was
a viable approach.

As such, to perform operations that mutate values, you have to
reassign the value from the result of an operation.

So, ```h.SetResolution(10)``` becomes ```h = h.SetResolution(10)```

Dealing with individual fields/properties inside a struct becomes
a little more lengthy, but I think the payoff is worth it.

With operations becoming more external, along with the
reassignments needed, I started leaning towards making the
functions external as static extension methods.

In addition, it made a bit more sense to me since I could
segregate them into the domain of which struct they worked on.

While it makes sense in the C code to provide functions that
work on multiple data types due to having a requirement to
access data provided by the corresponding ```.h``` file during
compilation, it becomes a little noisy to me.  Additionally,
switching from procedural to OOP is going to require some
structural changes.

Ergo, operations on data structures are contained in these
extension method classes.  Some will get integrated back to
their data structures at a later iteration such as 3.7.1.#
