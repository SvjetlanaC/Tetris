using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class GameState
    {

        private Block currentBlock;
        private readonly int delayScore = 500; //Game gets faster after every 500 scored points! 

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                //for (int i = 0; i < 2; i++)
                //{
                //    currentBlock.MoveBlock(1, 0);

                //    if (!CanBlockFit())
                //    {
                //        currentBlock.MoveBlock(-1, 0);
                //    }
                //}
            }
        }

        public GameGrid GameGrid
        {
            get;

        }

        public ArrayOfBlocks ArrayOfBlocks
        {
            get;
        }

        public bool GameOver
        {
            get;
            private set;
        }

        public int Score
        {
            get;
            private set;
        }

        public Block HeldBlock
        {
            get;
            private set;
        }

        public bool CanHold
        {
            get;
            private set;
        }
        public GameState()
        {
            GameGrid = new GameGrid(22,10);
            ArrayOfBlocks = new ArrayOfBlocks();
            CurrentBlock = ArrayOfBlocks.GetAndUpdateNextBlock();
            CanHold = true;
        }

        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }

            if(HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = ArrayOfBlocks.GetAndUpdateNextBlock();
            }
            else
            {
                Block tmp=CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }
            CanHold = false;
        }

        private bool CanBlockFit()
        {
            for(int i=0; i < CurrentBlock.BlockPosition().Count(); i++)
            {
                Coordinate c = CurrentBlock.BlockPosition().ElementAt(i);
                if (!GameGrid.IsCellEmpty(c.X, c.Y)) 
                {
                    return false;
                }

            }
            return true;
        }

        public void RotateBlockCW()
        {
            CurrentBlock.Rotate();

            if (!CanBlockFit())
            {
                CurrentBlock.RotateCounterCW();
            }
        }

        public void RotateBlockCounterCW()
        {
            CurrentBlock.RotateCounterCW();

            if (!CanBlockFit())
            {
                CurrentBlock.Rotate();
            }

        }

        public void MoveBlockLeft()
        {
            CurrentBlock.MoveBlock(0, -1);

            if (!CanBlockFit())
            {
                CurrentBlock.MoveBlock(0,1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.MoveBlock(0, 1);

            if (!CanBlockFit())
            {
                CurrentBlock.MoveBlock(0,-1);
            }
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void SetScore(int numberOfClearedRows)
        {
            switch (numberOfClearedRows)
            {
                case 1:
                    Score += 40;
                    break;
                case 2:
                    Score += 100;
                    break;
                case 3:
                    Score += 300;
                    break;
                case 4:
                    Score += 1200;
                    break;
                default:
                    break;
            }
        }

        private static int numberOfDelaySteps = 0;
        public bool CheckScoreForDelay()
        {
            int numberOfPoints = Score / delayScore;
            if(Score >= delayScore && numberOfPoints > numberOfDelaySteps)
            {
                numberOfDelaySteps=numberOfPoints;
                Trace.WriteLine(numberOfDelaySteps);
                return true;

            }else
                 Trace.WriteLine("Nema promjene");
            
            return false;
        }

        private void PlaceBlock()
        {
            for (int i = 0; i < CurrentBlock.BlockPosition().Count(); i++)
            {
                Coordinate c = CurrentBlock.BlockPosition().ElementAt(i);
                GameGrid[c.X, c.Y] = CurrentBlock.Id;
            }

            int numberOfClearedRows = GameGrid.ClearFullRows();

            SetScore(numberOfClearedRows);

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = ArrayOfBlocks.GetAndUpdateNextBlock();
                CanHold = true;
            }
        }

        public void MoveBlockDown()
        {
            CurrentBlock.MoveBlock(1, 0);

            if (!CanBlockFit())
            {
                CurrentBlock.MoveBlock(-1, 0);
                PlaceBlock();
            }
        }

        private int TileHardDrop(Coordinate c)
        {
            int drop = 0;

            while (GameGrid.IsCellEmpty(c.X + drop + 1, c.Y))
            {
                drop++;
            }
            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            for(int i = 0; i < CurrentBlock.BlockPosition().Count(); i++)
            {
                Coordinate c = CurrentBlock.BlockPosition().ElementAt(i);
                drop = System.Math.Min(drop, TileHardDrop(c));
            }

            return drop;
        }

        public void HardDropBlock()
        {
            CurrentBlock.MoveBlock(BlockDropDistance(),0);
            PlaceBlock();
        }

    }
}
