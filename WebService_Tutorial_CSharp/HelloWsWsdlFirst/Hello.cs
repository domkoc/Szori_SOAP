namespace HelloWsWsdlFirst
{
    public class Hello : IHello
    {
        public async Task<string> SayHelloAsync(string name)
        {
            return $"Hello: {name}";
        }
    }
}