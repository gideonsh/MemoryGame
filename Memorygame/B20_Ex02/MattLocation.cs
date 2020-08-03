namespace B20_Ex02
{
    public class MattLocation
    {
        private int m_Row;
        private int m_Col;

        public MattLocation(int i_Row, int i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public int Row
        {
            get
            {
                return m_Row;
            }

            set
            {
                m_Row = value;
            }
        }

        public int Col
        {
            get
            {
                return m_Col;
            }

            set
            {
                m_Col = value;
            }
        }

        public static bool operator ==(MattLocation i_Cell1, MattLocation i_Cell2)
        {
            return i_Cell1.Row == i_Cell2.Row && i_Cell1.Col == i_Cell2.Col;
        }

        public static bool operator !=(MattLocation i_Cell1, MattLocation i_Cell2)
        {
            return i_Cell1.Row != i_Cell2.Row && i_Cell1.Col != i_Cell2.Col;
        }
    }
}
