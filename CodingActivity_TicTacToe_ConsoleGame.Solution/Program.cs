using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodingActivity_TicTacToe_ConsoleGame;

namespace CodingActivity_TicTacToe_ConsoleGame
{
    class Program
    {
        private static int roundsPlayed;
        private static int playerXWins;
        private static int playerOWins;
        private static int catsGames;

        static void Main(string[] args)
        {
            
            GameController gameController = new GameController(roundsPlayed, playerXWins, playerOWins, catsGames);
           
        }
    }
}
