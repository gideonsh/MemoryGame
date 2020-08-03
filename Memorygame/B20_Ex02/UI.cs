using System;
using System.Globalization;
using System.Text;

namespace B20_Ex02
{
    public class UI
    {
        private GameLogics m_Logic;

        public void StartMemoryGame()
        {
            string firstPlayerName = getPlayerName();
            Ex02.ConsoleUtils.Screen.Clear();
            string secondPlayerName = string.Empty;

            chooseAndSetOpponent();
            Ex02.ConsoleUtils.Screen.Clear();
            if (GameLogics.s_Opponent == GameLogics.ePlayer.Player2)
            {
                secondPlayerName = chooseOpponentName();
                Ex02.ConsoleUtils.Screen.Clear();
            }

            m_Logic = new GameLogics(firstPlayerName, secondPlayerName);
            playGame();
        }

        private void playGame()
        {
            bool toContinue;
            int rows, columns;
            do
            {
                getBoardSize(out rows, out columns);
                m_Logic.SetNewBoard(rows, columns);
                Ex02.ConsoleUtils.Screen.Clear();
                gameRunning();
                printEndGameScreen();
                askToKeepOnPlaying(out toContinue);
                if (toContinue == true)
                {
                    m_Logic.ResetGame();
                }

                Ex02.ConsoleUtils.Screen.Clear();
            }
            while (toContinue == true);

            m_Logic.ExitGame();
        }

        private void gameRunning()
        {
            MattLocation pick1, pick2;

            while (m_Logic.IsGameOver() == false)
            {
                Ex02.ConsoleUtils.Screen.Clear();
                if (m_Logic.IsPlayerHumanTurn())
                {
                    pick1 = pickCard();
                    pick2 = pickCard();
                }
                else
                {
                    m_Logic.GetPicksForPlayerAI(out pick1, out pick2);
                    printCurrentSituation(pick1);
                    System.Threading.Thread.Sleep(3000);
                    m_Logic.OpenCard(pick2);
                }

                printCurrentSituation(pick2);
                m_Logic.PlayTurn(pick1, pick2);
            }
        }

        private void printCurrentSituation(MattLocation i_Pick)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            printBoard();
            Console.WriteLine(playerTurnInfo());
            if (m_Logic.GetPlayerTurn == GameLogics.ePlayer.PlayerAI)
            {
                Console.WriteLine("\n * Player AI picked : {0}{1} * ", Convert.ToChar(i_Pick.Col + 65), i_Pick.Row + 1);
            }
        }

        private void askToKeepOnPlaying(out bool io_ToContinue)
        {
            string errorMsg;
            do
            {
                Console.WriteLine("Would you like to play again?");
                string input = Console.ReadLine();
                io_ToContinue = m_Logic.CheckIfContinue(input, out errorMsg);
                if (errorMsg != string.Empty)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    Console.WriteLine(errorMsg);
                }
            }
            while (errorMsg != string.Empty);
        }

        private string getPlayerName()
        {
            string errorMsg;
            bool result;
            string name;

            do
            {
                Console.WriteLine("Please enter first player name: ");
                name = Console.ReadLine();
                result = GameLogics.IsPlayerNameValid(name, out errorMsg);
                if (result == false)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    Console.WriteLine(errorMsg);
                }
            }
            while (result == false);

            return name;
        }

        private void chooseAndSetOpponent()
        {
            string errorMsg;
            bool result;
            string choiseString;
            do
            {
                Console.WriteLine("Choose your opponent: (1) for second player (2) for AI player");
                choiseString = Console.ReadLine();
                result = GameLogics.IsChoiseValid(choiseString, out errorMsg);
                if (result == false)
                {
                    printErrorMsg(errorMsg);
                }
            }
            while (result == false);

            GameLogics.SetOpponentType(int.Parse(choiseString));
        }

        private void printErrorMsg(string i_ErrorMsg)
        {
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(i_ErrorMsg);
        }

        private string chooseOpponentName()
        {
            string name;

            Console.WriteLine("Please enter second player name:");
            name = Console.ReadLine();

            return name;
        }

        private void getBoardSize(out int io_Rows, out int io_Columns)
        {
            string errorMsg;
            bool result;
            string rows;
            string columns = string.Empty;

            do
            {
                Console.WriteLine("Please enter board rows size and then columns size:");
                rows = Console.ReadLine();
                result = GameLogics.IsSizeBoardCorrect(rows, out errorMsg);

                if (result == true)
                {
                    columns = Console.ReadLine();
                    result = GameLogics.IsSizeBoardCorrect(columns, out errorMsg);
                    if (result == true)
                    {
                        result = GameLogics.IsBoardSizesValid(int.Parse(rows), int.Parse(columns), out errorMsg);
                    }
                }

                if (result == false)
                {
                    printErrorMsg(errorMsg);
                }
            }
            while (result == false);

            io_Rows = int.Parse(rows);
            io_Columns = int.Parse(columns);
        }

        private MattLocation pickCard()
        {
            string errorMsg;
            string cardLocation;
            MattLocation location = null;
            bool result;
            do
            {
                printBoard();
                Console.WriteLine(playerTurnInfo());
                Console.WriteLine("Please enter column character and the row number to open a card:");
                cardLocation = Console.ReadLine();
                if (m_Logic.IsValidColumn(cardLocation, out errorMsg) != false)
                {
                    if (m_Logic.IsValidRow(cardLocation, out errorMsg) != false)
                    {
                        cardLocation = cardLocation.ToUpper();
                        if (m_Logic.IsCellValid(Convert.ToInt32(cardLocation[1] - '0') - 1, Convert.ToInt32(cardLocation[0] - 'A'), out errorMsg) != false)
                        { 
                            location = new MattLocation(Convert.ToInt32(cardLocation[1] - '0') - 1, Convert.ToInt32(cardLocation[0] - 'A'));
                            m_Logic.OpenCard(location);
                        }
                    }
                }

                if (errorMsg != string.Empty)
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    result = false;
                    Console.WriteLine(errorMsg);
                }
                else
                {
                    result = true;
                }
            }
            while (result == false);

            return location;
        }

        private string playerTurnInfo()
        {
            string playerInfoString = "Current turn: ";
            switch (m_Logic.GetPlayerTurn)
            {
                case GameLogics.ePlayer.Player1:
                    playerInfoString += playerInfo(GameLogics.ePlayer.Player1);
                    break;

                case GameLogics.ePlayer.Player2:
                    playerInfoString += playerInfo(GameLogics.ePlayer.Player2);
                    break;

                case GameLogics.ePlayer.PlayerAI:
                    playerInfoString += playerInfo(GameLogics.ePlayer.PlayerAI);
                    break;
            }

            return playerInfoString;
        }

        private void printEndGameScreen()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine(buildEndGameScreenPrint());
        }

        private StringBuilder buildEndGameScreenPrint()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string opponentBody;
            string headline = @"Game ended!
The results are:";
            string body = string.Format("First player  {0}  scored: {1}\n", m_Logic.FirstPlayer.Name, m_Logic.FirstPlayer.Score);

            if (m_Logic.Opponent == GameLogics.ePlayer.Player2)
            {
                opponentBody = string.Format("Second player  {0}  scored: {1}", m_Logic.SecondPlayer.Name, m_Logic.SecondPlayer.Score);
            }
            else
            {
                opponentBody = string.Format("Second player  {0}  scored: {1}", m_Logic.ComputerPlayer.Name, m_Logic.ComputerPlayer.Score);
            }

            body += opponentBody;

            stringBuilder.AppendLine(headline);
            stringBuilder.AppendLine(body);
            stringBuilder.AppendLine(buildWinnerFormat());

            return stringBuilder;
        }

        private string buildWinnerFormat()
        {
            string winnerFormat;
            string opponentName = string.Empty;
            int opponentScore = 0;

            switch (m_Logic.Opponent)
            {
                case GameLogics.ePlayer.Player2:
                    opponentName = m_Logic.SecondPlayer.Name;
                    opponentScore = m_Logic.SecondPlayer.Score;
                    break;

                case GameLogics.ePlayer.PlayerAI:
                    opponentName = m_Logic.ComputerPlayer.Name;
                    opponentScore = m_Logic.ComputerPlayer.Score;
                    break;
            }

            if (m_Logic.FirstPlayer.Score > opponentScore)
            {
                winnerFormat = string.Format("{0} has won the game! Congratulations!", m_Logic.FirstPlayer.Name);
            }
            else if (m_Logic.FirstPlayer.Score < opponentScore)
            {
                winnerFormat = string.Format("{0} has won the game! Congratulations!", opponentName);
            }
            else
            {
                winnerFormat = "No one won! It's a TIE.";
            }

            return winnerFormat;
        }

        private string playerInfo(GameLogics.ePlayer i_PlayerType)
        {
            string info = string.Empty;

            switch (i_PlayerType)
            {
                case GameLogics.ePlayer.Player1:
                    info = m_Logic.FirstPlayer.Name + " with current score: " + m_Logic.FirstPlayer.Score;
                    break;

                case GameLogics.ePlayer.Player2:
                    info = m_Logic.SecondPlayer.Name + " with current score: " + m_Logic.SecondPlayer.Score;
                    break;

                case GameLogics.ePlayer.PlayerAI:
                    info = m_Logic.ComputerPlayer.Name + " with current score: " + m_Logic.ComputerPlayer.Score;
                    break;
            }

            return info;
        }

        private void printBoard()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(firstLineCols('A').ToString());
            stringBuilder.AppendLine(lineSeperate('=').ToString());
            for (int row = 0; row < m_Logic.Gameboard.Rows; row++)
            {
                stringBuilder.Append((row + 1).ToString() + " |");
                for (int col = 0; col < m_Logic.Gameboard.Cols; col++)
                {
                    if (m_Logic.Gameboard.Board[row, col].Visible == true)
                    {
                        stringBuilder.Append(" " + m_Logic.Gameboard.Board[row, col].Data + " ");
                    }
                    else
                    {
                        stringBuilder.Append(' ', 3);
                    }

                    stringBuilder.Append("|");
                }

                stringBuilder.Append(Environment.NewLine);
                stringBuilder.AppendLine(lineSeperate('=').ToString());
            }

            Console.WriteLine(stringBuilder);
        }

        private StringBuilder lineSeperate(char i_SeprateCharacter)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("  ");
            for (int i = 0; i < m_Logic.Gameboard.Cols; i++)
            {
                stringBuilder.Append(i_SeprateCharacter, 4);
            }

            stringBuilder.Append(i_SeprateCharacter);

            return stringBuilder;
        }

        private StringBuilder firstLineCols(char i_ColoumsChar)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("   ");
            for (int i = 0; i < m_Logic.Gameboard.Cols; i++)
            {
                stringBuilder.Append(" " + i_ColoumsChar + " ");
                stringBuilder.Append(" ");
                i_ColoumsChar++;
            }

            return stringBuilder;
        }
    }
}
