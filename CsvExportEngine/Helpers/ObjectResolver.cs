namespace CsvExportEngine.Helpers
{
    using System;

    internal class ObjectResolver : IObjectResolver
    {
        private static readonly object locker = new object();
        private static IObjectResolver current = new ObjectResolver();

        public static IObjectResolver Current
        {
            get
            {
                lock (locker)
                {
                    return current;
                }
            }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("IObjectResolver cannot be null.");
                }

                lock (locker)
                {
                    current = value;
                }
            }
        }

        public ObjectResolver()
        {
        }

        /// <summary>
        /// Resolves (instantiates) an object of the given <see cref="Type"/> and provided constructor arguments
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        public object Resolve(Type type, params object[] constructorArgs)
        {
            return ReflectionHelper.CreateInstanceWithoutContractResolver(type, constructorArgs);
        }

        /// <summary>
        /// Resolves (instantiates) an object of the type <typeparamref name="T"/> with the provided constructor arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        public T Resolve<T>(params object[] constructorArgs)
        {
            return (T)Resolve(typeof(T), constructorArgs);
        }
    }
}