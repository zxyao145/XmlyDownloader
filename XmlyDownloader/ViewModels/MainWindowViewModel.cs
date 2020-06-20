using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using XmlyDownloader.Commands;
using XmlyDownloader.Services;

namespace XmlyDownloader.Models
{
    public class MainWindowViewModel : ViewModelBase,IDisposable
    {
        IDownloadService _downloadService;

        private string url; // = "https://www.ximalaya.com/xiangsheng/25335291/";

        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                this.RaisePropertyChanged("url");
            }
        } 


        private List<DownloadItemViewModel> downloadItems;
        public List<DownloadItemViewModel> DownloadItems
        {
            get { return downloadItems; }
            set
            {
                downloadItems = value;
                this.RaisePropertyChanged("DownloadItems");
            }
        }


        private string _downloadStatus = "未进行下载过";

        public string DownloadStatus
        {
            get { return _downloadStatus; }
            set
            {
                _downloadStatus = value;
                this.RaisePropertyChanged("DownloadStatus");
            }
        }



        public DelegateCommand DownloadCommand { get; set; }
        private void Download(object parameter)
        {
            var selectItems = DownloadItems
                .Where(e => e.IsEnable && e.Selected)
                .Select(e=>e.Item)
                .ToList();
            _downloadService.AddRange(selectItems);
        }


        public DelegateCommand SearchCommand { get; set; }
        private async Task Search(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(Url))
            {
                var result = await _downloadService.ParsePagingInfo(url);
                var downloadItemViewModels = new List<DownloadItemViewModel>();
                foreach (var item in result)
                {
                    downloadItemViewModels.Add(new DownloadItemViewModel()
                    {
                        Item = item,
                        Selected = true
                    });
                }
                DownloadItems = downloadItemViewModels;
            }
        }

        public void Dispose()
        {
            _downloadService.StopDownload();
        }

        public MainWindowViewModel()
        {
            this.DownloadCommand = new DelegateCommand();
            DownloadCommand.ExecuteAction = new Action<object>(Download);
            SearchCommand = new DelegateCommand();
            SearchCommand.ExecuteActionAsync += new Func<object,Task>(Search);
            _downloadService = new DownloadService();
            _downloadService.OnDownloaded += _downloadService_OnDownloaded;
            _downloadService.StartListenDownload();
        }

        private void _downloadService_OnDownloaded(DownloadItem obj)
        {
            var vm = DownloadItems.SingleOrDefault(e => e.Item == obj);
            if(vm != null)
            {
                vm.IsEnable = false;
            }

            this.DownloadStatus = $"{obj.DirName}/{obj.Index}-{obj.Title} 下载完毕!";
        }
    }
}
