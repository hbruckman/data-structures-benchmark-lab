# DataStructuresBenchmark
Benchmarking of several data structures for teaching purposes.
# Statistics:
- Mean: average elapsed ticks across 1000 runs
- Std Dev: standard deviation of elapsed ticks across 1000 runs
- Min/Max: minimum and maximum elapsed ticks observed
- Example Interpretation:
	- A: mean = 500, std-dev = 20
	- B: mean = 700, std-dev = 25
	- → Clear difference
	- A: mean = 500, std-dev = 150
	- B: mean = 600, std-dev = 160
	- → Likely indistinguishable
		sb.AppendLine();


# Why ArrayList (List) is fastest for “Add all values”

Interpretation:

Even though it resizes and copies, List<T> is fastest because:

- contiguous memory
- very few allocations
- CPU cache efficiency

Key learning point:

- Memory locality beats theoretical disadvantages.

# Why DoublyLinkedList (LinkedList) is slower

Interpretation:

Each insertion allocates a node and updates references. So:

- many allocations
- poor cache usage
- pointer-based structure

Key learning point:

- Even though both operations are O(1), O(1) for LinkedList.AddLast hides large constant factors.

# Why HashTable (Dictionary) is slowest to build

Interpretation:

Each insertion does more work:

- hashing
- bucket selection
- collision handling
- possible rehashing

# Real-life lessons

- Big-O ignores constants and memory effects.
- Contiguous memory is extremely powerful.
- Allocations are expensive.
- Data structures are optimized for specific operations.
