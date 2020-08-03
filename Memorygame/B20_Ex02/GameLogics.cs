using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class GameLogics
    {
        public static ePlayer s_Opponent;

        private Player m_Player1;
        private Player m_Player2;
        private AIPlayer m_PlayerAI;
        private GameBoard m_GameBoard;
        private int m_TurnNumber;

        public enum ePlayer
        {
            Player1,
            Player2,
            PlayerAI,
        }

        public GameLogics(string i_Player1Name, string i_Player2Name)
        {
            m_Player1 = new Player(i_Player1Name);
            if (i_Player2Name != string.Empty)
            {
                m_Player2 = new Player(i_Player2Name);
                m_PlayerAI = null;
                s_Opponent = ePlayer.Player2;
            }
            else
            {
                m_PlayerAI = new AIPlayer();
                m_Player2 = null;
                s_Opponent = ePlayer.PlayerAI;
            }

            m_TurnNumber = 1;
        }

        public static bool IsPlayerNameValid(string i_Name, out string io_ErrorMsg)
        {
            io_ErrorMsg = string.Empty;
            bool result = true;
            if (i_Name == string.Empty || i_Name.StartsWith(" ") == true)
            {
                result = false;
                io_ErrorMsg = "- - - - Player name is not valid- - - - \n";
            }

            return result;
        }

        public static bool IsNumeric(char i_Input, out string io_ErrorMsg)
        {
            io_ErrorMsg = string.Empty;
            bool result = true;
            if (char.IsDigit(i_Input) == false)
            {
                result = false;
                io_ErrorMsg = "- - - - Row input not numeric - - - - \n";
            }

            return result;
        }

        public static bool IsChoiseValid(string i_Choise, out string io_ErrorMsg)
        {
            bool result = true;
            io_ErrorMsg = string.Empty;
            if (i_Choise != "2" && i_Choise != "1")
            {
                io_ErrorMsg = "- - - - Invalid choise - - - - \n";
                result = false;
            }

            return result;
        }

        public static void SetOpponentType(int i_Type)
        {
            s_Opponent = (ePlayer)i_Type;
        }

        public static bool IsSizeBoardCorrect(string i_Input, out string io_ErrorMsg)
        {
            bool result = true;
            if (i_Input.Length != 1 || i_Input == string.Empty)
            {
                result = false;
                io_ErrorMsg = "- - - - Wrong input - - - - \n";
            }
            else if (IsNumeric(i_Input[0], out io_ErrorMsg) == false)
            {
                result = false;
            }
            else if (int.Parse(i_Input) > 6 || int.Parse(i_Input) < 4)
            {
                result = false;
                io_ErrorMsg = "- - - - Input is not in valid range - - - - \n";
            }

            return result;
        }

        public static bool IsBoardSizesValid(int i_Rows, int i_Cols, out string io_ErrorMsg)
        {
            bool result = true;
            io_ErrorMsg = string.Empty;
            if (i_Rows < 4 || i_Rows > 6 || i_Cols < 4 || i_Cols > 6)
            {
                io_ErrorMsg = "- - - - Values not in range - - - -\n";
                result = false;
            }
            else if (((i_Rows * i_Cols) % 2) == 1)
            {
                io_ErrorMsg = "- - - - Odd amount of cells - - - -\n";
                result = false;
            }

            return result;
        }

        public void SetNewBoard(int i_Rows, int i_Cols)
        {
            m_GameBoard = new GameBoard(i_Rows, i_Cols);
        }

        public GameBoard Gameboard
        {
            get
            {
                return m_GameBoard;
            }
        }

        public ePlayer GetPlayerTurn
        {
            get
            {
                ePlayer playerType;
                if(m_TurnNumber % 2 == 1)
                {
                    playerType = ePlayer.Player1;
                }
                else
                {
                    playerType = s_Opponent;
                }

                return playerType;
            }
        }

        public Player FirstPlayer
        {
            get
            {
                return m_Player1;
            }
        }

        public Player SecondPlayer
        {
            get
            {
                return m_Player2;
            }
        }

        public AIPlayer ComputerPlayer
        {
            get
            {
                return m_PlayerAI;
            }
        }

        public ePlayer Opponent
        {
            get
            {
                return s_Opponent;
            }
        }

        public bool IsGameOver()
        {
            return m_GameBoard.CouplesLeft == 0;
        }

        private bool isPairFound(MattLocation i_Location1, MattLocation i_Location2)
        {
            bool result = false;
            if (m_GameBoard.Board[i_Location1.Row, i_Location1.Col] == m_GameBoard.Board[i_Location2.Row, i_Location2.Col])
            {
                result = true;
            }

            return result;
        }

        public bool IsCellValid(int i_Rows, int i_Cols, out string io_ErrorMsg)
        {
            bool result = true;
            io_ErrorMsg = string.Empty;
            if(m_GameBoard.Board[i_Rows, i_Cols].Visible == true)
            {
                result = false;
                io_ErrorMsg = "- - - - Card has been picked already - - - - \n";
            }

            return result;
        }

        public bool IsPlayerHumanTurn()
        {
            return GetPlayerTurn != ePlayer.PlayerAI;
        }

        public bool IsValidColumn(string i_Input, out string io_ErrorMsg)
        {
            io_ErrorMsg = string.Empty;
            bool result = true;
            i_Input = i_Input.ToUpper();

            checkIfToExitGame(i_Input);

            if (i_Input.Length != 2 || Convert.ToInt32(i_Input[0]) - 65 > m_GameBoard.Cols || Convert.ToInt32(i_Input[0]) - 65 < 0) 
            {
                result = false;
                io_ErrorMsg = "- - - - Column does not exsit - - - - \n";
            }
            
            return result;
        }

        public bool IsValidRow(string i_Input, out string io_ErrorMsg)
        {
            io_ErrorMsg = string.Empty;
            bool result = true;

            checkIfToExitGame(i_Input);

            if (IsNumeric(i_Input[1], out io_ErrorMsg) == false)
            {
                result = false;
                io_ErrorMsg = "- - - - Input not numeric - - - - \n";
            }
            else if(Convert.ToInt32(i_Input[1] - '0') > m_GameBoard.Rows || Convert.ToInt32(i_Input[1] - '0') - 1 < 0)
            {
                result = false;
                io_ErrorMsg = "- - - - Row does not exsit - - - - \n";
            }

            return result;
        }

        public bool CheckIfContinue(string i_Input, out string io_ErrorMsg)
        {
            io_ErrorMsg = string.Empty;
            bool result;
            switch(i_Input.ToLower())
            {
                case "yes":
                    result = true;
                    break;

                case "no":
                    result = false;
                    break;

                default:
                    io_ErrorMsg = "- - - - Wrong input - - - - \n";
                    result = false;
                    break;
            }

            return result;
        }

        public void OpenCard(MattLocation i_Location)
        {
            m_GameBoard.Board[i_Location.Row, i_Location.Col].Show();
            Ex02.ConsoleUtils.Screen.Clear();
        }

        private void closeCards(MattLocation i_Location1, MattLocation i_Location2)
        {
            m_GameBoard.Board[i_Location1.Row, i_Location1.Col].Hide();
            m_GameBoard.Board[i_Location2.Row, i_Location2.Col].Hide();
            if (s_Opponent == ePlayer.PlayerAI)
            {
                saveToMemory(i_Location1, m_GameBoard.Board[i_Location1.Row, i_Location1.Col].Key, i_Location2, m_GameBoard.Board[i_Location2.Row, i_Location2.Col].Key);
            }
        }

        public void PlayTurn(MattLocation i_Pick1, MattLocation i_Pick2)
        {
            if (isPairFound(i_Pick1, i_Pick2) == false)
            {
                System.Threading.Thread.Sleep(2000);
                closeCards(i_Pick1, i_Pick2);
                m_TurnNumber++;
            }
            else
            {
                if(s_Opponent == ePlayer.PlayerAI)
                {
                    System.Threading.Thread.Sleep(2000);
                    deleteFromMemoryAndMovesOfPlayerAI(i_Pick1, i_Pick2);
                }

                m_GameBoard.CouplesLeft--;
                addScore(GetPlayerTurn);
            }
        }

        private void deleteFromMemoryAndMovesOfPlayerAI(MattLocation i_Pick1, MattLocation i_Pick2)
        {
            if (m_PlayerAI.IsCellInMemory(i_Pick1) == true)
            {
                removeFromPlayerAIList(i_Pick1, m_PlayerAI.Memory);
            }

            if (m_PlayerAI.IsCellInMemory(i_Pick2) == true)
            {
                removeFromPlayerAIList(i_Pick2, m_PlayerAI.Memory);
            }

            if(m_PlayerAI.IsCellInMoves(i_Pick1) == true)
            {
                removeFromPlayerAIList(i_Pick1, m_PlayerAI.Moves);
            }

            if (m_PlayerAI.IsCellInMoves(i_Pick2) == true)
            {
                removeFromPlayerAIList(i_Pick2, m_PlayerAI.Moves);
            }
        }

        private void saveMemoryOrAddMove(MattLocation i_Location, int i_CardKey)
        {
            MemoryCell matchMemoryCell;
            if (m_PlayerAI.IsCellInMemory(i_Location) == false)
            {
                MemoryCell temp = new MemoryCell(i_Location, i_CardKey);
                if (isFoundMatchCell(temp.CardKey, out matchMemoryCell) == false)
                {
                    m_PlayerAI.Memory.Add(temp);
                }
                else
                {
                    m_PlayerAI.Moves.Add(temp);
                    m_PlayerAI.Moves.Add(matchMemoryCell);
                    m_PlayerAI.Memory.Remove(matchMemoryCell);
                }
            }
        }

        private void saveToMemory(MattLocation i_Location1, int i_CardKey1, MattLocation i_Location2, int i_CardKey2)
        {
            saveMemoryOrAddMove(i_Location1, i_CardKey1);
            saveMemoryOrAddMove(i_Location2, i_CardKey2);
        }

        private void removeFromPlayerAIList(MattLocation i_Location, List<MemoryCell> i_List)
        {
            int cardKey = m_GameBoard.Board[i_Location.Row, i_Location.Col].Key;
            foreach(var memoryCell in i_List)
            {
                if(memoryCell.Location == i_Location && memoryCell.CardKey == cardKey)
                {
                    i_List.Remove(memoryCell);
                    break;
                }
            }
        }

        private bool isFoundMatchCell(int i_FirstCellCardKey, out MemoryCell i_MatchMemoryCell)
        {
            bool result = false;
            i_MatchMemoryCell = null;

            foreach (var cellMemory in m_PlayerAI.Memory)
            {
                if (cellMemory.CardKey == i_FirstCellCardKey)
                {
                    result = true;
                    i_MatchMemoryCell = cellMemory;
                    break;
                }
            }

            return result;
        }

        public void GetPicksForPlayerAI(out MattLocation io_Pick1, out MattLocation io_Pick2)
        {
            MemoryCell matchMemoryCell;

            if (m_PlayerAI.IsNoMove() == false)
            {
                m_PlayerAI.GetMove(out io_Pick1);
                OpenCard(io_Pick1);
                m_PlayerAI.GetMove(out io_Pick2);
            }
            else
            {
                generateRandomPick(out io_Pick1);
                OpenCard(io_Pick1);
                int firstPickCardKey = m_GameBoard.Board[io_Pick1.Row, io_Pick1.Col].Key;
                if (isFoundMatchCell(firstPickCardKey, out matchMemoryCell) == false)
                {
                    m_PlayerAI.Memory.Add(new MemoryCell(io_Pick1, firstPickCardKey));
                    generateRandomPick(out io_Pick2);
                }
                else
                {
                    io_Pick2 = matchMemoryCell.Location;
                }
            }
        }

        private void generateRandomPick(out MattLocation io_Pick)
        {
            int randomIndexInPossibleMoves;
            List<MattLocation> possibleMoves = new List<MattLocation>();
            Random random = new Random();
            bool stop;

            foreach (var cell in m_GameBoard.Board)
            {
                if(cell.Visible == false)
                {
                    possibleMoves.Add(cell.Location);
                }
            }

            do
            {
                randomIndexInPossibleMoves = random.Next(possibleMoves.Count);
                if(m_PlayerAI.IsCellInMemory(possibleMoves[randomIndexInPossibleMoves]) == true)
                {
                    possibleMoves.RemoveAt(randomIndexInPossibleMoves);
                    stop = false;
                }
                else
                {
                    stop = true;
                }
            } 
            while (stop == false);

            io_Pick = possibleMoves[randomIndexInPossibleMoves];
        }

        private void addScore(ePlayer i_PlayerType)
        {
            switch (i_PlayerType)
            {
                case ePlayer.Player1:
                    m_Player1.AddScore();
                    break;

                case ePlayer.Player2:
                        m_Player2.AddScore();
                    break;

                case ePlayer.PlayerAI:
                        m_PlayerAI.AddScore();
                    break;
            }
        }

        public void ResetGame()
        {
            m_Player1.Reset();
            if (s_Opponent == ePlayer.Player2)
            {
                m_Player2.Reset();
            }
            else
            {
                m_PlayerAI.Reset();
            }
        }

        private void checkIfToExitGame(string i_Input)
        {
            i_Input = i_Input.ToLower();
            if(i_Input == "q" || i_Input == "quit")
            {
                ExitGame();
            }
        }

        public void ExitGame()
        {
            Environment.Exit(1);
        }
    }
}
