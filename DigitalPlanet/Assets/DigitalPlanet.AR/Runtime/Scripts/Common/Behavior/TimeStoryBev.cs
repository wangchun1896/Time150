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
            timeStoryTexture = transform.Find("ͼƬTexture").GetComponent<RawImage>();
            // dianZanText= transform.Find("ͼƬTexture/���޿�/����Text").GetComponent<TextMeshProUGUI>();
            zhanweiText = transform.Find("ͼƬTexture/ռλ����").GetComponent<TextMeshProUGUI>();
            descriptionText = transform.Find("����Text").GetComponent<TextMeshProUGUI>();
            touXiang = transform.Find("ͷ������Content/ͷ��Texture").GetComponent<RawImage>();
            nameText = transform.Find("ͷ������Content/����Text").GetComponent<Text>();
            clickButton = timeStoryTexture.GetComponent<Button>();
            clickButton.onClick.AddListener(Clicked);
            RegisterHeightChangeEventsForEachChildObject();
            // �ҵ������� TimeStoryContentCtl ���
            timeStoryContentCtl = transform.parent.GetComponent<TimeStoryContentCtl>();
            //ע�� TimeStoryBev �ĸ߶ȱ仯�¼��� TimeStoryContentCtl
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
            // �ҵ������� TimeStoryContentCtl ���
            timeStoryContentCtl = null;
            //ע�� TimeStoryBev �ĸ߶ȱ仯�¼��� TimeStoryContentCtl

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
            // ��ʼ���ܸ߶�
            float totalHeight = 0f;
            float space = GetComponent<VerticalLayoutGroup>().spacing;
            space = (space != 0) ? (transform.childCount - 1) * space : 0;

            // ��������һ��������
            foreach (Transform child in transform)
            {
                // ����������ĸ߶�
                RectTransform childRectTransform = child.GetComponent<RectTransform>();
                if (childRectTransform != null)
                {
                    totalHeight += childRectTransform.rect.height; // ��ȡ�߶Ȳ��ۼ�
                }
            }
            totalHeight += space;
            // ���õ�ǰ����ĸ߶�
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // �����Լ��ĸ߶ȣ��ɸ�����Ҫ����ê����Ի����ȷ��Ч��
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
            //����ͼƬ
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
            ////����
            // if (storyInfo["cid"] != null)
            //{
            //    if (!GameManager.Instance.isTarget)//�Լ�
            //        GameManager.Instance.RequestUserLikeData(storyInfo["cid"].ToString(), UserLikeHandler);
            //    else//�ο�
            //        GameManager.Instance.RequestUserLikeData_target(storyInfo["cid"].ToString(), UserLikeHandler_target);
            //}
            //��������
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
            //ͷ��ͼƬ
            //string userTouXiangTexture_url = storyInfo["creator"]["user_image"].ToString();
            //StartCoroutine(DownloadHelper.Instance.DownloadImageTexture(userTouXiangTexture_url, OnStoryCreatorTextureDownloadSuccess, OnTextureDownloadError));
            SetStoryCreateTouXiang(FindObjectOfType<UserInfoPanel>().xingZhuTouXiangTexture.texture as Texture2D);
            //�������ǳ�
            nameText.text = storyInfo["creator"]["user_name"].ToString();
        }

        //private void UserLikeHandler(string obj)
        //{
        //    Debug.Log("@@@" + obj);
        //    // ������� JSON
        //    JObject jsonObject = JObject.Parse(obj);
        //    // ��ȡ data �ֶε��ַ�������
        //    string dataString = (string)jsonObject["data"];
        //    // �����ڲ��� JSON �ַ���
        //    JObject dataObject = JObject.Parse(dataString);
        //    // �� JObject ����ȡ thumb ��ֵ
        //    int thumbValue = (int)dataObject["thumb"];
        //    dianZanText.text = thumbValue.ToString();
        //}
        //private void UserLikeHandler_target(string obj)
        //{
        //    Debug.Log("@@@" + obj);
        //    // ������� JSON
        //    JObject jsonObject = JObject.Parse(obj);
        //    // ��ȡ data �ֶε��ַ�������
        //    string dataString = (string)jsonObject["data"];
        //    // �����ڲ��� JSON �ַ���
        //    JObject dataObject = JObject.Parse(dataString);
        //    // �� JObject ����ȡ thumb ��ֵ
        //    int thumbValue = (int)dataObject["thumb"];
        //    dianZanText.text = thumbValue.ToString();
        //}

        // ���سɹ��Ļص�
        private void SetStoryCreateTouXiang(Texture2D texture)
        {
            // ����������κ������������飬������ʾͼƬ
            // Debug.Log("�û�ͷ������Ϊ����ͷ��.");
            // ���磬���Խ�����Ӧ�õ�һ��������
            // GetComponent<Renderer>().material.mainTexture = texture;
            touXiang.texture = texture;
        }
        // ���سɹ��Ļص�
        private void OnStoryCreatorTextureDownloadSuccess(Texture2D texture)
        {
            // ����������κ������������飬������ʾͼƬ
            Debug.Log("����ͷ�����سɹ�.");
            // ���磬���Խ�����Ӧ�õ�һ��������
            // GetComponent<Renderer>().material.mainTexture = texture;
            touXiang.texture = texture;
        }
        // ���سɹ��Ļص�
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
        // ����ʧ�ܵĻص�
        private void OnTimeStoryTextureDownloadErrorOrNoData(string error = null)
        {
            //// ����һ���µ�������СΪ 290x320
            //Texture2D texture = new Texture2D(290, 320, TextureFormat.ARGB32, false);

            //// ������ɫ����
            //Color[] colors = new Color[] { color1, color2, color3, color4 };
            //Color targetColor = colors[UnityEngine.Random.Range(0, colors.Length)];

            //// ������������
            //Color[] pixels = new Color[290 * 320];
            //for (int i = 0; i < pixels.Length; i++)
            //{
            //    pixels[i] = targetColor; // ��ÿ����������Ϊ���ѡ���Ŀ����ɫ
            //}

            //// ����ɫ����Ӧ��������
            //texture.SetPixels(pixels);
            //texture.Apply(); // Ӧ�ñ��

            // ������������ֵ�� RawImage ������

            // ������������ֵ��RawImage���
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
            Debug.Log("ͷ����󣬼��URL: " + error);
        }




        private void Clicked()
        {
            // ��������ӵ������ʱҪִ�е��߼��������ӡ��Ϣ��ı���ɫ
            Debug.Log(gameObject.name + " @" + storyDetal);
            //#if UNITY_EDITOR
            //GameObject tish = Instantiate(Resources.Load<GameObject>("Canvas"));
            //tish.GetComponentInChildren<Text>().text = storyDetal;
            //#endif


            //��ʼ����������Native���ͳ�����Ϣ
            if (string.IsNullOrEmpty(storyDetal)) Debug.LogError("ʱ�ս�����ϸ��ϢΪ��");
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