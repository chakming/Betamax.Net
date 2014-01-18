namespace Model
{
    public class ConcreteService : IService
    {
        public ConcreteService()
        {
        }

        public Response Get(Request request)
        {
            return new Response
            {
                Property1 = request.Property3,
                Property2 = request.Property1,
                Property3 = request.Property2
            };
        }
    }
}