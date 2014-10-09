namespace JsonNet.Internal
{
    class ValueEnumerator : System.Collections.IEnumerator
    {
        private Value ivalue = null;
        public ValueEnumerator(Value v)
        {
            ivalue = v;

        }

        int position = -1;
        public void Reset()
        {
            position = -1;
        }

        public bool MoveNext()
        {
            position++;
            return position < ivalue.Count;
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }
        public Value Current
        {
            get { return ivalue[position]; }
        }
    }
}
