using System;

namespace B20_Ex02
{
    public class BoardCell<T>
    {
        private readonly MattLocation m_Location;
        private T m_Data;
        private int m_Key;
        private bool m_Visible;

        public BoardCell(T i_Data, MattLocation i_Location, int i_Key)
        {
            m_Data = i_Data;
            m_Location = i_Location;
            m_Key = i_Key;
        }

        public static bool operator ==(BoardCell<T> cell1, BoardCell<T> cell2)
        {
            return cell1.Key == cell2.Key;
        }

        public static bool operator !=(BoardCell<T> cell1, BoardCell<T> cell2)
        {
            return cell1.Key != cell2.Key;
        }

        public bool Visible
        {
            get
            {
                return m_Visible;
            }
        }

        public T Data
        {
            get
            {
                if(m_Visible == true)
                {
                    return m_Data;
                }
                else
                {
                    throw new Exception("Cell was not picked yet.");
                }
            }
        }

        public int Key
        {
            get
            {
                return m_Key;
            }

            set
            {
                m_Key = value;
            }
        }

        public MattLocation Location
        {
            get
            {
                return m_Location;
            }
        }

        public void Show()
        {
            m_Visible = true;
        }

        public void Hide()
        {
            m_Visible = false;
        }
    }
}
