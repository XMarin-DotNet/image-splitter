# ImageSplitter
[![Build status](https://ci.appveyor.com/api/projects/status/b1bjh1obv73n88lf?svg=true)](https://ci.appveyor.com/project/gerich-home/image-splitter)

Splits image into a set of tiles given tile size and a file

# Usage:
```
ImageSplitter.exe <input-file-name> <desired-tile-width> <desired-tile-height> <tile-file-extension>
```

## Example:
```
ImageSplitter.exe input.bmp 32 32 png
```

# Notes:
All output is placed into the directory: `out_<desired-tile-width>_<desired-tile-height>`

Output contains `_Result.<tile-file-extension>` file with the grid containing all tiles with their numbers plus a set of all tiles

If the input file sizes are not divisible by desired tile sizes then truncated tiles (for the last row and/or column) will be also produced

# Author:
Sergey Gerasimov

# License:
MSPL
