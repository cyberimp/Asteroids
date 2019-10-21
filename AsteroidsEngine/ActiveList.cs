using System;
using System.Collections;
using System.Collections.Generic;

namespace AsteroidsEngine
{
    public class ActiveList : IEnumerable<Entity>
    {
        #region IEnumerator
        
        private class ActiveEnumerator : IEnumerator<Entity>
        {
            private readonly List<Entity> _collection;
            private readonly int _lastActive;
            private readonly ActiveList _parent;

            public int Counter { get; private set; } = -1;

            public ActiveEnumerator(List<Entity> collection, int lastActive, ActiveList parent)
            {
                _collection = collection;
                _lastActive = lastActive;
                _parent = parent;
            }
            
            public bool MoveNext()
            {
                return (++Counter <= _lastActive);
            }

            public void Reset()
            {
                Counter = -1;
            }

            public Entity Current
            {
                get
                {
                    try
                    {
                        if (Counter > _lastActive)
                            throw new IndexOutOfRangeException(
                                $"Tried to traverse to inactive, {Counter} > {_lastActive}");
                        return _collection[Counter];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        throw new InvalidOperationException(e.Message);
                    }
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _parent.DoneEnumerating();
            }
        }

        private readonly List<ActiveEnumerator> _enumerators = new List<ActiveEnumerator>();

        private void DoneEnumerating()
        {
            var length = _enumerators.Count - 1;
            _enumerators.RemoveAt(length);
            if (0 >= length)
                Compress();
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            var enumerator = new ActiveEnumerator(_collection, _lastActive, this);
            _enumerators.Add(enumerator);
            return enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        #endregion
        
        private readonly List<Entity> _collection = new List<Entity>();
        private readonly List<int> _deadPool = new List<int>();
        private readonly List<Entity> _newPool = new List<Entity>();
        private int _lastActive = -1;
       
        private void Compress()
        {
            foreach (var dead in _deadPool)
            {
                DeactivateInternal(dead);
            }
            
            _deadPool.Clear();

            foreach (var newborn in _newPool)
            {
                AddInternal(newborn);
            }
            
            _newPool.Clear();
        }

        private void SwapWithLastActive(int index)
        {
            var temp = _collection[_lastActive];
            _collection[_lastActive] = _collection[index];
            _collection[index] = temp;
        }
        
        private void AddInternal(Entity entity)
        {
            _collection.Add(entity);
            ++_lastActive;
            if (_lastActive < _collection.Count - 1)
                SwapWithLastActive(_collection.Count - 1);
        }

        private void DeactivateInternal(int index)
        {
            _collection[index].Active = false;
            SwapWithLastActive(index);
            --_lastActive;
        }

        public void Add(Entity entity)
        {
            _newPool.Add(entity);
        }

        private void Deactivate(int index)
        {
            _deadPool.Add(index);
        }

        public void DeactivateCurrent(int deep = 0)
        {
            var length = _enumerators.Count - 1;
            if (deep > length)
                throw new IndexOutOfRangeException("tried to deactivate too deep");
            Deactivate(_enumerators[length - deep].Counter);
        }
        
        public void DeactivateAll()
        {
            foreach (var entity in _collection)
            {
                entity.Active = false;
            }

            _lastActive = -1;
        }

        public Entity GetLast()
        {
            if (_lastActive >= _collection.Count - 1)
                return null;
            
            _collection[++_lastActive].Active = true;
            return _collection[_lastActive];
        }
    }
}