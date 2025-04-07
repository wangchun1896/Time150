using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public string userInforData;
    public bool isTarget = false;
    private void OnEnable()
    {
        InitHttpData_main();
    }
   
   
    public void InitHttpData_main()
    {
#if UNITY_EDITOR
        isTarget = !string.Equals(GameInfo.tempUser_Id, GameInfo.tempTargetUser_Id);//�����ȾͲ���Target״̬
#else
        isTarget = !string.Equals(GameInfo.User_Id, GameInfo.Target_Id);
#endif
        if (!isTarget)
        {
            
           // Debug.Log("��ǰ���û�->�Լ�");
            //�����û�����
            RequestUserInforData_main();
            //����������
            RequestUserTimeCapsuleData_main();
            //�����������
            RequestUserTimeStory_main();
        }else
        {
           // Debug.Log("��ǰ���û�->Ŀ���û�");
            //����Target�û�����
            RequestUserInforData_main_target();
            //����Target��������
            RequestUserTimeCapsuleData_main_target();
            //����Target��������
            RequestUserTimeStory_main_target();
        }
        //�����û��������
        RequestUserAdvertisementInforData_main();
    }
    public void InitHttpData_star()
    {
        //Debug.Log("��ǰ���û�->�Լ�");
        //�����û�����
        RequestUserInforData_star();
        //������������
        RequestUserStarLuckData_star();
        //������������
        RequestUserTimeCapsuleData_star();
        //#if UNITY_EDITOR
        //        isTarget = !string.Equals(GameInfo.tempUser_Id, GameInfo.tempTargetUser_Id);
        //#else
        //        isTarget = !string.Equals(GameInfo.User_Id, GameInfo.Target_Id);
        //#endif
        //        if (!isTarget)
        //        {
        //            Debug.Log("��ǰ���û�->�Լ�");
        //            //�����û�����
        //            RequestUserInforData_star();
        //            //������������
        //            RequestUserStarLuckData_star();
        //            //������������
        //            RequestUserTimeCapsuleData_star();
        //        }
        //        else
        //        {
        //            Debug.Log("��ǰ���û�->Ŀ���û�");
        //            //����Target�û�����
        //            RequestUserInforData_star_target();
        //            //����Target��������
        //            RequestUserStarLuckData_star_target();
        //            //����Target��������
        //            RequestUserTimeCapsuleData_star_target();
        //        }
    }
    //******************1*********************
    /// <summary>
    /// ������-�����û�����
    /// </summary>
    public void RequestUserInforData_main()
    {
#if UNITY_EDITOR
        UserInfoData userInfoData = new UserInfoData { user_id = GameInfo.tempUser_Id,target_user_id= GameInfo.tempUser_Id };
#else
        UserInfoData userInfoData = new UserInfoData { user_id = GameInfo.User_Id,target_user_id= GameInfo.User_Id };
#endif
        string param = JsonUtility.ToJson(userInfoData);
        string needData = GetNeedHttpData(GameInfo.userInfoInterface, param);
        userInforData = needData;
        ActionEventHandler.Instance.Dispatch(GameInfo.userInfo_main_Dispatch_Index, needData);
    }
    /// <summary>
    /// ������-����Target�û�����
    /// </summary>
    public void RequestUserInforData_main_target()
    {
#if UNITY_EDITOR
        UserInfoData userInfoData = new UserInfoData { user_id = GameInfo.tempUser_Id,target_user_id= GameInfo.tempTargetUser_Id };
#else
        UserInfoData userInfoData = new UserInfoData { user_id = GameInfo.User_Id,target_user_id= GameInfo.Target_Id };
#endif
        string param = JsonUtility.ToJson(userInfoData);
        string needData = GetNeedHttpData(GameInfo.userInfoInterface, param);
        userInforData = needData;
        ActionEventHandler.Instance.Dispatch(GameInfo.userInfo_main_Dispatch_Index, needData);
    }
    //*******************1********************
    //*******************2********************
    /// <summary>
    /// ��������-�����û�����
    /// </summary>
    public void RequestUserInforData_star()
    {
#if UNITY_EDITOR
        UserInfoData userInfoData = new UserInfoData { user_id = GameInfo.tempUser_Id, target_user_id = GameInfo.tempUser_Id };
#else
        UserInfoData userInfoData = new UserInfoData { user_id = GameInfo.User_Id,target_user_id= GameInfo.User_Id };
#endif
        string param = JsonUtility.ToJson(userInfoData);
        string needData = GetNeedHttpData(GameInfo.userInfoInterface, param);
        userInforData = needData;
        ActionEventHandler.Instance.Dispatch(GameInfo.userInfo_star_Dispatch_Index, needData);
    }
    /// <summary>
    /// ��������-����Target�û�����
    /// </summary>
    public void RequestUserInforData_star_target()
    {
#if UNITY_EDITOR
        UserInfoData userInfoData = new UserInfoData { user_id = GameInfo.tempUser_Id, target_user_id = GameInfo.tempTargetUser_Id };
#else
        UserInfoData userInfoData = new UserInfoData { user_id = GameInfo.User_Id,target_user_id= GameInfo.Target_Id };
#endif
        string param = JsonUtility.ToJson(userInfoData);
        string needData = GetNeedHttpData(GameInfo.userInfoInterface, param);
        userInforData = needData;
        ActionEventHandler.Instance.Dispatch(GameInfo.userInfo_star_Dispatch_Index, needData);
    }
    //*******************2********************

    //*******************3********************
    /// <summary>
    /// �����û���������
    /// </summary>
    public void RequestUserTimeCapsuleData_main()
    {
        List<SortOrder> orderList = new List<SortOrder>();
        SortOrder order = new SortOrder
        {
            order_column = TimeType.release_time.ToString(),
            order_type = SortType.desc.ToString()
        };
        orderList.Add(order);
       
#if UNITY_EDITOR
        UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
        {
            user_id = GameInfo.tempUser_Id,
            page_no = 1,
            page_size = 10,
            order = orderList,
            creator= GameInfo.tempUser_Id,
            sex=0,
        };
#else
            UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
            {
                user_id = GameInfo.User_Id,
                page_no = 1,
                page_size = 10,
                order = orderList,
                creator= GameInfo.User_Id,
                sex = 0,
            };
#endif
        string param = JsonUtility.ToJson(userTimeCapsuleData);
        string needData = GetNeedHttpData(GameInfo.userTimeCapsuleInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfo_main_Dispatch_Index, needData);
    }
    /// <summary>
    /// ����Target�û���������
    /// </summary>
    public void RequestUserTimeCapsuleData_main_target()
    {
        List<SortOrder> orderList = new List<SortOrder>();
        SortOrder order = new SortOrder
        {
            order_column = TimeType.release_time.ToString(),
            order_type = SortType.desc.ToString()
        };
        orderList.Add(order);

#if UNITY_EDITOR
        UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
        {
            user_id = GameInfo.tempUser_Id,
            page_no = 1,
            page_size = 10,
            order = orderList,
            creator = GameInfo.tempTargetUser_Id,
            sex = 0,
        };
#else
            UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
            {
                user_id = GameInfo.User_Id,
                page_no = 1,
                page_size = 10,
                order = orderList,
                creator= GameInfo.Target_Id,
                sex = 0,
            };
#endif
        string param = JsonUtility.ToJson(userTimeCapsuleData);
        string needData = GetNeedHttpData(GameInfo.userTimeCapsuleInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfo_main_Dispatch_Index, needData);
    }
    //*******************3********************
    //*******************4********************
    /// <summary>
    /// ����-�����û���������
    /// </summary>
    public void RequestUserTimeCapsuleData_star()
    {
        List<SortOrder> orderList = new List<SortOrder>();
        SortOrder order = new SortOrder
        {
            order_column = TimeType.release_time.ToString(),
            order_type = SortType.desc.ToString()
        };
        orderList.Add(order);
        //ȡ������
        //**************ʹ���û�������Ϣȡ������*****���û�������Ͳ�ȡ��********************
        if (string.IsNullOrEmpty(userInforData))
        {
            Debug.Log("Unity�������û���ϢΪ��");
            return;
        }
        JObject userObj = JObject.Parse(userInforData);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if(userObj["data"]==null)
        {
            Debug.Log("Unity��"+ userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if(userData["constellation"]==null)
        {
            Debug.Log("�û����ݴ����û���Ϣû������");
            return;
        }
        //***************************************

        string userStar = userData["constellation"].ToString();
        string starCode= GetStarTypeFromString(userStar).ToString();
#if UNITY_EDITOR
        UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
        {
            user_id = GameInfo.tempUser_Id,
            page_no = 1,
            page_size = 20,
            order = orderList,
           // creator = GameInfo.tempUser_Id,
            sign_code= starCode
        };
#else
            UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
            {
                user_id = GameInfo.User_Id,
                page_no = 1,
                page_size = 20,
                order = orderList,
               // creator= GameInfo.User_Id,
                sign_code=starCode
            };
#endif
        string param = JsonUtility.ToJson(userTimeCapsuleData);
        string needData = GetNeedHttpData(GameInfo.userTimeCapsuleInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfo_star_Dispatch_Index, needData);
    }
    /// <summary>
    /// ����-����Target�û���������
    /// </summary>
    public void RequestUserTimeCapsuleData_star_target()
    {
        List<SortOrder> orderList = new List<SortOrder>();
        SortOrder order = new SortOrder
        {
            order_column = TimeType.release_time.ToString(),
            order_type = SortType.desc.ToString()
        };
        orderList.Add(order);
        //ȡ������
        //**************ʹ���û�������Ϣȡ������*****���û�������Ͳ�ȡ��********************
        if (string.IsNullOrEmpty(userInforData))
        {
            Debug.Log("Unity�������û���ϢΪ��");
            return;
        }
        JObject userObj = JObject.Parse(userInforData);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if (userObj["data"] == null)
        {
            Debug.Log("Unity��" + userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if (userData["constellation"] == null)
        {
            Debug.Log("�û����ݴ����û���Ϣû������");
            return;
        }
        //***************************************

        string userStar = userData["constellation"].ToString();
        string starCode = GetStarTypeFromString(userStar).ToString();
#if UNITY_EDITOR
        UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
        {
            user_id = GameInfo.tempTargetUser_Id,
            page_no = 1,
            page_size = 20,
            order = orderList,
           // creator = GameInfo.tempTargetUser_Id,
            sign_code = starCode
        };
#else
            UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
            {
                user_id = GameInfo.Target_Id,
                page_no = 1,
                page_size = 20,
                order = orderList,
                //creator= GameInfo.Target_Id,
                sign_code=starCode
            };
#endif
        string param = JsonUtility.ToJson(userTimeCapsuleData);
        string needData = GetNeedHttpData(GameInfo.userTimeCapsuleInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfo_star_Dispatch_Index, needData);
    }
    //*******************4********************
    //*******************5********************
    /// <summary>
    /// ����ʱ�չ���
    /// </summary>
    public void RequestUserTimeStory_main()
    {
        List<SortOrder> orderList = new List<SortOrder>();
        SortOrder order = new SortOrder
        {
            order_column = TimeType.release_time.ToString(),
            order_type = SortType.desc.ToString()
        };
        orderList.Add(order);

        
#if UNITY_EDITOR
        UserTimeStoryData userTimeCapsuleData = new UserTimeStoryData
        {
            user_id = GameInfo.tempUser_Id,
            page_no = 1,
            page_size = 10,
            order = orderList,
            creator= GameInfo.tempUser_Id
        };
#else
            UserTimeStoryData userTimeCapsuleData = new UserTimeStoryData
            {
                user_id = GameInfo.User_Id,
                page_no = 1,
                page_size = 10,
                order = orderList,
                creator= GameInfo.User_Id
            };
#endif
        string param = JsonUtility.ToJson(userTimeCapsuleData);
        string needData = GetNeedHttpData(GameInfo.userTimeStoryInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeStoryInfo_main_Dispatch_Index, needData);
    }
    /// <summary>
    /// ����ʱ�չ���
    /// </summary>
    public void RequestUserTimeStory_main_target()
    {
        List<SortOrder> orderList = new List<SortOrder>();
        SortOrder order = new SortOrder
        {
            order_column = TimeType.release_time.ToString(),
            order_type = SortType.desc.ToString()
        };
        orderList.Add(order);


#if UNITY_EDITOR
        UserTimeStoryData userTimeCapsuleData = new UserTimeStoryData
        {
            user_id = GameInfo.tempUser_Id,
            page_no = 1,
            page_size = 10,
            order = orderList,
            creator= GameInfo.tempTargetUser_Id,
        };
#else
            UserTimeStoryData userTimeCapsuleData = new UserTimeStoryData
            {
                user_id = GameInfo.User_Id,
                page_no = 1,
                page_size = 10,
                order = orderList,
                creator= GameInfo.Target_Id,
            };
#endif
        string param = JsonUtility.ToJson(userTimeCapsuleData);
        string needData = GetNeedHttpData(GameInfo.userTimeStoryInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeStoryInfo_main_Dispatch_Index, needData);
    }
    //*******************5********************
    //*******************6********************
    /// <summary>
    /// �����û���������
    /// </summary>
    public void RequestUserStarLuckData_star()
    {
        //ȡ������
        //**************ʹ���û�������Ϣȡ������*****���û�������Ͳ�ȡ��********************
        if (string.IsNullOrEmpty(userInforData))
        {
            Debug.Log("Unity�������û���ϢΪ��");
            return;
        }
        JObject userObj = JObject.Parse(userInforData);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if (userObj["data"] == null)
        {
            Debug.Log("Unity��" + userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if (userData["constellation"] == null)
        {
            Debug.Log("�û����ݴ����û���Ϣû������");
            return;
        }
        string userStar = userData["constellation"].ToString();
        string starCode = GetStarTypeFromString(userStar).ToString();
        //***************************************
#if UNITY_EDITOR
        UserStarLuckData userStarLuckData = new UserStarLuckData
        {
            user_id = GameInfo.tempUser_Id,
             sign_code= starCode
        };
#else
           UserStarLuckData userStarLuckData = new UserStarLuckData
        {
            user_id = GameInfo.User_Id,
            sign_code= starCode
        };
#endif
        string param = JsonUtility.ToJson(userStarLuckData);
        string needData = GetNeedHttpData(GameInfo.userStarLuckInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userStarLuckInfo_star_Dispatch_Index, needData);
    }
    /// <summary>
    /// �����û���������
    /// </summary>
    public void RequestUserStarLuckData_star_target()
    {
        //ȡ������
        //**************ʹ���û�������Ϣȡ������*****���û�������Ͳ�ȡ��********************
        if (string.IsNullOrEmpty(userInforData))
        {
            Debug.Log("Unity�������û���ϢΪ��");
            return;
        }
        JObject userObj = JObject.Parse(userInforData);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if (userObj["data"] == null)
        {
            Debug.Log("Unity��" + userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if (userData["constellation"] == null)
        {
            Debug.Log("�û����ݴ����û���Ϣû������");
            return;
        }
        string userStar = userData["constellation"].ToString();
        string starCode = GetStarTypeFromString(userStar).ToString();
        //***************************************
#if UNITY_EDITOR
        UserStarLuckData userStarLuckData = new UserStarLuckData
        {
            user_id = GameInfo.tempTargetUser_Id,
            sign_code = starCode
        };
#else
           UserStarLuckData userStarLuckData = new UserStarLuckData
        {
            user_id = GameInfo.Target_Id,
            sign_code= starCode
        };
#endif
        string param = JsonUtility.ToJson(userStarLuckData);
        string needData = GetNeedHttpData(GameInfo.userStarLuckInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userStarLuckInfo_star_Dispatch_Index, needData);
    }
    //*******************6********************

    //*******************7********************
    /// <summary>
    /// �����û���������,��������
    /// </summary>
    public void RequestUserStarLuckData_star_ByStar(string starName)
    {
        //ȡ������
        //**************ʹ���û�������Ϣȡ������*****���û�������Ͳ�ȡ��********************
        if (string.IsNullOrEmpty(userInforData))
        {
            Debug.Log("Unity�������û���ϢΪ��");
            return;
        }
        JObject userObj = JObject.Parse(userInforData);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if (userObj["data"] == null)
        {
            Debug.Log("Unity��" + userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if (userData["constellation"] == null)
        {
            Debug.Log("�û����ݴ����û���Ϣû������");
            return;
        }
        string starCode = GetStarTypeFromString(starName).ToString();
#if UNITY_EDITOR
        UserStarLuckData userStarLuckData = new UserStarLuckData
        {
            user_id = GameInfo.tempUser_Id,
            sign_code = starCode,
        };
#else
           UserStarLuckData userStarLuckData = new UserStarLuckData
        {
            user_id = GameInfo.User_Id,
             sign_code = starCode,
            
        };
#endif
        string param = JsonUtility.ToJson(userStarLuckData);
        string needData = GetNeedHttpData(GameInfo.userStarLuckInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userStarLuckInfo_star_Dispatch_Index, needData);
    }
    /// <summary>
    /// ����Target�û���������,��������
    /// </summary>
    public void RequestUserStarLuckData_star_ByStar_target(string starName)
    {
        //ȡ������
        //**************ʹ���û�������Ϣȡ������*****���û�������Ͳ�ȡ��********************
        if (string.IsNullOrEmpty(userInforData))
        {
            Debug.Log("Unity�������û���ϢΪ��");
            return;
        }
        JObject userObj = JObject.Parse(userInforData);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if (userObj["data"] == null)
        {
            Debug.Log("Unity��" + userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if (userData["constellation"] == null)
        {
            Debug.Log("�û����ݴ����û���Ϣû������");
            return;
        }
        string starCode = GetStarTypeFromString(starName).ToString();
#if UNITY_EDITOR
        UserStarLuckData userStarLuckData = new UserStarLuckData
        {
            user_id = GameInfo.tempTargetUser_Id,
            sign_code = starCode,
        };
#else
           UserStarLuckData userStarLuckData = new UserStarLuckData
        {
            user_id = GameInfo.Target_Id,
             sign_code = starCode,
            
        };
#endif
        string param = JsonUtility.ToJson(userStarLuckData);
        string needData = GetNeedHttpData(GameInfo.userStarLuckInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userStarLuckInfo_star_Dispatch_Index, needData);
    }
    //*******************7********************
    //*******************8********************
    /// <summary>
    /// ����-�����û���������-ͨ���������ƣ���ǩ
    /// </summary>
    public void RequestUserTimeCapsuleData_star_ByStarAndTag(string starName,string tagContent)
    {
        List<SortOrder> orderList = new List<SortOrder>();
        SortOrder order = new SortOrder
        {
            order_column = TimeType.release_time.ToString(),
            order_type = SortType.desc.ToString()
        };
        orderList.Add(order);
        //ȡ������
        //**************ʹ���û�������Ϣȡ������*****���û�������Ͳ�ȡ��********************
        if (string.IsNullOrEmpty(userInforData))
        {
            Debug.Log("Unity�������û���ϢΪ��");
            return;
        }
        JObject userObj = JObject.Parse(userInforData);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if (userObj["data"] == null)
        {
            Debug.Log("Unity��" + userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if (userData["constellation"] == null)
        {
            Debug.Log("�û����ݴ����û���Ϣû������");
            return;
        }
        //***************************************
        string starCode = GetStarTypeFromString(starName).ToString();

        //List<string> tagList = new List<string>();
        //if (paramTags != null && paramTags.Count > 0)
        //{
        //    tagList = paramTags;
        //}


#if UNITY_EDITOR
        UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
        {
            user_id = GameInfo.tempUser_Id,
            page_no = 1,
            page_size = 20,
            order = orderList,
            // creator = GameInfo.tempUser_Id,
            sign_code = starCode,
            content = tagContent,
            sex = 0,
        };
#else
            UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
            {
                user_id = GameInfo.User_Id,
                page_no =1,
                page_size =20,
                order = orderList,
                //creator= GameInfo.User_Id,
                sign_code = starCode,
                content= tagContent,
                sex = 0,
            };
#endif
        string param = JsonUtility.ToJson(userTimeCapsuleData);
        string needData = GetNeedHttpData(GameInfo.userTimeCapsuleInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfo_star_Dispatch_Index, needData);
    }
    /// <summary>
    /// ����-����Target�û���������-ͨ���������ƣ���ǩ
    /// </summary>
    public void RequestUserTimeCapsuleData_star_ByStarAndTag_target(string starName, string tagContent)
    {
        List<SortOrder> orderList = new List<SortOrder>();
        SortOrder order = new SortOrder
        {
            order_column = TimeType.release_time.ToString(),
            order_type = SortType.desc.ToString()
        };
        orderList.Add(order);
        //ȡ������
        //**************ʹ���û�������Ϣȡ������*****���û�������Ͳ�ȡ��********************
        if (string.IsNullOrEmpty(userInforData))
        {
            Debug.Log("Unity�������û���ϢΪ��");
            return;
        }
        JObject userObj = JObject.Parse(userInforData);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if (userObj["data"] == null)
        {
            Debug.Log("Unity��" + userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if (userData["constellation"] == null)
        {
            Debug.Log("�û����ݴ����û���Ϣû������");
            return;
        }
        //***************************************
        string starCode = GetStarTypeFromString(starName).ToString();

        //List<string> tagList = new List<string>();
        //if (paramTags != null && paramTags.Count > 0)
        //{
        //    tagList = paramTags;
        //}


#if UNITY_EDITOR
        UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
        {
            user_id = GameInfo.tempTargetUser_Id,
            page_no = 1,
            page_size = 20,
            order = orderList,
            // creator = GameInfo.tempTargetUser_Id,
            sign_code = starCode,
            content = tagContent,
            sex = 0,
        };
#else
            UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
            {
                user_id = GameInfo.Target_Id,
                page_no =1,
                page_size =20,
                order = orderList,
                //creator= GameInfo.Target_Id,
                sign_code = starCode,
                content= tagContent,
                sex = 0,
            };
#endif
        string param = JsonUtility.ToJson(userTimeCapsuleData);
        string needData = GetNeedHttpData(GameInfo.userTimeCapsuleInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfo_star_Dispatch_Index, needData);
    }
    //*******************8********************
    //*******************9********************
    /// <summary>
    /// ����-�����û���������-ͨ���������ƣ��������ݣ��Ա������
    /// </summary>
    public void RequestUserTimeCapsuleData_star_ByStar_Conten_Sex_Age(string starName, string seachConten,int seachSex,string seachAge)
    {
        List<SortOrder> orderList = new List<SortOrder>();
        SortOrder order = new SortOrder
        {
            order_column = TimeType.release_time.ToString(),
            order_type = SortType.desc.ToString()
        };
        orderList.Add(order);
        //ȡ������
        //**************ʹ���û�������Ϣȡ������*****���û�������Ͳ�ȡ��********************
        if (string.IsNullOrEmpty(userInforData))
        {
            Debug.Log("Unity�������û���ϢΪ��");
            return;
        }
        JObject userObj = JObject.Parse(userInforData);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if (userObj["data"] == null)
        {
            Debug.Log("Unity��" + userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if (userData["constellation"] == null)
        {
            Debug.Log("�û����ݴ����û���Ϣû������");
            return;
        }
        //***************************************
        string starCode = GetStarTypeFromString(starName).ToString();
        string seachConten_ = "";
        if (!string.IsNullOrEmpty(seachConten))
            seachConten_ = seachConten;

#if UNITY_EDITOR
        UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
        {
            user_id = GameInfo.tempUser_Id,
            page_no = 1,
            page_size = 20,
            order = orderList,
            //creator = GameInfo.tempUser_Id,

            sign_code = starCode,
            sex=seachSex,
            content= seachConten_,
            age=seachAge
        };
#else
            UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
            {
                user_id = GameInfo.User_Id,
                page_no =1,
                page_size =20,
                order = orderList,
               // creator= GameInfo.User_Id,

                sign_code = starCode,
                sex=seachSex,
                content= seachConten_,
                age=seachAge
            };
#endif
        string param = JsonUtility.ToJson(userTimeCapsuleData);
        string needData = GetNeedHttpData(GameInfo.userTimeCapsuleInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfo_star_Dispatch_Index, needData);
    }

    /// <summary>
    /// ����-����Target�û���������-ͨ���������ƣ��������ݣ��Ա������
    /// </summary>
    public void RequestUserTimeCapsuleData_star_ByStar_Conten_Sex_Age_target(string starName, string seachConten, int seachSex, string seachAge)
    {
        List<SortOrder> orderList = new List<SortOrder>();
        SortOrder order = new SortOrder
        {
            order_column = TimeType.release_time.ToString(),
            order_type = SortType.desc.ToString()
        };
        orderList.Add(order);
        //ȡ������
        //**************ʹ���û�������Ϣȡ������*****���û�������Ͳ�ȡ��********************
        if (string.IsNullOrEmpty(userInforData))
        {
            Debug.Log("Unity�������û���ϢΪ��");
            return;
        }
        JObject userObj = JObject.Parse(userInforData);//��ȡ����
        // ȡ�� data �ֶΣ������䷴���л�
        if (userObj["data"] == null)
        {
            Debug.Log("Unity��" + userObj["msg"].ToString());
            return;
        }
        string dataJson = userObj["data"].ToString();
        JObject userData = JObject.Parse(dataJson);
        if (userData["constellation"] == null)
        {
            Debug.Log("�û����ݴ����û���Ϣû������");
            return;
        }
        //***************************************
        string starCode = GetStarTypeFromString(starName).ToString();
        string seachConten_ = "";
        if (!string.IsNullOrEmpty(seachConten))
            seachConten_ = seachConten;

#if UNITY_EDITOR
        UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
        {
            user_id = GameInfo.tempTargetUser_Id,
            page_no = 1,
            page_size = 20,
            order = orderList,
            //creator = GameInfo.tempTargetUser_Id,

            sign_code = starCode,
            sex = seachSex,
            content = seachConten_,
            age = seachAge
        };
#else
            UserTimeCapsuleData userTimeCapsuleData = new UserTimeCapsuleData
            {
                user_id = GameInfo.Target_Id,
                page_no =1,
                page_size =20,
                order = orderList,
               // creator= GameInfo.Target_Id,

                sign_code = starCode,
                sex=seachSex,
                content= seachConten_,
                age=seachAge
            };
#endif
        string param = JsonUtility.ToJson(userTimeCapsuleData);
        string needData = GetNeedHttpData(GameInfo.userTimeCapsuleInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userTimeCapsuleInfo_star_Dispatch_Index, needData);
    }
    //*******************9********************
    //*******************10********************
    /// <summary>
    /// �����û���ע״̬
    /// </summary>
    public void RequestUserFocusData_main_target(string buttonName)
    {
#if UNITY_EDITOR
        UserFocusData userFocusData = new UserFocusData { user_id = GameInfo.tempUser_Id, followed_id = GameInfo.tempTargetUser_Id };
#else
        UserFocusData userFocusData = new UserFocusData { user_id = GameInfo.User_Id,followed_id= GameInfo.Target_Id };
#endif
        string param = JsonUtility.ToJson(userFocusData);
        string needData = GetNeedHttpData(GameInfo.userFocusInterface_main, param);
        object[] eventParam = new object[2] { needData, buttonName };
        ActionEventHandler.Instance.Dispatch(GameInfo.userFocusInfo_main_Dispatch_Index, eventParam);
    }
    //*******************10********************

    /// <summary>
    /// ����Target�û���ע״̬
    /// </summary>
    public void RequestUserAddFriendData_main_target()
    {
        string myApp_Type = "";
#if UNITY_EDITOR
        myApp_Type = "Android";
#elif UNITY_ANDROID
        myApp_Type = "Android";
#elif UNITY_IOS
        myApp_Type = "IOS";
#endif

#if UNITY_EDITOR
        UserAddFriendData userAddFriendData = new UserAddFriendData 
        {   
            user_id = GameInfo.tempUser_Id, 
            friend_id = GameInfo.tempTargetUser_Id,
            app_type= myApp_Type
        };
#else
            UserAddFriendData userAddFriendData = new UserAddFriendData 
            { 
                user_id = GameInfo.User_Id,
                friend_id = GameInfo.Target_Id,
                app_type = myApp_Type
            };
#endif
        string param = JsonUtility.ToJson(userAddFriendData);
        string needData = GetNeedHttpData(GameInfo.userAddFriendInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userAddFriendInfo_main_Dispatch_Index, needData);
    }

  
    /// <summary>
    /// ������-�����û��������
    /// </summary>
    public void RequestUserAdvertisementInforData_main()
    {
#if UNITY_EDITOR
        UserAdvertisementInfoData userAdvertisementInfoData = new UserAdvertisementInfoData 
        { 
            user_id = GameInfo.tempUser_Id, 
            space_id = GameInfo.space_id 
        };
#else
        UserAdvertisementInfoData userAdvertisementInfoData = new UserAdvertisementInfoData 
        { 
            user_id = GameInfo.User_Id, 
            space_id = GameInfo.space_id 
        };
#endif
        string param = JsonUtility.ToJson(userAdvertisementInfoData);
        string needData = GetNeedHttpData(GameInfo.userAdvertisementInterface_main, param);
        ActionEventHandler.Instance.Dispatch(GameInfo.userAdvertisementInfo_main_Dispatch_Index, needData);
    }

    public void RequestUserLikeData(string cidParam, Action<string> callback)
    {
#if UNITY_EDITOR
        UserLikeData userLikeData = new UserLikeData {
            user_id = GameInfo.tempUser_Id,
            cid = cidParam,
        };
#else
         UserLikeData userLikeData = new UserLikeData {
            user_id = GameInfo.User_Id,
            cid = cidParam,
        };
#endif
        string param = JsonUtility.ToJson(userLikeData);
        string needData = GetNeedHttpData(GameInfo.userTimeStoryLikeInterface_main, param);
        callback?.Invoke(needData);
    }
    public void RequestUserLikeData_target(string cidParam,Action<string> callback)
    {
#if UNITY_EDITOR
        UserLikeData userLikeData = new UserLikeData
        {
            user_id = GameInfo.tempTargetUser_Id,
            cid = cidParam,
        };
#else
         UserLikeData userLikeData = new UserLikeData {
            user_id = GameInfo.Target_Id,
            cid = cidParam,
        };
#endif
        string param = JsonUtility.ToJson(userLikeData);
        string needData = GetNeedHttpData(GameInfo.userTimeStoryLikeInterface_main, param);
        callback?.Invoke(needData);
    }
    public StarType GetStarTypeFromString(string starTypeString)
    {
        switch (starTypeString)
        {
            case "ˮƿ��":
                return StarType.SHUIPING;
            case "˫����":
                return StarType.SHUANGYU;
            case "������":
                return StarType.BAIYANG;
            case "��ţ��":
                return StarType.JINNIU;
            case "˫����":
                return StarType.SHUANGZI;
            case "��з��":
                return StarType.JUXIE;
            case "ʨ����":
                return StarType.SHIZI;
            case "��Ů��":
                return StarType.CHUNV;
            case "�����":
                return StarType.TIANCHENG;
            case "��Ы��":
                return StarType.TIANXIE;
            case "������":
                return StarType.SHESHOU;
            case "Ħ����":
                return StarType.MOJIE;
            default:
                return StarType.NONE; // ���û��ƥ�䣬����null
        }
    }



    public string GetNeedHttpData(string interfaceName, string interfaceParam)
    {
#if UNITY_EDITOR
        return HttpTest.HttpTestFunc(interfaceName, interfaceParam);
#else
        return HttpHelper.HttpRequest(interfaceName, interfaceParam);
#endif
    }
}
