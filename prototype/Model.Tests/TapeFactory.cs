using mmSquare.Betamax;
using System;
using TechTalk.SpecFlow;

namespace Model.Tests
{
    public class TapeFactory
    {
        public Tape BuildTape(Int32 id)
        {
            return new OrderedFileTape(id.ToString());
        }
    }
}
