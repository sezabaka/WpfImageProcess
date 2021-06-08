using WpfImageProcess.FactoryMethods.Methods;

namespace WpfImageProcess.FactoryMethods.Abstracts
{
    public class GreenFilter : ImageMethod
    {
        public override IProcess GetProcess()
        {
            return new GreenFilterMethod();
        }
    }
}
