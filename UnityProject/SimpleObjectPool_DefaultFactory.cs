namespace Pool
{
    /// <summary>
    /// Create an object of object pool by default 
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    public class SimpleObjectPool_DefaultFactory<T> : SimpleObjectPool<T>
    where T : IPoolObject, new()
    {

        /// <summary>
        /// 使用空参构造函数创建池内对象的类
        /// </summary>
        private class PoolItemDefaultCreate : IPoolObjectFactory<T>
        {
            /// <summary>
            /// 用空参构造函数创建一个池
            /// </summary>
            public T CreateObject()
            {
                return new T();
            }

            public void OnDestroy()
            {

            }
        }

        /// <summary>
        /// Default
        /// </summary>
        public SimpleObjectPool_DefaultFactory()
          : base(new PoolItemDefaultCreate())
        {

        }

        /// <summary>
        /// Default with max object count
        /// </summary>
        /// <param name="maxObjectCount">max object count</param>
        public SimpleObjectPool_DefaultFactory(int maxObjectCount)
          : base(new PoolItemDefaultCreate(), maxObjectCount)
        {

        }


        /// <summary>
        /// Default with max object count and create count of per creation
        /// </summary>
        /// <param name="maxObjectCount">max object count</param>
        /// <param name="createCountPerCreation">create count of per creation</param>
        public SimpleObjectPool_DefaultFactory(int maxObjectCount, int createCountPerCreation)
          : base(new PoolItemDefaultCreate(), maxObjectCount, createCountPerCreation)
        {

        }
    }
}