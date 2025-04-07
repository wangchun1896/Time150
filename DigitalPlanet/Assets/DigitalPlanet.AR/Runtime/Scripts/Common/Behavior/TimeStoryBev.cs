using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TimeStar.Bridge;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TimeStar.DigitalPlant
{
    public class TimeStoryBev : AutoSizeBase
    {
        [TextArea]
        public string storyDetal;
        public List<Texture2D> zhanweiTextureList;
        JObject storyInfo;

        private RawImage timeStoryTexture;
        // private TextMeshProUGUI dianZanText;
        private TextMeshProUGUI descriptionText;
        private RawImage touXiang;
        private Text nameText;
        private Button clickButton;
        private TimeStoryContentCtl timeStoryContentCtl;
        private TextMeshProUGUI zhanweiText;

        protected override void Awake()
        {
            base.Awake();
            timeStoryTexture = transform.Find("图片Texture").GetComponent<RawImage>();
            // dianZanText= transform.Find("图片Texture/点赞框/点赞Text").GetComponent<TextMeshProUGUI>();
            zhanweiText = transform.Find("图片Texture/占位文字").GetComponent<TextMeshProUGUI>();
            descriptionText = transform.Find("描述Text").GetComponent<TextMeshProUGUI>();
            touXiang = transform.Find("头像名称Content/头像Texture").GetComponent<RawImage>();
            nameText = transform.Find("头像名称Content/名称Text").GetComponent<Text>();
            clickButton = timeStoryTexture.GetComponent<Button>();
            clickButton.onClick.AddListener(Clicked);
            RegisterHeightChangeEventsForEachChildObject();
            // 找到并缓存 TimeStoryContentCtl 组件
            timeStoryContentCtl = transform.parent.GetComponent<TimeStoryContentCtl>();
            //注册 TimeStoryBev 的高度变化事件到 TimeStoryContentCtl
            onHeightChange.AddListener(timeStoryContentCtl.CalculateAndSetHeight);

        }
        private void OnDestroy()
        {
            RemoveHeightChangeEventsForEachChildObject();
            clickButton.onClick.RemoveListener(Clicked);
            if (timeStoryTexture.texture != null)
            {
                timeStoryTexture.texture = null;
            }
            //if (!string.IsNullOrEmpty(dianZanText.text))
            //{
            //    dianZanText.text = "";
            //}
            if (!string.IsNullOrEmpty(zhanweiText.text))
            {
                zhanweiText.text = "";
            }
            if (!string.IsNullOrEmpty(descriptionText.text))
            {
                descriptionText.text = "";
            }
            if (touXiang.texture != null)
            {
                touXiang.texture = null;
            }
            if (!string.IsNullOrEmpty(nameText.text))
            {
                nameText.text = "";
            }
            //clickButton = null;
            onHeightChange.RemoveListener(timeStoryContentCtl.CalculateAndSetHeight);
            // 找到并缓存 TimeStoryContentCtl 组件
            timeStoryContentCtl = null;
            //注册 TimeStoryBev 的高度变化事件到 TimeStoryContentCtl

        }
        public void RegisterHeightChangeEventsForEachChildObject()
        {
            timeStoryTexture.GetComponent<AutoSizeBase>().onHeightChange.AddListener(CalculateAndSetHeight);
            descriptionText.GetComponent<AutoSizeBase>().onHeightChange.AddListener(CalculateAndSetHeight);
        }
        public void RemoveHeightChangeEventsForEachChildObject()
        {
            timeStoryTexture.GetComponent<AutoSizeBase>().onHeightChange.RemoveListener(CalculateAndSetHeight);
            descriptionText.GetComponent<AutoSizeBase>().onHeightChange.RemoveListener(CalculateAndSetHeight);
        }
        public override void CalculateAndSetHeight()
        {
            // 初始化总高度
            float totalHeight = 0f;
            float space = GetComponent<VerticalLayoutGroup>().spacing;
            space = (space != 0) ? (transform.childCount - 1) * space : 0;

            // 遍历所有一级子物体
            foreach (Transform child in transform)
            {
                // 计算子物体的高度
                RectTransform childRectTransform = child.GetComponent<RectTransform>();
                if (childRectTransform != null)
                {
                    totalHeight += childRectTransform.rect.height; // 获取高度并累加
                }
            }
            totalHeight += space;
            // 设置当前物体的高度
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // 设置自己的高度，可根据需要调整锚点和以获得正确的效果
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight);
                onHeightChange?.Invoke();
            }
        }
        private void Start()
        {
            if (string.IsNullOrEmpty(storyDetal)) return;
            storyInfo = JObject.Parse(storyDetal);
            //gameObject.name = storyInfo["description"].ToString();
            InitItem();
        }

        public void InitItem()
        {
            //storyInfo = JObject.Parse(storyDetal);
            //故事图片
            if (storyInfo["media_files"] != null)
            {
                // storyInfo["media_files"][0]!=null&& storyInfo["media_files"][0]["file_path"]!=null
                JArray media = storyInfo["media_files"] as JArray;
                string storyFace_image_url = "";
                if (media.Count > 0)
                {
                    switch (media[0]["media_type"].ToString())
                    {
                        case "image":
                            storyFace_image_url = media[0]["file_path"].ToString();
                            break;
                        case "video":
                            storyFace_image_url = media[0]["face_image"].ToString();
                            break;
                        default:
                            break;
                    }
                    StartCoroutine(DownloadHelper.Instance.DownloadImageTexture(storyFace_image_url, OnStoryTextureDownloadSuccess, OnTimeStoryTextureDownloadErrorOrNoData));
                }
                else
                {
                    OnTimeStoryTextureDownloadErrorOrNoData();
                }

            }
            ////点赞
            // if (storyInfo["cid"] != null)
            //{
            //    if (!GameManager.Instance.isTarget)//自己
            //        GameManager.Instance.RequestUserLikeData(storyInfo["cid"].ToString(), UserLikeHandler);
            //    else//游客
            //        GameManager.Instance.RequestUserLikeData_target(storyInfo["cid"].ToString(), UserLikeHandler_target);
            //}
            //故事描述
            if (storyInfo["description"] == null || string.IsNullOrEmpty(storyInfo["description"].ToString()))
                descriptionText.gameObject.SetActive(false);
            else
            {

                descriptionText.text = storyInfo["description"].ToString();
                zhanweiText.text = descriptionText.text;

                if (descriptionText.GetComponent<AutoSizeTextMeshProUGUI>() != null)
                {
                    descriptionText.GetComponent<AutoSizeTextMeshProUGUI>().SetText(descriptionText.text);
                }
            }
            //头像图片
            //string userTouXiangTexture_url = storyInfo["creator"]["user_image"].ToString();
            //StartCoroutine(DownloadHelper.Instance.DownloadImageTexture(userTouXiangTexture_url, OnStoryCreatorTextureDownloadSuccess, OnTextureDownloadError));
            SetStoryCreateTouXiang(FindObjectOfType<UserInfoPanel>().xingZhuTouXiangTexture.texture as Texture2D);
            //发布人昵称
            nameText.text = storyInfo["creator"]["user_name"].ToString();
        }

        //private void UserLikeHandler(string obj)
        //{
        //    Debug.Log("@@@" + obj);
        //    // 解析外层 JSON
        //    JObject jsonObject = JObject.Parse(obj);
        //    // 获取 data 字段的字符串内容
        //    string dataString = (string)jsonObject["data"];
        //    // 解析内部的 JSON 字符串
        //    JObject dataObject = JObject.Parse(dataString);
        //    // 从 JObject 中提取 thumb 的值
        //    int thumbValue = (int)dataObject["thumb"];
        //    dianZanText.text = thumbValue.ToString();
        //}
        //private void UserLikeHandler_target(string obj)
        //{
        //    Debug.Log("@@@" + obj);
        //    // 解析外层 JSON
        //    JObject jsonObject = JObject.Parse(obj);
        //    // 获取 data 字段的字符串内容
        //    string dataString = (string)jsonObject["data"];
        //    // 解析内部的 JSON 字符串
        //    JObject dataObject = JObject.Parse(dataString);
        //    // 从 JObject 中提取 thumb 的值
        //    int thumbValue = (int)dataObject["thumb"];
        //    dianZanText.text = thumbValue.ToString();
        //}

        // 下载成功的回调
        private void SetStoryCreateTouXiang(Texture2D texture)
        {
            // 这里可以做任何你想做的事情，比如显示图片
            // Debug.Log("用户头像设置为故事头像.");
            // 例如，可以将纹理应用到一个对象上
            // GetComponent<Renderer>().material.mainTexture = texture;
            touXiang.texture = texture;
        }
        // 下载成功的回调
        private void OnStoryCreatorTextureDownloadSuccess(Texture2D texture)
        {
            // 这里可以做任何你想做的事情，比如显示图片
            Debug.Log("故事头像下载成功.");
            // 例如，可以将纹理应用到一个对象上
            // GetComponent<Renderer>().material.mainTexture = texture;
            touXiang.texture = texture;
        }
        // 下载成功的回调
        private void OnStoryTextureDownloadSuccess(Texture2D texture)
        {
            if (texture != null)
            {
                timeStoryTexture.texture = texture;
                if (timeStoryTexture.GetComponent<AutoSizeRawImage>() != null)
                    timeStoryTexture.GetComponent<AutoSizeRawImage>().SetImageTexture(texture);
            }
            else
            {
                OnTimeStoryTextureDownloadErrorOrNoData();
            }
        }
        // 下载失败的回调
        private void OnTimeStoryTextureDownloadErrorOrNoData(string error = null)
        {
            //// 创建一个新的纹理，大小为 290x320
            //Texture2D texture = new Texture2D(290, 320, TextureFormat.ARGB32, false);

            //// 创建颜色数组
            //Color[] colors = new Color[] { color1, color2, color3, color4 };
            //Color targetColor = colors[UnityEngine.Random.Range(0, colors.Length)];

            //// 填充纹理的像素
            //Color[] pixels = new Color[290 * 320];
            //for (int i = 0; i < pixels.Length; i++)
            //{
            //    pixels[i] = targetColor; // 将每个像素设置为随机选择的目标颜色
            //}

            //// 将颜色数组应用于纹理
            //texture.SetPixels(pixels);
            //texture.Apply(); // 应用变更

            // 将创建的纹理赋值给 RawImage 的纹理

            // 将创建的纹理赋值给RawImage组件
            Texture2D texture = zhanweiTextureList[UnityEngine.Random.Range(0, zhanweiTextureList.Count - 1)];
            if (timeStoryTexture.texture != null)
            {
                timeStoryTexture.texture = texture;
                zhanweiText.gameObject.SetActive(true);
            }
            else
            {
                timeStoryTexture = transform.GetChild(0).GetComponent<RawImage>();
                timeStoryTexture.texture = texture;
                zhanweiText.gameObject.SetActive(true);

            }
            if (timeStoryTexture.GetComponent<AutoSizeRawImage>() != null)
                timeStoryTexture.GetComponent<AutoSizeRawImage>().SetImageTexture(texture);
        }
        private void OnTextureDownloadError(string error)
        {
            Debug.Log("头像错误，检查URL: " + error);
        }




        private void Clicked()
        {
            // 在这里添加点击物体时要执行的逻辑，例如打印信息或改变颜色
            Debug.Log(gameObject.name + " @" + storyDetal);
            //#if UNITY_EDITOR
            //GameObject tish = Instantiate(Resources.Load<GameObject>("Canvas"));
            //tish.GetComponentInChildren<Text>().text = storyDetal;
            //#endif


            //初始化场景后向Native发送场景信息
            if (string.IsNullOrEmpty(storyDetal)) Debug.LogError("时空胶囊详细信息为空");
#if !UNITY_EDITOR
        ToNativeData toNativeData_showTimeStory = new ToNativeData
        {
            command = CommandDataType.ShowTimeStory.ToString(),
            data = storyDetal
        };
        string data = JsonUtility.ToJson(toNativeData_showTimeStory);
        NativeBridge.Instance.SendMessageToNative(data);
#endif
        }



        private IEnumerator DestroyTish(GameObject tish)
        {
            yield return new WaitForSeconds(1f);
            Destroy(tish);
        }
    }
}