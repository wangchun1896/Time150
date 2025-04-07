using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCapsulesOnVertices : MonoBehaviour
{
    public GameObject jiaoNangPrefab; // �������Ԥ�Ƽ�
    public float jiaoNangSize = 0.5f; // ������Ĵ�С
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
        // ��ȡ����� MeshFilter ���
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("û���ҵ� MeshFilter �������ȷ���� GameObject ����һ�� MeshFilter �����");
            return;
        }

        // ��ȡ����
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;

        // �Ӷ��������ѡ�� 6 �������ظ�
        HashSet<int> selectedIndices = new HashSet<int>();
        while (selectedIndices.Count < jiaoNangCount)
        {
            int randomIndex = Random.Range(0, vertices.Length);
            selectedIndices.Add(randomIndex);
        }

        int num = 0;
        // ���������岢����λ��
        foreach (int index in selectedIndices)
        {

            Vector3 position = transform.TransformPoint(vertices[index]); // ���ֲ�����ת��Ϊ��������
            GameObject capsule = Instantiate(jiaoNangPrefab, position, Quaternion.identity);
            capsule.transform.localScale = Vector3.one * jiaoNangSize; // ����������Ĵ�С
            capsule.transform.parent = transform; // ����Ϊ������Ӷ���
            if (layerName != "")
            {
                capsule.layer = LayerMask.NameToLayer(layerName);
            }
            //��ӿɵ������
            if (capsule.GetComponent<SphereCollider>() == null)
            {
                capsule.AddComponent<SphereCollider>();
            }
            //��ӽ�����Ϣ��
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
                Destroy(child.gameObject); // ����������
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
