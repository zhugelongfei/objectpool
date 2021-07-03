namespace Pool
{
    /// <summary>
    /// Factory interface for object pool
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    public interface IPoolObjectFactory<T> where T : IPoolObject
    {
        /// <summary>
        /// Create object
        /// </summary>
        T CreateObject();

        /// <summary>
        /// Called when the pool is destroyed
        /// </summary>
        void OnDestroy();
    }
}