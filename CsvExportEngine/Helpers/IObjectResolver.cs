namespace CsvExportEngine.Helpers 
{ 
    using System;

    internal interface IObjectResolver
    {
        /// <summary>
        /// Returns an instance (resolves) the object of <see cref="Type"/> with the provided array of constructor arguments
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        object Resolve(Type type, params object[] constructorArgs);
    }
}
