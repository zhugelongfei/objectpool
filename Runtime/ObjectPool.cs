using System.Collections.Generic;

namespace Lonfee.ObjectPool
{
    /// <summary>
    /// Simple object pool
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    public class ObjectPool<T> where T : IPoolObject
    {
        /// <summary>
        /// factory for create object
        /// </summary>
        private IPoolObjectFactory<T> factory;

        /// <summary>
        /// per create count when need to create
        /// </summary>
        private int createCountPerCreation;

        /// <summary>
        /// max object count in pool
        /// </summary>
        private int maxObjectCount;

        /// <summary>
        /// pool
        /// <para>unused objects</para>
        /// </summary>
        private LinkedList<T> poolList;

        /// <summary>
        /// used set
        /// <para>current in using</para>
        /// </summary>
        private HashSet<T> usedCache;

        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="factory">Factory of object pool. It must not be null</param>
        /// <param name="initItemCount">Init count on pool create</param>
        /// <param name="createCountPerCreation">Create count per creation</param>
        /// <param name="maxItemCount">Maximum count of objects in the pool</param>
        public ObjectPool(IPoolObjectFactory<T> factory, int initItemCount = 1, int createCountPerCreation = 1, int maxItemCount = 10)
        {
            if (factory == null)
                throw new System.Exception("Factory of the object pool is null.");

            this.maxObjectCount = maxItemCount;
            this.createCountPerCreation = createCountPerCreation;
            this.factory = factory;
            poolList = new LinkedList<T>();
            usedCache = new HashSet<T>();

            // create item by init count
            if (initItemCount > 0)
                CreateObjectByCount(initItemCount);
        }

        /// <summary>
        /// Push object
        /// </summary>
        /// <param name="obj">Object</param>
        public void Push(T obj)
        {
            if (obj == null)
                return;

            // 1: remove from used collection
            if (usedCache.Contains(obj))
            {
                usedCache.Remove(obj);
            }

            // 2: push to pool
            if (poolList.Count >= maxObjectCount)
            {
                // oh, the pool is fully, destroy it.
                obj.OnDestroy();
            }
            else
            {
                // push it to pool
                obj.OnPush();
                poolList.AddFirst(obj);
            }
        }

        /// <summary>
        /// Push objects from collection
        /// </summary>
        /// <param name="objectList">Collection of object</param>
        public void Push(ICollection<T> objectList)
        {
            if (objectList == null)
                return;

            foreach (T item in objectList)
            {
                Push(item);
            }
        }

        /// <summary>
        /// Push all uesed objects
        /// </summary>
        public void PushAllUsedObject()
        {
            T[] itemArr = new T[usedCache.Count];
            int idx = 0;
            foreach (var item in usedCache)
            {
                itemArr[idx++] = item;
            }

            usedCache.Clear();

            Push(itemArr);
        }

        /// <summary>
        /// Pop object
        /// </summary>
        /// <returns>Free object</returns>
        public T Pop()
        {
            if (poolList.Count <= 0)
            {
                // create
                CreateObjectByCount(createCountPerCreation);
            }

            // pop
            T data = poolList.First.Value;
            poolList.RemoveFirst();
            data.OnPop();

            // add to usedcache
            usedCache.Add(data);
            return data;
        }

        /// <summary>
        /// Destroy pool
        /// </summary>
        public void Destroy()
        {
            // 1: clear all used objects
            PushAllUsedObject();

            // 2: destroy all objects
            LinkedListNode<T> curNode = poolList.First;
            while (curNode != null)
            {
                T item = curNode.Value;
                if (item == null)
                    break;

                item.OnDestroy();
                curNode.Value = default;
                curNode = curNode.Next;
            }

            // 3: clear pool
            poolList.Clear();

            // 4: destroy factory
            this.factory.OnDestroy();
        }

        /// <summary>
        /// Create object by count
        /// </summary>
        private void CreateObjectByCount(int count)
        {
            for (int i = 0; i < count; i++)
            {
                poolList.AddLast(factory.CreateObject());
            }
        }
    }
}