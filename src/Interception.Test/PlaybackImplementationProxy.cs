﻿using mmSquare.Betamax.Interception.Tests.Interfaces;
using NUnit.Framework;

namespace mmSquare.Betamax.Interception.Tests
{
    [TestFixture]
    public class PlaybackImplementationProxy
    {
        [Test]
        public void CanInstantiateProxyFromInterface()
        {
            var pi = new Player();
            var impl = pi.Play<TestInterface>();

            Assert.That(impl, Is.Not.Null);

            //impl.AskAndAnswer("Question");
        }
    }
}