using System;
using System.Collections.Generic;
using System.Text;

namespace B20_Ex02
{
    public class MemoryCell
    {
        private MattLocation m_Location;
        private int m_CardKey;

        public MemoryCell(MattLocation i_Location, int i_Cardkey)
        {
            m_Location = i_Location;
            m_CardKey = i_Cardkey;
        }

        public MattLocation Location
        {
            get
            {
                return m_Location;
            }
        }

        public int CardKey
        {
            get
            {
                return m_CardKey;
            }
        }
    }
}
