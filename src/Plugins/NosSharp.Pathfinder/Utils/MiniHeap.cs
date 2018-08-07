using System;

namespace NosSharp.Pathfinder.Utils
{
    public class MiniHeap<T> where T : IComparable
    {
        private T[] _array;
        private int _size;
        private int _capacity = 16;

        public MiniHeap()
        {
            _array = new T[_capacity];
            _size = 0;
        }

        public void Add(T ele)
        {
            if (_size + 1 == Count())
            {
                EnsureCapacity();
            }
            int min = 0,
                max = _size;
            int index = -1;
            while (index == -1)
            {
                int half = (int)Math.Floor((min + max) / 2D);
                int r = ele.CompareTo(_array[half]);
                if (r == 0 || max - min == 1)
                {
                    index = max;
                }
                else if (r < 0) { max = half; }
                else if (r > 0) { min = half; }
            }
            
            _size++;
            T tmp = _array[index];
            _array[index] = ele;
            int n = index + 1;
            while (n < _size)
            {
                T tmp2 = _array[n];
                _array[n++] = tmp;
                tmp = tmp2;
            }
            
        }

        public T Pop()
        {
            T tmp = _array[0];
            _size--;
            int n = 0;
            while (n < _size)
            {
                _array[n++] = _array[n];
            }
            return tmp;
        }

        public int Count() => _array.Length;

        private void EnsureCapacity()
        {
            T[] tmp = new T[_capacity*=2];
            Array.Copy(_array, 0, tmp, 0, _size);
            _array = tmp;
        }
    }
}