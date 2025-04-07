using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeStar.DigitalPlant
{
    /// <summary>
    /// 给native的命令类型
    /// </summary>
    public enum CommandDataType
    {
        initHttpPath,

        initSceneLoaded,//初始化场景
        returnNative,//返回Native
        ReleaseTimeCapsule,//发布时空胶囊
        ReleaseTimeCapsule_star,//星座发布时空胶囊
        ReleaseTimeStory,//发布时光故事
        MuskBuildsMars,//马斯克建火星

        ShowTimeCapsule,//展示时空胶囊
        ShowTimeCapsule_star,//星座场景-展示时空胶囊
        ShowTimeStory,//展示时光故事
        ShowAD,//展示广告

        CreateUserBirthday,//创建用户生日

        ARClockIn,//ar打卡
        RequestUserLocation,//请求用户位置


        NotificationCenter,//通知中心
        EnjoyHome,//乐享家园
        PersonalLetter//私信
    }

    public enum StarType
    {
        NONE,
        SHUIPING,
        SHUANGYU,
        BAIYANG,
        JINNIU,
        SHUANGZI,
        JUXIE,
        SHIZI,
        CHUNV,
        TIANCHENG,
        TIANXIE,
        SHESHOU,
        MOJIE,

    }

    /// <summary>
    /// 时间类型
    /// </summary>
    public enum TimeType
    {
        /// <summary>
        /// 约会时间
        /// </summary>
        date_time,
        /// <summary>
        /// 发布时间
        /// </summary>
        release_time
    }

    /// <summary>
    /// 排序类型
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// 升序
        /// </summary>
        asc,
        /// <summary>
        /// 降序
        /// </summary>
        desc
    }
}