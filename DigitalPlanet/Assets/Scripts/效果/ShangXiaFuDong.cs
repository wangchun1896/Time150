using UnityEngine;
using System.Collections;

public class ShangXiaFuDong : MonoBehaviour
{
    [Header("浮动参数")]
    public float floatSpeed = 1f;      // 浮动频率
    public float floatHeight = 0.1f;  // 浮动幅度

    private Vector3 _startPosition;
    private float _randomPhase;

    void OnEnable()
    {
        _startPosition = transform.position;
        _randomPhase = Random.Range(0f, 2f * Mathf.PI);
        StartCoroutine(FloatMovement()); // 启动协程
    }

    IEnumerator FloatMovement()
    {
        while (true)
        {
            // 计算新位置
            float newY = _startPosition.y + Mathf.Sin(Time.time * floatSpeed + _randomPhase) * floatHeight;
            transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);

            // 控制协程执行频率（每帧执行一次，等同于Update，但可自由调整间隔）
            yield return null; 

            // 如果希望降低频率（例如每0.1秒更新一次）：
            // yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}