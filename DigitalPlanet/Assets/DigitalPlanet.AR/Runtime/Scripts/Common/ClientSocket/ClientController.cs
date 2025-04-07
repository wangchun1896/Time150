
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TimeStar.DigitalPlant
{
    public class ClientController : MonoBehaviour
    {

        void Start()
        {
            //1.链接服务器
            NetWorkSocket.Instance.Connect("172.16.11.165", 12345);
            ActionEventHandler.Instance.AddEventListener(1, OnReceiveCallBack);
        }

        private void OnReceiveCallBack(object[] param)
        {
            Debug.Log("客户端接收消息：" + param[0].ToString());
        }

        private void OnDestroy()
        {
            ActionEventHandler.Instance.RemoveEventListener(1, OnReceiveCallBack);
        }
        public void Send(string msg)
        {
            using (MMO_MemoryStream ms = new MMO_MemoryStream())
            {
                ms.WriteUTF8String("Hello form Clien");
                NetWorkSocket.Instance.SendMsg(ms.ToArray());
            }
        }
        int num;
        void Update()
        {
            //if(Input.GetKeyDown(KeyCode.Space))
            //{
            //    num++;
            //    Send(num.ToString());
            //}
        }
    }
}