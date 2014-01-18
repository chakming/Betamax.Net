namespace Model
{
    public class Consumer
    {
        private readonly IService _Service;

        /// <summary>
        /// Initializes a new instance of the Consumer class.
        /// </summary>
        public Consumer(IService service)
        {
            _Service = service;
        }
    }
}