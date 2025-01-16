using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DoWayLibrary;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;

namespace DoWayWPF
{
    public partial class MainWindow : Window
    {
        private Map _map;
        private BitmapImage _spriteSheet;
        private const int _cellSize = 50;
        private const int _spriteWidth = 256;
        private const int _spriteHeight = 256;
        private const int EmptySpriteIndex = 5;
        private int _selectedSpriteIndex = -1;
        private Rect[] spriteRects;
        private Point _startPoint;
        private Point _rectangleEndPoint;
        private Random _random = new Random();
        private Dictionary<int, ushort> spriteAvailability = new Dictionary<int, ushort>()
        {
            { 0, 0b_0000_1111_1111_0000 },
            { 1, 0b_0000_1110_1110_0110 },
            { 2, 0b_0110_1111_1111_0110 },
            { 3, 0b_0110_0110_0110_0110 },
            { 4, 0b_0000_0111_0111_0110 },
            { 5, 0b_0000_0000_0000_0000 },
            { 6, 0b_0110_1110_1110_0000 },
            { 7, 0b_0110_0111_0111_0000 },
            { 8, 0b_0110_1110_1110_0110 },
            { 9, 0b_0000_1111_1111_0110 },
            { 10, 0b_0110_1111_1111_0000 },
            { 11, 0b_0110_0111_0111_0110 }
        };
        private BitmapImage _treeSpriteSheet;
        private Rect[] _treeSpriteRects;
        private bool _isDrawing = false;
        private bool _isDrawingRectangle = false;
        private List<Point> _temporaryLine = new List<Point>();
        private List<Point> _temporaryRectangle = new List<Point>();
        private bool _isMovingMap = false;
        private Point? _mapMoveStartPoint = null;
        private bool _isSingleClick = true;
        private bool? _isHorizontal = null;
        public MainWindow()
        {
            InitializeComponent();
            LoadSpriteSheet();
            InitializeDefaultMap();
            LoadTreeSpriteSheet();
            RenderSpriteSelector();
        }
        private void InitializeDefaultMap()
        {
            const int defaultWidth = 10;
            const int defaultHeight = 10;
            _map = new Map(defaultWidth, defaultHeight);

            MapCanvas.Width = defaultWidth * _cellSize;
            MapCanvas.Height = defaultHeight * _cellSize;

            _map.Clear();

            RenderMap();
        }
        private void LoadSpriteSheet()
        {
            _spriteSheet = new BitmapImage(new Uri("pack://application:,,,/Resources/preview_22.jpg"));
            int columns = _spriteSheet.PixelWidth / _spriteWidth;
            int rows = _spriteSheet.PixelHeight / _spriteHeight;

            spriteRects = new Rect[columns * rows];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    spriteRects[row * columns + col] = new Rect(
                        col * _spriteWidth,
                        row * _spriteHeight,
                        _spriteWidth,
                        _spriteHeight);
                }
            }
        }
        private void LoadTreeSpriteSheet()
        {
            _treeSpriteSheet = new BitmapImage(new Uri("pack://application:,,,/Resources/tree_4x4.png"));
            _treeSpriteRects = new Rect[16];
            const int treeSize = 300;

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    int index = row * 4 + col;
                    _treeSpriteRects[index] = new Rect(
                        col * treeSize,
                        row * treeSize,
                        treeSize,
                        treeSize
                    );
                }
            }
        }
        private ImageBrush[] GenerateSpriteBrushes()
        {
            int spriteSize = 256;
            int columns = _spriteSheet.PixelWidth / spriteSize;
            int rows = _spriteSheet.PixelHeight / spriteSize;

            var spriteBrushes = new ImageBrush[columns * rows];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    int index = row * columns + col;
                    var croppedBitmap = new CroppedBitmap(
                        _spriteSheet,
                        new Int32Rect(col * spriteSize, row * spriteSize, spriteSize, spriteSize)
                    );
                    spriteBrushes[index] = new ImageBrush(croppedBitmap);
                }
            }
            return spriteBrushes;
        }
        private void RenderSpriteSelector()
        {
            SpriteSelectorGrid.Children.Clear();
            if (_spriteSheet == null) return;

            var spriteBrushes = GenerateSpriteBrushes();

            for (int i = 0; i < spriteBrushes.Length; i++)
            {
                Border border = new Border
                {
                    Width = 64,
                    Height = 64,
                    Margin = new Thickness(5),
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                    Background = spriteBrushes[i]
                };

                int spriteIndex = i;
                border.MouseLeftButtonDown += (s, e) =>
                {
                    _selectedSpriteIndex = spriteIndex;
                    HighlightSelectedSprite(spriteIndex);
                };

                SpriteSelectorGrid.Children.Add(border);
            }
        }
        private void HighlightSelectedSprite(int selectedIndex)
        {
            foreach (var child in SpriteSelectorGrid.Children)
            {
                if (child is Border border)
                {
                    border.BorderBrush = Brushes.Black;
                }
            }

            if (selectedIndex >= 0 && selectedIndex < SpriteSelectorGrid.Children.Count)
            {
                var selectedBorder = SpriteSelectorGrid.Children[selectedIndex] as Border;
                selectedBorder.BorderBrush = Brushes.Red;
                if (SelectedSpritePreview != null)
                {
                    SelectedSpritePreview.Background = selectedBorder.Background;
                }
            }
        }
        private void RenderMap()
        {
            MapCanvas.Children.Clear();
            if (_map == null) return;

            var spriteBrushes = GenerateSpriteBrushes();

            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    if (_map.Elements[x, y] == null)
                    {
                        _map.Elements[x, y] = new MapElement(5);
                    }

                    var element = _map.Elements[x, y];
                    if (element.SpriteIndex < 0 || element.SpriteIndex >= spriteBrushes.Length)
                        continue;

                    Rectangle rect = new Rectangle
                    {
                        Width = _cellSize,
                        Height = _cellSize,
                        Fill = spriteBrushes[element.SpriteIndex]
                    };

                    Border border = new Border
                    {
                        Width = _cellSize,
                        Height = _cellSize,
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0.5),
                        Child = rect
                    };

                    Canvas.SetLeft(border, x * _cellSize);
                    Canvas.SetTop(border, y * _cellSize);

                    MapCanvas.Children.Add(border);
                }
            }
        }
        private void MapCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                _isDrawingRectangle = true;
                _temporaryRectangle.Clear();

                Point clickPosition = e.GetPosition(MapCanvas);
                int startX = (int)(clickPosition.X / _cellSize);
                int startY = (int)(clickPosition.Y / _cellSize);

                _startPoint = new Point(startX, startY);
                _rectangleEndPoint = _startPoint;
            }
            else
            {
                _isDrawing = true;
                _isSingleClick = true;
                _isHorizontal = null;
                _temporaryLine.Clear();

                Point clickPosition = e.GetPosition(MapCanvas);
                int x = (int)(clickPosition.X / _cellSize);
                int y = (int)(clickPosition.Y / _cellSize);

                _startPoint = new Point(x, y);
                _temporaryLine.Add(_startPoint);
            }
        }
        private void MapCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(MapCanvas);
                int x = (int)(currentPosition.X / _cellSize);
                int y = (int)(currentPosition.Y / _cellSize);

                if (_temporaryLine.Count > 0 &&
                    _temporaryLine[_temporaryLine.Count - 1].X == x &&
                    _temporaryLine[_temporaryLine.Count - 1].Y == y)
                    return;

                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    _temporaryLine.Clear();

                    int startX = (int)_startPoint.X;
                    int startY = (int)_startPoint.Y;

                    for (int i = Math.Min(startX, x); i <= Math.Max(startX, x); i++)
                    {
                        _temporaryLine.Add(new Point(i, startY));
                        _temporaryLine.Add(new Point(i, y)); 
                    }
                    for (int i = Math.Min(startY, y); i <= Math.Max(startY, y); i++)
                    {
                        _temporaryLine.Add(new Point(startX, i));
                        _temporaryLine.Add(new Point(x, i)); 
                    }
                }
                else
                {
                    if (_isHorizontal == null)
                    {
                        if (Math.Abs(x - _startPoint.X) >= Math.Abs(y - _startPoint.Y))
                            _isHorizontal = true;
                        else
                            _isHorizontal = false;
                    }

                    if (_isHorizontal == true && y == (int)_startPoint.Y)
                    {
                        _temporaryLine.Add(new Point(x, y));
                    }
                    else if (_isHorizontal == false && x == (int)_startPoint.X)
                    {
                        _temporaryLine.Add(new Point(x, y));
                    }
                }

                RenderTemporaryFigure();
            }
        }
        private void RenderTemporaryFigure()
        {
            MapCanvas.Children.Clear();
            RenderMap();

            foreach (var point in _temporaryLine)
            {
                Rectangle tempRect = new Rectangle
                {
                    Width = _cellSize,
                    Height = _cellSize,
                    Fill = new SolidColorBrush(Color.FromArgb(128, 0, 0, 255)), 
                    Stroke = Brushes.Blue,
                    StrokeThickness = 1
                };

                Canvas.SetLeft(tempRect, point.X * _cellSize);
                Canvas.SetTop(tempRect, point.Y * _cellSize);
                MapCanvas.Children.Add(tempRect);
            }
        }
        private void MapCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDrawing)
            {
                if (_isDrawing)
                {
                    _isDrawing = false;

                    if (_temporaryLine.Count > 0)
                    {
                        foreach (var point in _temporaryLine)
                        {
                            int x = (int)point.X;
                            int y = (int)point.Y;

                            if (_map.IsValidPosition(x, y))
                            {
                                int spriteIndex = GetRoadSpriteIndex(x, y);
                                _map.SetElement(x, y, new MapElement(spriteIndex));
                            }
                        }
                    }

                    _temporaryLine.Clear();
                    RenderMap();
                    return;
                }

                if (_isSingleClick && _temporaryLine.Count > 0)
                {
                    Point point = _temporaryLine[0];
                    int x = (int)point.X;
                    int y = (int)point.Y;

                    if (x >= 0 && y >= 0 && x < _map.Width && y < _map.Height)
                    {
                        _map.SetElement(x, y, new MapElement(_selectedSpriteIndex));
                        RenderMap();
                    }
                }
                else if (_temporaryLine.Count > 0)
                {
                    foreach (var point in _temporaryLine)
                    {
                        int x = (int)point.X;
                        int y = (int)point.Y;

                        int spriteIndex = GetRoadSpriteIndex(x, y);
                        _map.SetElement(x, y, new MapElement(spriteIndex));
                    }

                    _temporaryLine.Clear();
                    RenderMap();
                }
            }
        }
        private void MapCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMovingMap = true;
            _mapMoveStartPoint = e.GetPosition(MapGrid);
        }
        private void MapCanvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMovingMap = false;
            _mapMoveStartPoint = null;
        }
        private void MapCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_map == null) return;
            Point mousePosition = e.GetPosition(MapCanvas);
            double zoomFactor = e.Delta > 0 ? 1.1 : 0.9;
            var transformGroup = MapCanvas.LayoutTransform as TransformGroup ?? new TransformGroup();
            var scaleTransform = transformGroup.Children.OfType<ScaleTransform>().FirstOrDefault() ?? new ScaleTransform(1, 1);
            var translateTransform = transformGroup.Children.OfType<TranslateTransform>().FirstOrDefault() ?? new TranslateTransform();
            if (!transformGroup.Children.Contains(scaleTransform)) transformGroup.Children.Add(scaleTransform);
            if (!transformGroup.Children.Contains(translateTransform)) transformGroup.Children.Add(translateTransform);

            double oldScaleX = scaleTransform.ScaleX;
            double oldScaleY = scaleTransform.ScaleY;
            scaleTransform.ScaleX = Math.Max(0.5, Math.Min(5, oldScaleX * zoomFactor));
            scaleTransform.ScaleY = Math.Max(0.5, Math.Min(5, oldScaleY * zoomFactor));

            Point relativeMousePosition = new Point(
                (mousePosition.X - translateTransform.X) / oldScaleX,
                (mousePosition.Y - translateTransform.Y) / oldScaleY
            );

            translateTransform.X -= (relativeMousePosition.X * scaleTransform.ScaleX - relativeMousePosition.X * oldScaleX);
            translateTransform.Y -= (relativeMousePosition.Y * scaleTransform.ScaleY - relativeMousePosition.Y * oldScaleY);

            ApplyOffsetConstraints(MapCanvas, scaleTransform, translateTransform);

            MapCanvas.LayoutTransform = transformGroup;
        }
        private void ApplyOffsetConstraints(Canvas canvas, ScaleTransform scaleTransform, TranslateTransform translateTransform)
        {
            double gridWidth = canvas.ActualWidth * scaleTransform.ScaleX;
            double gridHeight = canvas.ActualHeight * scaleTransform.ScaleY;

            double maxOffsetX = Math.Max(0, (canvas.ActualWidth - gridWidth) / 2);
            double maxOffsetY = Math.Max(0, (canvas.ActualHeight - gridHeight) / 2);

            translateTransform.X = Math.Max(-gridWidth + maxOffsetX, Math.Min(maxOffsetX, translateTransform.X));
            translateTransform.Y = Math.Max(-gridHeight + maxOffsetY, Math.Min(maxOffsetY, translateTransform.Y));
        }
        private void ExportToPng_Click(object sender, RoutedEventArgs e)
        {
            if (_map == null)
            {
                MessageBox.Show("Карта не создана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Files (*.png)|*.png",
                DefaultExt = "png"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportMapToPng(saveFileDialog.FileName);
            }
        }
        private void ExportMapToPng(string filePath)
        {
            if (_map == null)
            {
                MessageBox.Show("Карта не создана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                int mapWidth = _map.Width * _cellSize;
                int mapHeight = _map.Height * _cellSize;
                double dpi = 300;
                double scaleFactor = dpi / 96.0;

                var drawingVisual = new DrawingVisual();
                using (var context = drawingVisual.RenderOpen())
                {
                    context.DrawRectangle(Brushes.DarkGray, null, new Rect(0, 0, mapWidth, mapHeight));

                    for (int x = 0; x < _map.Width; x++)
                    {
                        for (int y = 0; y < _map.Height; y++)
                        {
                            var element = _map.Elements[x, y];

                            if (element == null)
                            {
                                element = new MapElement(5);
                                _map.Elements[x, y] = element;
                            }

                            if (element.SpriteIndex < 0 || element.SpriteIndex >= spriteRects.Length)
                                throw new Exception($"Некорректный SpriteIndex: {element.SpriteIndex} для элемента на позиции ({x}, {y}).");

                            var croppedBitmap = new CroppedBitmap(
                                _spriteSheet,
                                new Int32Rect(
                                    (int)spriteRects[element.SpriteIndex].X,
                                    (int)spriteRects[element.SpriteIndex].Y,
                                    (int)spriteRects[element.SpriteIndex].Width,
                                    (int)spriteRects[element.SpriteIndex].Height));

                            var brush = new ImageBrush(croppedBitmap) { Stretch = Stretch.Fill };
                            var rect = new Rect(x * _cellSize, y * _cellSize, _cellSize, _cellSize);
                            context.DrawRectangle(brush, null, rect);
                            context.DrawRectangle(null, new Pen(Brushes.Black, 1), rect);
                        }
                    }
                }

                var renderTarget = new RenderTargetBitmap(
                    (int)(mapWidth * scaleFactor),
                    (int)(mapHeight * scaleFactor),
                    dpi,
                    dpi,
                    PixelFormats.Pbgra32);

                renderTarget.Render(drawingVisual);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    var pngEncoder = new PngBitmapEncoder();
                    pngEncoder.Frames.Add(BitmapFrame.Create(renderTarget));
                    pngEncoder.Save(fileStream);
                }

                MessageBox.Show("Карта успешно сохранена в PNG с высоким качеством.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении PNG: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddTrees_Click(object sender, RoutedEventArgs e)
        {
            if (_map == null)
            {
                MessageBox.Show("Карта не создана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            const int cellTreeSize = 12;

            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    var element = _map.Elements[x, y];

                    if (element == null || !spriteAvailability.TryGetValue(element.SpriteIndex, out ushort availableMask))
                        continue;

                    GenerateTreesInCell(x, y, cellTreeSize, availableMask);
                }
            }
        }
        private void GenerateTreesInCell(int x, int y, int cellTreeSize, ushort availableMask)
        {
            for (int i = 0; i < 16; i++)
            {
                if ((availableMask & (1 << i)) != 0 || _random.Next(5) != 0)
                    continue;

                int localX = i % 4;
                int localY = i / 4;

                double treeX = x * _cellSize + localX * cellTreeSize;
                double treeY = y * _cellSize + localY * cellTreeSize;

                if (treeX + cellTreeSize > _map.Width * _cellSize || treeY + cellTreeSize > _map.Height * _cellSize)
                    continue;

                int treeSpriteIndex = _random.Next(0, _treeSpriteRects.Length);

                AddTreeToCanvas(treeX, treeY, cellTreeSize, treeSpriteIndex);
            }
        }
        private void AddTreeToCanvas(double treeX, double treeY, int treeSize, int treeSpriteIndex)
        {
            var treeRect = new Rectangle
            {
                Width = treeSize,
                Height = treeSize,
                Fill = new ImageBrush
                {
                    ImageSource = _treeSpriteSheet,
                    Viewbox = _treeSpriteRects[treeSpriteIndex],
                    ViewboxUnits = BrushMappingMode.Absolute,
                    Stretch = Stretch.UniformToFill
                }
            };

            Canvas.SetLeft(treeRect, treeX);
            Canvas.SetTop(treeRect, treeY);

            MapCanvas.Children.Add(treeRect);
        }
        private void SetMapSize_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MapWidthInput.Text, out int width) && int.TryParse(MapHeightInput.Text, out int height) && width > 0 && height > 0)
            {
                _map = new Map(width, height);
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        _map.SetElement(x, y, new MapElement(EmptySpriteIndex));
                    }
                }

                MapCanvas.Width = width * _cellSize;
                MapCanvas.Height = height * _cellSize;
                RenderMap();
            }
        }
        private void SaveMap_Click(object sender, RoutedEventArgs e)
        {
            if (_map == null)
            {
                MessageBox.Show("Карта не создана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Map Files (*.map)|*.map",
                DefaultExt = "map",
                Title = "Сохранить карту"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        _map.SaveToStream(stream);
                    }
                    MessageBox.Show("Карта успешно сохранена.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении карты: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void LoadMap_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Map Files (*.map)|*.map",
                DefaultExt = "map",
                Title = "Загрузить карту"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var stream = new FileStream(openFileDialog.FileName, FileMode.Open))
                    {
                        _map = Map.LoadFromStream(stream);
                    }

                    MapCanvas.Width = _map.Width * _cellSize;
                    MapCanvas.Height = _map.Height * _cellSize;
                    RenderMap();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке карты: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ClearMap_Click(object sender, RoutedEventArgs e)
        {
            if (_map == null)
            {
                MessageBox.Show("Карта не создана.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    if (_map.IsValidPosition(x, y))
                    {
                        _map.SetElement(x, y, new MapElement(EmptySpriteIndex));
                    }
                }
            }
            RenderMap(); 
        }
        private int GetRoadSpriteIndex(int x, int y)
        {
            if (_map == null || !_map.IsValidPosition(x, y)) return EmptySpriteIndex;

            bool hasTop = _map.IsValidPosition(x, y - 1) && _map.Elements[x, y - 1]?.SpriteIndex != EmptySpriteIndex;
            bool hasBottom = _map.IsValidPosition(x, y + 1) && _map.Elements[x, y + 1]?.SpriteIndex != EmptySpriteIndex;
            bool hasLeft = _map.IsValidPosition(x - 1, y) && _map.Elements[x - 1, y]?.SpriteIndex != EmptySpriteIndex;
            bool hasRight = _map.IsValidPosition(x + 1, y) && _map.Elements[x + 1, y]?.SpriteIndex != EmptySpriteIndex;

            if (hasTop && hasBottom && hasLeft && hasRight) return 2; // Перекресток
            if (hasTop && hasBottom && hasLeft) return 11; // T-образный перекресток (влево)
            if (hasTop && hasBottom && hasRight) return 8; // T-образный перекресток (вправо)
            if (hasLeft && hasRight && hasTop) return 9; // T-образный перекресток (вверх)
            if (hasLeft && hasRight && hasBottom) return 10; // T-образный перекресток (вниз)
            if (hasLeft && hasRight) return 0; // Горизонтальная дорога
            if (hasTop && hasBottom) return 3; // Вертикальная дорога
            if (hasTop && hasRight) return 1; // Угол вверх-вправо
            if (hasTop && hasLeft) return 4; // Угол вверх-влево
            if (hasBottom && hasRight) return 6; // Угол вниз-вправо
            if (hasBottom && hasLeft) return 7; // Угол вниз-влево

            return EmptySpriteIndex;
        }
    }
}