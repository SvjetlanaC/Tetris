using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal abstract class Block
    {
        private int rotationState;
        private Coordinate offset;

        protected abstract Coordinate[][] Coordinates { get; } 
        protected abstract Coordinate StartOffset { get; }

        public abstract int Id { get; }

        public Block()
        {
            offset=new Coordinate(StartOffset.X,StartOffset.Y);
        }

        public IEnumerable<Coordinate> BlockPosition()
        {
            for(int i=0; i<Coordinates[rotationState].Length; i++)
            {
                yield return new Coordinate(Coordinates[rotationState][i].X + offset.X, Coordinates[rotationState][i].Y + offset.Y);
            }
        }

        public void Rotate()
        {
            rotationState = (rotationState + 1) % Coordinates.Length;
        }

        public void RotateCounterCW() 
        {
            if(rotationState == 0)
            {
                rotationState = Coordinates.Length-1;

            }
            else
            {
                rotationState--;
            }
        }

        public void MoveBlock(int row, int column)
        {
            offset.X += row;
            offset.Y += column;
        }

        public void Reset()
        {
            rotationState = 0;
            offset.X = StartOffset.X;
            offset.Y = StartOffset.Y;   
        }
    }
}
