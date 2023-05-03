using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Blocks;

namespace Tetris
{
    internal class ArrayOfBlocks
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock(),
        };

        public ArrayOfBlocks()
        {
            NextBlock = RandomNextBlock();
        }

        private readonly Random random = new Random();  

        public Block NextBlock { get; private set; }

        private Block RandomNextBlock() 
        {
            return blocks[random.Next(blocks.Length)];
        }

        public Block GetAndUpdateNextBlock()
        {
            Block block = NextBlock;

            do
            {
                NextBlock=RandomNextBlock();
            }
            while(block.Id == NextBlock.Id);
            return  block;
        }
    }
}
