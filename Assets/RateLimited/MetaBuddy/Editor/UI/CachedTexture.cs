using UnityEditor;
using UnityEngine;

namespace MetaBuddy.UI
{
    public class CachedTexture
    {
        private readonly string _path;
        private Texture2D _texture;

        public CachedTexture(string path)
        {
            _path = path;
        }
     
        public Texture2D Get
        {
            get
            {
                if(_texture == null)
                {
                    _texture = (Texture2D) EditorGUIUtility.Load(_path);
                }

                return _texture;
            }
        }
    }
}
