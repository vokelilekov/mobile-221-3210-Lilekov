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

    public class GameField
    {
        public List<Tile> Tiles { get; private set; }
        private const int MaxLayers = 10;
        private const int TileTypes = 10;

        public GameField()
        {
            Tiles = new List<Tile>();
            GenerateTiles();
        }

        private void GenerateTiles()
        {
            Tiles.Clear();
            var random = new Random();

            int figureType = random.Next(0, 4);
            int[] typeCounts = new int[TileTypes];

            switch (figureType)
            {
                case 0: 
                    GeneratePyramid(typeCounts);
                    break;
                case 1:
                    GenerateStairs(typeCounts);
                    break;
                case 2:
                    GenerateChessPattern(typeCounts);
                    break;
                case 3:
                    GenerateDiamond(typeCounts);
                    break;
            }

            ShuffleTiles();
            UpdateTileAvailability();
        }
        private void GeneratePyramid(int[] typeCounts)
        {
            int layers = 5;
            int tileIndex = 0;

            for (int layer = 0; layer < layers; layer++)
            {
                for (int x = 0; x < layers - layer; x++)
                {
                    for (int y = 0; y < layers - layer; y++)
                    {
                        int type = GetNextTileType(typeCounts);
                        Tiles.Add(new Tile
                        {
                            Index = tileIndex++,
                            Type = type,
                            Layer = layer,
                            X = x + layer,
                            Y = y + layer
                        });
                    }
                }
            }
        }

        private void GenerateStairs(int[] typeCounts)
        {
            int steps = 6;
            int tileIndex = 0;

            for (int step = 0; step < steps; step++)
            {
                for (int x = 0; x <= step; x++)
                {
                    int type = GetNextTileType(typeCounts);
                    Tiles.Add(new Tile
                    {
                        Index = tileIndex++,
                        Type = type,
                        Layer = step,
                        X = x,
                        Y = step - x
                    });
                }
            }
        }

        private void GenerateChessPattern(int[] typeCounts)
        {
            int size = 8;
            int tileIndex = 0;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if ((x + y) % 2 == 0)
                    {
                        int type = GetNextTileType(typeCounts);
                        Tiles.Add(new Tile
                        {
                            Index = tileIndex++,
                            Type = type,
                            Layer = 0,
                            X = x,
                            Y = y
                        });
                    }
                }
            }
        }

        private void GenerateDiamond(int[] typeCounts)
        {
            int radius = 4;
            int tileIndex = 0;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (Math.Abs(x) + Math.Abs(y) <= radius)
                    {
                        int type = GetNextTileType(typeCounts);
                        Tiles.Add(new Tile
                        {
                            Index = tileIndex++,
                            Type = type,
                            Layer = 0,
                            X = x + radius,
                            Y = y + radius
                        });
                    }
                }
            }
        }

        private int GetNextTileType(int[] typeCounts)
        {
            int minCount = typeCounts.Min();
            int type = Array.IndexOf(typeCounts, minCount);
            typeCounts[type]++;
            return type;
        }

        private void ShuffleTiles()
        {
            var random = new Random();
            Tiles = Tiles.OrderBy(_ => random.Next()).ToList();
        }


        public void UpdateTileAvailability()
        {
            foreach (var tile in Tiles)
            {
                tile.IsAvailable = !Tiles.Any(t =>
                    t.Layer > tile.Layer &&
                    Math.Abs(t.X - tile.X) <= 1 &&
                    Math.Abs(t.Y - tile.Y) <= 1);
            }
        }

        public void RemoveTile(Tile tile)
        {
            Tiles.Remove(tile);
            UpdateTileAvailability();
        }

        public bool IsGameWon()
        {
            return !Tiles.Any();
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
