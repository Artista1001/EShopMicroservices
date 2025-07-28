
namespace BuildingBlock.Exceptions
{
    public class NotFoundException: Exception // this is NotFound Generic Exception
    {
        public NotFoundException(string message): base(message)
        {
            
        }
        public NotFoundException(string name, object key): base($"Entity \"{name}\" ({key}) was not found.")
        {
            
        }
    }
}
