using System;
using System.Collections;
using System.Collections.Generic;

namespace TimeStar.DigitalPlant
{
    /// <summary>
    /// 下载与加载帮助类
    /// </summary>
    public class ProgressHandle
    {

        float downloadProgress;
        float unzipProgress;
        public bool isDownLoadStart;
        public bool isUnzipStart;
        bool isDownLoadFinsh;
        bool isUnzipFinsh;
        string key;
        public Action<string> downloadFinsh;
        public Action<string> unzipFinsh;


        public float DownloadProgress { get => downloadProgress; set => downloadProgress = value; }
        public float UnzipProgress { get => unzipProgress; set => unzipProgress = value; }
        public string Key { get => key; set => key = value; }
        public bool IsDownLoadFinsh { get => isDownLoadFinsh; set => isDownLoadFinsh = value; }
        public bool IsUnzipFinsh { get => isUnzipFinsh; set => isUnzipFinsh = value; }

        public void SetKey(string key)
        {
            this.key = key;
        }

    }
}