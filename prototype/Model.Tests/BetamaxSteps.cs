using mmSquare.Betamax;
using System;
using TechTalk.SpecFlow;
using FluentAssertions;
using System.Collections.Generic;

namespace Model.Tests
{
    [Binding]
    public class BetamaxSteps
    {
        private Response _PlaybackResponse;
        private Dictionary<Int32, Tape> _Tapes = new Dictionary<int, Tape>();

        private Tape GetTape(Int32 id)
        {
            if(!_Tapes.ContainsKey(id))
            {
                _Tapes.Add(id, new TapeFactory().BuildTape(id));
            }

            return _Tapes[id];
        }

        [Given(@"I record api call (.*) on tape (.*)")]
        public void GivenIRecordApiCallOnTape(int p0, int p1)
        {
            var tape = GetTape(p1);
            var recorder = new ServiceFactory().BuildRecorderService(tape);
            recorder.Get(new RequestFactory().BuildRequest(p0));
        }

        [When(@"I playback tape (.*)")]
        public void WhenIPlaybackTape(int p0)
        {
            var tape = GetTape(p0);
            var player = new ServiceFactory().BuildPlaybackService(tape);
            _PlaybackResponse = player.Get(null);
        }

        [Given(@"I clear tape (.*)")]
        public void GivenIClearTape(int p0)
        {
            var tape = GetTape(p0);
            tape.Erase();
        }

        
        [Then(@"api call (.*) should replay")]
        public void ThenApiCallShouldReplay(int p0)
        {
            var expected = new RequestFactory().BuildRequest(p0);
            _PlaybackResponse.Property1.Should().Be(expected.Property1);
            _PlaybackResponse.Property2.Should().Be(expected.Property2);
            _PlaybackResponse.Property3.Should().Be(expected.Property3);
        }
    }
}
