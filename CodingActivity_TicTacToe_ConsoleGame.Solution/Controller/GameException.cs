using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingActivity_TicTacToe_ConsoleGame.Controller
{
    //Inherit from main Exception class
    public class PositionChoiceOutOfRangeException : Exception
    {
        //use this constructor to set our own message.
        public PositionChoiceOutOfRangeException(string message) : base(message)
        {
        }
    }

    public class PoisitionChoiceAlreadyTakenException : Exception
    {
        public PoisitionChoiceAlreadyTakenException(string message) : base(message)
        {
        }
    }
}
