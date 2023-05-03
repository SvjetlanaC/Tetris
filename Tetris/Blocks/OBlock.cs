using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Blocks
{
    internal class OBlock : Block
    {
        public override int Id => 4;

        protected override Coordinate StartOffset => new Coordinate(0, 4);

        private readonly Coordinate[][] positions = new Coordinate[][]
        {
            new Coordinate[]{new (0,0), new (0, 1), new(1,0), new(1,1)},
            
        };

        protected override Coordinate[][] Coordinates => positions;
    }
}
