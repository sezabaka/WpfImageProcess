using WpfImageProcess.FactoryMethods.Methods;

namespace WpfImageProcess.FactoryMethods.Abstracts
{
    public class BlueFilter : ImageMethod
    {
        public override IProcess GetProcess()
        {
            return new BlueFilterMethod();
        }
    }
}
