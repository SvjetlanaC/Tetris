using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [System.Xml.Serialization.XmlInclude(typeof(ObservableCollection<TetrisHighscore>))]
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Resources/EmptyBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/BlueBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/RedBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/PurpleBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/GreenBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/DarkBlueBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/YellowBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/OrangeBlock.png",UriKind.Relative)),
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Resources/EmptyBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/IBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/JBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/LBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/OBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/SBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/TBlock.png",UriKind.Relative)),
            new BitmapImage(new Uri("Resources/ZBlock.png",UriKind.Relative)),
        };

        private readonly Image[,] imageControls;
        private GameState gameState = new GameState();

        private readonly int maxDelay = 1000;
        private readonly int minDelay = 100;
        private readonly int delayStep = 25;
        private int currentDelay;
        

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetUpGameCanvas(gameState.GameGrid);
            currentDelay = maxDelay;

            LoadHighscoreList();
        }


        public ObservableCollection<TetrisHighscore> HighscoreList
        {
            get; set;
        } = new ObservableCollection<TetrisHighscore>();

        private void LoadHighscoreList()
        {
            if (File.Exists("tetris_highscorelist.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<TetrisHighscore>));
                using (Stream reader = new FileStream("tetris_highscorelist.xml", FileMode.Open))
                {
                    List<TetrisHighscore> tempList = (List<TetrisHighscore>)serializer.Deserialize(reader);
                    HighscoreList.Clear();
                    foreach (var item in tempList.OrderByDescending(x => x.Score))
                        HighscoreList.Add(item);
                }

            }
        }
        private void SaveHighscoreList()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<TetrisHighscore>));
            using (Stream writer = new FileStream("tetris_highscorelist.xml", FileMode.Create))
            {
                serializer.Serialize(writer, HighscoreList);
            }
        }
        private void AddToHighscoreList_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = 0;
            // Where should the new entry be inserted?
            if ((HighscoreList.Count > 0) && (gameState.Score < HighscoreList.Max(x => x.Score)))
            {
                TetrisHighscore justAbove = HighscoreList.OrderByDescending(x => x.Score).First(x => x.Score >= gameState.Score);
                if (justAbove != null)
                    newIndex = HighscoreList.IndexOf(justAbove) + 1;
            }
            // Create & insert the new entry
            HighscoreList.Insert(newIndex, new TetrisHighscore()
            {
                PlayerName = NewHighScoreName.Text,
                Score = gameState.Score
            });
            // Make sure that the amount of entries does not exceed the maximum
            while (HighscoreList.Count > 10)
                HighscoreList.RemoveAt(10);

            SaveHighscoreList();

            NewHighScoreMenu.Visibility = Visibility.Hidden;
            HighScoreViewMenu.Visibility = Visibility.Visible;
        }

        private void DrawGrid(GameGrid grid)
        {
            for(int r = 0; r < grid.Rows; r++)
            {
                for(int c=0; c < grid.Columns; c++)
                {
                    int blockId = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[blockId];
                }
            }
        }

        private void DrawCurrentBlock(Block block)
        {
            for(int i = 0; i < block.BlockPosition().Count(); i++)
            {
                Coordinate c=block.BlockPosition().ElementAt(i);
                imageControls[c.X, c.Y].Opacity = 1;
                imageControls[c.X, c.Y].Source = tileImages[block.Id];
            }
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];   
            }
        }
        private void Draw(GameState gs)
        {
            DrawGrid(gs.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawCurrentBlock(gs.CurrentBlock);
            DrawNextBlock(gameState.ArrayOfBlocks);
            ScoreText.Text = "Score: " + gameState.Score;
            DrawHeldBlock(gameState.HeldBlock);
        }

        private Image[,] SetUpGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for(int r=0; r<grid.Rows; r++)
            {
                for(int c=0; c<grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };
                    Canvas.SetTop(imageControl, (r - 2) * cellSize);
                    Canvas.SetLeft(imageControl,c*cellSize);

                    GameCanvas.Children.Add(imageControl);
                    imageControls[r,c] = imageControl;
                }
            }
            return imageControls;
        }

        private async Task GameLoop()
        {
            Draw(gameState);
            while(!gameState.GameOver)
            {
                
                if (gameState.CheckScoreForDelay())
                {
                    currentDelay -= delayStep;
                    Trace.WriteLine(currentDelay);
                }
                
                await Task.Delay(currentDelay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }


            IsNewHighScore();


        }

        private void IsNewHighScore()
        {
            NewHighScoreName.Clear();
            bool isNewHighscore = false;
            if (gameState.Score > 0)
            {
                int lowestHighscore = (HighscoreList.Count > 0 ? HighscoreList.Min(x => x.Score) : 0);
                Trace.WriteLine(HighscoreList.Min(x => x.Score));
                if ((gameState.Score > lowestHighscore) || (HighscoreList.Count < 10))
                {
                    NewHighScoreMenu.Visibility = Visibility.Visible;

                    isNewHighscore = true;
                }
            }
            if (isNewHighscore)
            {

                NewHighScoreMenu.Visibility = Visibility.Visible;
            }
            else
            {
                NewHighScoreMenu.Visibility = Visibility.Hidden;
                GameOverMenu.Visibility = Visibility.Visible;
                TotalScoreText.Text = "Total score: " + gameState.Score;
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCounterCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Enter:
                    gameState.HardDropBlock();
                    break;
                default:
                    return;
            }
            Draw(gameState);
        }

        private void DrawNextBlock(ArrayOfBlocks blocks)
        {
            Block next = blocks.NextBlock;
            NextImage.Source=blockImages[next.Id];
        }

        private void TopScoreView_Click(object sender, RoutedEventArgs e)
        {
            
            LoadHighscoreList();
            GameOverMenu.Visibility = Visibility.Hidden;
            HighScoreViewMenu.Visibility = Visibility.Visible;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
           
            HighScoreViewMenu.Visibility = Visibility.Hidden;
            StartGameMenu.Visibility = Visibility.Visible;
        }

        private void BackToGameOver_Click(object sender, RoutedEventArgs e)
        {
            
            GameOverMenu.Visibility = Visibility.Visible;
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState=new GameState();
            GameOverMenu.Visibility=Visibility.Hidden;
            await GameLoop();
        }

        private async void StartGame_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            StartGameMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            for (int i = 0; i < block.BlockPosition().Count(); i++)
            {
                Coordinate c = block.BlockPosition().ElementAt(i);
                imageControls[c.X + dropDistance, c.Y].Opacity = 0.45;
                
                imageControls[c.X+dropDistance, c.Y].Source = tileImages[block.Id];
            }

        }
    }
}
