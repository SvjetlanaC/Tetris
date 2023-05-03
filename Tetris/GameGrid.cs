using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class GameGrid
    {
        private readonly int[,] grid;

        public int Rows
        { 
            get; 
        }

        public int Columns 
        {
            get;
        }
        
        public int this[int r,int c]
        {
            get => grid[r,c];
            set => grid[r,c] = value;   
        }

        public GameGrid(int rows, int columns)
        {
            grid = new int[rows,columns];
            Rows = rows;
            Columns = columns;
        }

        public bool IsInside(int row, int column)
        {
            return row >=0 && column >= 0 && row < Rows && column < Columns;
        }

        public bool IsCellEmpty(int row, int column) 
        {
            return IsInside(row,column) && grid[row,column] == 0;
        }

        public bool IsRowFull(int row)  
        {
            for(int i= 0; i < Columns; i++)
            {
                if(IsCellEmpty(row,i))//grid[row,i] == 0
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsRowEmpty(int row)
        {
            for(int i=0;i<Columns;i++)
            {
                if(grid[row,i] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void ClearRow(int row)
        {
            if(IsRowFull(row))
            {            
                for(int i = 0; i < Columns; i++)
                {
                    grid[row,i] = 0;
                }
            }
        }

        public void MoveRowDown(int row,int numberOfRows)
        {
            for(int i = 0; i < Columns; i++)
            {
                grid[row+numberOfRows,i] = grid[row,i];
                grid[row, i] = 0;
            }
        }

        public int ClearFullRows()
        {
            int numberOfClearedRows = 0;
            for(int i = Rows - 1; i >= 0; i--)
            {
                if(IsRowFull(i))
                {
                    ClearRow(i);
                    numberOfClearedRows++;
                }
                else if(numberOfClearedRows > 0)
                {
                    MoveRowDown(i, numberOfClearedRows);
                }
            }
            return numberOfClearedRows;
        }
    }
}
