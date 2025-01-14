using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DoWayLibrary;

namespace DoWayWinForms
{
    public partial class Form1 : Form
    {
        private Map _map;
        private const int CellSize = 50;
        private const int EmptySpriteIndex = 5;
        private Bitmap _spriteSheet;
        private int _selectedSpriteIndex = 5;
        private readonly MapViewport _viewport = new MapViewport();
        private SpriteSelector _spriteSelector;
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
        private Bitmap _treeSpriteSheet;
        private Rectangle[] _treeSpriteRects;
        private bool _isDrawingLine = false;
        private List<Point> _temporaryLine = new List<Point>();
        private List<Point> _temporaryRectangle = new List<Point>();
        private Point _startPoint;
        private bool? _isHorizontal = null;
        private bool _isLineStart = false;
        private Timer _mouseDownTimer;
        private const int MouseHoldThreshold = 300;
        private PictureBox selectedSpritePreview;
        private bool _isDrawingRectangle = false;
        private Point _rectangleStartPoint;
        private bool _isCtrlPressed = false;
        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            panelMap.DoubleBuffered(true);
            LoadSpriteSheet();
            LoadTreeSpriteSheet();
            InitializeMap(10, 10);
            CenterMapOnPanel();
            ApplyDarkTheme(this);
            selectedSpritePreview = pictureBoxSelectedSprite;

            _mouseDownTimer = new Timer();
            _mouseDownTimer.Interval = MouseHoldThreshold;
            _mouseDownTimer.Tick += MouseDownTimer_Tick;
            panelMap.Resize += PanelMap_Resize;
        }
        private void PanelMap_Resize(object sender, EventArgs e)
        {
            CenterMapOnPanel();
        }
        private void ApplyDarkTheme(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is Panel)
                {
                    control.BackColor = Color.FromArgb(45, 45, 48);
                }
                else if (control is Button button)
                {
                    button.BackColor = Color.FromArgb(63, 63, 70);
                    button.ForeColor = Color.White;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = Color.FromArgb(45, 45, 48);
                }
                else if (control is Label label)
                {
                    label.ForeColor = Color.White;
                    label.BackColor = Color.FromArgb(45, 45, 48);
                }
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = Color.FromArgb(30, 30, 30);
                    textBox.ForeColor = Color.White;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (control is TableLayoutPanel || control is FlowLayoutPanel || control is Panel)
                {
                    control.BackColor = Color.FromArgb(45, 45, 48);
                }
                else if (control is ListBox listBox)
                {
                    listBox.BackColor = Color.FromArgb(30, 30, 30);
                    listBox.ForeColor = Color.White;
                }
                else if (control is MenuStrip menuStrip)
                {
                    menuStrip.BackColor = Color.FromArgb(45, 45, 48);
                    menuStrip.ForeColor = Color.White;
                    foreach (ToolStripMenuItem menuItem in menuStrip.Items)
                    {
                        ApplyDarkThemeToMenuItems(menuItem);
                    }
                }
                ApplyDarkTheme(control);
            }

            if (parent is Form form)
            {
                form.BackColor = Color.FromArgb(30, 30, 30);
            }
        }
        private void ApplyDarkThemeToMenuItems(ToolStripMenuItem menuItem)
        {
            menuItem.BackColor = Color.FromArgb(45, 45, 48);
            menuItem.ForeColor = Color.White;

            foreach (ToolStripItem subItem in menuItem.DropDownItems)
            {
                if (subItem is ToolStripMenuItem subMenuItem)
                {
                    ApplyDarkThemeToMenuItems(subMenuItem);
                }
            }
        }
        private void InitializeUI()
        {
            panelMap.Dock = DockStyle.Fill;
            panelMap.BackColor = Color.DarkGray;
            panelMap.Paint += PanelMap_Paint;
            panelMap.MouseClick += PanelMap_MouseClick;
            panelMap.MouseDown += PanelMap_MouseDown;
            panelMap.MouseMove += PanelMap_MouseMove;
            panelMap.MouseUp += PanelMap_MouseUp;
            panelMap.MouseWheel += PanelMap_MouseWheel;

            panelSprites.Dock = DockStyle.Right;
            panelSprites.Width = 300;
            panelSprites.AutoScroll = true;
        }
        private void LoadSpriteSheet()
        {
            _spriteSheet = new Bitmap("Resources/preview_22.jpg");
            _spriteSelector = new SpriteSelector(_spriteSheet, 256, 256);
            RenderSpriteSelector();
        }
        private void LoadTreeSpriteSheet()
        {
            _treeSpriteSheet = new Bitmap("Resources/tree_4x4.png");
            const int treeSize = 300;

            _treeSpriteRects = new Rectangle[16];
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    int index = row * 4 + col;
                    _treeSpriteRects[index] = new Rectangle(
                        col * treeSize,
                        row * treeSize,
                        treeSize,
                        treeSize
                    );
                }
            }
        }
        private void InitializeMap(int width, int height)
        {
            _map = new Map(width, height);
            FillMapWithEmpty();
            panelMap.Invalidate();
        }
        private void CenterMapOnPanel()
        {
            if (_map == null || panelMap == null) return;

            int mapWidthInPixels = _map.Width * CellSize;
            int mapHeightInPixels = _map.Height * CellSize;

            int panelWidth = panelMap.Width;
            int panelHeight = panelMap.Height;

            float offsetX = (panelWidth - mapWidthInPixels) / 2f;
            float offsetY = (panelHeight - mapHeightInPixels) / 2f;

            _viewport.Offset = new PointF(offsetX, offsetY);

            panelMap.Invalidate();
        }
        private void RenderSpriteSelector()
        {
            var tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 3,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            for (int i = 0; i < _spriteSelector.SpriteCount; i++)
            {
                if (i == EmptySpriteIndex) continue;

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
                    int col = index % (_spriteSheet.Width / 256);
                    int row = index / (_spriteSheet.Width / 256);
                    e.Graphics.DrawImage(
                        _spriteSheet,
                        new Rectangle(0, 0, pictureBox.Width, pictureBox.Height),
                        new Rectangle(col * 256, row * 256, 256, 256),
                        GraphicsUnit.Pixel
                    );
                };

                pictureBox.Click += (s, e) =>
                {
                    _selectedSpriteIndex = (int)((PictureBox)s).Tag;
                    HighlightSelectedSprite(_selectedSpriteIndex);
                };

                tableLayoutPanel.Controls.Add(pictureBox);
            }

            panelSprites.Controls.Clear();
            panelSprites.Controls.Add(tableLayoutPanel);
        }
        private void HighlightSelectedSprite(int index)
        {
            foreach (Control control in panelSprites.Controls)
            {
                if (control is TableLayoutPanel layoutPanel)
                {
                    foreach (Control child in layoutPanel.Controls)
                    {
                        if (child is PictureBox pictureBox)
                        {
                            pictureBox.BorderStyle = (int)pictureBox.Tag == index
                                ? BorderStyle.Fixed3D
                                : BorderStyle.FixedSingle;

                            pictureBox.BackColor = (int)pictureBox.Tag == index
                                ? Color.Red
                                : Color.Transparent;
                        }
                    }
                }
            }

            foreach (Control control in panelSprites.Controls)
            {
                if (control is Button button && button.Text == "Ластик")
                {
                    button.BackColor = index == -1 ? Color.Red : Color.FromArgb(63, 63, 70);
                }
            }
            UpdateSelectedSpritePreview(index);
        }
        private void HighlightRectangle(Point start, Point end)
        {
            _temporaryRectangle.Clear();

            int minX = Math.Min(start.X, end.X);
            int maxX = Math.Max(start.X, end.X);
            int minY = Math.Min(start.Y, end.Y);
            int maxY = Math.Max(start.Y, end.Y);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    _temporaryRectangle.Add(new Point(x, y));
                }
            }
        }
        private void PanelMap_Paint(object sender, PaintEventArgs e)
        {
            if (_map == null) return;

            e.Graphics.TranslateTransform(_viewport.Offset.X, _viewport.Offset.Y);
            e.Graphics.ScaleTransform(_viewport.ZoomFactor, _viewport.ZoomFactor);

            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    var element = _map.Elements[x, y] ?? MapElement.CreateEmpty();
                    var rect = new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize);

                    if (element.SpriteIndex >= 0 && element.SpriteIndex < _spriteSelector.SpriteCount)
                    {
                        _spriteSelector.DrawSprite(e.Graphics, rect, element.SpriteIndex);
                    }

                    foreach (var sprite in element.AdditionalSprites)
                    {
                        var localRect = new Rectangle(
                            rect.X + sprite.LocalPosition.X * (CellSize / 4),
                            rect.Y + sprite.LocalPosition.Y * (CellSize / 4),
                            CellSize / 4,
                            CellSize / 4
                        );

                        var sourceRect = _treeSpriteRects[sprite.SpriteIndex];
                        e.Graphics.DrawImage(_treeSpriteSheet, localRect, sourceRect, GraphicsUnit.Pixel);
                    }

                    e.Graphics.DrawRectangle(Pens.Black, rect);
                }
            }

            if (_isDrawingLine && _temporaryLine.Count > 0)
            {
                foreach (var point in _temporaryLine)
                {
                    var rect = new Rectangle(point.X * CellSize, point.Y * CellSize, CellSize, CellSize);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.LightBlue)), rect);
                    e.Graphics.DrawRectangle(Pens.Blue, rect);
                }
            }

            if (_isDrawingRectangle && _temporaryRectangle.Count > 0)
            {
                foreach (var point in _temporaryRectangle)
                {
                    var rect = new Rectangle(point.X * CellSize, point.Y * CellSize, CellSize, CellSize);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, Color.LightGreen)), rect);
                    e.Graphics.DrawRectangle(Pens.Green, rect);
                }
            }
        }
        private void PanelMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            if (_isDrawingLine) return;

            if (_map == null || _selectedSpriteIndex < 0) return;

            var mapPoint = _viewport.TransformToMap(new Point(e.X, e.Y));
            int mapX = mapPoint.X / CellSize;
            int mapY = mapPoint.Y / CellSize;

            if (_map.IsValidPosition(mapX, mapY))
            {
                _map.SetElement(mapX, mapY, new MapElement(_selectedSpriteIndex));
                panelMap.Invalidate();
            }
        }
        private void PanelMap_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_map == null) return;

            PointF mousePositionInPanel = e.Location;
            PointF mousePositionOnMap = new PointF(
                (mousePositionInPanel.X - _viewport.Offset.X) / _viewport.ZoomFactor,
                (mousePositionInPanel.Y - _viewport.Offset.Y) / _viewport.ZoomFactor
            );
            float zoomChange = e.Delta > 0 ? 1.1f : 0.9f;
            float newZoomFactor = _viewport.ZoomFactor * zoomChange;

            newZoomFactor = Math.Max(0.5f, Math.Min(5f, newZoomFactor));

            _viewport.Offset = new PointF(
                mousePositionInPanel.X - mousePositionOnMap.X * newZoomFactor,
                mousePositionInPanel.Y - mousePositionOnMap.Y * newZoomFactor
            );

            _viewport.ZoomFactor = newZoomFactor;

            panelMap.Invalidate();
        }
        private void PanelMap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    _isDrawingRectangle = true;
                    var mapPoint = _viewport.TransformToMap(e.Location);
                    _rectangleStartPoint = new Point(mapPoint.X / CellSize, mapPoint.Y / CellSize);
                }
                else
                {
                    _mouseDownTimer.Start();
                    _isDrawingLine = true;
                    _isLineStart = true;
                    _temporaryLine.Clear();
                    _isHorizontal = null;

                    var mapPoint = _viewport.TransformToMap(e.Location);
                    _startPoint = new Point(mapPoint.X / CellSize, mapPoint.Y / CellSize);

                    if (_map.IsValidPosition(_startPoint.X, _startPoint.Y))
                    {
                        _temporaryLine.Add(_startPoint);
                    }

                    panelMap.Invalidate();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                _viewport.StartDrag(e.Location);
            }
        }
        private void PanelMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawingRectangle && e.Button == MouseButtons.Left)
            {
                var mapPoint = _viewport.TransformToMap(e.Location);
                var rectangleEndPoint = new Point(mapPoint.X / CellSize, mapPoint.Y / CellSize);
                HighlightRectangle(_rectangleStartPoint, rectangleEndPoint);
                panelMap.Invalidate();
            }
            else if (_isDrawingLine && e.Button == MouseButtons.Left)
            {
                var mapPoint = _viewport.TransformToMap(e.Location);
                var currentPoint = new Point(mapPoint.X / CellSize, mapPoint.Y / CellSize);

                if (_temporaryLine.Count > 0 && _temporaryLine[_temporaryLine.Count - 1] == currentPoint)
                    return;

                if (_isHorizontal == null)
                {
                    if (Math.Abs(currentPoint.X - _startPoint.X) > Math.Abs(currentPoint.Y - _startPoint.Y))
                        _isHorizontal = true;
                    else
                        _isHorizontal = false;
                }

                if ((_isHorizontal == true && currentPoint.Y == _startPoint.Y) ||
                    (_isHorizontal == false && currentPoint.X == _startPoint.X))
                {
                    if (!_temporaryLine.Contains(currentPoint))
                    {
                        _temporaryLine.Add(currentPoint);
                    }
                    else
                    {
                        int index = _temporaryLine.IndexOf(currentPoint);
                        _temporaryLine.RemoveRange(index + 1, _temporaryLine.Count - index - 1);
                    }
                }

                panelMap.Invalidate();
            }
            else if (_viewport.IsDragging && e.Button == MouseButtons.Right)
            {
                _viewport.Drag(e.Location);
                panelMap.Invalidate();
            }
        }
        private void MouseDownTimer_Tick(object sender, EventArgs e)
        {
            _mouseDownTimer.Stop();

            if (_isLineStart)
            {
                _isDrawingLine = true;
                _isLineStart = false;
                panelMap.Invalidate();
            }
        }
        private void PanelMap_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_isDrawingRectangle)
                {
                    var mapPoint = _viewport.TransformToMap(e.Location);
                    var rectangleEndPoint = new Point(mapPoint.X / CellSize, mapPoint.Y / CellSize);
                    DrawRectangle(_rectangleStartPoint, rectangleEndPoint);
                    _isDrawingRectangle = false;
                    panelMap.Invalidate();
                }

                if (_isDrawingLine)
                {
                    if (_temporaryLine.Count > 1)
                    {
                        if (_temporaryLine[0] == _temporaryLine[_temporaryLine.Count - 1])
                        {
                            foreach (var point in _temporaryLine)
                            {
                                if (_map.IsValidPosition(point.X, point.Y))
                                {
                                    _map.SetElement(point.X, point.Y, new MapElement(-1));
                                }
                            }
                        }
                        else
                        {
                            foreach (var point in _temporaryLine)
                            {
                                if (_map.IsValidPosition(point.X, point.Y))
                                {
                                    int spriteIndex = _isHorizontal == true ? 0 : 3;
                                    _map.SetElement(point.X, point.Y, new MapElement(spriteIndex));
                                    UpdateRoadConnections(point.X, point.Y);
                                }
                            }
                        }
                    }
                    else if (_temporaryLine.Count == 1)
                    {
                        var singlePoint = _temporaryLine[0];
                        if (_map.IsValidPosition(singlePoint.X, singlePoint.Y))
                        {
                            _map.SetElement(singlePoint.X, singlePoint.Y, new MapElement(_selectedSpriteIndex));
                        }
                    }
                }
                _temporaryLine.Clear();
                _isHorizontal = null;
                _isDrawingLine = false;

                panelMap.Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {
                _viewport.EndDrag();
            }
        }
        private void ExportMapToPng(string filePath)
        {
            if (_map == null)
            {
                MessageBox.Show("Карта не создана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int mapWidth = _map.Width * 256;
                int mapHeight = _map.Height * 256;

                using (var bitmap = new Bitmap(mapWidth, mapHeight))
                {
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

                        g.Clear(Color.DarkGray);

                        for (int x = 0; x < _map.Width; x++)
                        {
                            for (int y = 0; y < _map.Height; y++)
                            {
                                var element = _map.Elements[x, y] ?? MapElement.CreateEmpty();
                                var rect = new Rectangle(x * 256, y * 256, 256, 256);

                                if (element.SpriteIndex >= 0 && element.SpriteIndex < _spriteSelector.SpriteCount)
                                {
                                    _spriteSelector.DrawSprite(g, rect, element.SpriteIndex);
                                }

                                foreach (var sprite in element.AdditionalSprites)
                                {
                                    var spriteRect = new Rectangle(
                                        rect.X + sprite.LocalPosition.X * (256 / 4),
                                        rect.Y + sprite.LocalPosition.Y * (256 / 4),
                                        256 / 4,
                                        256 / 4
                                    );

                                    var sourceRect = _treeSpriteRects[sprite.SpriteIndex];
                                    g.DrawImage(_treeSpriteSheet, spriteRect, sourceRect, GraphicsUnit.Pixel);
                                }
                            }
                        }
                    }

                    bitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    MessageBox.Show("Карта успешно сохранена в PNG.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении PNG: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSetMapSize_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtWidth.Text, out int width) && int.TryParse(txtHeight.Text, out int height) && width > 0 && height > 0)
            {
                InitializeMap(width, height);
            }
            else
            {
                MessageBox.Show("Введите корректные размеры карты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void SaveFile(string filter, Action<Stream> saveAction)
        {
            using (var dialog = new SaveFileDialog { Filter = filter })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = new FileStream(dialog.FileName, FileMode.Create))
                    {
                        saveAction(stream);
                    }
                }
            }
        }
        private void OpenFile(string filter, Action<Stream> loadAction)
        {
            using (var dialog = new OpenFileDialog { Filter = filter })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (var stream = new FileStream(dialog.FileName, FileMode.Open))
                    {
                        loadAction(stream);
                    }
                }
            }
        }
        private void FillMapWithEmpty()
        {
            for (int x = 0; x < _map.Width; x++)
                for (int y = 0; y < _map.Height; y++)
                    _map.SetElement(x, y, new MapElement(EmptySpriteIndex));
        }
        private void GenerateTrees()
        {
            if (_map == null)
            {
                MessageBox.Show("Карта не создана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    for (int i = 0; i < 16; i++)
                    {
                        if ((availableMask & (1 << i)) != 0 || _random.Next(5) != 0)
                            continue;

                        int localX = i % 4;
                        int localY = i / 4;

                        int treeSpriteIndex = _random.Next(0, _treeSpriteRects.Length);

                        element.AdditionalSprites.Add(new AdditionalSprite
                        {
                            SpriteIndex = treeSpriteIndex,
                            LocalPosition = new Point(localX, localY)
                        });
                    }
                }
            }

            panelMap.Invalidate();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void UpdateSelectedSpritePreview(int index)
        {
            int spriteIndex = index == -1 ? EmptySpriteIndex : index;

            if (spriteIndex >= 0 && spriteIndex < _spriteSelector.SpriteCount)
            {
                var spriteWidth = 256;
                var spriteHeight = 256;

                int columns = _spriteSheet.Width / spriteWidth;
                int col = spriteIndex % columns;
                int row = spriteIndex / columns;

                var spriteRect = new Rectangle(col * spriteWidth, row * spriteHeight, spriteWidth, spriteHeight);

                var previewImage = new Bitmap(spriteWidth, spriteHeight);
                using (var g = Graphics.FromImage(previewImage))
                {
                    g.DrawImage(_spriteSheet, new Rectangle(0, 0, spriteWidth, spriteHeight), spriteRect, GraphicsUnit.Pixel);
                }

                selectedSpritePreview.Image = previewImage;
                selectedSpritePreview.BackColor = Color.Transparent;
            }
            else
            {
                selectedSpritePreview.Image = null;
                selectedSpritePreview.BackColor = Color.FromArgb(63, 63, 70);
            }
        }
        private int GetRoadSpriteIndex(int x, int y)
        {
            if (_map == null || !_map.IsValidPosition(x, y)) return -1;

            bool hasTop = _map.IsValidPosition(x, y - 1) && IsRoad(_map.Elements[x, y - 1]?.SpriteIndex);
            bool hasBottom = _map.IsValidPosition(x, y + 1) && IsRoad(_map.Elements[x, y + 1]?.SpriteIndex);
            bool hasLeft = _map.IsValidPosition(x - 1, y) && IsRoad(_map.Elements[x - 1, y]?.SpriteIndex);
            bool hasRight = _map.IsValidPosition(x + 1, y) && IsRoad(_map.Elements[x + 1, y]?.SpriteIndex);

            if (hasTop && hasBottom && hasLeft && hasRight) return 2; // Перекрёсток
            if (hasTop && hasBottom && hasLeft) return 11; // T-образный перекрёсток (влево)
            if (hasTop && hasBottom && hasRight) return 8; // T-образный перекрёсток (вправо)
            if (hasLeft && hasRight && hasTop) return 9; // T-образный перекрёсток (вверх)
            if (hasLeft && hasRight && hasBottom) return 10; // T-образный перекрёсток (вниз)
            if (hasLeft && hasRight) return 0; // Горизонтальная дорога
            if (hasTop && hasBottom) return 3; // Вертикальная дорога
            if (hasTop && hasRight) return 1; // Угол вверх-вправо
            if (hasTop && hasLeft) return 4; // Угол вверх-влево
            if (hasBottom && hasRight) return 6; // Угол вниз-вправо
            if (hasBottom && hasLeft) return 7; // Угол вниз-влево

            return -1;
        }
        private bool IsRoad(int? spriteIndex)
        {
            return spriteIndex.HasValue && spriteIndex >= 0 && spriteIndex != 5;
        }
        private void UpdateRoadConnections(int x, int y)
        {
            if (_map == null) return;

            if (_map.Elements[x, y]?.SpriteIndex == 5) return;

            int newSpriteIndex = GetRoadSpriteIndex(x, y);
            if (newSpriteIndex >= 0)
            {
                _map.SetElement(x, y, new MapElement(newSpriteIndex));
            }

            foreach (var neighbor in new[] { (x, y - 1), (x, y + 1), (x - 1, y), (x + 1, y) })
            {
                if (_map.IsValidPosition(neighbor.Item1, neighbor.Item2))
                {
                    if (_map.Elements[neighbor.Item1, neighbor.Item2]?.SpriteIndex == 5) continue;

                    int neighborSpriteIndex = GetRoadSpriteIndex(neighbor.Item1, neighbor.Item2);
                    if (neighborSpriteIndex >= 0)
                    {
                        _map.SetElement(neighbor.Item1, neighbor.Item2, new MapElement(neighborSpriteIndex));
                    }
                }
            }
            panelMap.Invalidate();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.ControlKey)
            {
                _isDrawingRectangle = true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void DrawRectangle(Point start, Point end)
        {
            int minX = Math.Min(start.X, end.X);
            int maxX = Math.Max(start.X, end.X);
            int minY = Math.Min(start.Y, end.Y);
            int maxY = Math.Max(start.Y, end.Y);

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    if (x == minX && y == minY)
                    {
                        SetElementWithConnections(x, y, 6);
                    }
                    else if (x == maxX && y == minY)
                    {
                        SetElementWithConnections(x, y, 7);
                    }
                    else if (x == minX && y == maxY)
                    {
                        SetElementWithConnections(x, y, 1);
                    }
                    else if (x == maxX && y == maxY)
                    {
                        SetElementWithConnections(x, y, 4);
                    }
                    else if (y == minY || y == maxY)
                    {
                        SetElementWithConnections(x, y, 0);
                    }
                    else if (x == minX || x == maxX)
                    {
                        SetElementWithConnections(x, y, 3);
                    }
                }
            }
        }
        private void SetElementWithConnections(int x, int y, int defaultSprite)
        {
            if (!_map.IsValidPosition(x, y)) return;

            int newSpriteIndex = GetRoadSpriteIndex(x, y);
            if (newSpriteIndex < 0) newSpriteIndex = defaultSprite;
            _map.SetElement(x, y, new MapElement(newSpriteIndex));
            UpdateNeighborConnections(x, y);
        }
        private void UpdateNeighborConnections(int x, int y)
        {
            int[,] neighbors = {
            { 0, -1 }, 
            { 0, 1 }, 
            { -1, 0 }, 
            { 1, 0 }
            };

            for (int i = 0; i < neighbors.GetLength(0); i++)
            {
                int nx = x + neighbors[i, 0];
                int ny = y + neighbors[i, 1];

                if (_map.IsValidPosition(nx, ny) && IsRoad(_map.Elements[nx, ny]?.SpriteIndex))
                {
                    int neighborSpriteIndex = GetRoadSpriteIndex(nx, ny);
                    if (neighborSpriteIndex >= 0)
                    {
                        _map.SetElement(nx, ny, new MapElement(neighborSpriteIndex));
                    }
                }
            }
        }
        private void Form1_KeyUp_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                _isDrawingRectangle = false;
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                _isCtrlPressed = true;
            }
        }
        private void сохранитьКартуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile("Map Files (*.map)|*.map", stream => _map.SaveToStream(stream));
        }
        private void загрузитьКToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile("Map Files (*.map)|*.map", stream =>
            {
                _map = Map.LoadFromStream(stream);
                panelMap.Invalidate();
            });
        }
        private void экспортВPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog { Filter = "PNG Files (*.png)|*.png" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportMapToPng(saveFileDialog.FileName);
                }
            }
        }
        private void ластикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _selectedSpriteIndex = EmptySpriteIndex;
            HighlightSelectedSprite(-1);
        }
        private void очиститьКартуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_map == null)
            {
                MessageBox.Show("Карта не создана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FillMapWithEmpty();
            panelMap.Invalidate();
        }
        private void добавитьДеревьяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateTrees();
        }
        private void инструкцияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var helpForm = new HelpForm();
            helpForm.ShowDialog();
        }
    }
    public static class ControlExtensions
    {
        public static void DoubleBuffered(this Control control, bool enable)
        {
            var doubleBufferPropertyInfo = control.GetType()
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            doubleBufferPropertyInfo?.SetValue(control, enable, null);
        }
    }
}