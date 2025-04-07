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
/// ��ʼ������������ϸ�Native���͵���Ϣ
/// </summary>
[Serializable]
public class InitSceneToNativeData
{
    public string scene_Name;
  
}
/// <summary>
/// ����native���͵���ϸ����
/// </summary>
[Serializable]
public class ReturenNativeData
{
    public string scene_name;
}


/// <summary>
/// Native�����ĳ�����ת��Ϣ+�û�id+token
/// </summary>
[Serializable]
public class LoadSceneData
{
    //����
    public string scene_name;
    public string isDebug;
    public string user_id;
    public string user_token;
    //AR
    public string capsule_data;//������Ϣ
    public string user_location;//����γ�ȣ�longitude��latitude
}

public class UserLocation
{
    public double longitude;//����
    public double latitude;//γ��
}

public class CapsuleLocation
{
    public double longitude_capsule;//����
    public double latitude_capsule;//γ��
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
/// ���͵�Native����Ϣ������+����
/// </summary>
[Serializable]
public class ToNativeData
{
    public string command;

    public string data;
}
/// <summary>
/// ���͵�Unity����Ϣ������+����
/// </summary>
public class ToUnityData
{
    public string command;

    public string data;
}
/// <summary>
/// �����û����ݰ���
/// </summary>
[Serializable]
public class UserInfoData
{
    public string target_user_id;
    public string user_id;
}
/// <summary>
/// �����û���������
/// </summary>
[Serializable]
public class UserLikeData
{
    public string cid;
    public string user_id;
}

/// <summary>
/// ����AR��֤
/// </summary>
[Serializable]
public class UserARCheckData
{
    public string user_id;
    public string cid;
    public double longitude;//����
    public double latitude;
}

/// <summary>
/// �����û���ע���ݰ���
/// </summary>
[Serializable]
public class UserFocusData
{
    public string user_id;
    public string followed_id;
}

/// <summary>
/// �����û��Ӻ������ݰ���
/// </summary>
[Serializable]
public class UserAddFriendData
{
    public string user_id;
    public string friend_id;
    public string app_type;
}

/// <summary>
/// �û�ʱ�ս�������
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
    public int page_no;           // ��ǰҳ��
    public int page_size;         // ÿҳ��С
    public string user_id;        // �û�ID
    public string creator;       // ������
    public string start_time;     // ��ʼʱ��
    public string end_time;       // ����ʱ��
    public List<SortOrder> order;    // �������
    // ������ӹ��캯����������
}

/// <summary>
/// �����û��������
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
/// �û�������������
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
    public string data ;  // ����һ�� JSON �ַ������Ժ���Է����л�Ϊ RequestHeader
    public string format ;
    public string sdk_version ;
    public string sign ;
    public string version ;
    public string sign_type ;
    public string app_id ;
    public string platform ;
    public string req_no ;
    public string timestamp ; // ����ʹ�� DateTime ����
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
    public string timestamp ; // ʹ�� long ���ʹ���ʱ���
    public string version ;
}



