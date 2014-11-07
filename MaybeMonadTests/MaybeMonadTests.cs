using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaybeMonadImplementation;
using NUnit.Framework;

namespace MaybeMonadTests
{
    public class Person
    {
        public string Name { get;set; }
        public string Vorname { get;set; }
        public Person Nachbar { get; set; }
    }

    /// <summary>
    /// Offene Punkte:
    /// - Semantik With_SollteEineLeereListeLiefern_WennEsNullElementGibt
    /// </summary>
    [TestFixture]
    public class MaybeMonadTests
    {
        [Test]
        public void Maybe_SollteTypeInitializerZurVerfuegungStellen_MonadRule1()
        {
            var result = new Some<int>(4);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Maybe_SollteToMaybeImplementieren_MonadRule2()
        {
            var result = 4.ToMaybe();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void Maybe_SollteBindImplementieren_MonadRule3()
        {
            var result = 4.ToMaybe().Bind(      a =>
                         8.ToMaybe().Bind(      b =>
                        "Test".ToMaybe().Bind(  c =>
                        (a + " " + b + " " + c).ToMaybe()
                        )));

            Assert.That(result.Value, Is.EqualTo("4 8 Test"));
        }

        [Test]
        public void Maybe_SollteBindImplementieren_MonadRule3_FallNothing()
        {
            var result = 4.ToMaybe().Bind(a =>
                ((Person)null).ToMaybe().Bind(b =>
                    "Test".ToMaybe().Bind(c =>
                        (a + " " + b + " " + c).ToMaybe()
                        )));

            Assert.That(result.Value, Is.Null);
        }

        [Test]
        public void Maybe_SollteSelectManyImplementieren_DamitDeklarativeLinqSyntaxVerwendetWerdenKann()
        {
            var result = from a in 10.ToMaybe()
                            from b in " kleine ".ToMaybe()
                                from c in "Programmierer".ToMaybe()
                         select a + b + c;

            Assert.That(result.Value, Is.EqualTo("10 kleine Programmierer"));
        }

        [Test]
        public void Maybe_SollteSelectManyImplementieren_DamitDeklarativeLinqSyntaxVerwendetWerdenKann_FallNothing()
        {
            var result = from a in 10.ToMaybe()
                         from b in ((string)null).ToMaybe()
                         from c in "Programmierer".ToMaybe()
                         select a + b + c;

            Assert.That(result, Is.EqualTo(Maybe.None));
            Assert.That(result.Value, Is.Null);
        }

        [Test]
        public void Value_SollteKorrektenWertZurueckgebenWennEinerVorhanden()
        {
            var person = new Person();
            var result = person.ToMaybe();

            Assert.That(result.Value, Is.EqualTo(person));
        }

        [Test]
        public void Value_SollteNullZurueckgebenWennKeinerVorhanden()
        {
            var result = ((Person)null).ToMaybe();

            Assert.That(result.Value, Is.Null);
            Assert.That(result, Is.EqualTo(Maybe<Person>.None));
        }

        [Test]
        public void ToMaybe_SollteSomeAusNichtNullWertErzeugen()
        {
            var result = new Person().ToMaybe();

            Assert.That(result, Is.TypeOf(typeof(Some<Person>)));
            Assert.That(result, Is.Not.EqualTo(Maybe<Person>.None));
        }

        [Test]
        public void ToMaybe_SollteNoneAusNichtNullWertErzeugen()
        {
            var result = ((Person)null).ToMaybe();

            Assert.That(result, Is.TypeOf(typeof(None<Person>)));
        }

        [Test]
        public void With_SollteVorhandenWertKorrektBinden()
        {
            var dummy = new Person() { Name = "Petricek" };

            Assert.That(dummy.With(p => p.Name).Value, Is.EqualTo(dummy.Name));
        }


        [Test]
        public void With_SollteNoneBinden_WennAusgangsObjektNullIst()
        {
            var dummy = (Person)null;

            Assert.That(dummy.With(p => p.Name).Value, Is.Null);
        }

        [Test]
        public void With_SollteNoneBinden_WennPropertyNull()
        {
            var dummy = new Person() { Name = null };

            Assert.That(dummy.With(p => p.Name).Value, Is.Null);
        }

        [Test]
        public void None_SollteKorrekteValueTypeSemantikImplementieren()
        {
            Assert.That(Maybe.None, Is.EqualTo(Maybe.None));
        }

        [Test]
        public void None_SollteKorrekteValueTypeSemantikImplementieren_WennTypUnterschiedlich()
        {
            Assert.That(new None<Person>(), Is.EqualTo(Maybe.None));
        }

        [Test]
        public void None_SollteUnabhaengigVomTypImmerEqualsSein()
        {
            var person = ((Person) null).ToMaybe();

            Assert.That(person, Is.EqualTo(Maybe.None));
        }

        [Test]
        public void None_SollteNichtEqualsMitNullSein()
        {
            Assert.That(Maybe.None, Is.Not.EqualTo(null));
            Assert.That(Maybe.None, Is.Not.Null);
        }


        [Test]
        public void NoneGeneric_SollteNichtEqualsMitNullSein()
        {
            Assert.That(new None<Person>(), Is.Not.Null);
        }

        [Test]
        public void With_SollteMaybeMonadeErzeugen()
        {
            var maybe = new List<Person>().With(d => d.Name);

            Assert.That(maybe, Is.TypeOf(typeof(Some<IEnumerable<string>>)));
        }

        [Test]
        public void With_SollteEineLeereListeLiefern_WennEsEineLeereListeIst()
        {
            var maybe = new List<Person>().With(d => d.Nachbar).With(p => p.Name).Value;

            Assert.That(maybe, Is.Empty);
        }

        [Test]
        public void With_SollteEineLeereListeLiefern_WennEsNullElementGibt()
        {
            var maybe = new List<Person>() { new Person(), new Person() }.With(d => d.Nachbar).With(p => p.Name);

            Assert.That(maybe.Value, Is.Empty);
            Assert.That(maybe.Value, Is.EqualTo(new List<Person>()));
        }

        [Test]
        public void With_SollteNullElementeKorrektHerausfiltern()
        {
            var maybe = new List<Person>() { null, null }.With(d => d.Nachbar).With(p => p.Name);

            Assert.That(maybe.Value, Is.Empty);
            Assert.That(maybe.Value, Is.EqualTo(new List<string>()));
        }

        [Test]
        public void With_SollteLeereListeLiefern_WennAlleElementeNull()
        {
            var maybe = new List<Person>() { new Person(), null }.With(d => d.Nachbar).With(p => p.Name);

            Assert.That(maybe.Value, Is.EqualTo(new List<Person>()));
        }

        [Test]
        public void Equals_SollteTrueSein_WennGleicheReferenz()
        {
            var person = new Person();

            Assert.That(person.ToMaybe(), Is.EqualTo(person.ToMaybe()));
        }

        [Test]
        public void Equals_SollteFalseSein_WennNichtGleicheReferenz()
        {
            Assert.That(new Person().ToMaybe(), Is.Not.EqualTo(new Person().ToMaybe()));
        }

        [Test]
        public void ToString_SollteNoneLiefern()
        {
            Assert.That(((Person)null).ToMaybe().ToString(), Is.EqualTo("None"));
        }

        [Test]
        public void ToString_SollteToStringVonObjektLiefern()
        {
            Assert.That(new Person().ToMaybe().ToString(), Is.EqualTo(new Person().ToString()));
        }
    }
}
