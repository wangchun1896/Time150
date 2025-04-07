using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeStar.DigitalPlant
{
    /// <summary>
    /// ��native����������
    /// </summary>
    public enum CommandDataType
    {
        initHttpPath,

        initSceneLoaded,//��ʼ������
        returnNative,//����Native
        ReleaseTimeCapsule,//����ʱ�ս���
        ReleaseTimeCapsule_star,//��������ʱ�ս���
        ReleaseTimeStory,//����ʱ�����
        MuskBuildsMars,//��˹�˽�����

        ShowTimeCapsule,//չʾʱ�ս���
        ShowTimeCapsule_star,//��������-չʾʱ�ս���
        ShowTimeStory,//չʾʱ�����
        ShowAD,//չʾ���

        CreateUserBirthday,//�����û�����

        ARClockIn,//ar��
        RequestUserLocation,//�����û�λ��


        NotificationCenter,//֪ͨ����
        EnjoyHome,//�����԰
        PersonalLetter//˽��
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
    /// ʱ������
    /// </summary>
    public enum TimeType
    {
        /// <summary>
        /// Լ��ʱ��
        /// </summary>
        date_time,
        /// <summary>
        /// ����ʱ��
        /// </summary>
        release_time
    }

    /// <summary>
    /// ��������
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// ����
        /// </summary>
        asc,
        /// <summary>
        /// ����
        /// </summary>
        desc
    }
}