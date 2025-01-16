using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using MahjongGameLibrary;

namespace MahjongGame
{
    public partial class MainWindow : Window
    {
        private GameField gameField;
        private CollectionField collectionField;
        private BitmapImage[] tileSprites;
        private const string SpriteFolder = "Resources";
        private Difficulty selectedDifficulty = Difficulty.Easy;
        private Shape selectedShape;
        private int tileSize = 100;


        public MainWindow()
        {
            InitializeComponent();
            LoadSprites();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void LoadSprites()
        {
            tileSprites = Directory.GetFiles(SpriteFolder, "*.png")
                                    .OrderBy(f => f)
                                    .Select(f => new BitmapImage(new Uri(Path.GetFullPath(f))))
                                    .ToArray();
        }

        private void StartNewGame()
        {
            Array shapes = Enum.GetValues(typeof(Shape));
            Random random = new Random();
            selectedShape = (Shape)shapes.GetValue(random.Next(shapes.Length));
            gameField = new GameField(selectedDifficulty, selectedShape, ensureSolvable: true);
            collectionField = new CollectionField();
            RenderGameField();
            RenderCollectionField();
        }

        private void DifficultySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DifficultySelector.SelectedItem is ComboBoxItem selectedItem)
            {
                selectedDifficulty = Enum.TryParse(selectedItem.Tag.ToString(), out Difficulty difficulty) ? difficulty : Difficulty.Easy;
                StartNewGame();
            }
        }

        private void RenderGameField()
        {
            GameCanvas.Children.Clear();

            double canvasCenterX = GameCanvas.ActualWidth / 2;
            double canvasCenterY = GameCanvas.ActualHeight / 2;
            double scaleFactor = Math.Min(GameCanvas.ActualWidth / 600, GameCanvas.ActualHeight / 1200);
            double adaptiveTileSize = tileSize * scaleFactor;
            double figureWidth = adaptiveTileSize * (gameField.MaxLayers + 1);
            double figureHeight = adaptiveTileSize * (gameField.MaxLayers + 1);

            double offsetX = canvasCenterX - figureWidth / 2;
            double offsetY = canvasCenterY - figureHeight / 2;

            foreach (var tile in gameField.Tiles.OrderBy(t => t.Layer))
            {
                if (tileSprites == null || tileSprites.Length == 0 || tile.Type >= tileSprites.Length)
                    continue;

                Border border = new Border
                {
                    Width = adaptiveTileSize,
                    Height = adaptiveTileSize,
                    Background = Brushes.Beige,
                    BorderBrush = Brushes.DarkGray,
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(8),
                    Tag = tile
                };

                Image image = new Image
                {
                    Source = tileSprites[tile.Type],
                    Stretch = Stretch.Uniform
                };
                border.Child = image;

                double tileOffsetX = offsetX + tile.X * adaptiveTileSize + tile.Layer * 7 * scaleFactor;
                double tileOffsetY = offsetY + tile.Y * adaptiveTileSize - tile.Layer * 7 * scaleFactor;
                tileOffsetX = Math.Max(0, Math.Min(tileOffsetX, GameCanvas.ActualWidth - adaptiveTileSize));
                tileOffsetY = Math.Max(0, Math.Min(tileOffsetY, GameCanvas.ActualHeight - adaptiveTileSize));

                Canvas.SetLeft(border, tileOffsetX);
                Canvas.SetTop(border, tileOffsetY);

                GameCanvas.Children.Add(border);

                if (!tile.IsAvailable)
                {
                    Border overlay = new Border
                    {
                        Width = adaptiveTileSize,
                        Height = adaptiveTileSize,
                        Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)),
                        CornerRadius = new CornerRadius(8)
                    };

                    Canvas.SetLeft(overlay, tileOffsetX);
                    Canvas.SetTop(overlay, tileOffsetY);
                    GameCanvas.Children.Add(overlay);
                }

                border.MouseLeftButtonDown += (s, e) => Tile_Click(tile, border);
            }
        }


        private void RenderCollectionField()
        {
            if (CollectionFieldPanel == null)
                return;

            CollectionFieldPanel.Children.Clear();

            for (int i = 0; i < 7; i++)
            {
                Border border = new Border
                {
                    Width = 70,
                    Height = 70,
                    Background = Brushes.LightGray,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(2),
                    Margin = new Thickness(5),
                    CornerRadius = new CornerRadius(5)
                };

                if (i < collectionField.Tiles.Count)
                {
                    var tile = collectionField.Tiles[i];
                    if (tile.Type < tileSprites.Length)
                    {
                        Image image = new Image
                        {
                            Source = tileSprites[tile.Type],
                            Stretch = Stretch.Uniform
                        };

                        border.Child = image;
                        border.Background = Brushes.LightBlue;
                    }
                }

                CollectionFieldPanel.Children.Add(border);
            }

            RemoveTriplets();

            if (collectionField.IsFull)
            {
                MessageBox.Show("Вы проиграли! Поле заполнено.", "Проигрыш", MessageBoxButton.OK, MessageBoxImage.Information);
                StartNewGame();
            }

            CheckForWin();
        }

        private void Tile_Click(Tile tile, Border border)
        {
            if (!tile.IsAvailable || collectionField.IsFull)
                return;

            if (collectionField.AddTile(tile))
            {
                gameField.RemoveTile(tile);
                RenderGameField();
                RenderCollectionField();
            }
        }

        private void RemoveTriplets()
        {
            var removedTiles = collectionField.CheckForTriplets();

            if (removedTiles.Any())
            {
                foreach (var tile in removedTiles)
                {

                    var borderToRemove = CollectionFieldPanel.Children
                        .OfType<Border>()
                        .FirstOrDefault(b => b.Child is Image img && img.Source == tileSprites[tile.Type]);
                        AnimateTileContentRemoval(borderToRemove);
                }

                RenderCollectionField();
            }
        }

        private void AnimateTileContentRemoval(Border border)
        {
            if (border.Child is UIElement child)
            {
                var fadeOut = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    FillBehavior = FillBehavior.Stop
                };

                fadeOut.Completed += (s, e) =>
                {
                    border.Child = null;
                };

                child.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            }
        }

        private void CheckForWin()
        {
            if (gameField.Tiles.Count == 0)
            {
                MessageBox.Show("Поздравляем! Вы выиграли, все плитки разложены.", "Победа", MessageBoxButton.OK, MessageBoxImage.Information);
                StartNewGame();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RenderGameField();
        }
        private void StartNewGame(object sender, RoutedEventArgs e)
        {
            StartNewGame();
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
        private void ShowRules(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Правила игры Маджонг:\n\n" +
                            "1. Задача игры — очистить игровое поле, удаляя тройки одинаковых плиток.\n" +
                            "2. Плитки могут быть удалены только в том случае, если они не закрыты другими плитками.\n" +
                            "3. В игре есть несколько уровней сложности: Легкий, Средний, Сложный.\n" +
                            "4. У вас есть поле, куда собираются тройки плиток. Но помни, что места всего на 7 плиток!\n\n" +
                            "Удачи!");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    

        private void GameCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) 
            {
                tileSize += 5;
            }
            else if (e.Delta < 0)
            {
                if (tileSize > 30) 
                {
                    tileSize -= 5;
                }
            }

            RenderGameField();
        }

    }
}
