// ******************************************************************
//       /\ /|       @file       BaseSingleTon.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-07-26 19:48:56
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

namespace RabiSquare.RealisticOreGeneration
{
    public class BaseSingleTon<T> where T : class, new()
    {
        public static T Instance => Inner.InternalInstance;

        private static class Inner
        {
            internal static readonly T InternalInstance = new T();
        }
    }
}