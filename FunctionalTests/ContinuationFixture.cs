using NUnit.Framework;
using Functional;

namespace FunctionalTests
{
    [TestFixture]
    public class ContinuationFixture 
    {
        [Test]
        public void Validate_SollteValidierungAusfuehren()
        {
            var wasValidated = false;
            Continuation.Validate(() =>
            {
                wasValidated = true;
                return true;
            });

            Assert.That(wasValidated, Is.True);
        }

        [Test]
        public void Validate_SollteContinuationNichtAusfuehren_WennValidierungFehlgeschlagen()
        {
            var wasValidated = false;
            var wasContinued = false;
            var returnValue = Continuation.Validate(() =>
            {
                wasValidated = true;
                return false;
            }).AndContinueWith(() =>
            {
                wasContinued = true;
                return false;
            });

            Assert.That(wasValidated, Is.True, "Validierung muss ausgeführt worden sein");
            Assert.That(wasContinued, Is.False, "Continuation darf nicht ausgeführt worden sein");
            Assert.That(returnValue.IsNone, Is.True, "Es muss None zurückgegeben werden");
        }

        [Test]
        public void Validate_SollteContinuationAusfuehren_WennValidierungErfolgreich()
        {
            var wasValidated = false;
            var wasContinued = false;
            var returnValue = Continuation.Validate(() =>
            {
                wasValidated = true;
                return true;
            }).AndContinueWith(() =>
            {
                wasContinued = true;
                return false;
            });

            Assert.That(wasValidated, Is.True, "Validierung muss ausgeführt worden sein");
            Assert.That(wasContinued, Is.True, "Continuation muss ausgeführt worden sein");
            Assert.That(returnValue.IsSome, Is.True, "Es muss Some zurückgegeben werden");
            Assert.That(returnValue.Value, Is.False, "Es der Return Wert der Continuation weitergegeben werden.");
        }


        class JustAClass
        { }

        class JustAnotherClass
        { }

        [Test]
        public void Do_SollteActionAusfuehren()
        {
            var actionExecuted = false;
            Continuation<JustAClass>.Do(() =>
            {
                actionExecuted = true;
                return null;
            });

            Assert.That(actionExecuted, Is.True);
        }

        [Test]
        public void Do_SollteContinuationNichtAusfuehren_WennDoEinNoneZurueckgibt()
        {
            var actionExecuted = false;
            var wasContinued = false;
            var returnValue = Continuation<JustAClass>.Do(() =>
            {
                actionExecuted = true;
                return null;
            }).AndContinueWith(_ =>
            {
                wasContinued = true;
                return new JustAClass();
            });

            Assert.That(actionExecuted, Is.True, "Validierung muss ausgeführt worden sein");
            Assert.That(wasContinued, Is.False, "Continuation darf nicht ausgeführt worden sein");
            Assert.That(returnValue.IsNone, Is.True, "Es muss None zurückgegeben werden");
        }

        [Test]
        public void Do_SollteContinuationAusfuehren_WennValidierungErfolgreich()
        {
            var wasValidated = false;
            var wasContinued = false;
            var dummy = new JustAClass();
            var returnValue = Continuation<JustAClass>.Do(() =>
            {
                wasValidated = true;
                return dummy;
            }).AndContinueWith(doReturn =>
            {
                wasContinued = true;
                return doReturn;
            });

            Assert.That(wasValidated, Is.True, "Validierung muss ausgeführt worden sein");
            Assert.That(wasContinued, Is.True, "Continuation muss ausgeführt worden sein");
            Assert.That(returnValue.IsSome, Is.True, "Es muss Some zurückgegeben werden");
            Assert.That(returnValue.Value, Is.SameAs(dummy), "Es der Return Wert der Continuation weitergegeben werden.");
        }

        [Test]
        public void Do_SollteReturnWertAnContinuationAusfuehren_WennValidierungErfolgreich()
        {
            var dummy = new JustAClass();
            Continuation<JustAClass>.Do(() => dummy)
                                  .AndContinueWith(doReturn =>
            {
                Assert.That(doReturn, Is.SameAs(dummy));
                return new JustAnotherClass();
            });
        }
    }
}
