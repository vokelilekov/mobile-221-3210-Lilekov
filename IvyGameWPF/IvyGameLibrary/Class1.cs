using System;
using System.Collections.Generic;
using System.Linq;

namespace MahjongGameLibrary
{
    public class Tile
    {
        public int Index { get; set; }
        public int Type { get; set; }
        public int Layer { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsAvailable { get; set; } = false;
    }

    public enum Difficulty
    {
        Easy = 5,
        Medium = 7,
        Hard = 9
    }

    public enum Shape
    {
        Pyramid,
        Heart
    }

    public class GameField
    {
        public List<Tile> Tiles { get; private set; }
        private const int TileTypes = 10;
        public int MaxLayers;
        private Shape selectedShape;
        public bool isWFA;
        private static readonly Random random = new Random();

        public GameField(Difficulty difficulty, Shape shape, bool ensureSolvable = true, bool isWFA = false)
        {
            MaxLayers = (int)difficulty;
            selectedShape = shape;
            Tiles = new List<Tile>();

            if (ensureSolvable)
            {
                GenerateSolvableFigure();
            }
            else
            {
                GenerateRandomFigure();
            }
        }

        private void GenerateSolvableFigure()
        {
            Tiles.Clear();
            GenerateFigureWithLogic();
            ShuffleTiles();
            UpdateTileAvailability();
        }

        private void GenerateRandomFigure()
        {
            Tiles.Clear();
            var typeCounts = new int[TileTypes];
            EnsureTripleTiles(typeCounts);
            GenerateSymmetricalFigures(typeCounts);
            ShuffleTiles();
            UpdateTileAvailability();
        }

        private void GenerateFigureWithLogic()
        {
            var typeCounts = new int[TileTypes];
            EnsureTripleTiles(typeCounts);
            GenerateSymmetricalFigures(typeCounts);
        }

        private void EnsureTripleTiles(int[] typeCounts)
        {
            int totalTiles = MaxLayers * MaxLayers * 3;
            int repeatsPerType = Math.Max(6, totalTiles / TileTypes);

            for (int i = 0; i < TileTypes; i++)
            {
                typeCounts[i] = repeatsPerType;
            }

            int remainingTiles = totalTiles - TileTypes * repeatsPerType;

            while (remainingTiles > 0)
            {
                int randomType = random.Next(TileTypes);
                typeCounts[randomType]++;
                remainingTiles--;
            }
        }

        private void GenerateSymmetricalFigures(int[] typeCounts)
        {
            if (selectedShape == Shape.Pyramid)
            {
                GeneratePyramidWithIncreasedTriplets(typeCounts);
            }
            else if (selectedShape == Shape.Heart)
            {
                GenerateHeartWithIncreasedTriplets(typeCounts);
            }
        }

        public void GeneratePyramidWithIncreasedTriplets(int[] typeCounts)
        {
            var buffer = CreateTileBuffer(typeCounts);

            int centerOffset = MaxLayers / 2;
            int bufferIndex = 0;

            for (int layer = 0; layer < MaxLayers; layer++)
            {
                int layerSize = layer + 1;

                for (int x = 0; x < layerSize; x++)
                {
                    for (int y = 0; y < layerSize; y++)
                    {
                        if (bufferIndex >= buffer.Count) break;

                        var tile = buffer[bufferIndex++];
                        tile.Layer = layer;
                        tile.X = centerOffset - layer / 2 + x;
                        tile.Y = centerOffset - layer / 2 + y;

                        if (tile.X == centerOffset - layer / 2 || tile.Y == centerOffset - layer / 2)
                        {
                            tile.IsAvailable = true;
                        }

                        Tiles.Add(tile);
                    }
                }
            }
        }

        public void GenerateHeartWithIncreasedTriplets(int[] typeCounts)
        {
            var buffer = CreateTileBuffer(typeCounts);
            int[,] heartMatrix = GetHeartMatrix();

            int rows = heartMatrix.GetLength(0);
            int cols = heartMatrix.GetLength(1);
            int bufferIndex = 0;

            for (int layer = 0; layer < rows; layer++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (heartMatrix[layer, x] == 1 && bufferIndex < buffer.Count)
                    {
                        var tile = buffer[bufferIndex++];
                        tile.Layer = layer;
                        tile.X = x - cols / 4;
                        tile.Y = layer;
                        if (tile.X == cols / 4 || tile.Y == rows / 2)
                        {
                            tile.IsAvailable = true;
                        }

                        Tiles.Add(tile);
                    }
                }
            }
        }

        public void GeneratePyramid(int[] typeCounts)
        {
            var buffer = CreateTileBuffer(typeCounts);

            int centerOffset = MaxLayers / 2;
            int bufferIndex = 0;

            for (int layer = 0; layer < MaxLayers; layer++)
            {
                int layerSize = layer + 1;

                for (int x = 0; x < layerSize; x++)
                {
                    for (int y = 0; y < layerSize; y++)
                    {
                        if (bufferIndex >= buffer.Count) break;

                        var tile = buffer[bufferIndex++];
                        tile.Layer = layer;
                        tile.X = centerOffset - layer / 2 + x;
                        tile.Y = centerOffset - layer / 2 + y;

                        Tiles.Add(tile);
                    }
                }
            }
        }

        public void GenerateHeart(int[] typeCounts)
        {
            var buffer = CreateTileBuffer(typeCounts);
            int[,] heartMatrix = GetHeartMatrix();

            int rows = heartMatrix.GetLength(0);
            int cols = heartMatrix.GetLength(1);
            int bufferIndex = 0;

            for (int layer = 0; layer < rows; layer++)
            {
                for (int x = 0; x < cols; x++)
                {
                    if (heartMatrix[layer, x] == 1 && bufferIndex < buffer.Count)
                    {
                        var tile = buffer[bufferIndex++];
                        tile.Layer = layer;
                        tile.X = x - cols / 4;
                        tile.Y = layer;

                        Tiles.Add(tile);
                    }
                }
            }
        }

        public void GeneratePyramidForWFA(int maxLayers, List<Tile> tiles)
        {
            int index = 0;
            for (int layer = 0; layer < maxLayers; layer++)
            {
                int tilesInLayer = maxLayers - layer;
                for (int i = 0; i < tilesInLayer; i++)
                {
                    var tile = tiles[index++];
                    tile.Layer = maxLayers - 1 - layer;

                    tile.X = i - tilesInLayer / 2;
                    tile.Y = maxLayers - 1 - layer;
                    tile.IsAvailable = (layer == 0);

                    Tiles.Add(tile);
                }
            }
        }

        public void GenerateHeartForWFA(int maxLayers, List<Tile> tiles)
        {
            int index = 0;
            for (int layer = 0; layer < maxLayers; layer++)
            {
                int tilesInLayer = maxLayers - Math.Abs(layer - maxLayers / 2);
                for (int i = 0; i < tilesInLayer; i++)
                {
                    var tile = tiles[index++];
                    tile.Layer = maxLayers - 1 - layer;
                    tile.X = i - tilesInLayer / 2;
                    tile.Y = maxLayers - 1 - layer;

                    tile.IsAvailable = (layer == 0);

                    Tiles.Add(tile);
                }
            }
        }

        private List<Tile> CreateTileBuffer(int[] typeCounts)
        {
            int tileIndex = 0;
            var buffer = new List<Tile>();

            for (int type = 0; type < TileTypes; type++)
            {
                for (int count = 0; count < typeCounts[type]; count++)
                {
                    buffer.Add(new Tile
                    {
                        Index = tileIndex++,
                        Type = type,
                        IsAvailable = true
                    });
                }
            }

            return buffer.OrderBy(_ => random.Next()).ToList();
        }

        private int[,] GetHeartMatrix()
        {
            int size = MaxLayers;
            var heart = new int[size, size * 2 - 1];

            for (int i = 0; i < size; i++)
            {
                for (int j = size - i - 1; j < size + i; j++)
                {
                    heart[i, j] = 1;
                }
            }

            return heart;
        }

        private void ShuffleTiles()
        {
            Tiles = Tiles.OrderBy(_ => random.Next()).ToList();
        }

        public void RemoveTile(Tile tile)
        {
            Tiles.Remove(tile);
            UpdateTileAvailability();
        }

        private void UpdateTileAvailability()
        {
            foreach (var tile in Tiles)
            {
                if (isWFA)
                {
                    tile.IsAvailable = !Tiles.Any(t =>
                        t.Layer < tile.Layer &&
                        Math.Abs(t.X - tile.X) <= 1 &&
                        Math.Abs(t.Y - tile.Y) <= 1);
                }
                else
                {
                    tile.IsAvailable = !Tiles.Any(t =>
                        t.Layer > tile.Layer &&
                        Math.Abs(t.X - tile.X) <= 1 &&
                        Math.Abs(t.Y - tile.Y) <= 1);
                }
            }
        }
    }

    public class CollectionField
    {
        private const int MaxFieldSize = 7;
        public List<Tile> Tiles { get; private set; }
        public bool IsFull => Tiles.Count >= MaxFieldSize;

        public CollectionField()
        {
            Tiles = new List<Tile>();
        }

        public bool AddTile(Tile tile)
        {
            if (Tiles.Count < MaxFieldSize)
            {
                Tiles.Add(tile);
                return true;
            }
            return false;
        }

        public List<Tile> CheckForTriplets()
        {
            var triplets = new List<Tile>();
            var groups = Tiles.GroupBy(t => t.Type).Where(g => g.Count() >= 3).ToList();

            foreach (var group in groups)
            {
                triplets.AddRange(group.Take(3));
            }

            foreach (var tile in triplets)
            {
                Tiles.Remove(tile);
            }

            return triplets;
        }
    }
}
