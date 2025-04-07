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
            //��ӿɵ������
            recommendStarItem.transform.DOScale(1, 0.5f);
           //��ӽ�����Ϣ��
           RecommendStarBev rsb = recommendStarItem.AddComponent<RecommendStarBev>();
            rsb.Init(starWheelController);
            rsb.recommendStarDetal = recommendStarDateList[i].ToString();

        }

        //�ػ�content
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

        // �����������ÿһ��ֱ��������
        foreach (Transform child in parent)
        {
            // ��ȡ������� RectTransform
            RectTransform childRectTransform = child.GetComponent<RectTransform>();
            if (childRectTransform != null)
            {
                // ���� cellSize.y �� RectTransform �ĸ߶�
                totalCellSizeY += childRectTransform.rect.height; // ����ʹ�� childRectTransform.sizeDelta.y
            }
        }

        return totalCellSizeY;
    }
    public void Clean()
    {
        // ���  �Ƿ�Ϊ��
        
            foreach (Transform child in transform)
            {
                if (child != null && child.gameObject != null)
                {
                    Destroy(child.gameObject); // ����������
                }
            }
        
    }
}
