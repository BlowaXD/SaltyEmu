using System;
using System.Collections.Generic;

namespace ChickenAPI.Core.Utils
{
    public class Position<T> : IEquatable<Position<T>>
    {
        public Position(T x, T y)
        {
            X = x;
            Y = y;
        }

        public Position()
        {
        }

        public T X { get; set; }
        public T Y { get; set; }

        public bool Equals(Position<T> other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return EqualityComparer<T>.Default.Equals(X, other.X) && EqualityComparer<T>.Default.Equals(Y, other.Y);
        }

        public static bool operator ==(Position<T> obj1, Position<T> obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (obj1 is null)
            {
                return false;
            }

            if (obj2 is null)
            {
                return false;
            }

            return EqualityComparer<T>.Default.Equals(obj1.X, obj2.X) && EqualityComparer<T>.Default.Equals(obj1.Y, obj2.Y);
        }

        public static bool operator !=(Position<T> obj1, Position<T> obj2) => !(obj1 == obj2);

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == GetType() && Equals((Position<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return EqualityComparer<T>.Default.GetHashCode(X) * 397 ^ EqualityComparer<T>.Default.GetHashCode(Y);
            }
        }

        public override string ToString() => "(" + X + "," + Y + ")";
    }
}