using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TimeStar.Bridge;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager:MonoSingleton<UIManager>
{

    public GameObject mainCamera;
    public GameObject mainScene;
    public GameObject starCamera;
    public GameObject starScene;
    public GameObject starReturnMainButton;
    public XingZuoPanel xingZuoPanel;

    /// <summary>
    /// 切换星座场景
    /// </summary>
    public void MainToStar()
    {
       List<StarPanel> starPanels = FindChildComponents<StarPanel>();
        List<MainPanel> mainPanel = FindChildComponents<MainPanel>();
        //starPanels[0].transform.DOScaleX(1, 0.5f);
        //mainPanel[0].transform.DOScaleX(0, 0.5f);

        mainPanel[0].transform.DOLocalMoveY(5000, 0f);
        starPanels[0].transform.DOLocalMoveY(0, 0.5f);

        //切换星座场景
        GameManager.Instance.InitHttpData_star();
    }
    public void StarToMain()
    {
        List<StarPanel> starPanels = FindChildComponents<StarPanel>();
        List<MainPanel> mainPanel = FindChildComponents<MainPanel>();
        //starPanels[0].transform.DOScaleX(0, 0.5f);
        //mainPanel[0].transform.DOScaleX(1, 0.5f);
        starPanels[0].transform.DOLocalMoveY(5000, 0f);
        mainPanel[0].transform.DOLocalMoveY(0, 0.5f);
    }
    public void StarToMain_target()
    {
        List<StarPanel> starPanels = FindChildComponents<StarPanel>();
        List<MainPanel> mainPanel = FindChildComponents<MainPanel>();
        mainCamera.SetActive(true);
        mainScene.SetActive(true);
        starCamera.SetActive(false);
        starScene.SetActive(false);
        xingZuoPanel.XingZuoSearchAllInit();
        starPanels[0].transform.DOLocalMoveY(5000, 0f);
        mainPanel[0].transform.DOLocalMoveY(0, 0.5f);

       
    }
    // 通用查找方法
    public List<T> FindChildComponents<T>() where T : Component
    {
        List<T> foundComponents = new List<T>();

        // 遍历所有级别 1 的子物体
        foreach (Transform child in transform)
        {
            T component = child.GetComponent<T>();
            if (component != null)
            {
                foundComponents.Add(component);
            }
        }

        return foundComponents;
    }
}
