using MetaBuddy.Util;
using MetaBuddyLib.Analysis;

namespace MetaBuddy.App
{
    public interface IReadOnlyAnalysisModel
    {
        float AnalysisProgress { get; }

        LogEntry LastError { get; }

        AnalysisResults Results { get; }

        bool HasResults { get; }
        bool HasErrors { get; }

        bool AnalysisInProgress { get; }
    }
}
