using WpfImageProcess.FactoryMethods.Methods;

namespace WpfImageProcess.FactoryMethods.Abstracts
{
    public class GrayScale : ImageMethod
    {
        public override IProcess GetProcess()
        {
            return new GrayScaleMethod();
        }
    }
}
