namespace mmSquare.Betamax.Unity
{
    internal interface TapeObservable
    {
        void RegisterObserver(TapeObserver observer);

        void NotifyObservers();
    }
}