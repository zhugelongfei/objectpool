namespace Lonfee.ObjectPool
{
    /// <summary>
    /// Create an object of object pool by default 
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    public class ObjectPool_DefaultFactory<T> : ObjectPool<T>
        where T : IPoolObject, new()
    {

        /// <summary>
        /// Create object by default ctor
        /// </summary>
        private class PoolItemDefaultCreate : IPoolObjectFactory<T>
        {
            /// <summary>
            /// New object
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
        public ObjectPool_DefaultFactory(int initCount = 1, int createPerCreation = 1, int maxCount = 10)
          : base(new PoolItemDefaultCreate(), initCount, createPerCreation, maxCount)
        {

        }
    }
}