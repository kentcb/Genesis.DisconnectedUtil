namespace System.Reactive.Linq
{
    using System;

    public static class Extensions
    {
        private static readonly Random random = new Random();

        public static IObservable<TSource> DelayIf<TSource>(this IObservable<TSource> @this, bool condition, int minimumDelayMs, int maximumDelayMs)
        {
            if (!condition)
            {
                return @this;
            }
            else
            {
                return @this
                    .Delay(TimeSpan.FromMilliseconds(random.Next(minimumDelayMs, maximumDelayMs)));
            }
        }

        public static IObservable<TSource> DelayBetweenItemsIf<TSource>(this IObservable<TSource> @this, bool condition, int minimumDelayMs, int maximumDelayMs)
        {
            if (!condition)
            {
                return @this;
            }
            else
            {
                var delays = Observable
                    .Defer(
                        () =>
                        {
                            var delay = TimeSpan.FromMilliseconds(random.Next(minimumDelayMs, maximumDelayMs));
                            return Observable
                                .Return(Unit.Default)
                                .Delay(delay);
                        })
                    .Repeat();

                return delays
                    .Zip(@this, (_, item) => item);
            }
        }

        public static IObservable<TSource> ErrorIf<TSource>(this IObservable<TSource> @this, bool condition, int percentChance, Exception exception = null)
        {
            if (!condition || random.Next(100) > percentChance)
            {
                return @this;
            }

            return Observable.Throw<TSource>(exception ?? new InvalidOperationException("Random error."));
        }

        public static IObservable<TSource> ErrorIf<TSource>(this IObservable<TSource> @this, bool condition, int percentChance, IObservable<Exception> exception)
        {
            if (!condition || random.Next(100) > percentChance)
            {
                return @this;
            }

            return exception
                .Where(ex => ex != null)
                .FirstAsync()
                .SelectMany(ex => Observable.Throw<TSource>(ex));
        }
    }
}