namespace System.Linq
{
    using System;
    using System.Collections.Generic;

    public static class Extensions
    {
        private static readonly Random random = new Random();

        public static T RandomItem<T>(this IEnumerable<T> @this) =>
            @this.ElementAt(random.Next(0, @this.Count()));
    }
}