using mmSquare.Betamax;

namespace Model.Tests
{
    internal class ProxyFactory<T> where T : class
    {
        public T CreateInstance()
        {
            return new Player().Play<T>();
        }
    }
}