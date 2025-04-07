using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeStar.DigitalPlant
{
    public class ActionEventHandler : Singleton<ActionEventHandler>
    {
        public delegate void OnActionEventHandler(params object[] param);

        private Dictionary<ushort, List<OnActionEventHandler>> dic = new Dictionary<ushort, List<OnActionEventHandler>>();

        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="actionID"></param>
        /// <param name="handler"></param>
        public void AddEventListener(ushort actionID, OnActionEventHandler handler)
        {
            if (dic.ContainsKey(actionID))
            {
                dic[actionID].Add(handler);
            }
            else
            {
                List<OnActionEventHandler> listHandler = new List<OnActionEventHandler>();
                listHandler.Add(handler);
                dic[actionID] = listHandler;
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="actionID"></param>
        /// <param name="handler"></param>
        public void RemoveEventListener(ushort actionID, OnActionEventHandler handler)
        {
            if (dic.ContainsKey(actionID))
            {
                List<OnActionEventHandler> listHandler = dic[actionID];
                listHandler.Remove(handler);
                if (listHandler.Count == 0)
                {
                    dic.Remove(actionID);
                }
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="actionID"></param>
        /// <param name="handler"></param>
        public void RemoveAllEventListener()
        {
            if (dic.Count > 0)
            {
                dic.Clear();
            }
        }



        /// <summary>
        /// 批量派发
        /// </summary>
        /// <param name="actionID"></param>
        /// <param name="param"></param>
        public void Dispatch(ushort actionID, params object[] param)
        {
            if (dic.ContainsKey(actionID))
            {
                List<OnActionEventHandler> listHandler = dic[actionID];
                if (listHandler != null & listHandler.Count > 0)
                {
                    for (int i = 0; i < listHandler.Count; i++)
                    {
                        listHandler[i]?.Invoke(param);
                    }
                }
            }
            // 使用方式
            // ActionEventHandler.Instance.Dispatch(1, 9);
            // ActionEventHandler.Instance.AddEventListener(1, OnActionCallBack);
        }

        private void OnActionCallBack(object[] param)
        {
            //比如收到消息时进行消息监听，可分为不同类型，1，2，3，4，这里时监听并触发后返回的参数
            param.ToString();
            Debug.Log(param);
        }
    }
}