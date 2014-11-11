using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaybeMonadImplementation;
using NUnit.Framework;

namespace MaybeMonadTests
{
    public class Programmer
    {
        public string Name { get;set; }
        public string Vorname { get;set; }
        public Programmer Nachbar { get; set; }
    }

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
                ((Programmer)null).ToMaybe().Bind(b =>
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
            var programmer = new Programmer();
            var result = programmer.ToMaybe();

            Assert.That(result.Value, Is.EqualTo(programmer));
        }

        [Test]
        public void Value_SollteNullZurueckgebenWennKeinerVorhanden()
        {
            var result = ((Programmer)null).ToMaybe();

            Assert.That(result.Value, Is.Null);
            Assert.That(result, Is.EqualTo(Maybe.None));
        }

        [Test]
        public void ToMaybe_SollteSomeAusNichtNullWertErzeugen()
        {
            var result = new Programmer().ToMaybe();

            Assert.That(result, Is.TypeOf(typeof(Some<Programmer>)));
            Assert.That(result, Is.Not.EqualTo(Maybe.None));
        }

        [Test]
        public void ToMaybe_SollteNoneAusNichtNullWertErzeugen()
        {
            var result = ((Programmer)null).ToMaybe();

            Assert.That(result, Is.TypeOf(typeof(None<Programmer>)));
        }

        [Test]
        public void With_SollteVorhandenWertKorrektBinden()
        {
            var dummy = new Programmer() { Name = "Petricek" };

            Assert.That(dummy.With(p => p.Name).Value, Is.EqualTo(dummy.Name));
        }


        [Test]
        public void With_SollteNoneBinden_WennAusgangsObjektNullIst()
        {
            var dummy = (Programmer)null;

            Assert.That(dummy.With(p => p.Name).Value, Is.Null);
        }

        [Test]
        public void With_SollteNoneBinden_WennPropertyNull()
        {
            var dummy = new Programmer() { Name = null };

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
            Assert.That(new None<Programmer>(), Is.EqualTo(Maybe.None));
        }

        [Test]
        public void None_SollteUnabhaengigVomTypImmerEqualsSein()
        {
            var programmer = ((Programmer) null).ToMaybe();

            Assert.That(programmer, Is.EqualTo(Maybe.None));
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
            Assert.That(new None<Programmer>(), Is.Not.Null);
        }

        [Test]
        public void With_SollteMaybeMonadeErzeugen_WithEmptyList()
        {
            var maybe = new List<Programmer>().With(d => d.Name);

            Assert.That(maybe, Is.TypeOf(typeof(Some<IEnumerable<string>>)));
        }
        [Test]
        public void With_SollteMaybeMonadeErzeugen()
        {
            var programmers = new List<Programmer>()
            {
                new Programmer() { Name = "Schumann", Vorname = "Robert" },
                new Programmer() { Name = "Bach" },
                new Programmer() { Name = "Richter", Vorname = "Sviatoslav" },
                new Programmer()
            };
            var result = programmers.With(p => p.Vorname);
            Assert.That(result, Is.Not.EqualTo(Maybe.None));
            Assert.That(result.Value.Count(), Is.EqualTo(2));
            Assert.That(result.Value.Any(v => string.Equals(v, "Robert")));
            Assert.That(result.Value.Any(v => string.Equals(v, "Sviatoslav")));
        }

        [Test]
        public void With_SollteEineLeereListeLiefern_WennEsEineLeereListeIst()
        {
            var maybe = new List<Programmer>().With(d => d.Nachbar).With(p => p.Name).Value;

            Assert.That(maybe, Is.Empty);
        }

        [Test]
        public void With_SollteEineLeereListeLiefern_WennEsNullElementGibt()
        {
            var maybe = new List<Programmer>() { new Programmer(), new Programmer() }.With(d => d.Nachbar).With(p => p.Name);

            Assert.That(maybe.Value, Is.Empty);
            Assert.That(maybe.Value, Is.EqualTo(new List<Programmer>()));
        }

        [Test]
        public void With_SollteNullElementeKorrektHerausfiltern()
        {
            var maybe = new List<Programmer>() { null, null }.With(d => d.Nachbar).With(p => p.Name);

            Assert.That(maybe.Value, Is.Empty);
            Assert.That(maybe.Value, Is.EqualTo(new List<string>()));
        }

        [Test]
        public void With_SollteLeereListeLiefern_WennAlleElementeNull()
        {
            var maybe = new List<Programmer>() { new Programmer(), null }.With(d => d.Nachbar).With(p => p.Name);

            Assert.That(maybe.Value, Is.EqualTo(new List<Programmer>()));
        }

        [Test]
        public void Equals_SollteTrueSein_WennGleicheReferenz()
        {
            var programmer = new Programmer();

            Assert.That(programmer.ToMaybe(), Is.EqualTo(programmer.ToMaybe()));
        }

        [Test]
        public void Equals_SollteFalseSein_WennNichtGleicheReferenz()
        {
            Assert.That(new Programmer().ToMaybe(), Is.Not.EqualTo(new Programmer().ToMaybe()));
        }

        [Test]
        public void ToString_SollteNoneLiefern()
        {
            Assert.That(((Programmer)null).ToMaybe().ToString(), Is.EqualTo("None"));
        }

        [Test]
        public void ToString_SollteToStringVonObjektLiefern()
        {
            Assert.That(new Programmer().ToMaybe().ToString(), Is.EqualTo(new Programmer().ToString()));
        }

        [Test]
        public void OrElse_SollteAlternativenWertLiefern_WennNone()
        {
            var option = Maybe.None;

            var programmer = new Programmer();
            var result = option.OrElse(() => programmer);

            Assert.That(result, Is.EqualTo(programmer));
        }


        [Test]
        public void OrElse_SollteAlternativenMaybeLiefern_WennNone()
        {
            var option = Maybe.None;

            var programmer = new Programmer();
            var result = option.OrElse(() => new Some<Programmer>(programmer));

            Assert.That(result.Value, Is.EqualTo(programmer));
        }

        [Test]
        public void OrElse_SollteAlternativenWertLifern_WennNoneAndGeneric()
        {
            var option = ((Programmer)null).ToMaybe();

            var programmer = new Programmer();
            var result = option.OrElse(() => programmer);

            Assert.That(result, Is.EqualTo(programmer));  
        }


        [Test]
        public void OrElse_SollteWertLiefern_WennSome()
        {
            var programmer = new Programmer();
            var option = programmer.ToMaybe();

            var anotherprogrammer = new Programmer();
            var result = option.OrElse(() => anotherprogrammer);

            Assert.That(result, Is.EqualTo(programmer));
        }


        [Test]
        public void OrElse_SollteMaybeLiefern_WennSome()
        {
            var programmer = new Programmer();
            var option = programmer.ToMaybe();

            var anotherprogrammer = new Programmer();
            var result = option.OrElse(() => new Some<Programmer>(anotherprogrammer));

            Assert.That(result.Value, Is.EqualTo(programmer));
        }
    }
}
