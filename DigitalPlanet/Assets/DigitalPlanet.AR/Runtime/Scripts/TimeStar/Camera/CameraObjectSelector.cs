using UnityEngine;

namespace TimeStar.DigitalPlant
{
    public class CameraObjectSelector : MonoBehaviour
    {
        public enum SelectionEffectType
        {
            Simple,
            Breathing,
            Rotation,
            All
        }

        [Header("General Settings")]
        [Tooltip("选择效果类型")]
        public SelectionEffectType effectType = SelectionEffectType.Simple;
        [Tooltip("射线的最大检测距离")]
        public float maxRayDistance = 100f;
        [Tooltip("缩放动画的平滑度(值越小越平滑)")]
        public float smoothSpeed = 5f;
        [Tooltip("状态切换的缓冲时间(秒)")]
        public float stateChangeDelay = 0.2f;

        [Header("Simple Scale Effect")]
        [Tooltip("物体被选中时的放大倍数")]
        public float scaleFactor = 1.2f;

        [Header("Breathing Effect")]
        [Tooltip("物体被选中时的基础放大倍数")]
        public float baseScaleFactor = 1.1f;
        [Tooltip("呼吸效果的额外缩放幅度")]
        public float breathingAmount = 0.1f;
        [Tooltip("呼吸动画的速度")]
        public float breathingSpeed = 3f;

        [Header("Rotation Effect")]
        [Tooltip("旋转速度 (度/秒)")]
        public float rotationSpeed = 50f;
        [Tooltip("旋转轴")]
        public Vector3 rotationAxis = Vector3.up;
        [Tooltip("旋转插值速度")]
        public float rotationLerpSpeed = 2f;

        private Camera mainCamera;
        private GameObject currentTarget;
        private Vector3 originalScale;
        private Quaternion originalRotation;
        private float breathingTime;
        private bool isRotating;

        private float selectionTimer;
        private float deselectionTimer;
        private float currentRotationSpeed;
        private float targetRotationSpeed;
        private bool isFullySelected;
        private GameObject pendingTarget;
        private float effectStrength;
        private GameObject lastTarget;
        private Quaternion currentRotation;
        private bool isDeselecting;

        private GameObject cubeObject;

        void Start()
        {
            mainCamera = GetComponent<Camera>();
            if (mainCamera == null)
            {
                Debug.LogError("This script must be attached to a Camera!");
                enabled = false;
                return;
            }
        }

        // 递归查找子物体，包括未激活的对象
        private GameObject FindChildRecursively(Transform parent, string childName)
        {
            // 检查当前物体
            if (parent.name == childName)
            {
                return parent.gameObject;
            }

            // 遍历所有子物体，包括未激活的
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                GameObject found = FindChildRecursively(child, childName);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        void Update()
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                GameObject hitObject = hit.collider.gameObject;

                // 在射线命中对象的层级中查找Cube_001
                cubeObject = FindChildRecursively(hitObject.transform, "Cube_001");
                if (cubeObject != null)
                {
                    cubeObject.SetActive(true);
                }

                HandleTargetSelection(hitObject);
            }
            else
            {
                if (cubeObject != null)
                {
                    cubeObject.SetActive(false);
                }
                HandleDeselection();
            }

            UpdateEffects();
        }

        void HandleTargetSelection(GameObject hitObject)
        {
            if (currentTarget != hitObject)
            {
                if (pendingTarget != hitObject)
                {
                    pendingTarget = hitObject;
                    selectionTimer = 0;
                }

                selectionTimer += Time.deltaTime;

                if (selectionTimer >= stateChangeDelay)
                {
                    if (isDeselecting && currentTarget != null)
                    {
                        CompleteReset();
                    }

                    if (currentTarget != null)
                    {
                        lastTarget = currentTarget;
                        StartDeselection();
                    }

                    currentTarget = hitObject;
                    originalScale = currentTarget.transform.localScale;
                    originalRotation = currentTarget.transform.rotation;
                    currentRotation = originalRotation;
                    breathingTime = 0f;
                    isRotating = true;
                    effectStrength = 0f;
                    isFullySelected = false;
                    isDeselecting = false;
                }
            }
            else
            {
                effectStrength = Mathf.Min(effectStrength + Time.deltaTime / stateChangeDelay, 1f);
                isFullySelected = effectStrength >= 1f;
                deselectionTimer = 0f;
                isDeselecting = false;
            }
        }

        void HandleDeselection()
        {
            if (currentTarget != null)
            {
                deselectionTimer += Time.deltaTime;

                if (deselectionTimer >= stateChangeDelay)
                {
                    StartDeselection();
                }
            }

            pendingTarget = null;
            selectionTimer = 0f;
        }

        void StartDeselection()
        {
            if (currentTarget != null)
            {
                isDeselecting = true;
                effectStrength = Mathf.Max(effectStrength - Time.deltaTime / stateChangeDelay, 0f);

                if (effectStrength <= 0f)
                {
                    CompleteReset();
                }
            }
        }

        void UpdateEffects()
        {
            if (currentTarget == null) return;

            Vector3 targetScale = originalScale;

            if (effectType == SelectionEffectType.Simple || effectType == SelectionEffectType.All)
            {
                float currentScaleFactor = 1f + (scaleFactor - 1f) * effectStrength;
                targetScale = originalScale * currentScaleFactor;
            }

            if (effectType == SelectionEffectType.Breathing || effectType == SelectionEffectType.All)
            {
                breathingTime += Time.deltaTime;
                float breathingFactor = 1f + Mathf.Sin(breathingTime * breathingSpeed) * breathingAmount * effectStrength;
                targetScale = targetScale * (1f + (baseScaleFactor - 1f) * effectStrength) * breathingFactor;
            }

            currentTarget.transform.localScale = Vector3.Lerp(
                currentTarget.transform.localScale,
                targetScale,
                Time.deltaTime * smoothSpeed
            );

            if (effectType == SelectionEffectType.Rotation || effectType == SelectionEffectType.All)
            {
                targetRotationSpeed = isRotating ? rotationSpeed : 0f;

                if (isDeselecting)
                {
                    currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0f, Time.deltaTime * rotationLerpSpeed);
                }
                else
                {
                    currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, targetRotationSpeed * effectStrength,
                        Time.deltaTime * rotationLerpSpeed);
                }

                if (Mathf.Abs(currentRotationSpeed) > 0.01f)
                {
                    currentRotation *= Quaternion.AngleAxis(currentRotationSpeed * Time.deltaTime, rotationAxis);
                    currentTarget.transform.rotation = currentRotation;
                }
            }
        }

        void CompleteReset()
        {
            if (currentTarget != null)
            {
                currentTarget.transform.localScale = originalScale;
                currentTarget = null;
                isRotating = false;
                isFullySelected = false;
                effectStrength = 0f;
                currentRotationSpeed = 0f;
                isDeselecting = false;
            }

            if (cubeObject != null)
            {
                cubeObject.SetActive(false);
            }
        }

        void OnDisable()
        {
            CompleteReset();
        }
    }
}