using mmSquare.Betamax;
using System;
using TechTalk.SpecFlow;

namespace Model.Tests
{
    public class ServiceFactory
    {

        public IService BuildRecorderService(Tape tape)
        {
            var service = new Model.ConcreteService();
            var recorder = new Recorder(tape).Record<IService, ConcreteService>(service);
            return recorder;
        }

        public IService BuildPlaybackService(Tape tape)
        {
            var player = new Player(tape).Play<IService>();
            return player;
        }
    }
}
