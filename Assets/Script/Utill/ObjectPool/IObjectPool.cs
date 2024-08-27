using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.ObjectPool
{
    public interface IObjectPool<T>
    {
        T Get();
        void Release(T obj);
    }
}

