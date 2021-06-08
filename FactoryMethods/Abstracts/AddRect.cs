using WpfImageProcess.FactoryMethods.Methods;

namespace WpfImageProcess.FactoryMethods.Abstracts
{
    public class AddRect : ImageMethod
    {
        public override IProcess GetProcess()
        {
            return new AddRectMethod();
        }
    }
}
