using WpfImageProcess.FactoryMethods.Methods;

namespace WpfImageProcess.FactoryMethods.Abstracts
{
    public class RedFilter : ImageMethod
    {
        public override IProcess GetProcess()
        {
            return new RedFilterMethod();
        }
    }
}
