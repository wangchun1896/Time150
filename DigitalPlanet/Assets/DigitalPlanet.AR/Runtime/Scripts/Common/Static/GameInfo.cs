using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TimeStar.DigitalPlant
{
    public class GameInfo
    {
        public static string User_Id;
        public static string Target_Id;
        public static string User_Token;
        //public static bool MainReturn;
        //public static bool ArReturn;
        public static string IsDebug = "true";

        //����(����)DEV
        public static string Url_test = "http://10.156.211.7:8118";
        //UAT��������
        public static string Url = "https://uat-time-sloth-gateway.gomezhe.com";

        public static string Url_shengchan = "https://time-sloth-gateway.gomezsz.com";
        //��ʱ�û�ID
        public static string tempUser_Id = "520c93e96a0d44b9b3e010ce17aa8872";//694d3a3845ac473e8123d62db602f513,ef8e8ea7ae644ef99170c2b1d46eb78a
        public static string tempTargetUser_Id = "520c93e96a0d44b9b3e010ce17aa8872";//ceadd2ddf2754562874e61273ece599a//6bb1f1cad229411294b1ec0cb4ced906
        public static string User_Id_TT = "";
        public static string space_id = "1";
        //�ӿ�����
        public static string userInfoInterface = "user@user.operate.queryUserInfo";//�û���Ϣ
        public static string userTimeCapsuleInterface_main = "search@search.date.search_space_date";//����
        public static string userTimeStoryInterface_main = "search@search.date.search_date_story";//����
        public static string userTimeStoryLikeInterface_main = "search@search.date.search_date_story_detail";//ʱ���������
        public static string userStarLuckInterface_main = "search@search.sign.search_sign";//����
        public static string userFocusInterface_main = "user@user.operate.userFocus";//��ע
        public static string userAddFriendInterface_main = "user@user.operate.addFriend";//�Ӻ���
        public static string userAdvertisementInterface_main = "search@search.config.search_advertisement";//�Ӻ���

        public static string userARCheckInterface = "content@content.interact.ar";//ar��֤�ӿ�



        //native������������command
        public const string loadScene = "loadScene";//��¼
        public const string ReleaseTimeStoryReturn = "ReleaseTimeStoryReturn"; // ����ʱ�չ��·���
        public const string ReleaseTimeCapsuleReturn = "ReleaseTimeCapsuleReturn";//����ʱ�ս��ҷ���
        public const string DestroyTimeStoryReturn = "DestroyTimeStoryReturn"; // ɾ��ʱ�չ��·���
        public const string DestroyTimeCapsuleReturn = "DestroyTimeCapsuleReturn";//ɾ��ʱ�ս��ҷ���

        public const string CreateUserBirthdayReturn = "CreateUserBirthdayReturn";//�����û����շ���
        public const string NoCreateUserBirthdayReturn = "NoCreateUserBirthdayReturn";//û�����û����շ���
        public const string ReleaseTimeCapsuleReturn_star = "ReleaseTimeCapsuleReturn_star";//����-����ʱ�ս��ҷ���
        public const string DestroyTimeCapsuleReturn_star = "DestroyTimeCapsuleReturn_star";//����-ɾ��ʱ�ս��ҷ���

        public const string ReturnFromNativeToTargetMain = "ReturnFromNativeToTargetMain";//����-��native���ص�Ŀ��main����,  data�����target_id

        public const string ReturnFromMainToNative = "ReturnFromMainToNative";//������������native
        public const string ReturnFromARToNative = "ReturnFromARToNative";//��AR��������native


        //���ɷ�
        public static ushort userInfo_main_Dispatch_Index = 1;   //�������û���Ϣ�ɷ�
        public static ushort userTimeCapsuleInfo_main_Dispatch_Index = 2; //�������û�ʱ�ս�����Ϣ�ɷ�
        public static ushort userTimeStoryInfo_main_Dispatch_Index = 3; //�������û�ʱ�������Ϣ�ɷ�
        public static ushort userTimeCapsuleInfoRefresh_main_Dispatch_Index = 22; //�������û�ʱ�ս���-ˢ��-��Ϣ�ɷ�
        public static ushort userTimeStoryInfoRefresh_main_Dispatch_Index = 33; //�������û�ʱ�����-ˢ��-��Ϣ�ɷ�
        public static ushort userAdvertisementInfo_main_Dispatch_Index = 14;   //�������û������Ϣ�ɷ�
                                                                               //�����ɷ�
        public static ushort userInfo_star_Dispatch_Index = 4;   //����-�û���Ϣ�ɷ�
        public static ushort userStarLuckInfo_star_Dispatch_Index = 5; //����-������Ϣ�ɷ�
        public static ushort userTimeCapsuleInfo_star_Dispatch_Index = 6; //����-������Ϣ�ɷ�
                                                                          //��ע�ɷ�
        public static ushort userFocusInfo_main_Dispatch_Index = 7; //����-������Ϣ�ɷ�
                                                                    //�Ӻ����ɷ�
        public static ushort userAddFriendInfo_main_Dispatch_Index = 8; //����-������Ϣ�ɷ�
                                                                        //UI�����ɷ�
        public static ushort userEnter_main_Dispatch_Index = 10; //�û�-����������
        public static ushort targetEnter_main_Dispatch_Index = 11; //Ŀ��-����������
        public static ushort userEnter_star_Dispatch_Index = 12; //�û�-��������
        public static ushort targetEnter_star_Dispatch_Index = 13; //Ŀ��-��������


        //AR�ɷ�
        public static ushort userARCheck_Dispatch_Index = 15; //AR��֤


        //��ʱ��ӡ
        public static string Native_InitMessage;

        //��̨�����ֶ�
        public static string method = "method";
        public static string tt_req_header = "tt_req_header";

        public static string app_type = "1";
        public static string device = "2";
        public static string device_id = "";
        public static string authorization = "";
        public static string user_agent = "Androidv1.8.0/2201123C";
        public static string accept_encoding = "gzip";
        public static string login_type = "1";
        public static string timezone = "8";
        public static string lang = "zh";
        public static string timestamp = "";
        public static string version = "2.0.2";

        public static RequestData tokenData;

        //��ǩ
        public static string app_id = "aca35235edbc406da58279e0d8ff72bb";
        public static string app_id_shengchan = "79ce227737564a7dc402b2e3c16dc6e5";
        public static string app_id_production = "";
        public static string charset = "UTF-8";

        public static string format = "JSON";
        public static string sign_type = "MD5";

        public static string key = "MqjHZjB9MH8Jg9SYbxcB@kykJE5bF3nr";
        public static string key_shengchan = "MqjHZjB9MH8Jg9SYbxcB@kykJE5bF3nr";
        public static string key_production = "";


        //AR
        public static string capsule_data = "";
        public static string user_location = "";
    }
}