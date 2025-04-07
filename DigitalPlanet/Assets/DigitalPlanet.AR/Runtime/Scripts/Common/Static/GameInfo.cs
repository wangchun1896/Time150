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

        //测试(内网)DEV
        public static string Url_test = "http://10.156.211.7:8118";
        //UAT（公网）
        public static string Url = "https://uat-time-sloth-gateway.gomezhe.com";

        public static string Url_shengchan = "https://time-sloth-gateway.gomezsz.com";
        //临时用户ID
        public static string tempUser_Id = "520c93e96a0d44b9b3e010ce17aa8872";//694d3a3845ac473e8123d62db602f513,ef8e8ea7ae644ef99170c2b1d46eb78a
        public static string tempTargetUser_Id = "520c93e96a0d44b9b3e010ce17aa8872";//ceadd2ddf2754562874e61273ece599a//6bb1f1cad229411294b1ec0cb4ced906
        public static string User_Id_TT = "";
        public static string space_id = "1";
        //接口名称
        public static string userInfoInterface = "user@user.operate.queryUserInfo";//用户信息
        public static string userTimeCapsuleInterface_main = "search@search.date.search_space_date";//胶囊
        public static string userTimeStoryInterface_main = "search@search.date.search_date_story";//故事
        public static string userTimeStoryLikeInterface_main = "search@search.date.search_date_story_detail";//时光故事详情
        public static string userStarLuckInterface_main = "search@search.sign.search_sign";//运势
        public static string userFocusInterface_main = "user@user.operate.userFocus";//关注
        public static string userAddFriendInterface_main = "user@user.operate.addFriend";//加好友
        public static string userAdvertisementInterface_main = "search@search.config.search_advertisement";//加好友

        public static string userARCheckInterface = "content@content.interact.ar";//ar验证接口



        //native发回来的命令command
        public const string loadScene = "loadScene";//登录
        public const string ReleaseTimeStoryReturn = "ReleaseTimeStoryReturn"; // 发布时空故事返回
        public const string ReleaseTimeCapsuleReturn = "ReleaseTimeCapsuleReturn";//发布时空胶囊返回
        public const string DestroyTimeStoryReturn = "DestroyTimeStoryReturn"; // 删除时空故事返回
        public const string DestroyTimeCapsuleReturn = "DestroyTimeCapsuleReturn";//删除时空胶囊返回

        public const string CreateUserBirthdayReturn = "CreateUserBirthdayReturn";//创建用户生日返回
        public const string NoCreateUserBirthdayReturn = "NoCreateUserBirthdayReturn";//没创建用户生日返回
        public const string ReleaseTimeCapsuleReturn_star = "ReleaseTimeCapsuleReturn_star";//星座-发布时空胶囊返回
        public const string DestroyTimeCapsuleReturn_star = "DestroyTimeCapsuleReturn_star";//星座-删除时空胶囊返回

        public const string ReturnFromNativeToTargetMain = "ReturnFromNativeToTargetMain";//星座-从native返回到目标main场景,  data里面带target_id

        public const string ReturnFromMainToNative = "ReturnFromMainToNative";//从主场景返回native
        public const string ReturnFromARToNative = "ReturnFromARToNative";//从AR场景返回native


        //主派发
        public static ushort userInfo_main_Dispatch_Index = 1;   //主场景用户信息派发
        public static ushort userTimeCapsuleInfo_main_Dispatch_Index = 2; //主场景用户时空胶囊信息派发
        public static ushort userTimeStoryInfo_main_Dispatch_Index = 3; //主场景用户时光故事信息派发
        public static ushort userTimeCapsuleInfoRefresh_main_Dispatch_Index = 22; //主场景用户时空胶囊-刷新-信息派发
        public static ushort userTimeStoryInfoRefresh_main_Dispatch_Index = 33; //主场景用户时光故事-刷新-信息派发
        public static ushort userAdvertisementInfo_main_Dispatch_Index = 14;   //主场景用户广告信息派发
                                                                               //星座派发
        public static ushort userInfo_star_Dispatch_Index = 4;   //星座-用户信息派发
        public static ushort userStarLuckInfo_star_Dispatch_Index = 5; //星座-运势信息派发
        public static ushort userTimeCapsuleInfo_star_Dispatch_Index = 6; //星座-胶囊信息派发
                                                                          //关注派发
        public static ushort userFocusInfo_main_Dispatch_Index = 7; //星座-胶囊信息派发
                                                                    //加好友派发
        public static ushort userAddFriendInfo_main_Dispatch_Index = 8; //星座-胶囊信息派发
                                                                        //UI显隐派发
        public static ushort userEnter_main_Dispatch_Index = 10; //用户-进入主场景
        public static ushort targetEnter_main_Dispatch_Index = 11; //目标-进入主场景
        public static ushort userEnter_star_Dispatch_Index = 12; //用户-进入星座
        public static ushort targetEnter_star_Dispatch_Index = 13; //目标-进入星座


        //AR派发
        public static ushort userARCheck_Dispatch_Index = 15; //AR验证


        //临时打印
        public static string Native_InitMessage;

        //后台所需字段
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

        //验签
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