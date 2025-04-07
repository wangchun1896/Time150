using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// Http访问帮助类
/// </summary>
public class HttpHelper
{
    public static object initObj = new object();
    public static int timeout = 10000;

    /// <summary>
    /// 后台网络请求
    /// </summary>
    /// <param name="interfaceName">接口名称</param>
    /// <param name="interfaceParam">接口需要参数</param>
    /// <returns></returns>
    public static string HttpRequest(string interfaceName, string interfaceParam)
    {
        //string path = GameInfo.Url_test;
        string path;
        if (GameInfo.IsDebug=="true")
            path = GameInfo.Url; //UAT
        else
            path = GameInfo.Url_shengchan;
        
       
        string userInfoInterFace = interfaceName;
        List<string> h = new List<string>();
        List<string> b = new List<string>();
        h.Add(GameInfo.method);
        b.Add(userInfoInterFace);
        h.Add(GameInfo.tt_req_header);
        long currentTimeStampMilliseconds = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
        //公共
        RequestData tt_req_header_content = new RequestData
        {
            app_type = GameInfo.app_type,
            device = GameInfo.device,
            device_id = GameInfo.device_id,
            authorization = GameInfo.authorization,
            user_agent = GameInfo.user_agent,
            accept_encoding = GameInfo.accept_encoding,
            login_type = GameInfo.login_type,
            timezone = GameInfo.timezone,
            lang = GameInfo.lang,
            timestamp = currentTimeStampMilliseconds.ToString(),
            version = GameInfo.version,
        };
        string tt_req_header = JsonUtility.ToJson(tt_req_header_content);
        b.Add(tt_req_header);

        //验签
        //验签
        string m_sign = CalculateMD5(BuildRequestString(interfaceParam, currentTimeStampMilliseconds));
        DataCheckStore dataCheckStore = new DataCheckStore
        {
            app_id = GameInfo.app_id,
            charset = GameInfo.charset,
            data = interfaceParam,
            format = GameInfo.format,
            req_no = currentTimeStampMilliseconds.ToString(),
            timestamp = currentTimeStampMilliseconds.ToString(),
            sign_type = GameInfo.sign_type,
            version = GameInfo.version,
            sign = m_sign,
        };
        string data = JsonUtility.ToJson(dataCheckStore);
        string s = PostDataGetHtml(path, data, h, b);
        return s;
    }

    private static string BuildRequestString(string interfaceData, long timeNow)
    {
        if (GameInfo.IsDebug != "true")
        {
            GameInfo.app_id = GameInfo.app_id_shengchan;
            GameInfo.key = GameInfo.key_shengchan; //shengchan
        }
       
        string s = $"app_id={GameInfo.app_id}" +
                     $"&charset={GameInfo.charset}" +
                     $"&data={interfaceData}" +
                     $"&format={GameInfo.format}" +
                     $"&req_no={timeNow}" +
                     $"&sign_type={GameInfo.sign_type}" +
                     $"&timestamp={timeNow}" +
                     $"&version={GameInfo.version}" +
                     $"&key={GameInfo.key}";
        return s;
    }
    public static string CalculateMD5(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            // 将输入字符串转换为字节数组
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            // 计算哈希值
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            // 根据 Java 示例的逻辑构建结果字符串
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                // 这里模仿了 Java 中的处理逻辑
                sb.Append(Convert.ToString((b & 0xFF) | 0x100, 16).Substring(1, 2)); // 确保持续两位
            }
            return sb.ToString().ToUpper(); // 转换为大写
        }
    }

    public static string PostDataGetHtml(string url, string postData, List<string> header = null, List<string> content= null)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);
            Uri uri = new Uri(url);
            HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
            if (req == null)
            {
                return "NetworkError@" + new ArgumentNullException("httpWebRequest_1:").Message;
            }



            StringBuilder requestLog = new StringBuilder();

            if (header.Count>0 && content.Count > 0)
            {
                for (int i = 0; i < header.Count; i++)
                {
                    req.Headers.Add(header[i], content[i]);
                    requestLog.AppendLine($"Header: {header[i]} = {content[i]}");
                }
            }
            req.Method = "POST";
          
            req.Accept = "application/json";
            req.ContentType = "application/json;charset=utf-8";
            // 移除显式设置连接属性的行
            // req.Connection = "Keep-Alive"; // 注释掉或删除
            req.UserAgent = "okhttp/4.12.0";
            req.ServicePoint.Expect100Continue = false;

            // 报文打印
            //requestLog.AppendLine($"Request Method: {req.Method}");
            //requestLog.AppendLine($"Creating HttpWebRequest for URL: {url}");
            //requestLog.AppendLine($"Content Type: {req.ContentType}");
            //requestLog.AppendLine($"User Agent: {req.UserAgent}");
            //requestLog.AppendLine($"postData: {postData}");
            //Debug.Log(requestLog.ToString());


            Stream outStream = req.GetRequestStream();
            outStream.Write(data, 0, data.Length);
            outStream.Close();
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            if (res == null)
            {

                return "NetworkError@" + new ArgumentNullException("HttpWebResponse_2:").Message;
            }

            Stream inStream = res.GetResponseStream();
            var sr = new StreamReader(inStream, Encoding.UTF8);
            string htmlResult = sr.ReadToEnd();
            //Debug.Log(htmlResult);
            return htmlResult;
        }
        catch (Exception ex)
        {
            return "NetworkError@" + ex.Message;
        }
    }

    //public IEnumerator PostDataGetHtml(string url, string postData, List<string> headers = null, List<string> contents = null,System.Action<string> callback = null)
    //{
    //    // 创建 UnityWebRequest
    //    using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, postData))
    //    {
    //        // 设置请求头
    //        //if (headers != null)
    //        //{
                
    //        //    foreach (var header in headers)
    //        //    {
    //        //        www.SetRequestHeader(header, "YourValueHere"); // 你可以根据需求设置请求头的值
    //        //    }
    //        //}
    //        if (headers.Count > 0 && contents.Count > 0)
    //        {
    //            for (int i = 0; i < headers.Count; i++)
    //            {
    //                www.SetRequestHeader(headers[i], contents[i]);
    //            }
    //        }

    //        // 设置ContentType
    //        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postData));
    //        www.downloadHandler = new DownloadHandlerBuffer();
    //        www.SetRequestHeader("Content-Type", "application/json;charset=utf-8");

    //        // 发送请求并等待响应
    //        yield return www.SendWebRequest();

    //        // 处理错误
    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.LogError("NetworkError@" + www.error);
    //            callback?.Invoke("NetworkError@" + www.error);
    //        }
    //        else
    //        {
    //            // 请求成功，获取返回的数据
    //            string htmlResult = www.downloadHandler.text;
    //            Debug.Log(htmlResult);
    //            callback?.Invoke(htmlResult);
    //        }
    //    }
    //}
    ///// <summary>
    /// HttpWebRequest 通过Post
    /// </summary>
    /// <param name="url">URI</param>
    /// <param name="postData">post数据</param>
    /// <returns></returns>
    public static string PostDataGetHtml(string url, string postData,string header=null,string content=null )
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);
            Uri uri = new Uri(url);
            HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
            if (req == null)
            {
                return "NetworkError@" + new ArgumentNullException("httpWebRequest").Message;
            }
            if(header!=null&&content!=null)
            {
                
                req.Headers.Add(header, content);
            }
           
            req.Method = "POST";
            req.ContentType = "application/json";
            req.ServicePoint.Expect100Continue = false;
            Stream outStream = req.GetRequestStream();
            outStream.Write(data, 0, data.Length);
            outStream.Close();
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            if (res == null)
            {
                
                return "NetworkError@" + new ArgumentNullException("HttpWebResponse").Message;
            }
            Stream inStream = res.GetResponseStream();
            var sr = new StreamReader(inStream, Encoding.UTF8);
            string htmlResult = sr.ReadToEnd();
            return htmlResult;
        }
        catch (Exception ex)
        {
            return "NetworkError@" + ex.Message;
        }
    }
    public static string PostDownLoadDataGetHtml(string url,  string header = null, string content = null)
    {
        try
        {
            Uri uri = new Uri(url);
            HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
            if (req == null)
            {
                return "NetworkError@" + new ArgumentNullException("httpWebRequest").Message;
            }
            if (header != null && content != null)
            {

                req.Headers.Add(header, content);
            }

            req.Method = "POST";
            req.ContentType = "application/json";
            req.ServicePoint.Expect100Continue = false;
            //Stream outStream = req.GetRequestStream();
            //outStream.Write(data, 0, data.Length);
            //outStream.Close();
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            if (res == null)
            {

                return "NetworkError@" + new ArgumentNullException("HttpWebResponse").Message;
            }
            Stream inStream = res.GetResponseStream();
            var sr = new StreamReader(inStream, Encoding.UTF8);
            string htmlResult = sr.ReadToEnd();
            return htmlResult;
        }
        catch (Exception ex)
        {
            return "NetworkError@" + ex.Message;
        }
    }
    public static bool JudgeDataPostHtml(string url, string postData, ref string responseContent)
    {
        bool flag = false;
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);

            Uri uri = new Uri(url);
            HttpWebRequest req = WebRequest.Create(uri) as HttpWebRequest;
            if (req == null)
            {
                flag = false;
                responseContent = "null";
            }
            req.Method = "POST";
            req.KeepAlive = true;
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;
            req.AllowAutoRedirect = true;

            Stream outStream = req.GetRequestStream();
            outStream.Write(data, 0, data.Length);
            outStream.Close();

            var res = req.GetResponse() as HttpWebResponse;
            if (res.StatusCode == HttpStatusCode.OK)
            {
                flag = true;
                Stream inStream = res.GetResponseStream();
                if (inStream == null)
                {
                    flag = false;
                    responseContent = "null";
                }
                var sr = new StreamReader(inStream, Encoding.UTF8);
                responseContent = sr.ReadToEnd();
            }
            else
            {
                flag = false;
                responseContent = "null";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            System.Diagnostics.Debug.WriteLine(ex.Message);
            flag = false;
            responseContent = "null";
        }
        return flag;
    }
    public static bool JudgeDataGetHtml(string url, ref string responseContent, ref string StatusCode)
    {
        bool flag = false;
        try
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "GET";
            //对发送的数据不使用缓存
            httpWebRequest.AllowWriteStreamBuffering = false;
            httpWebRequest.Timeout = timeout;
            httpWebRequest.ServicePoint.Expect100Continue = false;

            HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();
            StatusCode = webRespon.StatusCode.ToString();
            if (webRespon.StatusCode == HttpStatusCode.OK)
            {
                flag = true;
                Stream webStream = webRespon.GetResponseStream();
                if (webStream == null)
                {
                    flag = false;
                    responseContent = "null";
                }
                StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
                responseContent = streamReader.ReadToEnd();
                webRespon.Close();
                streamReader.Close();
            }
            else
            {
                flag = false;
                responseContent = "null";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); 
            flag = false;
            responseContent = "null";
            StatusCode = ex.Message;
        }
        return flag;
    }
    /// <summary>
    /// HttpWebRequest 通过get
    /// </summary>
    /// <param name="url">URI</param>
    /// <returns></returns>
    public static string GetDataGetHtml(string url)
    {
        try
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "GET";
            //对发送的数据不使用缓存
            httpWebRequest.AllowWriteStreamBuffering = false;
            httpWebRequest.Timeout = timeout;
            httpWebRequest.ServicePoint.Expect100Continue = false;

            HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream webStream = webRespon.GetResponseStream();
            if (webStream == null)
            {
                return "网络错误(Network error)：" + new ArgumentNullException("webStream");
            }
            StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
            string responseContent = streamReader.ReadToEnd();

            webRespon.Close();
            streamReader.Close();

            return responseContent;
        }
        catch (Exception ex)
        {
            return "网络错误(Network error)：" + ex.Message;
        }
    }

    /// <summary>
    /// HttpWebRequest 通过get
    /// </summary>
    /// <param name="url">URI</param>
    /// <returns></returns>
    public static bool GetDataGetHtml(string url, string filePath, ref string mg)
    {
        try
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "GET";
            //对发送的数据不使用缓存
            httpWebRequest.AllowWriteStreamBuffering = false;
            httpWebRequest.Timeout = timeout;
            httpWebRequest.ServicePoint.Expect100Continue = false;

            HttpWebResponse webRespon = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream webStream = webRespon.GetResponseStream();
            if (webStream == null)
            {
                return false;
            }
            StreamReader streamReader = new StreamReader(webStream, Encoding.UTF8);
            string responseContent = streamReader.ReadToEnd();
            mg = responseContent;
            webRespon.Close();
            streamReader.Close();
            if (responseContent.ToUpper().IndexOf("NULL") > -1)
            {
                return false;
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                var buff = Encoding.UTF8.GetBytes(responseContent);
                fs.Write(buff, buff.Length, 0);
                fs.Close();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); 
            return false;
        }
    }


    /// <summary>
    /// 将本地文件上传到指定的服务器(HttpWebRequest方法)
    /// </summary>
    /// <param name="address">文件上传到的服务器</param>
    /// <param name="fileNamePath">要上传的本地文件（全路径）</param>
    /// <param name="saveName">文件上传后的名称</param>
    /// <returns>成功返回1，失败返回0</returns> 
    public static int Upload_Request(string address, string fileNamePath, string saveName)
    {
        // 要上传的文件
        try
        {
            if (!File.Exists(fileNamePath))
            {
                return 0;
            }
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            return Upload_Request(address, fs, saveName);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return 0;
        }
    }

    /// <summary>
    /// 将本地文件流上传到指定的服务器(HttpWebRequest方法)
    /// </summary>
    /// <param name="address">文件上传到的服务器</param>
    /// <param name="fileStream">要上传的本地文件流</param>
    /// <param name="saveName">文件上传后的名称</param>
    /// <returns>成功返回1，失败返回0</returns> 
    public static int Upload_Request(string address, Stream fileStream, string saveName)
    {
        int returnValue = 0;
        fileStream.Position = 0;
        var r = new BinaryReader(fileStream);
        //时间戳
        string strBoundary = "----------" + DateTime.Now.Ticks.ToString("x");
        byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");
        //请求头部信息
        StringBuilder sb = new StringBuilder();
        sb.Append("--");
        sb.Append(strBoundary);
        sb.Append("\r\n");
        sb.Append("Content-Disposition: form-data; name=\"");
        sb.Append("file");
        sb.Append("\"; filename=\"");
        sb.Append(saveName);
        sb.Append("\"");
        sb.Append("\r\n");
        sb.Append("Content-Type: ");
        sb.Append("application/octet-stream");
        sb.Append("\r\n");
        sb.Append("\r\n");
        string strPostHeader = sb.ToString();
        byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);
        try
        {
            // 根据uri创建HttpWebRequest对象
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(new Uri(address));
            httpReq.Method = "POST";
            //对发送的数据不使用缓存
            httpReq.AllowWriteStreamBuffering = false;
            //设置获得响应的超时时间（300秒）
            httpReq.Timeout = timeout;
            httpReq.ServicePoint.Expect100Continue = false;
            httpReq.ContentType = "multipart/form-data; boundary=" + strBoundary;
            long length = fileStream.Length + postHeaderBytes.Length + boundaryBytes.Length;
            long fileLength = fileStream.Length;
            httpReq.ContentLength = length;
            byte[] buffer = new byte[fileLength];
            Stream postStream = httpReq.GetRequestStream();
            //发送请求头部消息
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            int size = r.Read(buffer, 0, buffer.Length);
            postStream.Write(buffer, 0, size);
            //添加尾部的时间戳
            postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
            postStream.Close();
            //获取服务器端的响应
            HttpWebResponse webRespon = (HttpWebResponse)httpReq.GetResponse();
            if (webRespon.StatusCode == HttpStatusCode.OK) //如果服务器未响应，那么继续等待相应                 
            {
                Stream s = webRespon.GetResponseStream();
                StreamReader sr = new StreamReader(s);
                //读取服务器端返回的消息
                String sReturnString = sr.ReadLine();
                s.Close();
                sr.Close();
                fileStream.Close();
                if (sReturnString == "Success")
                {
                    returnValue = 1;
                }
                else
                {
                    returnValue = 0;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            returnValue = 0;
        }
        return returnValue;
    }
    /// <summary>
    /// 将本地文件上传到指定服务器上（HttpWebRequest方法），并传递相应参数
    /// </summary>
    /// <param name="url">文件上传到的服务器</param>
    /// <param name="fileKeyName">类型（此处为文件--file）</param>
    /// <param name="filePath">要上传的本地文件（全路径）</param>
    /// <param name="filename">文件上传后的名称</param>
    /// <param name="stringDict">参数集合</param>
    /// <param name="timeOut">请求时效</param>
    /// <returns></returns>
    public static string HttpPostData(string url, string fileKeyName, string filePath, string filename, NameValueCollection stringDict, int timeOut = 900000)
    {
        string responseContent;
        try
        {
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // 最后的结束符
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 设置属性
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // 写入文件
            const string filePartHeader = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" + "Content-Type: application/octet-stream\r\n\r\n";
            var header = string.Format(filePartHeader, fileKeyName, filename);
            var headerbytes = Encoding.UTF8.GetBytes(header);

            memStream.Write(beginBoundary, 0, beginBoundary.Length);
            memStream.Write(headerbytes, 0, headerbytes.Length);

            var buffer = new byte[1024];
            int bytesRead; // =0

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }

            // 写入字符串的Key
            var stringKeyHeader = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"" + "\r\n\r\n{1}\r\n";

            foreach (byte[] formitembytes in from string key in stringDict.Keys select string.Format(stringKeyHeader, key, stringDict[key]) into formitem select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符
            memStream.Write(endBoundary, 0, endBoundary.Length);

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            fileStream.Close();
            httpWebResponse.Close();
            webRequest.Abort();
        }
        catch (Exception ex)
        {
            responseContent = ex.Message;
        }
        return responseContent;
    }
    /// <summary>
    /// Http下载文件支持断点续传
    /// </summary>
    /// <param name="uri">下载地址</param>
    /// <param name="filefullpath">存放完整路径（含文件名）</param>
    /// <param name="size">每次多的大小</param>
    /// <returns>下载操作是否成功</returns>
    public static bool HttpDownLoadFiles(string uri, string filefullpath, int size = 1024)
    {
        try
        {
            string fileDirectory = System.IO.Path.GetDirectoryName(filefullpath);
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            string fileFullPath = filefullpath;
            string fileTempFullPath = filefullpath + ".tmp";

            if (File.Exists(fileFullPath))
            {
                return true;
            }
            else
            {
                if (File.Exists(fileTempFullPath))
                {
                    FileStream fs = new FileStream(fileTempFullPath, FileMode.Append, FileAccess.Write, FileShare.Write);

                    byte[] buffer = new byte[size];
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

                    request.Timeout = timeout;
                    request.AddRange((int)fs.Length);

                    Stream ns = request.GetResponse().GetResponseStream();

                    long contentLength = request.GetResponse().ContentLength;

                    int length = ns.Read(buffer, 0, buffer.Length);

                    while (length > 0)
                    {
                        fs.Write(buffer, 0, length);

                        buffer = new byte[size];

                        length = ns.Read(buffer, 0, buffer.Length);
                    }

                    fs.Close();
                    File.Move(fileTempFullPath, fileFullPath);

                }
                else
                {
                    FileStream fs = new FileStream(fileTempFullPath, FileMode.Create);

                    byte[] buffer = new byte[size];
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    request.Timeout = timeout;
                    request.AddRange((int)fs.Length);

                    Stream ns = request.GetResponse().GetResponseStream();

                    long contentLength = request.GetResponse().ContentLength;

                    int length = ns.Read(buffer, 0, buffer.Length);

                    while (length > 0)
                    {
                        fs.Write(buffer, 0, length);

                        buffer = new byte[size];

                        length = ns.Read(buffer, 0, buffer.Length);
                    }

                    fs.Close();
                    File.Move(fileTempFullPath, fileFullPath);

                }
                return true;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public static void HttpDownLoadFiles(string uri, string filefullpath, Action<string, bool> action, int size = 1024)
    {
        try
        {
            string fileDirectory = System.IO.Path.GetDirectoryName(filefullpath);

            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            string fileFullPath = filefullpath;
            string fileTempFullPath = filefullpath + ".tmp";
            if (File.Exists(fileFullPath))
            {
                action?.Invoke(uri, true);
            }
            else
            {
                if (File.Exists(fileTempFullPath))
                {
                    FileStream fs = new FileStream(fileTempFullPath, FileMode.Append, FileAccess.Write, FileShare.Write);

                    byte[] buffer = new byte[size];
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

                    request.Timeout = timeout;
                    request.AddRange((int)fs.Length);
                    HttpWebResponse hwr = request.GetResponse() as HttpWebResponse;
                    Stream ns = hwr.GetResponseStream();
                    int length = ns.Read(buffer, 0, buffer.Length);
                    while (length > 0)
                    {
                        fs.Write(buffer, 0, length);
                        buffer = new byte[size];

                        length = ns.Read(buffer, 0, buffer.Length);
                    }

                    fs.Close();
                    File.Move(fileTempFullPath, fileFullPath);
                    action?.Invoke(uri, true);

                }
                else
                {
                    FileStream fs = new FileStream(fileTempFullPath, FileMode.Create);

                    byte[] buffer = new byte[size];
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    request.Timeout = timeout;
                    HttpWebResponse hwr = request.GetResponse() as HttpWebResponse;
                    Stream ns = hwr.GetResponseStream();
                    int length = ns.Read(buffer, 0, buffer.Length);
                    while (length > 0)
                    {
                        fs.Write(buffer, 0, length);
                        buffer = new byte[size];
                        length = ns.Read(buffer, 0, buffer.Length);
                    }

                    fs.Close();
                    File.Move(fileTempFullPath, fileFullPath);
                    action?.Invoke(uri, true);

                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            action?.Invoke(uri, false);
        }
    }
    /// <summary>
    /// Http下载文件
    /// </summary>
    /// <param name="uri">下载地址</param>
    /// <param name="filefullpath">存放完整路径（含文件名）</param>
    /// <param name="size">每次多的大小</param>
    /// <returns>下载操作是否成功</returns>
    public static bool DownLoadFiles(string uri, string filefullpath, int size = 1024)
    {
        try
        {
            if (File.Exists(filefullpath))
            {
                try
                {
                    File.Delete(filefullpath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            string fileDirectory = System.IO.Path.GetDirectoryName(filefullpath);
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            FileStream fs = new FileStream(filefullpath, FileMode.Create);
            byte[] buffer = new byte[size];
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Timeout = timeout;
            request.AddRange((int)fs.Length);

            Stream ns = request.GetResponse().GetResponseStream();

            long contentLength = request.GetResponse().ContentLength;

            int length = ns.Read(buffer, 0, buffer.Length);

            while (length > 0)
            {
                fs.Write(buffer, 0, length);

                buffer = new byte[size];

                length = ns.Read(buffer, 0, buffer.Length);
            }
            fs.Close();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public static bool DownLoadFiles(string uri, string filefullpath, ProgressHandle progressHandle, int size = 1024)
    {
        long all = 0;
        long current = 0;
        try
        {
            if (File.Exists(filefullpath))
            {
                try
                {
                    File.Delete(filefullpath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            string fileDirectory = System.IO.Path.GetDirectoryName(filefullpath);
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            FileStream fs = new FileStream(filefullpath, FileMode.Create);
            byte[] buffer = new byte[size];
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Timeout = timeout;
            request.AddRange((int)fs.Length);

            Stream ns = request.GetResponse().GetResponseStream();

            all = ns.Length;
            long contentLength = request.GetResponse().ContentLength;

            int length = ns.Read(buffer, 0, buffer.Length);

            while (length > 0)
            {
                fs.Write(buffer, 0, length);

                buffer = new byte[size];
                current += length;
                progressHandle.DownloadProgress = (float)current / all;
                length = ns.Read(buffer, 0, buffer.Length);
            }
            if (progressHandle.downloadFinsh != null)
            {
                progressHandle.downloadFinsh.Invoke(progressHandle.Key);
            }
            fs.Close();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }


    public static bool HttpDownLoadFiles(string uri, string filefullpath, ProgressHandle progressHandle, int size = 1024)
    {
        long all = 0;
        long current = 0;
        try
        {
            string fileDirectory = System.IO.Path.GetDirectoryName(filefullpath);
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            string fileFullPath = filefullpath;
            string fileTempFullPath = filefullpath + ".tmp";

            if (File.Exists(fileFullPath))
            {
                progressHandle.DownloadProgress = 1;
                if (progressHandle.downloadFinsh != null)
                {
                    progressHandle.IsDownLoadFinsh = true;
                    progressHandle.downloadFinsh.Invoke(progressHandle.Key);
                }
                return true;
            }
            else
            {
                if (File.Exists(fileTempFullPath))
                {
                    FileStream fs = new FileStream(fileTempFullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    request.Timeout = timeout;
                    request.AddRange((int)fs.Length);
                    current += fs.Length;
                    all += fs.Length;
                    object[] objs = new object[] { request, all, current, size, progressHandle, fs, fileTempFullPath, fileFullPath };
                    request.BeginGetResponse(CallBack, objs);

                }
                else
                {
                    FileStream fs = new FileStream(fileTempFullPath, FileMode.Create);
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                    request.Timeout = timeout;
                    object[] objs = new object[] { request, all, current, size, progressHandle, fs, fileTempFullPath, fileFullPath };
                    request.BeginGetResponse(CallBack, objs);
                }
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    static void CallBack(IAsyncResult asyncResult)
    {
        object[] objs = asyncResult.AsyncState as object[];
        HttpWebRequest hwr = objs[0] as HttpWebRequest;
        WebResponse wr = hwr.EndGetResponse(asyncResult);
        long all = (long)objs[1];
        long current = (long)objs[2];
        int size = (int)objs[3];
        ProgressHandle progressHandle = objs[4] as ProgressHandle;
        FileStream fs = objs[5] as FileStream;
        string fileTempFullPath = objs[6].ToString();
        string fileFullPath = objs[7].ToString();
        all += wr.ContentLength;
        Stream ns = wr.GetResponseStream();
        byte[] buffer = new byte[size];
        int length = ns.Read(buffer, 0, buffer.Length);
        while (length > 0)
        {
            fs.Write(buffer, 0, length);

            buffer = new byte[size];
            current += length;
            progressHandle.DownloadProgress = (float)current / all;
            length = ns.Read(buffer, 0, buffer.Length);
        }
        fs.Close();
        File.Move(fileTempFullPath, fileFullPath);
        if (progressHandle.downloadFinsh != null)
        {
            progressHandle.IsDownLoadFinsh = true;
            progressHandle.downloadFinsh.Invoke(progressHandle.Key);
        }
    }
}