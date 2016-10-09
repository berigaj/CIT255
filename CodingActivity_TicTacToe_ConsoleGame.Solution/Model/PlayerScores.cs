using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingActivity_TicTacToe_ConsoleGame.Model
{
    public class PlayerScores
    {
       public string PlayerXName { get; set; }
       public int PlayerXWins { get; set; }
       
       public string PlayerOName { get; set; }
       public int PlayerOWins { get; set; }
        
       public string GameDate { get; set; }  
    }
}
