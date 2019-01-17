using System;
using FunctionalTools;
using NUnit.Framework;

namespace FunctionalTests
{
    [TestFixture]
    public class FunctionalExtensionsFixture
    {
        [Test]
        public void ApplyPartially_ShouldReduceFrom1To0()
        {
            Func<int, string> myFunc = (a) => a + "foo";

            Assert.That(myFunc(3), Is.EqualTo(myFunc.ApplyPartially(3)()));
        }

        [Test]
        public void ApplyPartially_ShouldReduceFrom2To1()
        {
            Func<int, string, string> myFunc = (a, b) => a + b;

            Assert.That(myFunc(3, "a"), Is.EqualTo(myFunc.ApplyPartially(3)("a")));
        }

        [Test]
        public void ApplyPartially_ShouldReduceFrom3To2()
        {
            Func<int, int, string, string> myFunc = (a, b, c) => a + b + c;

            Assert.That(myFunc(3, 5, "a"), Is.EqualTo(myFunc.ApplyPartially(3)(5, "a")));
        }

        [Test]
        public void ApplyPartially_ShouldReduceFrom3To1()
        {
            Func<int, int, string, string> myFunc = (a, b, c) => a + b + c;

            Assert.That(myFunc(3, 5, "a"), Is.EqualTo(myFunc.ApplyPartially(3, 5)("a")));
        }

        [Test]
        public void ApplyPartially_ShouldReduceFrom4To3()
        {
            Func<int, int, decimal, string, string> myFunc = (a, b, c, d) => a + b + c + d;

            Assert.That(myFunc(3, 5, 3.2m, "a"), Is.EqualTo(myFunc.ApplyPartially(3)(5, 3.2m, "a")));
        }

        [Test]
        public void ApplyPartially_ShouldReduceFrom4To2()
        {
            Func<int, int, decimal, string, string> myFunc = (a, b, c, d) => a + b + c + d;

            Assert.That(myFunc(3, 5, 3.2m, "a"), Is.EqualTo(myFunc.ApplyPartially(3, 5)(3.2m, "a")));
        }

        [Test]
        public void Curry_ShouldReduce2ParamTo1ParamFunctions()
        {
            Func<string, decimal, Tuple<string, decimal>> myFunc = (a, b)
                => new Tuple<string, decimal>(a, b);
            
            Assert.That(myFunc("a", 1.0m), Is.EqualTo(myFunc.Curry()("a")(1.0m)));
        }

        [Test]
        public void Curry_ShouldReduce3ParamTo1ParamFunctions()
        {
            Func<string, decimal, int, Tuple<string, decimal, int>> myFunc = (a, b, c)
                => new Tuple<string, decimal, int>(a, b, c);
            
            Assert.That(myFunc("a", 1.0m, 3), Is.EqualTo(myFunc.Curry()("a")(1.0m)(3)));
        }

        [Test]
        public void Curry_ShouldReduce4ParamTo1ParamFunctions()
        {
            Func<string, decimal, int, object, Tuple<string, decimal, int, object>> myFunc = (a, b, c, d)
                => new Tuple<string, decimal, int, object>(a, b, c, d);

            var programmer = new Programmer();
            Assert.That(myFunc("a", 1.0m, 3, programmer), Is.EqualTo(myFunc.Curry()("a")(1.0m)(3)(programmer)));
        }
    }
}
