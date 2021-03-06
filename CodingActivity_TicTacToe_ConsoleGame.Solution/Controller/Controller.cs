﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingActivity_TicTacToe_ConsoleGame
{
    public class GameController
    {
        #region FIELDS
        //
        // track game and round status
        //
        private bool _playingGame;
        private bool _playingRound;

        private int _roundNumber;
        private int row, column;

        private Gameboard.PlayerPiece PlayerPiece;
        private Gameboard.GameboardState PlayerState;


        //
        // track the results of multiple rounds
        //
        private int _playerXNumberOfWins;
        private int _playerONumberOfWins;
        private int _numberOfCatsGames;


        //
        // instantiate  a Gameboard object
        // instantiate a GameView object and give it access to the Gameboard object
        //
        private static Gameboard _gameboard = new Gameboard();
        private static ConsoleView _gameView = new ConsoleView(_gameboard);

        #endregion

        #region PROPERTIES



        #endregion

        #region CONSTRUCTORS

        public GameController(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            InitializeGame();
            PlayGame(roundsPlayed, playerXWins, playerOWins, catsGames);
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Initialize the multi-round game.
        /// </summary>
        public void InitializeGame()
        {
            //
            // Initialize game variables
            //
            _playingGame = true;
            _playingRound = true;
            _roundNumber = 0;
            _playerONumberOfWins = 0;
            _playerXNumberOfWins = 0;
            _numberOfCatsGames = 0;

            //
            // Initialize game board status
            //
            _gameboard.InitializeGameboard();
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
                DisplayMainMenu(roundsPlayed, playerXWins, playerOWins, catsGames);
            }

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
        /// display the MainMenu screen
        /// </summary>
        public void DisplayMainMenu(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            Console.Clear();

            Console.CursorVisible = false;

            ConsoleUtil.HeaderText = "The Tic-Tac-Toe Game";
            ConsoleUtil.DisplayReset();


            Console.WriteLine(ConsoleUtil.Center("a. Play a New Round"));
            Console.WriteLine(ConsoleUtil.Center("b. View Rules"));
            Console.WriteLine(ConsoleUtil.Center("c. View Current Game Stats"));
            Console.WriteLine(ConsoleUtil.Center("d. View Historic Game Stats"));
            Console.WriteLine(ConsoleUtil.Center("e. Save Game Results"));
            Console.WriteLine(ConsoleUtil.Center("f. Quit"));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(ConsoleUtil.Center("Enter a letter to go to that screen"));

            Console.WriteLine();

            Console.CursorVisible = true;

            int numOfPlayerAttempts = 0;
            int maxNumOfPlayerAttempts = 3;
            bool validMenuItem = true;

            while (numOfPlayerAttempts <= maxNumOfPlayerAttempts && validMenuItem)
            {
                ConsoleKeyInfo response = Console.ReadKey(true);

                switch (response.KeyChar)
                {
                    case 'A':
                        _gameView.DisplayGameArea(roundsPlayed, playerXWins, playerOWins, catsGames);
                        break;
                    case 'B':
                        _gameView.DisplayRulesScreen(roundsPlayed, playerXWins, playerOWins, catsGames);
                        break;
                    case 'C':
                        _gameView.DisplayCurrentGameStatus(roundsPlayed, playerXWins, playerOWins, catsGames);
                        break;
                    case 'D':
                        break;
                    default:
                        Console.WriteLine("\t\t That was an invalid key, please try again!");
                        numOfPlayerAttempts++;
                        validMenuItem = false;
                        break;
                }
              

                else if (response.Key == ConsoleKey.D)
                {
                    _gameView.DisplayHistoricGameStatus(roundsPlayed, playerXWins, playerOWins, catsGames);
                }
                else if (response.Key == ConsoleKey.E)
                {
                    _gameView.DisplayGetYesNoSaveGamePrompt();
                }
                else if (response.Key == ConsoleKey.F)
                {
                    DisplayExitPrompt();
                }


            }


            DisplayExitPrompt();
        }

        /// <summary>
        /// prompts the user about who goes first
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
                    _gameView.DisplayGameArea(roundsPlayed, playerXWins, playerOWins, catsGames);
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

        /// <summary>
        /// displays the who goes first method
        /// </summary>
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
        /// Game Loop
        /// </summary>
        public void PlayGame(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            DisplayWelcomeScreen(roundsPlayed, playerXWins, playerOWins, catsGames);

            while (_playingGame)
            {
                //
                // Round loop
                //
                while (_playingRound)
                {
                    //
                    // Perform the task associated with the current game and round state
                    //
                    ManageGameStateTasks(roundsPlayed, playerXWins, playerOWins, catsGames);

                    //
                    // Evaluate and update the current game board state
                    //
                    _gameboard.UpdateGameboardState();

                    //
                    // insert try/catch/catch block here
                    //
                    try
                    {
                        _gameboard.SetPlayerPiece(new GameboardPosition(row, column), Gameboard.PlayerPiece.O);
                    }
                    catch (Controller.PositionChoiceOutOfRangeException ex)
                    {
                        Console.WriteLine("I think you tried an illegal move!");
                        Console.WriteLine(ex.Message);
                    }
                    catch (Controller.PoisitionChoiceAlreadyTakenException ex)
                    {

                        Console.WriteLine("That move is not allowed!");
                        Console.WriteLine(ex.Message);
                    }
                }

                //
                // Round Complete: Display the results
                //
                _gameView.DisplayCurrentGameStatus(_roundNumber, _playerXNumberOfWins, _playerONumberOfWins, _numberOfCatsGames);

                //
                // Confirm no major user errors
                //
                if (_gameView.CurrentViewState != ConsoleView.ViewState.PlayerUsedMaxAttempts ||
                    _gameView.CurrentViewState != ConsoleView.ViewState.PlayerTimedOut)
                {
                    //
                    // Prompt user to play another round
                    //
                    if (_gameView.DisplayNewRoundPrompt())
                    {
                        _gameboard.InitializeGameboard();
                        _gameView.InitializeView();
                        _playingRound = true;
                    }
                }
                //
                // Major user error recorded, end game
                //
                else
                {
                    _playingGame = false;
                }
            }

            _gameView.DisplayClosingScreen();
        }

        /// <summary>
        /// manage each new task based on the current game state
        /// </summary>
        private void ManageGameStateTasks(int roundsPlayed, int playerXWins, int playerOWins, int catsGames)
        {
            switch (_gameView.CurrentViewState)
            {
                case ConsoleView.ViewState.Active:
                    _gameView.DisplayGameArea(roundsPlayed, playerXWins, playerOWins, catsGames);

                    switch (_gameboard.CurrentRoundState)
                    {
                        case Gameboard.GameboardState.NewRound:
                            _roundNumber++;
                            _gameboard.CurrentRoundState = Gameboard.GameboardState.PlayerXTurn;
                            break;

                        case Gameboard.GameboardState.PlayerXTurn:
                            ManagePlayerTurn(Gameboard.PlayerPiece.X);
                            break;

                        case Gameboard.GameboardState.PlayerOTurn:
                            ManagePlayerTurn(Gameboard.PlayerPiece.O);
                            break;

                        case Gameboard.GameboardState.PlayerXWin:
                            _playerXNumberOfWins++;
                            _playingRound = false;
                            break;

                        case Gameboard.GameboardState.PlayerOWin:
                            _playerONumberOfWins++;
                            _playingRound = false;
                            break;

                        case Gameboard.GameboardState.CatsGame:
                            _numberOfCatsGames++;
                            _playingRound = false;
                            break;

                        default:
                            break;
                    }
                    break;
                case ConsoleView.ViewState.PlayerTimedOut:
                    _gameView.DisplayTimedOutScreen();
                    _playingRound = false;
                    break;
                case ConsoleView.ViewState.PlayerUsedMaxAttempts:
                    _gameView.DisplayMaxAttemptsReachedScreen();
                    _playingRound = false;
                    _playingGame = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Attempt to get a valid player move. 
        /// If the player chooses a location that is taken, the CurrentRoundState remains unchanged,
        /// the player is given a message indicating so, and the game loop is cycled to allow the player
        /// to make a new choice.
        /// </summary>
        /// <param name="currentPlayerPiece">identify as either the X or O player</param>
        private void ManagePlayerTurn(Gameboard.PlayerPiece currentPlayerPiece)
        {
            GameboardPosition gameboardPosition = _gameView.GetPlayerPositionChoice();

            if (_gameView.CurrentViewState != ConsoleView.ViewState.PlayerUsedMaxAttempts)
            {
                //
                // player chose an open position on the game board, add it to the game board
                //
                if (_gameboard.GameboardPositionAvailable(gameboardPosition))
                {
                    _gameboard.SetPlayerPiece(gameboardPosition, currentPlayerPiece);
                }
                //
                // player chose a taken position on the game board
                //
                else
                {
                    _gameView.DisplayGamePositionChoiceNotAvailableScreen();
                }
            }
        }

        /// <summary>       
        /// save player stats to data file
        /// </summary>
        /// <param name="playerData">string that is written to data file.</param>
        private static void SavePlayerHistory(string playerData)
        {
            try
            {
                //
                // use StreamWriter to add data to file
                //
                // StreamWriter sWriter = new StreamWriter(DataSettings.DataFilePath, true);

                // using (sWriter)
                // {
                //    sWriter.WriteLine(playerData);
                // }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// display a list of historic score
        /// </summary>
        /// <param name="historicScores"></param>
        private void DisplayHistoricScores(List<Model.HistoricScore> historicScores)
        {
            Console.WriteLine("List of Historical Scores");

            _gameView.DisplayContinuePrompt();
        }

        /// <summary>
        /// Read all historic player scores
        /// </summary>
        /// <returns>list of historic scores</returns>
        private static List<Model.HistoricScore> ReadPlayerHistory()
        {
            const char delineator = ','; // delineator in a CSV file

            List<Model.HistoricScore> historicScores = new List<Model.HistoricScore>();

            //
            // create lists to hold the historic score strings
            //
            List<string> historicScoresStringList = new List<string>();

            try
            {
                //
                // initialize a StreamReader object for reading from a file
                //

                //
                // read all data from the data file
                // note: using statement
                //

            }
            //
            // an I/O error was encountered
            //
            catch (Exception)
            {

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

                }
            }

            return historicScores;
        }

        #endregion
    }
}
