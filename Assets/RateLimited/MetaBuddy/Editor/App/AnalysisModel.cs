using System.Threading.Tasks;
using MetaBuddy.Util;
using MetaBuddyLib.Analysis;

namespace MetaBuddy.App
{
    public class AnalysisModel : IReadOnlyAnalysisModel
    {
        public Task<AnalysisResults> AnalysisTask = null;
        public float AnalysisProgress { get; set; }

        public LogEntry LastError { get; set; }

        public AnalysisResults Results { get; set; }

        public bool HasResults
        {
            get
            {
                return Results != null;
            }
        }

        public bool HasErrors
        {
            get
            {
                return AnalysisTask == null
                    && Results != null
                    && Results.Errors != null
                    && Results.Errors.ErrorCount > 0;
            }
        }

        public bool AnalysisInProgress
        {
            get
            {
                return AnalysisTask != null;
            }
        }
    }

}
