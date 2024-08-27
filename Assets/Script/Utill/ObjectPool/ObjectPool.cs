using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.ObjectPool
{
    public class ObjectPool<T> : IObjectPool<T> where T : Component
    {
        private readonly Stack<T> _objects = new Stack<T>();
        private readonly T _prefab;
        private readonly Transform _parent;

        public ObjectPool(T prefab, int initialCapacity, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;

            for (int i = 0; i < initialCapacity; i++)
            {
                T obj = Object.Instantiate(_prefab, _parent);
                obj.gameObject.SetActive(false);
                _objects.Push(obj);
            }
        }

        public T Get()
        {
            if (_objects.Count > 0)
            {
                T obj = _objects.Pop();
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                T obj = Object.Instantiate(_prefab, _parent);
                return obj;
            }
        }

        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
            _objects.Push(obj);
        }
    }
}