using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace XmlyDownloader.Models
{
    public class DownloadItemViewModel:ViewModelBase
    {

        private DownloadItem downloadItem;

        public DownloadItem Item
        {
            get { return downloadItem; }
            set
            {
                downloadItem = value;
                this.RaisePropertyChanged("Item");
            }
        }


        private bool selected;

        public bool Selected
        {
            get { return selected; }
            set     
            {
                selected = value;
                this.RaisePropertyChanged("Selected");
            }
        }

        private bool _isEnable = true;

        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                _isEnable = value;
                this.RaisePropertyChanged("IsEnable");
            }
        }


    }
}
