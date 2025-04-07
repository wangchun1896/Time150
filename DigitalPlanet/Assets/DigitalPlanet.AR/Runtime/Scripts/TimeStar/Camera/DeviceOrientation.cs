using System.Collections.Generic;
using UnityEngine;

namespace TimeStar.DigitalPlant
{
    public class DeviceOrientation : MonoBehaviour
    {
        public float verticalThreshold = 0.3f;
        public float flatThreshold = 0.3f;
        public float accelThreshold = 0.2f;
        public float gyroThreshold = 1.0f;
        public int stableFrames = 5; 
        private int _uprightFrameCount = 0;
        private int _flatFrameCount = 0;
        private int _heldFrameCount = 0;
        private readonly Queue<Vector3> _accelQueue = new Queue<Vector3>();
        private readonly Queue<Vector3> _gyroQueue = new Queue<Vector3>();
        public int filterSize = 5;
        void Start()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            Input.gyro.enabled = true;
            Input.compass.enabled = true;
        }
        void Update()
        {
            Vector3 rawAccel = GetAdjustedAcceleration();
            Vector3 accel = GetFilteredData(rawAccel, _accelQueue);
            Vector3 gyro = GetFilteredData(Input.gyro.rotationRateUnbiased, _gyroQueue);
            bool isFlat = IsDeviceFlat(accel, flatThreshold);
            if (isFlat)
            {
                _flatFrameCount++;
            }
            else
            {
                _flatFrameCount = 0;
            }
            if (_flatFrameCount >= stableFrames)
            {
                _uprightFrameCount = 0;
                _heldFrameCount = 0;
            }
            if (!isFlat)
            {
                bool isUpright = IsDeviceUpright(accel, verticalThreshold);
                if (isUpright)
                {
                    _uprightFrameCount++;
                }
                else
                {
                    _uprightFrameCount = 0;
                }
                if (_uprightFrameCount >= stableFrames)
                {
                }
                bool isBeingHeld = IsDeviceBeingHeld(accel, gyro, accelThreshold, gyroThreshold);
                if (isBeingHeld)
                {
                    _heldFrameCount++;
                }
                else
                {
                    _heldFrameCount = 0;
                }
                if (_heldFrameCount >= stableFrames && isUpright)
                {
                }
            }
            Quaternion gyroAttitude = Input.gyro.attitude;
            gyroAttitude = GyroToUnity(gyroAttitude);
            transform.localRotation = gyroAttitude;
            float yaw = Input.compass.trueHeading;
            string direction = GetDirection(yaw);
            //Debug.Log(direction);
        }
        Vector3 GetFilteredData(Vector3 current, Queue<Vector3> queue)
        {
            queue.Enqueue(current);
            if (queue.Count > filterSize)
                queue.Dequeue();
            Vector3 sum = Vector3.zero;
            foreach (var item in queue)
                sum += item;
            return sum / queue.Count;
        }
        Vector3 GetAdjustedAcceleration()
        {
            Vector3 accel = Input.acceleration;
            ScreenOrientation orientation = Screen.orientation;
            switch (orientation)
            {
                case ScreenOrientation.Portrait:
                    return new Vector3(accel.x, accel.y, accel.z);
                case ScreenOrientation.LandscapeLeft:
                    return new Vector3(accel.y, -accel.x, accel.z);
                case ScreenOrientation.LandscapeRight:
                    return new Vector3(-accel.y, accel.x, accel.z);
                case ScreenOrientation.PortraitUpsideDown:
                    return new Vector3(-accel.x, -accel.y, accel.z);
                default:
                    return accel;
            }
        }
        bool IsDeviceUpright(Vector3 accel, float threshold)
        {
            float deltaY = Mathf.Abs(Mathf.Abs(accel.y) - 1.0f);
            float deltaX = Mathf.Abs(accel.x);
            float deltaZ = Mathf.Abs(accel.z);
            return (deltaY < threshold) && (deltaX < threshold) && (deltaZ < threshold);
        }
        bool IsDeviceFlat(Vector3 accel, float threshold)
        {
            float deltaZ = Mathf.Abs(Mathf.Abs(accel.z) - 1.0f);
            float deltaX = Mathf.Abs(accel.x);
            float deltaY = Mathf.Abs(accel.y);
            return (deltaZ < threshold) && (deltaX < threshold) && (deltaY < threshold);
        }
        bool IsDeviceBeingHeld(Vector3 accel, Vector3 gyro, float accelThreshold, float gyroThreshold)
        {
            float accelMagnitude = Mathf.Abs(accel.magnitude - 1.0f);
            bool isMovingAccel = accelMagnitude > accelThreshold;
            float gyroRate = gyro.magnitude;
            bool isMovingGyro = gyroRate > gyroThreshold;
            return isMovingAccel || isMovingGyro;
        }
        string GetDirection(float yaw)
        {
            if (yaw >= 337.5f || yaw < 22.5f)
                return "北";
            else if (yaw >= 22.5f && yaw < 67.5f)
                return "东北";
            else if (yaw >= 67.5f && yaw < 112.5f)
                return "东";
            else if (yaw >= 112.5f && yaw < 157.5f)
                return "东南";
            else if (yaw >= 157.5f && yaw < 202.5f)
                return "南";
            else if (yaw >= 202.5f && yaw < 247.5f)
                return "西南";
            else if (yaw >= 247.5f && yaw < 292.5f)
                return "西";
            else
                return "西北";
        }
        private static Quaternion GyroToUnity(Quaternion q)
        {
            Quaternion rotatedQ = new Quaternion(q.x, q.y, -q.z, -q.w);
            Quaternion extraRotation = Quaternion.Euler(90, 0, 0);
            return extraRotation * rotatedQ;
        }
    }
}