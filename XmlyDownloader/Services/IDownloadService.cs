using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XmlyDownloader.Models;

namespace XmlyDownloader.Services
{
    public interface IDownloadService
    {
        Task<List<DownloadItem>> ParsePagingInfo(string url);
        event Action<DownloadItem> OnDownloaded;

        Task StartListenDownload();

        void StopDownload();


        void Add(DownloadItem downloadItem);
        void AddRange(List<DownloadItem> downloadItems);
    }
}
