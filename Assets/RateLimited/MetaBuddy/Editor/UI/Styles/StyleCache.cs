using UnityEngine;

namespace MetaBuddy.UI.Styles
{
    public class StyleCache
    {
        private GUIStyle _instance = null;
        private readonly IStyleFactory _factory = null;

        public StyleCache(IStyleFactory factory)
        {
            _factory = factory;
        }

        public GUIStyle Get
        {
            get
            {
                _instance = _instance ?? _factory.Create();
                return _instance;
            }
        }

        public void Reset()
        {
            _instance = null;
        }
    }
}
