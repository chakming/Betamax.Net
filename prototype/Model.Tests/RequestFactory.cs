using mmSquare.Betamax;
using System;
using TechTalk.SpecFlow;

namespace Model.Tests
{
    public class RequestFactory
    {
        public Request BuildRequest(int p0)
        {
            return new Request { Property1 = p0.ToString(), Property2 = p0.ToString(), Property3 = p0.ToString() };
        }
    }
}
