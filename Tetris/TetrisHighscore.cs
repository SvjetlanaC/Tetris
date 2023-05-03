using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    [System.Xml.Serialization.XmlInclude(typeof(TetrisHighscore))]
    public class TetrisHighscore
    {       

        public string? PlayerName { get; set; }
        public int Score { get; set; }
    }
}
