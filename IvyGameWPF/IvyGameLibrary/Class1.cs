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
        Medium = 10,
        Hard = 15
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

        public GameField(Difficulty difficulty, Shape shape, bool ensureSolvable = true)
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
            //do
            //{
                Tiles.Clear();
                GenerateFigureWithLogic();
                ShuffleTiles();
                UpdateTileAvailability();
            //}
            //while (!IsSolvable());
        }

        private void GenerateRandomFigure()
        {
            Tiles.Clear();
            var random = new Random();
            var typeCounts = new int[TileTypes];
            EnsureTripleTiles(typeCounts);
            GenerateSymmetricalFigures(typeCounts);
            ShuffleTiles();
            UpdateTileAvailability();
        }

        private void GenerateFigureWithLogic()
        {
            var random = new Random();
            var typeCounts = new int[TileTypes];
            EnsureTripleTiles(typeCounts);
            GenerateSymmetricalFigures(typeCounts);
        }

        private void EnsureTripleTiles(int[] typeCounts)
        {
            int totalTiles = MaxLayers * MaxLayers * 3;
            int repeatsPerType = Math.Max(3, totalTiles / TileTypes);

            for (int i = 0; i < TileTypes; i++)
            {
                typeCounts[i] = repeatsPerType;
            }

            int remainingTiles = totalTiles - TileTypes * repeatsPerType;
            var random = new Random();

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
                GeneratePyramid(typeCounts);
            }
            else if (selectedShape == Shape.Heart)
            {
                GenerateHeart(typeCounts);
            }
        }

        private void GeneratePyramid(int[] typeCounts)
        {
            var random = new Random();
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

        private void GenerateHeart(int[] typeCounts)
        {
            var random = new Random();
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

            var random = new Random();
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
            var random = new Random();
            Tiles = Tiles.OrderBy(_ => random.Next()).ToList();
        }

        //private bool IsSolvable()
        //{
        //    var simulatedTiles = Tiles.ToList();
        //    var collectionField = new CollectionField();

        //    while (simulatedTiles.Count > 0)
        //    {
        //        var availableTiles = simulatedTiles.Where(t => t.IsAvailable).ToList();

        //        if (!availableTiles.Any())
        //        {
        //            return false;
        //        }

        //        foreach (var tile in availableTiles.Take(7))
        //        {
        //            collectionField.AddTile(tile);
        //            simulatedTiles.Remove(tile);
        //        }

        //        var triplets = collectionField.CheckForTriplets();
        //        if (!triplets.Any())
        //        {
        //            return false;
        //        }

        //        UpdateTileAvailability(simulatedTiles);
        //    }

        //    return true;
        //}


        public void RemoveTile(Tile tile)
        {
            Tiles.Remove(tile);
            UpdateTileAvailability();
        }

        private void UpdateTileAvailability()
        {
            foreach (var tile in Tiles)
            {
                tile.IsAvailable = !Tiles.Any(t =>
                    t.Layer > tile.Layer &&
                    Math.Abs(t.X - tile.X) <= 1 &&
                    Math.Abs(t.Y - tile.Y) <= 1);
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
