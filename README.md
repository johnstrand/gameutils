# GameUtils

This is a collection of utilities for game development (and other things). It is a work in progress.

The library is split into several modules:
	* `Animation` - Easing, offsetting, and other animation utilities
	* `Console` - Various utilities for using ANSI escape codes to format console output
	* `Extensions`- A collection of extension methods for various types
	* `Math` - Methods for working with vectors, matrices, and floating point numbers
	* `Types` - A collection of useful types
	  * `Color` - A color type that can be converted to and from Vector3 and Vector4
	  * `Color.Names` - A collection of named colors, based on https://en.wikipedia.org/wiki/Web_colors
	  * `Dijkstra` - A pathfinding algorithm
	  * `Ray` - A ray type, with methods for intersection testing
	  * `SynchronizedCollection` - A base-class for a thread-safe and tick-based collection