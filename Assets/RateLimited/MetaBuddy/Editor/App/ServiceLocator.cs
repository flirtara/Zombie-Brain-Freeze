namespace MetaBuddy.App
{
    public static class ServiceLocator
    {
        private static ServiceRegistry _registry;

        public static ServiceRegistry Registry
        {
            get
            {
                if(_registry == null)
                {
                    _registry = new ServiceRegistry();
                }

                return _registry;
            }
        }
    }
}
