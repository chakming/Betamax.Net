using FluentAssertions;
using Microsoft.Practices.Unity;
using mmSquare.Betamax;
using NUnit.Framework;

namespace Model.Tests
{
    [TestFixture]
    public class Given_A_Consumer
    {
        [Test]
        public void Record_And_Playback_Works()
        {
            var service = new Model.ConcreteService();
            var recorder = new Recorder().Record<IService, ConcreteService>(service);

            var request = new Model.Request
            {
                Property1 = "A",
                Property2 = "B",
                Property3 = "C"
            };

            var response = recorder.Get(request); // record it

            var container = new UnityContainer();
            container.RegisterType<IService>(new InjectionFactory(c => new ProxyFactory<IService>().CreateInstance()));

            var serviceProxy = container.Resolve<IService>();

            var result = serviceProxy.Get(null); // note: it doesn't matter what you pass in, the proxy will be used at this point

            result.Property1.Should().Be(response.Property1);
        }
    }
}