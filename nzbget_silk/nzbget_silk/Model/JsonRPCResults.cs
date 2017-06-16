using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nzbget_silk.Model
{
    public class JsonRPCNewsServer
    {
        public long ID { get; set; }
        public bool Active { get; set; }
    }

    public class JsonRPCStatus
    {
        public long RemainingSizeLo { get; set; }
        public long RemainingSizeHi { get; set; }
        public long RemainingSizeMB { get; set; }
        public long ForcedSizeLo { get; set; }
        public long ForcedSizeHi { get; set; }
        public long ForcedSizeMB { get; set; }
        public long DownloadedSizeLo { get; set; }
        public long DownloadedSizeHi { get; set; }
        public long DownloadedSizeMB { get; set; }
        public long MonthSizeLo { get; set; }
        public long MonthSizeHi { get; set; }
        public long MonthSizeMB { get; set; }
        public long DaySizeLo { get; set; }
        public long DaySizeHi { get; set; }
        public long DaySizeMB { get; set; }
        public long ArticleCacheLo { get; set; }
        public long ArticleCacheHi { get; set; }
        public long ArticleCacheMB { get; set; }
        public long DownloadRate { get; set; }
        public long AverageDownloadRate { get; set; }
        public long DownloadLimit { get; set; }
        public long ThreadCount { get; set; }
        public long ParJobCount { get; set; }
        public long PostJobCount { get; set; }
        public long UrlCount { get; set; }
        public long UpTimeSec { get; set; }
        public long DownloadTimeSec { get; set; }
        public bool ServerPaused { get; set; }
        public bool DownloadPaused { get; set; }
        public bool Download2Paused { get; set; }
        public bool ServerStandBy { get; set; }
        public bool PostPaused { get; set; }
        public bool ScanPaused { get; set; }
        public bool QuotaReached { get; set; }
        public long FreeDiskSpaceLo { get; set; }
        public long FreeDiskSpaceHi { get; set; }
        public long FreeDiskSpaceMB { get; set; }
        public long ServerTime { get; set; }
        public long ResumeTime { get; set; }
        public bool FeedActive { get; set; }
        public long QueueScriptCount { get; set; }
        public List<JsonRPCNewsServer> NewsServers { get; set; }
    }

    public class JsonRPCParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class JsonRPCServerStat
    {
        public long ServerID { get; set; }
        public long SuccessArticles { get; set; }
        public long FailedArticles { get; set; }
    }

    public class JsonRPCGroup
    {
        public long RemainingSizeLo { get; set; }
        public long RemainingSizeHi { get; set; }
        public long RemainingSizeMB { get; set; }
        public long PausedSizeLo { get; set; }
        public long PausedSizeHi { get; set; }
        public long PausedSizeMB { get; set; }
        public long RemainingFileCount { get; set; }
        public long RemainingParCount { get; set; }
        public long MaxPriority { get; set; }
        public long ActiveDownloads { get; set; }
        public string Status { get; set; }
        public long NZBID { get; set; }
        public string NZBName { get; set; }
        public string Kind { get; set; }
        public string URL { get; set; }
        public string NZBFilename { get; set; }
        public string DestDir { get; set; }
        public string FinalDir { get; set; }
        public string Category { get; set; }
        public string ParStatus { get; set; }
        public string ExParStatus { get; set; }
        public string UnpackStatus { get; set; }
        public string MoveStatus { get; set; }
        public string ScriptStatus { get; set; }
        public string DeleteStatus { get; set; }
        public string MarkStatus { get; set; }
        public string UrlStatus { get; set; }
        public long FileSizeLo { get; set; }
        public long FileSizeHi { get; set; }
        public long FileSizeMB { get; set; }
        public long FileCount { get; set; }
        public long MinPostTime { get; set; }
        public long MaxPostTime { get; set; }
        public long TotalArticles { get; set; }
        public long SuccessArticles { get; set; }
        public long FailedArticles { get; set; }
        public long Health { get; set; }
        public long CriticalHealth { get; set; }
        public string DupeKey { get; set; }
        public long DupeScore { get; set; }
        public string DupeMode { get; set; }
        public long DownloadedSizeLo { get; set; }
        public long DownloadedSizeHi { get; set; }
        public long DownloadedSizeMB { get; set; }
        public long DownloadTimeSec { get; set; }
        public long PostTotalTimeSec { get; set; }
        public long ParTimeSec { get; set; }
        public long RepairTimeSec { get; set; }
        public long UnpackTimeSec { get; set; }
        public long MessageCount { get; set; }
        public long ExtraParBlocks { get; set; }
        public List<JsonRPCParameter> Parameters { get; set; }
        public List<object> ScriptStatuses { get; set; }
        public List<JsonRPCServerStat> ServerStats { get; set; }
        public string PostInfoText { get; set; }
        public long PostStageProgress { get; set; }
        public long PostStageTimeSec { get; set; }
    }
}
