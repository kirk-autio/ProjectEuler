using Xunit.Abstractions;

namespace ProjectEuler
{
    public abstract class Problems
    {
        protected Problems(ITestOutputHelper output) => Output = output;

        protected ITestOutputHelper Output { get; }
    }
}