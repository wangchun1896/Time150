using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpTest : MonoBehaviour
{
    private void Start()
    {
        //UserInfoData userInfoData = new UserInfoData { user_id = GameInfo.tempUser_Id };
        //string j= JsonUtility.ToJson(userInfoData);
        //HttpTestFunc(GameInfo.userInfoInterface,j);
    }
    public static string HttpTestFunc( string interfaceName, string interfaceParam)
    {
        string path;
        if (GameInfo.IsDebug == "true")
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
        
        RequestData tt_req_header_content = new RequestData
        {
            app_type = GameInfo.app_type,
            device = GameInfo.device,
            device_id = "e1389f78-5019-4b6f-8b89-f24504f1d953",
            authorization = "Bearer eyJhbGciOiJIUzI1NiJ9.eyJ1c2VyX2lkIjoiNzI5NjA0ODU2MDQyODI3MzY2NCIsImlzcyI6InRvbmd0b25nIiwic3ViIjoiNzI5NjA0ODU2MDQyODI3MzY2NCIsImV4cCI6MTc0MzY1MTc0NCwiaWF0IjoxNzQxMDU5NzQ0fQ.5HQdzlYVAEogzYeRHngNYknxAO7kRV7NHPJxGEdR81I",
            user_agent = "Androidv1.8.0/2201123C",
            accept_encoding = "gzip",
            login_type = "1",
            timezone = "8",
            lang = "zh",
            timestamp = currentTimeStampMilliseconds.ToString(),
            version = "2.0.2",
        };
        string tt_req_header = JsonUtility.ToJson(tt_req_header_content);
        b.Add(tt_req_header);

        //验签
        string m_sign = CalculateMD5(BuildRequestString(interfaceParam, currentTimeStampMilliseconds));
        DataCheckStore dataCheckStore = new DataCheckStore
        {
            //app_id = "",
            //charset = "UTF-8",
            //data = interfaceParam,
            //format = "JSON",
            //req_no = currentTimeStampMilliseconds.ToString(),
            //sign_type = "MD5",


            app_id = GameInfo.app_id,
            charset = GameInfo.charset,
            data = interfaceParam,
            format = GameInfo.format,
            req_no = currentTimeStampMilliseconds.ToString(),
            timestamp = currentTimeStampMilliseconds.ToString(),
            sign_type = GameInfo.sign_type,
            version = GameInfo.version,
            sign = m_sign,

            //app_id = 1234 &
            //charset = UTF - 8 &
            //data ={ "a":"b"}&
            //format=JSON&
            //req_no=1742781633915&
            //sign_type=MD5&
            //version=1.0&
            //key=key
        };
        string data = JsonUtility.ToJson(dataCheckStore);
        string s =HttpHelper.PostDataGetHtml(path, data, h, b);
        return s;
    }
    private static string BuildRequestString(string interfaceData,long timeNow)
    {
        if (GameInfo.IsDebug != "true")
        {
            GameInfo.app_id = GameInfo.app_id_shengchan;
            GameInfo.key = GameInfo.key_shengchan; //shengchan
        }
        string s= $"app_id={GameInfo.app_id}" +
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


}
