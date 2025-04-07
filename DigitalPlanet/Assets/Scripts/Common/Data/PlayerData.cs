using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData 
{
  
}
[Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class Rotation
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class NativeMessage
{
    public Position position;
    public Rotation rotation;
    public float modelType;
    public string id;
    public string data;
    public List<string> ids;
}

/// <summary>
/// 初始化场景加载完毕给Native发送的消息
/// </summary>
[Serializable]
public class InitSceneToNativeData
{
    public string scene_Name;
  
}
/// <summary>
/// 返回native发送的详细数据
/// </summary>
[Serializable]
public class ReturenNativeData
{
    public string scene_name;
}


/// <summary>
/// Native发来的场景跳转消息+用户id+token
/// </summary>
[Serializable]
public class LoadSceneData
{
    //公共
    public string scene_name;
    public string isDebug;
    public string user_id;
    public string user_token;
    //AR
    public string capsule_data;//胶囊信息
    public string user_location;//经度纬度，longitude，latitude
}

public class UserLocation
{
    public double longitude;//经度
    public double latitude;//纬度
}

public class CapsuleLocation
{
    public double longitude_capsule;//经度
    public double latitude_capsule;//纬度
}

[Serializable]
public class TargetSceneData
{
    public string target_id;
}
[Serializable]
public class IdData
{
    public string userid;
}

[Serializable]
public class TTUserIdData
{
    public string tt_user_id;
}

/// <summary>
/// 发送到Native的消息，命令+数据
/// </summary>
[Serializable]
public class ToNativeData
{
    public string command;

    public string data;
}
/// <summary>
/// 发送到Unity的消息，命令+数据
/// </summary>
public class ToUnityData
{
    public string command;

    public string data;
}
/// <summary>
/// 请求用户数据包体
/// </summary>
[Serializable]
public class UserInfoData
{
    public string target_user_id;
    public string user_id;
}
/// <summary>
/// 请求用户点赞数据
/// </summary>
[Serializable]
public class UserLikeData
{
    public string cid;
    public string user_id;
}

/// <summary>
/// 请求AR验证
/// </summary>
[Serializable]
public class UserARCheckData
{
    public string user_id;
    public string cid;
    public double longitude;//经度
    public double latitude;
}

/// <summary>
/// 请求用户关注数据包体
/// </summary>
[Serializable]
public class UserFocusData
{
    public string user_id;
    public string followed_id;
}

/// <summary>
/// 请求用户加好友数据包体
/// </summary>
[Serializable]
public class UserAddFriendData
{
    public string user_id;
    public string friend_id;
    public string app_type;
}

/// <summary>
/// 用户时空胶囊数据
/// </summary>
[Serializable]
public class UserTimeCapsuleData
{
    public int page_no;
    public int page_size;
    public string user_id;
    public string creator;
    public string start_time;
    public string end_time;
    public string sign_code;
    public List<string> tags;
    public int sex;
    public string content;
    public string age;
    public List<SortOrder> order;
    
}

[Serializable]
public class UserTimeStoryData
{
    public int page_no;           // 当前页码
    public int page_size;         // 每页大小
    public string user_id;        // 用户ID
    public string creator;       // 创建者
    public string start_time;     // 开始时间
    public string end_time;       // 结束时间
    public List<SortOrder> order;    // 排序参数
    // 可以添加构造函数、方法等
}

/// <summary>
/// 请求用户广告数据
/// </summary>
[Serializable]
public class UserAdvertisementInfoData
{
    public string user_id;
    public string space_id;
}
[Serializable]
public class SortOrder
{
    public string order_column;
    public string order_type;
}
/// <summary>
/// 用户星座运势数据
/// </summary>
[Serializable]
public class UserStarLuckData
{
    public string user_id;
    public string sign_code;
    public string search_date;
}

[Serializable]
public class DataCheckStore
{
    public string charset ;
    public string data ;  // 这是一个 JSON 字符串，稍后可以反序列化为 RequestHeader
    public string format ;
    public string sdk_version ;
    public string sign ;
    public string version ;
    public string sign_type ;
    public string app_id ;
    public string platform ;
    public string req_no ;
    public string timestamp ; // 可以使用 DateTime 类型
}

/// <summary>
/// 
/// </summary>
[Serializable]
public class RequestData
{
    public string app_type ;
    public string device ;
    public string device_id ;
    public string authorization ;
    public string user_agent ;
    public string accept_encoding ;
    public string login_type ;
    public string timezone ;
    public string lang ;
    public string timestamp ; // 使用 long 类型处理时间戳
    public string version ;
}



