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
        private int _selectedSpriteIndex = -1;
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
        private Point _startPoint;
        private bool? _isHorizontal = null;
        private bool _isLineStart = false;
        private Timer _mouseDownTimer;
        private const int MouseHoldThreshold = 300;

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            panelMap.DoubleBuffered(true);
            LoadSpriteSheet();
            LoadTreeSpriteSheet();
            InitializeMap(10, 10);
            ApplyDarkTheme(this);
            StyleButtonsInToolbar();

            _mouseDownTimer = new Timer();
            _mouseDownTimer.Interval = MouseHoldThreshold;
            _mouseDownTimer.Tick += MouseDownTimer_Tick;
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
                ApplyDarkTheme(control);
            }
            if (parent is Form form)
            {
                form.BackColor = Color.FromArgb(30, 30, 30);
            }
        }
        private void StyleButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.FromArgb(63, 63, 70);
            button.ForeColor = Color.White;

            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(90, 90, 90);
            button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(63, 63, 70);
            button.MouseDown += (s, e) => button.BackColor = Color.FromArgb(45, 45, 48);
            button.MouseUp += (s, e) => button.BackColor = Color.FromArgb(90, 90, 90);
        }
        private void StyleButtonsInToolbar()
        {
            foreach (Control control in panelToolBar.Controls)
            {
                if (control is Button button)
                {
                    StyleButton(button);
                }
            }
        }
        private void InitializeUI()
        {
            panelToolBar.Dock = DockStyle.Top;
            panelToolBar.Height = 50;

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
        private void RenderSpriteSelector()
        {
            var tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 3,
                RowCount = 4,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            _spriteSelector.Render(tableLayoutPanel, index =>
            {
                _selectedSpriteIndex = index;
                HighlightSelectedSprite(index);
            });

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
            else if (e.Button == MouseButtons.Right)
            {
                _viewport.StartDrag(e.Location); // Начало перемещения карты
            }
        }

        private void PanelMap_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawingLine && e.Button == MouseButtons.Left)
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
                                    _map.SetElement(point.X, point.Y, new MapElement(EmptySpriteIndex));
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

        private void btnSaveMap_Click(object sender, EventArgs e)
        {
            SaveFile("Map Files (*.map)|*.map", stream => _map.SaveToStream(stream));
        }
        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            OpenFile("Map Files (*.map)|*.map", stream =>
            {
                _map = Map.LoadFromStream(stream);
                panelMap.Invalidate();
            });
        }
        private void btnExportToPng_Click(object sender, EventArgs e)
        {
            using (var saveFileDialog = new SaveFileDialog { Filter = "PNG Files (*.png)|*.png" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ExportMapToPng(saveFileDialog.FileName);
                }
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
                int mapWidth = _map.Width * CellSize; 
                int mapHeight = _map.Height * CellSize; 

                using (var bitmap = new Bitmap(mapWidth, mapHeight))
                {
                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.Clear(Color.DarkGray);

                        for (int x = 0; x < _map.Width; x++)
                        {
                            for (int y = 0; y < _map.Height; y++)
                            {
                                var element = _map.Elements[x, y] ?? MapElement.CreateEmpty();
                                var rect = new Rectangle(x * CellSize, y * CellSize, CellSize, CellSize);

                                _spriteSelector.DrawSprite(g, rect, element.SpriteIndex);
                                g.DrawRectangle(Pens.Black, rect);
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
        private void btnClearMap_Click(object sender, EventArgs e)
        {
            if (_map == null)
            {
                MessageBox.Show("Карта не создана.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FillMapWithEmpty();
            panelMap.Invalidate();
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
        private void btnAddTrees_Click(object sender, EventArgs e)
        {
            GenerateTrees();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

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