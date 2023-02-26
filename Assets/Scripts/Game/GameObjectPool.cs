#region

using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

#endregion

namespace Game
{
    /**
     * Simple Object Pool for Generic MonoBehaviour
     */
    public class GameObjectPool<T> where T : MonoBehaviour
    {
        public Transform PoolParent;
        
        private ObjectPool<T> _pool;
        private T _prefab;
        
        /**
         * the parent of pool object
         */
        private Transform _parent;

        /**
         * Create a GameObjectPool that pools gameObject of the given
         * prefab and set it to the given parent
         */
        public GameObjectPool(T prefab, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            _pool = new ObjectPool<T>(
                CreatT,
                PoolT,
                ReleaseT
            );
        }
        
        /**
         * Get object from pool with out initialization or set to parent
         */
        public T GetRaw() => _pool.Get();
        
        /**
         * Get object from pool and initialize with the given function
         * and set the object to parent
         */
        public T Get(Action<T> initialize)
        {
            T newObject = _pool.Get();
            initialize.Invoke(newObject);
            newObject.transform.SetParent(_parent, false);
            return newObject;
        }
        
        /**
         * Return object to the pool
         */
        public void Release(T item) => _pool.Release(item);

        private T CreatT() => 
            Object.Instantiate(_prefab, _parent).GetComponent<T>();
        
        private void PoolT(T item) =>
            item.gameObject.SetActive(true);

        private void ReleaseT(T item) => 
            item.gameObject.SetActive(false);
    }
}