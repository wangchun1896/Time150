using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TimeStar.DigitalPlant;
using UnityEngine;
using UnityEngine.Serialization;

namespace TimeStar.Prefabs
{
    public class PrefabData : MonoBehaviour
    {
        public string data;
    }

    public class PrefabSpawner : MonoBehaviour
    {
        public static PrefabSpawner Instance { get; private set; }
        private readonly Dictionary<string, GameObject> _spawnedPrefabs = new Dictionary<string, GameObject>();
        [FormerlySerializedAs("TimeCapsule2D")] [SerializeField] public GameObject timeCapsule2D;
        [FormerlySerializedAs("TimeCapsule3D")] [SerializeField] public GameObject timeCapsule3D;
        [FormerlySerializedAs("TimeCapsule3D-1")] [SerializeField] public GameObject timeCapsule3D1;

        private readonly Vector3 _cameraPosition = new Vector3(0, 0, -10);

        private void Awake()
        {
            ActionEventHandler.Instance.AddEventListener(GameInfo.userInfo_main_Dispatch_Index, OnARCapsuleClick);
        }

        private void OnARCapsuleClick(object[] param)
        {
            string checkInfo = param[0].ToString();
            Debug.Log("返回的AR胶囊验证消息：" + checkInfo);
            JObject checkObj = JObject.Parse(checkInfo);//提取命令
            if(checkObj["code"].ToString()=="000")
            {
                Debug.Log("AR胶囊验证成功");
                //给native发消息
            }
                                                        

        }

        private void Start()
        {
            //2D例子1
            // PrefabSpawner spawner = GetComponent<PrefabSpawner>();
            // Vector3 spawnPosition = new Vector3(0, -1.6f, -8.9f);
            // Vector3 spawnRotation = new Vector3(-20.9f, -51, 19.9f);
            // string data = "{\"message\":\"Hello from Unity type3\"}";
            // StartCoroutine(spawner.SpawnPrefabWithDelay(spawnPosition, spawnRotation, 3,"0", data));
            
           // StartCoroutine(DelayedSpawn());
            
            //3D例子1
            //spawnPosition = new Vector3(6, -3, -3);
            //spawnRotation = new Vector3(0, 0, 0);
            //data = "{\"message\":\"Hello from Unity type31\"}";
            //StartCoroutine(spawner.SpawnPrefabWithDelay(spawnPosition, spawnRotation, 3,"1", data));

            
            //
            // spawnPosition = new Vector3(29, 16, 16);
            // spawnRotation = new Vector3(0, 0, 0);
            // data = "{\"message\":\"Hello from Unity 2\"}";
            // StartCoroutine(spawner.SpawnPrefabWithDelay(spawnPosition, spawnRotation, 2, "1",data));
            //
            // spawnPosition = new Vector3(66, 1, 26);
            // spawnRotation = new Vector3(0, 0, 0);
            // data = "{\"message\":\"Hello from Unity 2\"}";
            // StartCoroutine(spawner.SpawnPrefabWithDelay(spawnPosition, spawnRotation, 2, 2, data));
        }
       
        public IEnumerator DelayedSpawn()
        {
            yield return new WaitForSeconds(0.2f); // 2-second delay before spawning

            //例子1
            PrefabSpawner spawner = GetComponent<PrefabSpawner>();
            // Z 轴上：
            // 1. 动态的 8.9 / 固定 -8.9  接近于重合深度距离
            // 2. 动态的 12f 比 8.9f 看起来在更远的位置。
            // 结论：只要延时加载后，动态生成的坐标和固定生成的坐标可以保持一致。
            
            Vector3 spawnPosition = new Vector3(0, -1.6f, 1.9f); 
            Vector3 spawnRotation = new Vector3(-20.9f, -51, 19.9f);
            ToNativeData toNativeData = new ToNativeData
            {
                command = CommandDataType.ARClockIn.ToString(),
                data = ""
            };
            string data = "{\"message\":\"Hello from Unity type3\"}";
            StartCoroutine(spawner.SpawnPrefabWithDelay(spawnPosition, spawnRotation, 3, "0", data));
        }

        public void AsyCreateCapsule(string data)
        {
            StartCoroutine(CreateCapsule(data));
        }
        public IEnumerator CreateCapsule(string data)
        {
           // JObject m_data = JObject.Parse(data);
           //string id= m_data["cid"].ToString();
            //Vector3 spawnPosition = new Vector3(0, -1.6f, 1.9f);
            //Vector3 spawnRotation = new Vector3(-20.9f, -51, 19.9f);
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), -0.5f, UnityEngine.Random.Range(1f, 1.5f));
            Vector3 spawnRotation = new Vector3(0f, 0, 0f);
            GameObject prefabToSpawn = timeCapsule3D;
            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            spawnedObject.transform.rotation = Quaternion.Euler(spawnRotation);
          //  spawnedObject.name = "胶囊_" + id;
            _spawnedPrefabs[spawnedObject.name] = spawnedObject;
            PrefabData prefabData = spawnedObject.AddComponent<PrefabData>();
            prefabData.data = data;
           // spawnedObject.AddComponent<Drag3DObject>();
            spawnedObject.AddComponent<ClickDetector>();
            OnPrefabSpawned(spawnedObject);
            yield return null;
        }
        public IEnumerator SpawnPrefabWithDelay(Vector3 position, Vector3 rotation, float modelType, string id,
            string data)
        {
            GameObject prefabToSpawn;
            bool is2DModel = !Mathf.Approximately(modelType, 3);

            // if (!is2DModel)
            // {
            //     Debug.LogError("Temporary not support 3D capsule.");
            //     yield break;
            // }

            if (Mathf.Approximately(modelType, 3))
            {
                prefabToSpawn = timeCapsule3D;
            }else if (Mathf.Approximately(modelType, 31))
            {
                prefabToSpawn = timeCapsule3D1;
            }
            else if (Mathf.Approximately(modelType, 2))
            {
                prefabToSpawn = timeCapsule2D;
            }
            else
            {
                prefabToSpawn = timeCapsule2D;
            }

            if (prefabToSpawn == null)
            {
                Debug.LogError("Prefab is not assigned!");
                yield break;
            }

            yield return new WaitForSeconds(1f);
            GameObject spawnedObject = Instantiate(prefabToSpawn, position, Quaternion.identity);

            if (is2DModel)
            {
                Vector3 directionToCamera = (_cameraPosition - position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera);
                spawnedObject.transform.rotation = lookRotation;
            }
            else
            {
                spawnedObject.transform.rotation = Quaternion.Euler(rotation);
            }

            spawnedObject.name = "Prefab_" + id;
            _spawnedPrefabs[id] = spawnedObject;
            PrefabData prefabData = spawnedObject.AddComponent<PrefabData>();
            prefabData.data = data;
            spawnedObject.AddComponent<ClickDetector>();
            OnPrefabSpawned(spawnedObject);
        }

        private void OnPrefabSpawned(GameObject spawnedObject)
        {
            Debug.Log("Prefab spawned: " + spawnedObject.name);
        }

        public void DestroyPrefab(string id)
        {
            if (_spawnedPrefabs.ContainsKey(id))
            {
                GameObject obj = _spawnedPrefabs[id];
                _spawnedPrefabs.Remove(id);
                Destroy(obj);
            }
            else
            {
                Debug.LogWarning("No prefab found with id: " + id);
            }
        }

        public void DestroyPrefabs(List<string> ids)
        {
            foreach (string id in ids)
            {
                DestroyPrefab(id);
            }
        }

        public void DestroyAllPrefabs()
        {
            Debug.LogWarning("DestroyAllPrefabs");
            foreach (var obj in _spawnedPrefabs.Values)
            {
                Debug.LogWarning("DestroyAllPrefabs: " + obj);
                Destroy(obj);
            }

            _spawnedPrefabs.Clear();
        }
        private void OnDestroy()
        {
            DestroyAllPrefabs();
        }
    }

    public class ClickDetector : MonoBehaviour
    {
        private void OnMouseDown()
        {
            // PrefabSpawner.Instance.DestroyPrefabs(new List<float>{1,2});
            // PrefabSpawner.Instance.DestroyAllPrefabs();
            PrefabData prefabData = GetComponent<PrefabData>();
            if (prefabData != null)
            {
                Debug.Log("点击胶囊数据: " + prefabData.data);
                //请求AR验证接口
                ARMnager.Instance.RequestUserARCheck();

                //if (NativeBridge.Instance != null)
                //{
                //    NativeBridge.Instance.SendMessageToNative(prefabData.data);
                //}

            }
            
        }
    }
}