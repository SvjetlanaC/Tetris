using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Blocks
{
    internal class SBlock : Block
    {
        public override int Id => 5;

        protected override Coordinate StartOffset => new Coordinate(0, 3);

        private readonly Coordinate[][] positions = new Coordinate[][]
        {
            new Coordinate[]{new (0,1), new (0, 2), new(1,0), new(1,1)},
            new Coordinate[]{new (0,1),new (1,1), new (1,2), new (2,2)},
            new Coordinate[]{new (1,1), new(1,2),new(2,0), new (2,1)},
            new Coordinate[] {new (0,0), new (1,0), new (1,1),new (2,1)}
        };

        protected override Coordinate[][] Coordinates => positions;
    }
}
