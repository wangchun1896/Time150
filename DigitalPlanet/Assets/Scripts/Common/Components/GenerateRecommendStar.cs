using DG.Tweening;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateRecommendStar : MonoBehaviour
{
    public GameObject recommendStar_Item;
    [SerializeField]
    public int cecommendStarCount = 0;
    public GridLayoutGroup gridLayoutGroup;
    public StarWheelController starWheelController;

    void Start()
    {
    }
    public void CreateRecommendStar(JArray recommendStarDateList = null)
    {
        if (recommendStarDateList == null || recommendStarDateList.Count == 0) return;
        for (int i = 0; i < recommendStarDateList.Count; i++)
        {
            GameObject recommendStarItem = Instantiate(recommendStar_Item);
            recommendStarItem.transform.parent = transform;
            //添加可点击代码
            recommendStarItem.transform.DOScale(1, 0.5f);
           //添加胶囊信息类
           RecommendStarBev rsb = recommendStarItem.AddComponent<RecommendStarBev>();
            rsb.Init(starWheelController);
            rsb.recommendStarDetal = recommendStarDateList[i].ToString();

        }

        //重绘content
        RectTransform tr = gridLayoutGroup.GetComponent<RectTransform>();
        float y = (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y) * (Mathf.CeilToInt(recommendStarDateList.Count / 2) + 2);
        tr.sizeDelta = new Vector2(tr.sizeDelta.x, y);
        RectTransform contentAll_RT = transform.parent.GetComponent<RectTransform>();
        contentAll_RT.sizeDelta = new Vector2(contentAll_RT.sizeDelta.x,
                        CalculateTotalCellSizeY(contentAll_RT));
        contentAll_RT.anchoredPosition = new Vector2(contentAll_RT.anchoredPosition.x, 0);

    }

    float CalculateTotalCellSizeY(RectTransform parent)
    {
        float totalCellSizeY = 0f;

        // 遍历父物体的每一个直接子物体
        foreach (Transform child in parent)
        {
            // 获取子物体的 RectTransform
            RectTransform childRectTransform = child.GetComponent<RectTransform>();
            if (childRectTransform != null)
            {
                // 假设 cellSize.y 是 RectTransform 的高度
                totalCellSizeY += childRectTransform.rect.height; // 或者使用 childRectTransform.sizeDelta.y
            }
        }

        return totalCellSizeY;
    }
    public void Clean()
    {
        // 检查  是否为空
        
            foreach (Transform child in transform)
            {
                if (child != null && child.gameObject != null)
                {
                    Destroy(child.gameObject); // 销毁子物体
                }
            }
        
    }
}
