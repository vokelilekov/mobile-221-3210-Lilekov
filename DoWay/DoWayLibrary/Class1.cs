using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DoWayLibrary
{
    public class Map
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public MapElement[,] Elements { get; private set; }

        public Map(int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Размеры карты должны быть больше 0");

            Width = width;
            Height = height;
            Elements = new MapElement[width, height];
            Clear();
        }
        public void SetElement(int x, int y, MapElement element)
        {
            if (IsValidPosition(x, y))
                Elements[x, y] = element;
        }
        public void Clear()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    Elements[x, y] = new MapElement(5);
        }
        public void SaveToStream(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{Width},{Height}");
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        var element = Elements[x, y];
                        writer.Write(element?.ToDataString() ?? "-1");
                        if (x < Width - 1) writer.Write(",");
                    }
                    writer.WriteLine();
                }
            }
        }
        public static Map LoadFromStream(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var size = reader.ReadLine().Split(',');
                int width = int.Parse(size[0]);
                int height = int.Parse(size[1]);
                var map = new Map(width, height);

                for (int y = 0; y < height; y++)
                {
                    var line = reader.ReadLine().Split(',');
                    for (int x = 0; x < width; x++)
                    {
                        if (int.TryParse(line[x], out int spriteIndex) && spriteIndex >= 0)
                        {
                            map.Elements[x, y] = new MapElement(spriteIndex);
                        }
                    }
                }

                return map;
            }
        }
        public void GenerateTreesWithMask(int treeSpriteCount, int chancePerCell, Dictionary<int, ushort> spriteAvailability, Random random)
        {
            if (chancePerCell <= 0 || spriteAvailability == null) return;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var element = Elements[x, y];

                    if (element == null || !spriteAvailability.ContainsKey(element.SpriteIndex))
                        continue;

                    ushort availableMask = spriteAvailability[element.SpriteIndex];

                    for (int i = 0; i < 16; i++)
                    {
                        if ((availableMask & (1 << i)) != 0)
                            continue;

                        if (random.Next(100) >= chancePerCell)
                            continue;

                        int randomTreeIndex = random.Next(0, treeSpriteCount);
                        Elements[x, y] = new MapElement(randomTreeIndex);
                    }
                }
            }
        }

        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }
    }
    public class MapElement
    {
        public int SpriteIndex { get; set; }
        public List<AdditionalSprite> AdditionalSprites { get; set; } = new List<AdditionalSprite>();
        public MapElement()
        {
            SpriteIndex = 5; 
        }
        public MapElement(int spriteIndex)
        {
            SpriteIndex = spriteIndex;
        }
        public static MapElement CreateEmpty()
        {
            return new MapElement { SpriteIndex = 5 };
        }
        public string ToDataString()
        {
            return SpriteIndex.ToString();
        }
    }
    public class AdditionalSprite
    {
        public int SpriteIndex { get; set; }
        public Point LocalPosition { get; set; }
    }
    public class SpriteSelector
    {
        private readonly Bitmap _spriteSheet;
        private readonly int _spriteWidth;
        private readonly int _spriteHeight;

        public SpriteSelector(Bitmap spriteSheet, int spriteWidth, int spriteHeight)
        {
            _spriteSheet = spriteSheet;
            _spriteWidth = spriteWidth;
            _spriteHeight = spriteHeight;
        }

        public void Render(TableLayoutPanel panel, Action<int> onSpriteSelected)
        {
            panel.Controls.Clear();

            int totalSprites = (_spriteSheet.Width / _spriteWidth) * (_spriteSheet.Height / _spriteHeight);
            for (int i = 0; i < totalSprites; i++)
            {
                var pictureBox = new PictureBox
                {
                    Width = 80,
                    Height = 80,
                    Margin = new Padding(5),
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = i
                };

                pictureBox.Paint += (s, e) =>
                {
                    var index = (int)((PictureBox)s).Tag;
                    int col = index % (_spriteSheet.Width / _spriteWidth);
                    int row = index / (_spriteSheet.Width / _spriteWidth);
                    e.Graphics.DrawImage(_spriteSheet,
                        new Rectangle(0, 0, pictureBox.Width, pictureBox.Height),
                        new Rectangle(col * _spriteWidth, row * _spriteHeight, _spriteWidth, _spriteHeight),
                        GraphicsUnit.Pixel);
                };

                pictureBox.Click += (s, e) =>
                {
                    onSpriteSelected?.Invoke((int)((PictureBox)s).Tag);
                };

                panel.Controls.Add(pictureBox);
            }
        }

        public void DrawSprite(Graphics graphics, Rectangle rect, int spriteIndex)
        {
            int columns = _spriteSheet.Width / _spriteWidth;
            int col = spriteIndex % columns;
            int row = spriteIndex / columns;

            graphics.DrawImage(
                _spriteSheet,
                rect,
                new Rectangle(col * _spriteWidth, row * _spriteHeight, _spriteWidth, _spriteHeight),
                GraphicsUnit.Pixel);
        }

        public int SpriteCount => (_spriteSheet.Width / _spriteWidth) * (_spriteSheet.Height / _spriteHeight);
    }
    public class MapViewport
    {
        public PointF Offset { get; set; } = new PointF(0, 0);
        public float ZoomFactor { get; set; } = 1.0f;

        private PointF _dragStartPoint;
        private bool _isDragging;

        public bool IsDragging => _isDragging;

        public void StartDrag(Point startPoint)
        {
            _dragStartPoint = new PointF(startPoint.X - Offset.X, startPoint.Y - Offset.Y);
            _isDragging = true;
        }

        public void Drag(Point currentPoint)
        {
            Offset = new PointF(currentPoint.X - _dragStartPoint.X, currentPoint.Y - _dragStartPoint.Y);
        }

        public void EndDrag()
        {
            _isDragging = false;
        }

        public void UpdateZoom(float delta)
        {
            ZoomFactor += delta;
            if (ZoomFactor < 0.1f) ZoomFactor = 0.1f;
        }

        public Point TransformToMap(Point screenPoint)
        {
            return new Point(
                (int)((screenPoint.X - Offset.X) / ZoomFactor),
                (int)((screenPoint.Y - Offset.Y) / ZoomFactor)
            );
        }

        public Point TransformToScreen(Point mapPoint)
        {
            return new Point(
                (int)(mapPoint.X * ZoomFactor + Offset.X),
                (int)(mapPoint.Y * ZoomFactor + Offset.Y)
            );
        }
    }
}
