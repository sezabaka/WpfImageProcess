using WpfImageProcess.FactoryMethods.Methods;

namespace WpfImageProcess.FactoryMethods.Abstracts
{
    public class Invert : ImageMethod
    {
        public override IProcess GetProcess()
        {
            return new InvertMethod();
        }
    }
}
