namespace CsvExportEngine.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    internal static class ReflectionHelper
    {
        private static readonly Dictionary<int, Dictionary<int, Delegate>> funcArgCache = new Dictionary<int, Dictionary<int, Delegate>>();
        private static readonly object locker = new object();

        /// <summary>
        /// Dynamically creates an object instance of the type <typeparamref name="T"/> with the given array of constructor arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static T CreateInstance<T>(params object[] args)
        {
            return (T)CreateInstance(typeof(T), args);
        }

        /// <summary>
        /// ynamically creates an object instance with the given array of constructor arguments
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static object CreateInstance(Type type, params object[] args)
        {
            return ObjectResolver.Current.Resolve(type, args);
        }

        /// <summary>
        /// Creates an instance of the given <see cref="Type"/> with the provided constructor arguments
        /// by calling the default parameterless constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        internal static object CreateInstanceWithoutContractResolver(Type type, params object[] args)
        {
            Dictionary<int, Delegate> funcCache;
            lock (locker)
            {
                if (!funcArgCache.TryGetValue(args.Length, out funcCache))
                {
                    funcArgCache[args.Length] = funcCache = new Dictionary<int, Delegate>();
                }
            }

            var typeHashCodes =
                new List<Type> { type }
                .Union(args.Select(a => a.GetType()))
                .Select(t => t.UnderlyingSystemType.GetHashCode());
            var key = string.Join("|", typeHashCodes).GetHashCode();

            Delegate func;
            lock (locker)
            {
                if (!funcCache.TryGetValue(key, out func))
                {
                    funcCache[key] = func = CreateInstanceDelegate(type, args);
                }
            }

            try
            {
                return func.DynamicInvoke(args);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Returns a default value of the type (Only used for value types)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T Default<T>()
        {
            return default(T);
        }

        /// <summary>
        /// Creates a delegate that will create an instance of the given <see cref="Type"/> with the provided constructor arguments
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
#pragma warning disable S3011 // Make sure that this accessibility bypass is safe here.
        private static Delegate CreateInstanceDelegate(Type type, params object[] args)
        {
            Delegate compiled;

            if (type.GetTypeInfo().IsValueType)
            {
                var method = typeof(ReflectionHelper).GetMethod(nameof(Default), BindingFlags.Static | BindingFlags.NonPublic);
                method = method.MakeGenericMethod(type);
                compiled = Expression.Lambda(Expression.Call(method)).Compile();
            }
            else
            {
                var argumentTypes = args.Select(a => a.GetType()).ToArray();
                var argumentExpressions = argumentTypes.Select((t, i) => Expression.Parameter(t, "var" + i)).ToArray();
                var constructorInfo = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, argumentTypes, null);
                if (constructorInfo == null)
                {
                    throw new InvalidOperationException("No public parameterless constructor found.");
                }

                var constructor = Expression.New(constructorInfo, argumentExpressions);
                compiled = Expression.Lambda(constructor, argumentExpressions).Compile();
            }

            return compiled;
        }
#pragma warning restore S3011 // Make sure that this accessibility bypass is safe here.
    }
}
