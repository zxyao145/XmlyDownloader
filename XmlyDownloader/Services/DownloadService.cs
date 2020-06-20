using HtmlAgilityPack;
using Jil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XmlyDownloader.Models;

namespace XmlyDownloader.Services
{
   

    public class DownloadService : IDownloadService
    {
        public event Action<DownloadItem> OnDownloaded;


        private Queue<DownloadItem> _downloadQueue = new Queue<DownloadItem>();
        private List<DownloadItem> _searchResult = new List<DownloadItem>();

        private string _saveDir = AppContext.BaseDirectory;

        private string _saveSubDir1 = "1downloadResult";

        /// <summary>
        /// page parser client
        /// </summary>
        private HttpClient xmlyClicnt;


        /// <summary>
        /// static resource client
        /// </summary>
        private HttpClient _downloadClient;
        private bool _stop;

        public DownloadService()
        {
            Init();
        }

        private void Init()
        {
            var sockets = new SocketsHttpHandler()
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(10),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
                MaxConnectionsPerServer = 10,
                UseCookies = true
            };
            //var url = "https://www.ximalaya.com/xiqu/6113767/";

            var http = new HttpClient(sockets);
            //http.DefaultRequestHeaders.Host = "www.ximalaya.com/";
            http.DefaultRequestHeaders.Referrer = new Uri("https://www.ximalaya.com/");
            http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Chrome", "78.0.3904.87"));
            http.BaseAddress = new Uri("https://www.ximalaya.com");
            xmlyClicnt = http;


            _downloadClient = new HttpClient(sockets);
            //_downloadClient.DefaultRequestHeaders.Host = "www.ximalaya.com/";
            _downloadClient.DefaultRequestHeaders.Referrer = new Uri("https://www.ximalaya.com/");
            _downloadClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Chrome", "78.0.3904.87"));

        }

        /// <summary>
        /// 获取专辑中所有音乐
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<List<DownloadItem>> ParsePagingInfo(string url)
        {
            _searchResult.Clear();
            await ParsePagingInfoHandle(url, xmlyClicnt);

            return _searchResult;
        }

        /// <summary>
        /// 解析专辑中的所有音乐
        /// </summary>
        /// <param name="url">专辑url</param>
        /// <param name="httpClient"></param>
        /// <param name="pageNode"></param>
        /// <returns></returns>
        private async Task ParsePagingInfoHandle(string url, HttpClient httpClient, HtmlNode pageNode = null)
        {
            var text = await httpClient.GetStringAsync(url);

            var html = new HtmlDocument();
            html.LoadHtml(text);

            var xqName = html.DocumentNode.SelectSingleNode("//h1[@class=\"title lO_\"]").InnerText;

            var node = html.DocumentNode
                .SelectSingleNode("//div[@class=\"sound-list _Qp\"]");
            if (node != null)
            {
                var aArr = node.SelectNodes("./ul/li//a");
                int index = 0;
                foreach (var a in aArr)
                {
                    var title = a.Attributes["title"].Value;
                    var href = a.Attributes["href"].Value;// /xiqu/6113767/26628886

                    _searchResult.Add(new DownloadItem()
                    {
                        DirName = xqName,
                        Title = title,
                        Index = index++,
                        Href = "/revision/play/v1/audio?ptype=1&id="
                        + href.Split("/").Reverse().ToList()[0]
                    });
                }
                if (pageNode == null)
                {
                    var pages = node.SelectNodes("./div//a");
                    if (pages != null)
                    {
                        foreach (var page in pages)
                        {
                            if (page != null)
                            {
                                var href = page.Attributes["href"].Value;
                                await ParsePagingInfoHandle(href, httpClient, page);
                            }
                        }
                    }
                }
            }
        }

        private bool _hasStopd = false;


        public Task StartListenDownload()
        {
            _hasStopd = false;
            _stop = false;

            return Task.Run(async () =>
            {
                var rand = new Random();
                while (!_stop)
                {
                    if (_downloadQueue.Count == 0)
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        var item = _downloadQueue.Dequeue();
                        var jsonStr = await xmlyClicnt.GetStringAsync(item.Href);

                        var downloadUrl = GetM4a(jsonStr);

                        var bytes = await _downloadClient.GetByteArrayAsync(downloadUrl);

                        var file = Path.Combine
                            (_saveDir, _saveSubDir1,
                            item.DirName,
                            item.Index + "-" + item.Title + ".m4a");

                        var dir = Path.GetDirectoryName(file);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        else if (File.Exists(file))
                        {
                            File.Delete(file);
                        }
                        await using var fs = new System.IO.FileStream(file, System.IO.FileMode.CreateNew);
                        fs.Write(bytes, 0, bytes.Length);
                        //暂停 200 ~ 3200毫秒
                        int sleepTime = (int)(rand.NextDouble() * 3 * 1000 + 200);
                        OnDownloaded?.Invoke(item);
                        Thread.Sleep(sleepTime);
                    }
                }
                _hasStopd = true;
            });
        }

        public void StopDownload()
        {
            _stop = true;
            while (!_hasStopd)
            {
                Thread.Sleep(200);
            }
        }


        //{"ret":200,
        //"data":
        //{"trackId":26628886,"canPlay":true,"isPaid":false,"hasBuy":true,
        //"src":"https://fdfs.xmcdn.com/group24/M02/A7/0B/wKgJNVhKyNfRf_szAXz2YvP1I0g650.m4a",
        //"albumIsSample":false,"sampleDuration":0,
        //"isBaiduMusic":false,"firstPlayStatus":true}}
        private string GetM4a(string jsonStr)
        {
            var json = JSON.DeserializeDynamic(jsonStr);
            string url = json.data.src;
            return url;
        }


        public void Add(DownloadItem downloadItem)
        {
            this._downloadQueue.Enqueue(downloadItem);
        }

        public void AddRange(List<DownloadItem> downloadItems)
        {
            foreach (var item in downloadItems)
            {
                Add(item);
            }
        }

    }
}
