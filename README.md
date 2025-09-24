Input:
```
XXXXXXXXXX
X........X
X.S......X
X...X....X
X...X.XX.X
X...X.X..X
X..XXX...X
X....B...X
X....C...X
XXXXXXXXXX
```

Output:
16

Input is a 10x10 map, where:
X - Wall
S - Player
B - Box
C - Finish
. - Empty Space

Program finds the shortest path using BFS algorithm. It creates paralel cases, tracking all paths lengths and orientation of storeKeeper at each maps square.
Program ends when paths are getting too long, or  a valid path is found and all remaining paths are longer then the right one.
