using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class AIPlayer
    {
        private const string m_Name = "AI Player";
        private int m_Score;
        private List<MemoryCell> m_Memory;
        private List<MemoryCell> m_Moves;

        public AIPlayer() 
        {
            m_Score = 0;
            m_Memory = new List<MemoryCell>();
            m_Moves = new List<MemoryCell>();
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }
        }

        public List<MemoryCell> Memory
        {
            get
            {
                return m_Memory;
            }
        }

        public List<MemoryCell> Moves
        {
            get
            {
                return m_Moves;
            }
        }

        public bool IsNoMove()
        {
            bool result = false;
            if (m_Moves.Count == 0)
            {
                result = true;
            }

            return result;
        }

        public void AddScore()
        {
            m_Score++;
        }

        public bool IsCellInMemory(MattLocation i_Location)
        {
            bool result = false;
            foreach (var cell in m_Memory)
            {
                if (i_Location == cell.Location)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool IsCellInMoves(MattLocation i_Location)
        {
            bool result = false;
            foreach (var moveCell in m_Moves)
            {
                if (i_Location == moveCell.Location)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public void AddNewMove(MemoryCell i_Move1, MemoryCell i_Move2) //// add the two chooses to the moves of AI
        {
            m_Moves.Add(i_Move1);
            m_Moves.Add(i_Move2);
        }

        public void GetMove(out MattLocation io_Pick)
        {
            io_Pick = null;
            if (m_Moves.Count != 0)
            {
                io_Pick = m_Moves[0].Location;
                m_Moves.RemoveAt(0);
            }
        }

        public void Reset()
        {
            if (m_Memory != default && m_Memory.Count != 0)
            {
                m_Memory.Clear();
            }

            if (m_Moves != default && m_Moves.Count != 0)
            {
                m_Moves.Clear();
            }

            m_Score = 0;
        }
    }
}
