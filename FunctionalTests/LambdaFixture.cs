using System;
using Functional;
using NUnit.Framework;

namespace FunctionalTests
{
    [TestFixture]
    public class LambdaFixture
    {
        [Test]
        public void F_ErlaubtMitAnonymenTypenImplizitDefinierteVariablenZuErzeugen()
        {
            var implicitlyTypedFunc = Lambda.F(() => new { Name = "Mauricio Scheffer" });

            Assert.That(implicitlyTypedFunc().Name, Is.EqualTo("Mauricio Scheffer"));
        }

        [Test]
        public void F_SollteKorrekteFuncErzeugen_MitKeinemArgument()
        {
            Func<int> explicitlyTypedFunc = () => 5;
            var implicitlyTypedFunc = Lambda.F(() => 5)();
            
            Assert.AreEqual(explicitlyTypedFunc(), implicitlyTypedFunc);
        }

        [Test]
        public void F_SollteKorrekteFuncErzeugen_MitEinemArgument()
        {
            Func<int, string> explicitlyTypedFunc = val => "test" + val;
            var implicitlyTypedFunc = Lambda.F((int val) => "test" + val);

            Assert.AreEqual(explicitlyTypedFunc(4), implicitlyTypedFunc(4));
        }


        [Test]
        public void F_SollteKorrekteFuncErzeugen_MitZweiArgument()
        {
            Func<int, double, string> explicitlyTypedFunc = (val1, val2) => "test" + val1 + val2;
            var implicitlyTypedFunc = Lambda.F((int val1, double val2) => "test" + val1 + val2);

            Assert.AreEqual(explicitlyTypedFunc(4, 5.5), implicitlyTypedFunc(4, 5.5));
        }
    }
}
