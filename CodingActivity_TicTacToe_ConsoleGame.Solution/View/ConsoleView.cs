using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.IO;

namespace CodingActivity_TicTacToe_ConsoleGame
{
    public class ConsoleView
    {
        #region ENUMS

        public enum ViewState
        {
            Active,
            PlayerTimedOut, // TODO Track player time on task
            PlayerUsedMaxAttempts
        }

        #endregion

        #region FIELDS

        private const int GAMEBOARD_VERTICAL_LOCATION = 4;

        private const int POSITIONPROMPT_VERTICAL_LOCATION = 15;
        private const int POSITIONPROMPT_HORIZONTAL_LOCATION = 3;

        private const int MESSAGEBOX_VERTICAL_LOCATION = 18;

        private const int TOP_LEFT_ROW = 3;
        private const int TOP_LEFT_COLUMN = 7;

        private Gameboard _gameboard;
        private ViewState _currentViewStat;

        private int roundsPlayed;
        private int playerXWins;
        private int playerOWins;
        private int catsGames;


        public MenuOption menuOption;


        #endregion

        #region PROPERTIES
        public ViewState CurrentViewState
        {
            get { return _currentViewStat; }
            set { _currentViewStat = value; }
        }

        #endregion

        #region CONSTRUCTORS

        public ConsoleView(Gameboard gameboard)
        {
            _gameboard = gameboard;

            InitializeView();

        }

        #endregion

        #region METHODS

        /// <summary>
        /// Initialize the console view
        /// </summary>
        public void InitializeView()
        {
            _currentViewStat = ViewState.Active;

            InitializeConsole();
        }

        /// <summary>
        /// configure the console window
        /// </summary>
        public void InitializeConsole()
        {
            ConsoleUtil.WindowWidth = ConsoleConfig.windowWidth;
            ConsoleUtil.WindowHeight = ConsoleConfig.windowHeight;

            Console.BackgroundColor = ConsoleConfig.bodyBackgroundColor;
            Console.ForegroundColor = ConsoleConfig.bodyBackgroundColor;

            ConsoleUtil.WindowTitle = "4 X 4 Tic-tac-toe Game";
        }

        /// <summary>
        /// display the Continue prompt
        /// </summary>
        public void DisplayContinuePrompt()
        {
            Console.CursorVisible = false;

            Console.WriteLine();

            ConsoleUtil.DisplayMessage("Press any key to continue.");
            ConsoleKeyInfo response = Console.ReadKey();

            Console.WriteLine();

            Console.CursorVisible = true;
        }

        /// <summary>
        /// display the Exit prompt on a clean screen
        /// </summary>
        public void DisplayExitPrompt()
        {
            ConsoleUtil.DisplayReset();

            Console.CursorVisible = false;

            Console.WriteLine();
            Console.Write("\t Thank you for playing the game. Press any key to Exit.");

            Console.ReadKey();

            System.Environment.Exit(1);
        }

        /// <summary>
        /// display the MainMenu prompt
        /// </summary>
        public void DisplayMainMenu()
        {
            string userResponse;

            Console.Clear();

            Console.CursorVisible = false;

            ConsoleUtil.HeaderText = "The Tic-Tac-Toe Game";
            ConsoleUtil.DisplayReset();


            Console.WriteLine(ConsoleUtil.Center("1. Play a New Round"));
            Console.WriteLine(ConsoleUtil.Center("2. View Rules"));
            Console.WriteLine(ConsoleUtil.Center("3. View Current Game Stats"));
            Console.WriteLine(ConsoleUtil.Center("4. View Historic Game Stats"));
            Console.WriteLine(ConsoleUtil.Center("5. Save Game Results"));
            Console.WriteLine(ConsoleUtil.Center("6. Quit"));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(ConsoleUtil.Center("Enter a number to go to that screen"));

            Console.WriteLine();

            Console.CursorVisible = true;


            userResponse = Console.ReadLine();
            menuOption = (MenuOption)Enum.Parse(typeof(MenuOption), userResponse);

            // thing in here is what we test
            switch (userResponse)
            {
                case ("1"):
                    Console.Clear();
                    ChooseFirstPlayer();
                    break;
                case ("2"):
                    DisplayRulesScreen();
                    break;
                case ("3"):
                    DisplayCurrentGameStatus(roundsPlayed, playerXWins, playerOWins, catsGames);
                    break;
                case ("4"):
                    List<Model.PlayerScores> historicScores = new List<Model.PlayerScores>();

                    //
                    // attempt to read from the data file
                    //
                    try
                    {
                        historicScores = ReadPlayerHistory();
                        DisplayHistoricGameStatus(historicScores);
                    }
                    //
                    // catch the first I/O error
                    //
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");

                        DisplayContinuePrompt();
                    }

                    break;
                case ("5"):
                    string playerHistory;
                    playerHistory = GetPlayerHistory(roundsPlayed, playerXWins, playerOWins);

                    //
                    // attempt to write to file
                    //
                    try
                    {
                        WritePlayerHistory(playerHistory);
                        DisplayContinuePrompt();
                        DisplayMainMenu();
                    }
                    //
                    // catch the first I/O error
                    //
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");

                        DisplayContinuePrompt();
                    }

                    break;
                case ("6"):
                    DisplayExitPrompt();
                    break;
                default:
                    break;
            }

            switch (menuOption)
            {
                case MenuOption.None:
                    break;
                case MenuOption.ChooseFirstPlayer:
                    break;
                case MenuOption.ViewRules:
                    break;
                case MenuOption.ViewCurrentGameResults:
                    break;
                case MenuOption.ViewHistoricGameStats:
                    break;
                case MenuOption.Quit:
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// display the session timed out screen
        /// </summary>
        public void DisplayTimedOutScreen()
        {
            ConsoleUtil.HeaderText = "Session Timed Out!";
            ConsoleUtil.DisplayReset();

            DisplayMessageBox("It appears your session has timed out.");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// displays the who goes first method
        /// </summary>
        public void ChooseFirstPlayer()
        {
            Console.CursorVisible = false;

            int playerFirstChoice;

            // asks the player who wants to go first
            ConsoleUtil.HeaderText = "Who Goes First?";
            ConsoleUtil.DisplayReset();

            Console.WriteLine(ConsoleUtil.Center("1. Player X"));
            Console.WriteLine(ConsoleUtil.Center("2. Player O"));
            Console.WriteLine(ConsoleUtil.Center("3. Let Us Decide"));
            playerFirstChoice = Convert.ToInt32(Console.ReadLine());


            // while (playerFirstChoice)
            // {
            if (playerFirstChoice == 1)
            {
                _gameboard.CurrentRoundState = Gameboard.GameboardState.PlayerXTurn;              
                DisplayGameArea();
            }
            else if (playerFirstChoice == 2)
            {
                _gameboard.CurrentRoundState = Gameboard.GameboardState.PlayerOTurn;
                DisplayGameArea();
            }
            else if (playerFirstChoice == 3)
            {
                Random rnd = new Random();
                int choice = rnd.Next(1, 3);
                if (choice == 1)
                {
                    _gameboard.CurrentRoundState = Gameboard.GameboardState.PlayerXTurn;                 
                    DisplayGameArea();
                }
                else
                {
                    _gameboard.CurrentRoundState = Gameboard.GameboardState.PlayerOTurn;
                    DisplayGameArea();
                }
            }
        }

        /// <summary>
        /// display the maximum attempts reached screen
        /// </summary>
        public void DisplayMaxAttemptsReachedScreen()
        {
            StringBuilder sb = new StringBuilder();

            ConsoleUtil.HeaderText = "Maximum Attempts Reached!";
            ConsoleUtil.DisplayReset();

            sb.Append(" It appears that you are having difficulty entering your");
            sb.Append(" choice. Please refer to the instructions and play again.");

            DisplayMessageBox(sb.ToString());

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Inform the player that their position choice is not available
        /// </summary>
        public void DisplayGamePositionChoiceNotAvailableScreen()
        {
            StringBuilder sb = new StringBuilder();

            ConsoleUtil.HeaderText = "Position Choice Unavailable";
            ConsoleUtil.DisplayReset();

            sb.Append(" It appears that you have chosen a position that is already");
            sb.Append(" taken. Please try again.");

            DisplayMessageBox(sb.ToString());

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display the welcome screen
        /// </summary>
        public void DisplayWelcomeScreen()
        {
            StringBuilder sb = new StringBuilder();

            ConsoleUtil.HeaderText = "The Tic-Tac-Toe Game";
            ConsoleUtil.DisplayReset();

            Console.WriteLine(ConsoleUtil.Center("Written by Jen Berigan and Alex Briggs"));
            Console.WriteLine(ConsoleUtil.Center("Northwestern Michigan College"));
            Console.WriteLine(ConsoleUtil.Center("Version: Sprint 3"));
            Console.WriteLine();

            sb.Clear();
            sb.AppendFormat(ConsoleUtil.Center("This application is designed to allow two players to play a game of tic-tac-toe. Though there's a bit of a twist...we have a 4x4 gameboard"));
            ConsoleUtil.DisplayMessage(sb.ToString());
            Console.WriteLine();
            Console.WriteLine();

            sb.Clear();
            Console.WriteLine(ConsoleUtil.Center("Press Enter to continue."));
            ConsoleUtil.DisplayMessage(sb.ToString());
            Console.WriteLine();

            int numOfPlayerAttempts = 0;
            int maxNumOfPlayerAttempts = 3;

            // While loop validates the choice of enter, escape, or any wrong key
            bool playGame = false;
            while (numOfPlayerAttempts <= maxNumOfPlayerAttempts && !playGame)
            {
                ConsoleKeyInfo info = Console.ReadKey();

                if (info.Key == ConsoleKey.Enter)
                {
                    playGame = true;
                }
                else
                {
                    Console.WriteLine("\t\t That was an invalid answer, please try again!");
                    numOfPlayerAttempts++;
                }
            }

            if (!playGame)
            {
                DisplayExitPrompt();
            }
            else
            {
                DisplayMainMenu();
            }

        }

        /// <summary>
        /// display the rules screen
        /// </summary>
        public void DisplayRulesScreen()
        {
            StringBuilder sb = new StringBuilder();

            ConsoleUtil.HeaderText = "The Rules";
            ConsoleUtil.DisplayReset();

            Console.WriteLine(ConsoleUtil.Center("This is tic-tac-toe with a twist."));
            Console.WriteLine(ConsoleUtil.Center("Instead of a 3x3 gameboard, we have a 4x4 gameboard."));
            Console.WriteLine();
            Console.WriteLine(ConsoleUtil.Center("You can win by getting four up, down, and diagonal."));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            sb.Clear();
            Console.WriteLine(ConsoleUtil.Center("Press Enter to Continue, Press Esc to Quit"));
            Console.WriteLine();

            ConsoleKeyInfo info = Console.ReadKey();
            if (info.Key == ConsoleKey.Escape)
            {
                DisplayExitPrompt();
            }
            else if (info.Key == ConsoleKey.Enter)
            {
                DisplayMainMenu();
            }

        }

        /// <summary>
        /// display a closing screen when the user quits the application
        /// </summary>
        public void DisplayClosingScreen()
        {
            ConsoleUtil.HeaderText = "The Tic-tac-toe Game";
            ConsoleUtil.DisplayReset();

            ConsoleUtil.DisplayMessage("Thank you for using The Tic-tac-toe Game.");

            DisplayContinuePrompt();
        }

        public void DisplayGameArea()
        {
            ConsoleUtil.HeaderText = "Current Game Board";
            ConsoleUtil.DisplayReset();

            DisplayGameboard();
            DisplayGameStatus();
        }

        public void DisplayCurrentGameStatus(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            ConsoleUtil.HeaderText = "Current Game Status";
            ConsoleUtil.DisplayReset();

            double playerXPercentageWins = (double)playerXWins / roundsPlayed;
            double playerOPercentageWins = (double)playerOWins / roundsPlayed;
            double percentageOfCatsGames = (double)catsGames / roundsPlayed;

            ConsoleUtil.DisplayMessage("Rounds Played: " + roundsPlayed);
            ConsoleUtil.DisplayMessage("Rounds for Player X: " + playerXWins + " - " + String.Format("{0:P2}", playerXPercentageWins));
            ConsoleUtil.DisplayMessage("Rounds for Player O: " + playerOWins + " - " + String.Format("{0:P2}", playerOPercentageWins));
            ConsoleUtil.DisplayMessage("Cat's Games: " + catsGames + " - " + String.Format("{0:P2}", percentageOfCatsGames));

            DisplayContinuePrompt();
        }


        public bool DisplayNewRoundPrompt()
        {
            ConsoleUtil.HeaderText = "Continue or Quit";
            ConsoleUtil.DisplayReset();

            return DisplayGetYesNoPrompt("Would you like to play another round?");
        }

        public void DisplayGameStatus()
        {
            StringBuilder sb = new StringBuilder();

            switch (_gameboard.CurrentRoundState)
            {
                case Gameboard.GameboardState.NewRound:
                    //
                    // The new game status should not be an necessary option here
                    //
                    break;
                case Gameboard.GameboardState.PlayerXTurn:
                    DisplayMessageBox("It is currently Player X's turn.");
                    break;
                case Gameboard.GameboardState.PlayerOTurn:
                    DisplayMessageBox("It is currently Player O's turn.");
                    break;
                case Gameboard.GameboardState.PlayerXWin:
                    DisplayMessageBox("Player X Wins! Press any key to continue.");

                    Console.CursorVisible = false;
                    Console.ReadKey();
                    Console.CursorVisible = true;
                    break;
                case Gameboard.GameboardState.PlayerOWin:
                    DisplayMessageBox("Player O Wins! Press any key to continue.");

                    Console.CursorVisible = false;
                    Console.ReadKey();
                    Console.CursorVisible = true;
                    break;
                case Gameboard.GameboardState.CatsGame:
                    DisplayMessageBox("Cat's Game! Press any key to continue.");

                    Console.CursorVisible = false;
                    Console.ReadKey();
                    Console.CursorVisible = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// display a message box
        /// <param name="message"></param>
        /// </summary>
        public void DisplayMessageBox(string message)
        {
            string leftMargin = new String(' ', ConsoleConfig.displayHorizontalMargin);
            string topBottom = new String('*', ConsoleConfig.windowWidth - 2 * ConsoleConfig.displayHorizontalMargin);

            StringBuilder sb = new StringBuilder();

            Console.SetCursorPosition(0, MESSAGEBOX_VERTICAL_LOCATION);
            Console.WriteLine(leftMargin + topBottom);

            Console.WriteLine(ConsoleUtil.Center("Game Status"));

            ConsoleUtil.DisplayMessage(message);

            Console.WriteLine(Environment.NewLine + leftMargin + topBottom);

            Console.WriteLine(ConsoleUtil.Center("Press ESC key at anytime to exit. Press Enter to continue."));

        }

        /// <summary>
        /// display the current game board
        /// </summary>
        private void DisplayGameboard()
        {
            //
            // move cursor below header
            //
            Console.SetCursorPosition(0, GAMEBOARD_VERTICAL_LOCATION);

            Console.Write("\t\t\t        |---+---+---+---|\n");

            for (int i = 0; i < 4; i++)
            {
                Console.Write("\t\t\t        | ");

                for (int j = 0; j < 4; j++)
                {
                    if (_gameboard.PositionState[i, j] == Gameboard.PlayerPiece.None)
                    {
                        Console.Write(" " + " | ");
                    }
                    else
                    {
                        Console.Write(_gameboard.PositionState[i, j] + " | ");
                    }

                }

                Console.Write("\n\t\t\t        |---+---+---+---|\n");
            }


        }

        /// <summary>
        /// display prompt for a player's next move
        /// </summary>
        /// <param name="coordinateType"></param>
        private void DisplayPositionPrompt(string coordinateType)
        {
            //
            // Clear line by overwriting with spaces
            //
            Console.SetCursorPosition(POSITIONPROMPT_HORIZONTAL_LOCATION, POSITIONPROMPT_VERTICAL_LOCATION);
            Console.Write(new String(' ', ConsoleConfig.windowWidth));
            //
            // Write new prompt
            //
            Console.SetCursorPosition(POSITIONPROMPT_HORIZONTAL_LOCATION, POSITIONPROMPT_VERTICAL_LOCATION);
            Console.Write("Enter " + coordinateType + " number: ");


        }

        /// <summary>
        /// Display a Yes or No prompt with a message
        /// </summary>
        /// <param name="promptMessage">prompt message</param>
        /// <returns>bool where true = yes</returns>
        private bool DisplayGetYesNoPrompt(string promptMessage)
        {
            bool yesNoChoice = false;
            bool validResponse = false;
            string userResponse;

            while (!validResponse)
            {
                ConsoleUtil.DisplayReset();

                ConsoleUtil.DisplayPromptMessage(promptMessage + "(yes/no)");
                userResponse = Console.ReadLine();

                if (userResponse.ToUpper() == "YES")
                {
                    validResponse = true;
                    yesNoChoice = true;
                }
                else if (userResponse.ToUpper() == "NO")
                {
                    validResponse = true;
                    yesNoChoice = false;

                    DisplayMainMenu();
                }
                else
                {
                    ConsoleUtil.DisplayMessage(
                        "It appears that you have entered an incorrect response." +
                        " Please enter either \"yes\" or \"no\"."
                        );
                    DisplayContinuePrompt();
                }
            }

            return yesNoChoice;
        }

        /// <summary>
        /// Get a player's position choice within the correct range of the array
        /// Note: The ConsoleView is allowed access to the GameboardPosition struct.
        /// </summary>
        /// <returns>GameboardPosition</returns>
        public GameboardPosition GetPlayerPositionChoice()
        {
            //
            // Initialize gameboardPosition with -1 values
            //
            GameboardPosition gameboardPosition = new GameboardPosition(-1, -1);

            ConsoleKeyInfo escapingGame = Console.ReadKey();
            if (escapingGame.Key == ConsoleKey.Escape)
            {
                DisplayExitPrompt();
            }
            //
            // Get row number from player.
            //
            gameboardPosition.Row = PlayerCoordinateChoice("Row");

            //
            // Get column number.
            //
            if (CurrentViewState != ViewState.PlayerUsedMaxAttempts)
            {
                gameboardPosition.Column = PlayerCoordinateChoice("Column");
            }

            return gameboardPosition;

        }

        /// <summary>
        /// Validate the player's coordinate response for integer and range
        /// </summary>
        /// <param name="coordinateType">an integer value within proper range or -1</param>
        /// <returns></returns>
        private int PlayerCoordinateChoice(string coordinateType)
        {
            int tempCoordinate = -1;
            int numOfPlayerAttempts = 1;
            int maxNumOfPlayerAttempts = 4;




            while ((numOfPlayerAttempts <= maxNumOfPlayerAttempts))
            {
                DisplayPositionPrompt(coordinateType);

                if (int.TryParse(Console.ReadLine(), out tempCoordinate))
                {


                    //
                    // Player response within range
                    //
                    if (tempCoordinate >= 1 && tempCoordinate <= _gameboard.MaxNumOfRowsColumns)
                    {
                        return tempCoordinate;
                    }
                    //
                    // Player response out of range
                    //
                    else
                    {
                        DisplayMessageBox(coordinateType + "Your selection is out of range. Choices are limited to (1,2,3, or 4)");
                    }
                }

                //
                // Player response cannot be parsed as integer
                //
                else
                {
                    DisplayMessageBox(coordinateType + " Your selection is not a valid number. Choices are limited to (1,2,3, or 4)");
                }
                //
                // Increment the number of player attempts
                //


                numOfPlayerAttempts++;
            }

            //
            // Player used maximum number of attempts, set view state and return
            //
            CurrentViewState = ViewState.PlayerUsedMaxAttempts;
            return tempCoordinate;
        }

        /// <summary>
        /// display a list of historic score
        /// </summary>
        /// <param name="historicScores"></param>
        private void DisplayHistoricGameStatus(List<Model.PlayerScores> historicScores)
        {
            ConsoleUtil.HeaderText = "List of Historical Scores";
            ConsoleUtil.DisplayReset();

            foreach (var historicScore in historicScores)
            {
                Console.WriteLine($"{historicScore.PlayerXName} {historicScore.PlayerXScore}, {historicScore.PlayerOName} {historicScore.PlayerOScore}");
            }


            DisplayContinuePrompt();
            DisplayMainMenu();
        }

        /// <summary>
        /// display prompts to save game scores
        /// </summary>
        /// <returns>string to write line to text file</returns>
        private static string GetPlayerHistory(int roundsPlayed, int playerXWins, int playerOWins)
        {
            ConsoleUtil.HeaderText = "Save Game Stats";
            ConsoleUtil.DisplayReset();

            Console.WriteLine("Enter each player's name to save this session's stats.");
            //
            // get player name and score
            //
            Console.Write("Enter player X's name: ");
            string playerXName = Console.ReadLine();
            int playerXScore = roundsPlayed * playerXWins;

            //
            // generate the record string for the data file using the 
            // StringBuilder class
            //
            StringBuilder sb = new StringBuilder();
            sb.Append(playerXName + Data.DataSettings.Delineator);
            sb.Append(playerXScore + Data.DataSettings.Delineator);

            Console.Write("Enter the player O's name: ");
            string playerOName = Console.ReadLine();
            int playerOScore = roundsPlayed * playerOWins;
 
            sb.Append(playerOName + Data.DataSettings.Delineator);
            sb.Append(playerOScore + Data.DataSettings.Delineator);

            return sb.ToString();
        }

        /// <summary>
        /// write player history to text file
        /// </summary>
        /// <param name="playerHistory">string that is the line to write to the text file</param>
        private static void WritePlayerHistory(string playerHistory)
        {
            try
            {
                //
                // initialize a StreamWriter object for writing to a file
                //
                StreamWriter sWriter = new StreamWriter(Data.DataSettings.DataFilePath, true);

                //
                // read all data from the data file
                //
                using (sWriter)
                {
                    sWriter.WriteLine(playerHistory);
                }

            }
            //
            // an I/O error was encountered
            //
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Read all historic player scores
        /// </summary>
        /// <returns>list of historic scores</returns>
        private static List<Model.PlayerScores> ReadPlayerHistory()
        {
            const char delineator = ','; // delineator in a CSV file

            List<Model.PlayerScores> historicScores = new List<Model.PlayerScores>();

            //
            // create lists to hold the historic score strings
            //
            List<string> historicScoresStringList = new List<string>();

            try
            {
                //
                // initialize a StreamReader object for reading from a file
                //
                StreamReader sReader = new StreamReader(Data.DataSettings.DataFilePath);

                //
                // read all data from the data file
                //
                using (sReader)
                {
                    //
                    // keep reading lines of text until the end of the file is reached
                    //
                    while (!sReader.EndOfStream)
                    {
                        historicScoresStringList.Add(sReader.ReadLine());
                    }
                }

            }
            //
            // an I/O error was encountered
            //
            catch (Exception)
            {
                Console.WriteLine("There currently are no scores to display.");
            }

            //
            // separate each line of text from the file into HistoricScore objects
            //
            if (historicScoresStringList != null)
            {
                //
                // separate lines into fields and build out the list of historic scores
                //
                foreach (string historicScore in historicScoresStringList)
                {
                    string[] fields = historicScore.Split(delineator);

                    historicScores.Add(new Model.PlayerScores() { PlayerXName = fields[0], PlayerXScore = Convert.ToInt32(fields[1]), PlayerOName = fields[2], PlayerOScore = Convert.ToInt32(fields[3]) });
                }
            }

            return historicScores;
        }


        #endregion
    }
}
