using System;
using System.Collections.Generic;
using System.Text;

namespace XmlyDownloader.Models
{
    public class DownloadItem
    {
        /// <summary>
        /// 用来区分是专辑中的第几个
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 专辑名，用来作为文件夹名
        /// </summary>
        public string DirName { get; set; }

        /// <summary>
        /// 歌曲名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string Href { get; set; }

    }
}
