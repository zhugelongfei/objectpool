namespace Pool
{

    /// <summary>
    /// Interface of object in the object pool
    /// </summary>
    public interface IPoolObject
    {
        /// <summary>
        /// On push
        /// </summary>
        void OnPush();

        /// <summary>
        /// On pop
        /// </summary>
        void OnPop();

        /// <summary>
        /// Free resources on destroy
        /// </summary>
        void OnDestroy();
    }
}