using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        public MainWindow()
        {
            InitializeComponent();
            LoadSprites();
            StartNewGame();
        }

        private void LoadSprites()
        {
            tileSprites = Directory.GetFiles(SpriteFolder, "*.png")
                                    .OrderBy(f => f)
                                    .Select(f => new BitmapImage(new Uri(System.IO.Path.GetFullPath(f))))
                                    .ToArray();
        }

        private void StartNewGame()
        {
            gameField = new GameField();
            collectionField = new CollectionField();
            RenderGameField();
            RenderCollectionField();
        }

        private void RenderGameField()
        {
            GameCanvas.Children.Clear();

            foreach (var tile in gameField.Tiles.OrderBy(t => t.Layer))
            {
                if (tileSprites == null || tileSprites.Length == 0 || tile.Type >= tileSprites.Length)
                    continue;

                Brush backgroundBrush = tile.IsAvailable
                    ? Brushes.Beige 
                    : new SolidColorBrush(Color.FromRgb(150, 200, 255));

                Border border = new Border
                {
                    Width = 50,
                    Height = 50,
                    Background = backgroundBrush,
                    BorderBrush = Brushes.DarkGray,
                    BorderThickness = new Thickness(2),
                    Margin = new Thickness(tile.Layer * 5, tile.Layer * 5, 0, 0),
                    CornerRadius = new CornerRadius(8)
                };

                Image image = new Image
                {
                    Source = tileSprites[tile.Type],
                    Stretch = Stretch.Uniform
                };
                border.Child = image;

                Canvas.SetLeft(border, tile.X * 50);
                Canvas.SetTop(border, tile.Y * 50);

                border.MouseLeftButtonDown += (s, e) =>
                {
                    if (tile.IsAvailable && collectionField.AddTile(tile))
                    {
                        gameField.RemoveTile(tile);
                        var removedTiles = collectionField.CheckForTriplets();
                        if (removedTiles.Any())
                        {
                            RenderCollectionField();
                        }
                        RenderGameField();
                        RenderCollectionField();
                    }
                };

                GameCanvas.Children.Add(border);
            }
        }
        private void RenderCollectionField()
        {
            CollectionFieldPanel.Children.Clear();

            for (int i = 0; i < 7; i++)
            {
                Border border = new Border
                {
                    Width = 50,
                    Height = 50,
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

            var removedTiles = collectionField.CheckForTriplets();
            if (removedTiles.Any())
            {
                foreach (var tile in removedTiles)
                {
                    var borderToRemove = CollectionFieldPanel.Children
                        .OfType<Border>()
                        .FirstOrDefault(b => b.Child is Image img && img.Source == tileSprites[tile.Type]);

                    if (borderToRemove != null)
                    {
                        AnimateTileRemoval(borderToRemove);
                    }
                }
            }

            if (collectionField.IsFull)
            {
                MessageBox.Show("Вы проиграли! Поле заполнено.", "Проигрыш", MessageBoxButton.OK, MessageBoxImage.Information);
                StartNewGame();
            }
        }

        private void StartNewGame(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void AnimateTileRemoval(Border tile)
        {
            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(2.5),
                FillBehavior = FillBehavior.Stop
            };

            fadeOutAnimation.Completed += (s, e) =>
            {
                if (tile.Parent is Panel parentPanel)
                {
                    parentPanel.Children.Remove(tile);
                }
            };

            tile.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
        }

    }
}
