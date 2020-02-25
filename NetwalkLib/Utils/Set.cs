using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NetwalkLib
{
    public class Set<T> : IEnumerable<T>
    {
        private readonly HashSet<T> _set;
        private readonly T _representative;
        
        public Set(T representative)
        {
            _set = new HashSet<T>();
            _set.Add(representative);
            _representative = representative;
        }
        
        public T Representative => _representative;

        public void Add(T newItem)
        {
            if (!_set.Add(newItem))
            {
                throw new InvalidOperationException("Set already contained that value.");
            }
        }
        
        public void Merge(Set<T> other)
        {
            foreach (var item in other._set)
            {
                if (!_set.Add(item))
                {
                    throw new InvalidOperationException("Attempted to merge non-unique item.");
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (obj is Set<T> typedObj)
            {
                return object.ReferenceEquals(typedObj.Representative, _representative);
            }
            
            return false;
        }

        public override int GetHashCode()
        {
            return _representative.GetHashCode();
        }

        public override string ToString()
        {
            return _representative.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}