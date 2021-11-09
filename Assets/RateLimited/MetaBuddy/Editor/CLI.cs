using MetaBuddy.App;
using UnityEditor;

namespace MetaBuddy
{
    public static class CLI
    {
        public static void Run()
        {
            var controller = ServiceLocator.Registry.AnalysisController;

            var analysis = controller.RunAnalysisSync(AnalysisContext.CLI);
          
            var exitCode = (analysis.Errors.ErrorCount > 0) ? 1 : 0;

            EditorApplication.Exit(exitCode);
        }
    }
}
