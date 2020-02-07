using System.Collections;
using System.Collections.Generic;

namespace AsteroidsEngine
{
    public class ActiveList : IEnumerable<Entity>
    {
        #region IEnumerator

        private int _counter; 
        public IEnumerator<Entity> GetEnumerator()
        {
            for (var i = 0; i <=_lastActive; i++)
            {
                _counter = i;
                yield return _collection[_counter];
            }
            Compress();
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
            {
                SwapWithLastActive(_collection.Count - 1);
            }
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

        public void DeactivateCurrent()
        {
            Deactivate(_counter);
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