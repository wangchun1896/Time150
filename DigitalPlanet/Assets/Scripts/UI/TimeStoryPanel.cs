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
            // 获取两个 RectTransform 的高度
            story.transform.parent = (i % 2 == 0) ? timeStory_L_Content : timeStory_R_Content;//偶数放左边，奇数放右边
            //添加可点击代码
            story.transform.localScale = Vector3.one;
            //添加胶囊信息类
            TimeStoryBev stb = story.AddComponent<TimeStoryBev>();
            stb.storyDetal = mainUserStoryDateList[i].ToString();
            stb.zhanweiTextureList = zhanWeiTextureList;

        }
    }
   public void CleanStory()
    {
        // 检查 timeStory_L_Content 是否为空
        if (timeStory_L_Content != null)
        {
            foreach (Transform child in timeStory_L_Content)
            {
                if (child != null && child.gameObject != null)
                {
                    Destroy(child.gameObject); // 销毁子物体
                    //ObjectPool.Instance.CollectObject(child.gameObject);
                }
            }
        }

        // 检查 timeStory_R_Content 是否为空
        if (timeStory_R_Content != null)
        {
            foreach (Transform child in timeStory_R_Content)
            {
                if (child != null && child.gameObject != null)
                {
                    //ObjectPool.Instance.CollectObject(child.gameObject);
                    Destroy(child.gameObject); // 销毁子物体
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
