using System.Collections.Generic;

namespace Pool
{
    /// <summary>
    /// Simple object pool
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    public class SimpleObjectPool<T> where T : IPoolObject
    {
        /// <summary>
        /// 创建池物体的工厂
        /// </summary>
        private IPoolObjectFactory<T> factory;

        /// <summary>
        /// 每次需要创建物体时，创建的数量
        /// </summary>
        private int createCountPerCreation;

        /// <summary>
        /// 空闲物体最大数量
        /// </summary>
        private int maxObjectCount;

        /// <summary>
        /// 池列表
        /// <para>空闲物体列表</para>
        /// </summary>
        private LinkedList<T> poolList;

        /// <summary>
        /// 使用列表
        /// <para>正在使用的物体列表</para>
        /// </summary>
        private HashSet<T> usedCache;

        /// <summary>
        /// Constructor function
        /// </summary>
        /// <param name="factory">Factory of object pool. It must not be null</param>
        /// <param name="maxItemCount">Maximum count of objects in the pool</param>
        /// <param name="createCountPerCreation">Create count per creation</param>
        public SimpleObjectPool(IPoolObjectFactory<T> factory, int maxItemCount = 10, int createCountPerCreation = 5)
        {
            if (factory == null)
                throw new System.Exception("Factory of the object pool is null.");

            this.maxObjectCount = maxItemCount;
            this.createCountPerCreation = createCountPerCreation;
            this.factory = factory;
            poolList = new LinkedList<T>();
            usedCache = new HashSet<T>();
        }

        /// <summary>
        /// Push object
        /// </summary>
        /// <param name="obj">Object</param>
        public void Push(T obj)
        {
            if (obj == null)
                return;

            //如果当前使用集合中包含此物体，那么从使用集合中移除此物体
            if (usedCache.Contains(obj))
            {
                usedCache.Remove(obj);
            }

            if (poolList.Count >= maxObjectCount)
            {
                //空闲物体已达到最大值，不放入池，直接销毁
                obj.OnDestroy();
            }
            else
            {
                //执行放入回调
                obj.OnPush();
                //放入池中
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

            //遍历集合，放入池中
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