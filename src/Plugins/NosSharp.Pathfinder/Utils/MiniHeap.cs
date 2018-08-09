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

        /// <summary>
        /// Add an element in the array and sort the array, increase the array capacity if it is full
        /// </summary>
        /// <param name="ele"></param>
        /// <param name="sort"></param>
        public void Add(T ele, bool sort = true)
        {
            if (_size + 1 == _capacity)
            {
                IncreaseCapacity();
            }
            if (!sort || _size == 0)
            {
                _array[_size++] = ele;
                return;
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

        /// <summary>
        /// Remove the first element of the array and returns it
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            T tmp = _array[0];
            _size--;
            int n = 0;
            while (n <= _size)
            {
                _array[n++] = _array[n];
            }
            return tmp;
        }

        /// <summary>
        /// Get the first element of the array which correspond to the condition given, return default(T) if there isn't any element
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Get(Func<T, bool> predicate)
        {
            for (int i = 0; i < _size; i++)
            {
                T item = _array[i];
                if (predicate(item)) return item;
            }
            return default(T);
        }
        /// <summary>
        /// Returns the number of element in the array
        /// </summary>
        /// <returns></returns>
        public int Count() => _size;

        /// <summary>
        /// Increase the array capacity
        /// </summary>
        private void IncreaseCapacity()
        {
            T[] tmp = new T[_capacity*=2];
            Array.Copy(_array, 0, tmp, 0, _size);
            _array = tmp;
        }
    }
}