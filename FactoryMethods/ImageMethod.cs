namespace WpfImageProcess.FactoryMethods
{
    public abstract class ImageMethod
    {
        public abstract IProcess GetProcess();

        public override string ToString()
        {
            return this.GetType().ToString();
        }
    }
}
