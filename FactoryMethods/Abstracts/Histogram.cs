using WpfImageProcess.FactoryMethods.Methods;

namespace WpfImageProcess.FactoryMethods.Abstracts
{
    public class Histogram : ImageMethod
    {
        public override IProcess GetProcess()
        {
            return new HistogramMethod();
        }
    }
}
