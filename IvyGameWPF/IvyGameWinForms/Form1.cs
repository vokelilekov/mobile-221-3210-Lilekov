using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MahjongGameLibrary;

namespace IvyGameWinForms
{
    public partial class Form1 : Form
    {
        private GameField gameField;
        private CollectionField collectionField;
        private Bitmap[] tileSprites;
        private const string SpriteFolder = "Resources";
        private Difficulty selectedDifficulty = Difficulty.Easy;
        private Shape selectedShape;
        private double tileSize = 70;

        public Form1()
        {
            InitializeComponent();
            LoadSprites();
            InitializeGame();
        }
        private void InitializeGame()
        {
            selectedShape = (new Random()).Next(2) == 0 ? Shape.Pyramid : Shape.Heart;
            gameField = new GameField(selectedDifficulty, selectedShape, ensureSolvable: true, isWFA: true);
            collectionField = new CollectionField();

            Console.WriteLine("Инициализация игры...");

            var tiles = new List<Tile>();
            for (int i = 0; i < gameField.MaxLayers * gameField.MaxLayers; i++)
            {
                tiles.Add(new Tile { Index = i, Type = i % 10 });
            }

            int[] typeCounts = new int[10];
            for (int i = 0; i < 10; i++)
            {
                typeCounts[i] = gameField.MaxLayers * 3;
            }

            if (selectedShape == Shape.Pyramid)
            {
                Console.WriteLine("Генерация пирамиды...");
                gameField.GeneratePyramidForWFA(gameField.MaxLayers, tiles);
            }
            else if (selectedShape == Shape.Heart)
            {
                Console.WriteLine("Генерация сердца...");
                gameField.GenerateHeartForWFA(gameField.MaxLayers, tiles);
            }

            RenderGameField();
            RenderCollectionField();
        }

        private void LoadSprites()
        {
            tileSprites = Directory.GetFiles(SpriteFolder, "*.png")
                                   .OrderBy(f => f)
                                   .Select(f => new Bitmap(f))
                                   .ToArray();

            Console.WriteLine("Загружено спрайтов: " + tileSprites.Length);
        }
        private void RenderGameField()
        {
            GamePanel.Controls.Clear();
            double adaptiveTileSize = tileSize;

            double canvasCenterX = GamePanel.Width / 2;
            double canvasCenterY = GamePanel.Height / 2;

            double figureWidth = adaptiveTileSize * (gameField.MaxLayers + 1);
            double figureHeight = adaptiveTileSize * (gameField.MaxLayers + 1);

            double offsetX = canvasCenterX - figureWidth / 2;
            double offsetY = canvasCenterY - figureHeight / 2;

            foreach (var tile in gameField.Tiles.OrderBy(t => t.Layer))
            {
                if (tileSprites == null || tileSprites.Length == 0 || tile.Type >= tileSprites.Length)
                    continue;

                PictureBox pictureBox = new PictureBox
                {
                    Width = (int)adaptiveTileSize,
                    Height = (int)adaptiveTileSize,
                    Image = tileSprites[tile.Type],
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Tag = tile
                };

                pictureBox.BackColor = Color.White;
                pictureBox.Padding = new Padding(5);
                pictureBox.BorderStyle = BorderStyle.FixedSingle;

                double tileOffsetX = offsetX + tile.X * adaptiveTileSize + tile.Layer * 7;
                double tileOffsetY = offsetY + tile.Y * adaptiveTileSize - tile.Layer * 7;

                pictureBox.Location = new Point((int)tileOffsetX, (int)tileOffsetY);

                GamePanel.Controls.Add(pictureBox);

                if (!tile.IsAvailable)
                {
                    pictureBox.Paint += (s, e) =>
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, 0, 0, 0)), 3, 3, pictureBox.Width - 6, pictureBox.Height - 6);
                    };
                }
                else
                {
                    pictureBox.BackColor = Color.LightGray;
                }

                pictureBox.MouseClick += (s, e) => Tile_Click(tile, pictureBox);
            }
        }
        private void RenderCollectionField()
        {
            CollectionPanel.Controls.Clear();

            for (int i = 0; i < 7; i++)
            {
                var tileButton = new Button
                {
                    Width = 50,
                    Height = 50,
                    BackgroundImageLayout = ImageLayout.Zoom,
                    BackgroundImage = null,
                    Tag = null
                };

                tileButton.Click += (sender, e) => CollectionCell_Click(i);

                CollectionPanel.Controls.Add(tileButton);

                tileButton.Location = new Point(10 + i * 60, 10);
            }

            if (collectionField.IsFull)
            {
                MessageBox.Show("Вы проиграли! Поле заполнено.", "Проигрыш", MessageBoxButtons.OK, MessageBoxIcon.Information);
                StartNewGame();
            }

            FillCollectionFieldWithTiles();

            CheckForWin();
        }
        private void FillCollectionFieldWithTiles()
        {
            for (int i = 0; i < collectionField.Tiles.Count && i < 7; i++)
            {
                var tile = collectionField.Tiles[i];

                Button tileButton = CollectionPanel.Controls.OfType<Button>().ElementAt(i);
                tileButton.BackgroundImage = tileSprites[tile.Type];
                tileButton.Tag = tile;
            }

            RemoveTriplets();
        }
        private void RemoveTriplets()
        {
            var removedTiles = collectionField.CheckForTriplets();
            if (removedTiles.Any())
            {
                foreach (var tile in removedTiles)
                {
  
                    var tileButton = CollectionPanel.Controls.OfType<Button>()
                        .FirstOrDefault(b => b.Tag == tile);

                    if (tileButton != null)
                    {
                        tileButton.BackgroundImage = null;
                        tileButton.Tag = null;
                    }
                }
            }
        }
        private void CollectionCell_Click(int index)
        {
            if (collectionField.Tiles.Count > index && !collectionField.IsFull)
            {
                var tile = gameField.Tiles.FirstOrDefault(t => t.IsAvailable);

                if (tile != null)
                {
                    bool added = collectionField.AddTile(tile);
                    if (added)
                    {
                        gameField.RemoveTile(tile);
                        RenderGameField();
                        RenderCollectionField();
                    }
                }
            }
        }
        private void Tile_Click(Tile tile, PictureBox pictureBox)
        {
            if (tile.IsAvailable && !collectionField.IsFull)
            {
                bool added = collectionField.AddTile(tile);
                if (added)
                {
                    gameField.RemoveTile(tile);
                    RenderGameField();
                    RenderCollectionField();
                }
            }
        }
        private void CheckForWin()
        {
            if (gameField.Tiles.Count == 0)
            {
                MessageBox.Show("Поздравляем! Вы выиграли, все плитки разложены.", "Победа", MessageBoxButtons.OK, MessageBoxIcon.Information);
                StartNewGame();
            }
        }
        private void StartNewGame()
        {
            gameField = new GameField(selectedDifficulty, Shape.Pyramid, ensureSolvable: true);
            collectionField = new CollectionField();

            RenderGameField();
            RenderCollectionField();
        }
        private void StartNewGameButton_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }
        private void DifficultySelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DifficultySelector.SelectedItem != null)
            {
                string selectedItemText = DifficultySelector.SelectedItem.ToString();

                if (selectedItemText == "Легкий")
                {
                    selectedDifficulty = Difficulty.Easy;
                }
                else if (selectedItemText == "Средний")
                {
                    selectedDifficulty = Difficulty.Medium;
                }
                else if (selectedItemText == "Сложный")
                {
                    selectedDifficulty = Difficulty.Hard;
                }
                else
                {
                    selectedDifficulty = Difficulty.Easy;
                }
                InitializeGame();
            }
        }
        private void buttonPlus_Click(object sender, EventArgs e)
        {
            tileSize += 10;
            RenderGameField();
        }
        private void buttonMinus_Click(object sender, EventArgs e)
        {
            tileSize -= 10;
            RenderGameField();
        }
    }
}
