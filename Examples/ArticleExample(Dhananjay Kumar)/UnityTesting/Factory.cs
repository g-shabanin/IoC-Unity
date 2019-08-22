namespace UnityTesting
{
    using Microsoft.Practices.Unity;

    public class Factory
    {
        static public IPlayer CreateInstance()
        {
            IUnityContainer _container = new UnityContainer();
            _container.RegisterType(typeof(IPlayer), typeof(Player));
            IPlayer obj = _container.Resolve<IPlayer>();
            return obj;
        }
    }
}
