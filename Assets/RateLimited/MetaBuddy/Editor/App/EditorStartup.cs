using UnityEditor;

namespace MetaBuddy.App
{
    public static class EditorStartup
    {
        [InitializeOnLoadMethod]
        public static void OnEditorStartup()
        {
            var controller = ServiceLocator.Registry.AnalysisController;
            controller.RunAnalysisSync(AnalysisContext.EditorStart);
        }
    }
}

