﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

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
        private Gameboard.PlayerPiece PlayerPiece;
        private Gameboard.GameboardState PlayerState;

        #endregion

        #region PROPERTIES
        public ViewState CurrentViewState
        {
            get { return _currentViewStat; }
            set { _currentViewStat = value; }
        }

        public class Key { }

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
            Console.Write("\t\t Thank you for playing the game. Press any key to Exit.");

            Console.ReadKey();

            System.Environment.Exit(1);
        }

        /// <summary>
        /// display the save game screen
        /// </summary>
        public void DisplaySaveGameScreen()
        {

        }

        /// <summary>
        /// display the MainMenu prompt
        /// </summary>
        public void DisplayMainMenu(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            Console.Clear();

            Console.CursorVisible = false;

            ConsoleUtil.HeaderText = "The Tic-Tac-Toe Game";
            ConsoleUtil.DisplayReset();


            Console.WriteLine(ConsoleUtil.Center("A. Play a New Round"));
            Console.WriteLine(ConsoleUtil.Center("B. View Rules"));
            Console.WriteLine(ConsoleUtil.Center("C. View Current Game Stats"));
            Console.WriteLine(ConsoleUtil.Center("D. View Historic Game Stats"));
            Console.WriteLine(ConsoleUtil.Center("E. Save Game Results"));
            Console.WriteLine(ConsoleUtil.Center("F. Quit"));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(ConsoleUtil.Center("Enter a letter to go to that screen"));

            Console.WriteLine();

            Console.CursorVisible = true;

            int numOfPlayerAttempts = 0;
            int maxNumOfPlayerAttempts = 3;

            while (numOfPlayerAttempts <= maxNumOfPlayerAttempts)
            {
                ConsoleKeyInfo response = Console.ReadKey();

                if (response.Key == ConsoleKey.A)
                {
                    DisplayGetFirstPlayer(roundsPlayed, playerXWins, playerOWins, catsGames);
                }
                else if (response.Key == ConsoleKey.B)
                {
                    DisplayRulesScreen(roundsPlayed, playerXWins, playerOWins, catsGames);
                }
                else if (response.Key == ConsoleKey.C)
                {
                    DisplayCurrentGameStatus(roundsPlayed, playerXWins, playerOWins, catsGames);
                }
                else if (response.Key == ConsoleKey.D)
                {
                    DisplayHistoricGameStatus(roundsPlayed, playerXWins, playerOWins, catsGames);
                }
                else if (response.Key == ConsoleKey.E)
                {
                    DisplaySaveGameScreen();
                }
                else if (response.Key == ConsoleKey.F)
                {
                    DisplayExitPrompt();
                }
                else
                {
                    Console.WriteLine("\t\t That was an invalid key, please try again!");
                    numOfPlayerAttempts++;
                }

                
            }

            DisplayExitPrompt();
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
        public Gameboard.PlayerPiece DisplayGetFirstPlayer(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            Console.CursorVisible = false;

            int playerFirstChoice;

            // asks the player who wants to go first
            ConsoleUtil.HeaderText = "Who Goes First?";
            ConsoleUtil.DisplayReset();

            Console.WriteLine(ConsoleUtil.Center("A. Player X"));
            Console.WriteLine(ConsoleUtil.Center("B. Player O"));
            Console.WriteLine(ConsoleUtil.Center("C. Let Us Decide"));

            int numOfPlayerAttempts = 0;
            int maxNumOfPlayerAttempts = 3;

            while ((numOfPlayerAttempts <= maxNumOfPlayerAttempts))
            {
                ConsoleKeyInfo response = Console.ReadKey();

                if (response.Key == ConsoleKey.A)
                {
                    PlayerState = Gameboard.GameboardState.PlayerXTurn;
                    DisplayGameArea(roundsPlayed, playerXWins, playerOWins, catsGames);
                }
                else if (response.Key == ConsoleKey.B)
                {
                    PlayerState = Gameboard.GameboardState.PlayerOTurn;
                    DisplayFirstPlayer(roundsPlayed, playerXWins, playerOWins, catsGames);
                }
                else if (response.Key == ConsoleKey.C)
                {
                    Random rnd = new Random();
                    int choice = rnd.Next(1, 3);
                    if (choice == 1)
                    {
                        PlayerState = Gameboard.GameboardState.PlayerXTurn;
                        DisplayFirstPlayer(roundsPlayed, playerXWins, playerOWins, catsGames);
                    }
                    else
                    {
                        PlayerState = Gameboard.GameboardState.PlayerOTurn;
                        DisplayFirstPlayer(roundsPlayed, playerXWins, playerOWins, catsGames);
                    }
                }
                else
                {
                    Console.WriteLine("You have pressed an incorrect key!");
                    numOfPlayerAttempts++;
                }
                
            }


            DisplayExitPrompt();
            return PlayerPiece;

        }

        public void DisplayFirstPlayer(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            //if (Convert.ToBoolean(PlayerPiece = Gameboard.PlayerPiece.X))
            //{
            //    PlayerState = Gameboard.GameboardState.PlayerXTurn;
            //    DisplayGameArea(roundsPlayed, playerXWins, playerOWins, catsGames);
            //}
            //else
            //{
            //    DisplayExitPrompt();
            //}
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
        public void DisplayWelcomeScreen(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            StringBuilder sb = new StringBuilder();

            ConsoleUtil.HeaderText = "The Tic-Tac-Toe Game";
            ConsoleUtil.DisplayReset();

            Console.WriteLine(ConsoleUtil.Center("Written by Jen Berigan and Alex Briggs"));
            Console.WriteLine(ConsoleUtil.Center("Northwestern Michigan College"));
            Console.WriteLine(ConsoleUtil.Center("Version: Sprint 2"));
            Console.WriteLine();

            sb.Clear();
            sb.AppendFormat(ConsoleUtil.Center("This application is designed to allow two players to play a game of tic-tac-toe. Though there's a bit of a twist...we have a 4x4 gameboard"));
            ConsoleUtil.DisplayMessage(sb.ToString());
            Console.WriteLine();
            Console.WriteLine();

            sb.Clear();
            Console.WriteLine(ConsoleUtil.Center("Press ESC key at anytime to exit."));
            ConsoleUtil.DisplayMessage(sb.ToString());
            Console.WriteLine();

            int numOfPlayerAttempts = 0;
            int maxNumOfPlayerAttempts = 3;

            // While loop validates the choice of enter, escape, or any wrong key
            while (numOfPlayerAttempts <= maxNumOfPlayerAttempts)
            {
                ConsoleKeyInfo info = Console.ReadKey();

                if (info.Key == ConsoleKey.Escape)
                {
                    DisplayExitPrompt();
                }
                else if (info.Key == ConsoleKey.Enter)
                {
                    DisplayMainMenu(roundsPlayed, playerXWins, playerOWins, catsGames);
                }
                else
                {
                    Console.WriteLine("\t\t That was an invalid answer, please try again!");
                    numOfPlayerAttempts++;
                }

                
            }

            DisplayExitPrompt();
        }

        /// <summary>
        /// display the rules screen
        /// </summary>
        public void DisplayRulesScreen(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            StringBuilder sb = new StringBuilder();

            ConsoleUtil.HeaderText = "The Rules";
            ConsoleUtil.DisplayReset();

            Console.WriteLine(ConsoleUtil.Center("This is tic-tac-toe with a twist."));
            Console.WriteLine(ConsoleUtil.Center("Instead of a 3x3 gameboard, we have a 4x4 gameboard."));
            Console.WriteLine();
            Console.WriteLine(ConsoleUtil.Center("You can win by getting four up, down, diagonal, and four corners."));
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
                DisplayMainMenu(roundsPlayed, playerXWins, playerOWins, catsGames);
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

        /// <summary>
        /// display game area
        /// </summary>
        public void DisplayGameArea(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            ConsoleUtil.HeaderText = "Current Game Board";
            ConsoleUtil.DisplayReset();

            DisplayGameboard();
            DisplayGameStatus(roundsPlayed, playerXWins, playerOWins, catsGames);
        }

        /// <summary>
        /// displays the current game status
        /// <param name="roundsPlayed"></param>
        /// <param name="playerXWins"></param>
        /// <param name="playerOWins"></param>
        /// <param name="catsGames"></param>
        /// </summary>
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

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("\t Press Enter to go back to Main Menu");
            ConsoleKeyInfo response = Console.ReadKey();

            if (response.Key == ConsoleKey.Enter)
            {
                DisplayMainMenu(roundsPlayed, playerXWins, playerOWins, catsGames);
            }



        }

        /// <summary>
        /// displays the historic game status
        /// <param name="roundsPlayed"></param>
        /// <param name="playerXWins"></param>
        /// <param name="playerOWins"></param>
        /// <param name="catsGames"></param>
        /// </summary>
        public void DisplayHistoricGameStatus(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            ConsoleUtil.HeaderText = "Historic Game Status";
            ConsoleUtil.DisplayReset();

            double playerXPercentageWins = (double)playerXWins / roundsPlayed;
            double playerOPercentageWins = (double)playerOWins / roundsPlayed;
            double percentageOfCatsGames = (double)catsGames / roundsPlayed;

            ConsoleUtil.DisplayMessage("Rounds Played: " + roundsPlayed);
            ConsoleUtil.DisplayMessage("Rounds for Player X: " + playerXWins + " - " + String.Format("{0:P2}", playerXPercentageWins));
            ConsoleUtil.DisplayMessage("Rounds for Player O: " + playerOWins + " - " + String.Format("{0:P2}", playerOPercentageWins));
            ConsoleUtil.DisplayMessage("Cat's Games: " + catsGames + " - " + String.Format("{0:P2}", percentageOfCatsGames));

            Console.WriteLine();
            Console.WriteLine();
            Console.Write("\t Press Enter to go back to Main Menu");
            ConsoleKeyInfo response = Console.ReadKey();

            if (response.Key == ConsoleKey.Enter)
            {
                DisplayMainMenu(roundsPlayed, playerXWins, playerOWins, catsGames);
            }


        }

        /// <summary>
        /// display new round prompt
        /// </summary>
        public bool DisplayNewRoundPrompt()
        {
            ConsoleUtil.HeaderText = "Continue or Quit";
            ConsoleUtil.DisplayReset();

            return DisplayGetYesNoPrompt("Would you like to play another round?", 0, 0, 0, 0);
        }

        /// <summary>
        /// display the game status screen
        /// </summary>
        public void DisplayGameStatus(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
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
                    DisplayMessageBox("Player X Wins! Press Enter key to go to main.");

                    Console.CursorVisible = false;
                    ConsoleKeyInfo response = Console.ReadKey();

                    if (response.Key == ConsoleKey.Enter)
                    {
                        DisplayMainMenu(roundsPlayed, playerXWins, playerOWins, catsGames);
                    }
                    Console.CursorVisible = true;
                    break;
                case Gameboard.GameboardState.PlayerOWin:
                    DisplayMessageBox("Player O Wins! Press Enter key to go to main menu.");

                    Console.CursorVisible = false;
                    ConsoleKeyInfo playerrepsonse = Console.ReadKey();

                    if (playerrepsonse.Key == ConsoleKey.Enter)
                    {
                        DisplayMainMenu(roundsPlayed, playerXWins, playerOWins, catsGames);
                    }
                    Console.CursorVisible = true;
                    break;
                case Gameboard.GameboardState.CatsGame:
                    DisplayMessageBox("Cat's Game! Press Enter key to go to main menu.");

                    Console.CursorVisible = false;
                    ConsoleKeyInfo myresponse = Console.ReadKey();

                    if (myresponse.Key == ConsoleKey.Enter)
                    {
                        DisplayMainMenu(roundsPlayed, playerXWins, playerOWins, catsGames);
                    }
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

            Console.WriteLine(ConsoleUtil.Center("Press ESC key at anytime to exit."));

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

                //if (Console.ReadKey().Key == ConsoleKey.Escape)
                //{
                //    DisplayExitPrompt();
                //}

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

        private bool DisplayGetYesNoPrompt(string promptMessage, int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
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
                    DisplayMainMenu(roundsPlayed, playerXWins, playerOWins, catsGames);
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
        /// Display a Yes or No prompt with a message
        /// </summary>
        /// <param name="promptMessage">prompt message</param>
        /// <returns>bool where true = yes</returns>

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

            // add escape key to quit code here
            ConsoleKeyInfo info = Console.ReadKey();
            if (info.Key == ConsoleKey.Escape)
            {
                DisplayExitPrompt();
            }


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




        #endregion
    }
}
