using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStoryPanel : MonoBehaviour
{
    public Transform timeStory_All_Content;
    public Transform timeStory_L_Content;
    public Transform timeStory_R_Content;
    public GameObject timeStory_Item;
    public List<Texture2D> zhanWeiTextureList;

    [SerializeField]
    public int storyCount = 6;



    public void CreateStory(JArray mainUserStoryDateList=null)
    {
        if (mainUserStoryDateList == null || mainUserStoryDateList.Count == 0) return;
        for (int i = 0; i < mainUserStoryDateList.Count; i++)
        {
           // GameObject story= ObjectPool.Instance.CreateObject("Item", timeStory_Item, Vector3.zero, Quaternion.identity);
          
            GameObject story = Instantiate(timeStory_Item);
            // ��ȡ���� RectTransform �ĸ߶�
            story.transform.parent = (i % 2 == 0) ? timeStory_L_Content : timeStory_R_Content;//ż������ߣ��������ұ�
            //��ӿɵ������
            story.transform.localScale = Vector3.one;
            //��ӽ�����Ϣ��
            TimeStoryBev stb = story.AddComponent<TimeStoryBev>();
            stb.storyDetal = mainUserStoryDateList[i].ToString();
            stb.zhanweiTextureList = zhanWeiTextureList;

        }
    }
   public void CleanStory()
    {
        // ��� timeStory_L_Content �Ƿ�Ϊ��
        if (timeStory_L_Content != null)
        {
            foreach (Transform child in timeStory_L_Content)
            {
                if (child != null && child.gameObject != null)
                {
                    Destroy(child.gameObject); // ����������
                    //ObjectPool.Instance.CollectObject(child.gameObject);
                }
            }
        }

        // ��� timeStory_R_Content �Ƿ�Ϊ��
        if (timeStory_R_Content != null)
        {
            foreach (Transform child in timeStory_R_Content)
            {
                if (child != null && child.gameObject != null)
                {
                    //ObjectPool.Instance.CollectObject(child.gameObject);
                    Destroy(child.gameObject); // ����������
                }
            }
        }
    }

    private void OnDisable()
    {
        CleanStory();
    }

    //public void OnDestroy()
    //{

    //    ObjectPool.Instance.ClearAll();
    //}
}
