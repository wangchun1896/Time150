using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCapsulesOnVertices : MonoBehaviour
{
    public GameObject jiaoNangPrefab; // 立方体的预制件
    public float jiaoNangSize = 0.5f; // 立方体的大小
    [SerializeField]
    public int jiaoNangCount = 6;
    [SerializeField]
    private string layerName = "";

    public Transform checkerTr;
    public GameObject jiaoNangTag;

  
    void OnEnable()
    {
        //CreateJiaoNang();
    }
   
    public void CreateJiaoNang(JArray jiaoNangList=null)
    {
        // 获取球体的 MeshFilter 组件
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("没有找到 MeshFilter 组件。请确保此 GameObject 上有一个 MeshFilter 组件。");
            return;
        }

        // 获取网格
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        // 从顶点中随机选择 6 个，不重复
        HashSet<int> selectedIndices = new HashSet<int>();
        while (selectedIndices.Count < jiaoNangCount)
        {
            int randomIndex = Random.Range(0, vertices.Length);
            selectedIndices.Add(randomIndex);
        }

        int num = 0;
        // 创建立方体并设置位置
        foreach (int index in selectedIndices)
        {

            Vector3 position = transform.TransformPoint(vertices[index]); // 将局部坐标转换为世界坐标
            GameObject capsule = Instantiate(jiaoNangPrefab, position, Quaternion.identity);
            capsule.transform.localScale = Vector3.one * jiaoNangSize; // 设置立方体的大小
            capsule.transform.parent = transform; // 设置为球体的子对象
            if (layerName != "")
            {
                capsule.layer = LayerMask.NameToLayer(layerName);
            }
            //添加可点击代码
            if (capsule.GetComponent<SphereCollider>() == null)
            {
                capsule.AddComponent<SphereCollider>();
            }
            //添加胶囊信息类
            CapsuleBev cb = capsule.AddComponent<CapsuleBev>();
            if (jiaoNangList != null && jiaoNangList.Count > 0)
            {
                cb.capsleDetal = jiaoNangList[num].ToString();
                num++;
                //  capsule.AddComponent<DistanceChecker>().Init(checkerTr, jiaoNangTag, cb.capsleDetal);
            }


        }
    }
    public void CleanCapsules()
    {
        foreach (Transform child in transform)
        {
            if (child != null && child.gameObject != null)
            {
                Destroy(child.gameObject); // 销毁子物体
            }
        }
    }
    private void OnDisable()
    {
        CleanCapsules();
    }
    private void OnDestroy()
    {
        CleanCapsules();
    }
}
