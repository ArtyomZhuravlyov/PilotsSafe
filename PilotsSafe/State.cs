using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotsSafe
{
    internal class State
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public bool IsTurn { get; set; }

        public State(int row, int column, bool isTurn)
        {
            Row = row;
            Column = column;
            IsTurn = isTurn;
        }
    }
}
