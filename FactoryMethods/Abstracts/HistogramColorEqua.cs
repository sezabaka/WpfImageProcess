using WpfImageProcess.FactoryMethods.Methods;

namespace WpfImageProcess.FactoryMethods.Abstracts
{
    public class HistogramColorEqua : ImageMethod
    {
        public override IProcess GetProcess()
        {
            return new HistogramColorEquaMethod();
        }
    }
}
