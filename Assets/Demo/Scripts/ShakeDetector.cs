using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    public float shakeThreshold = 2.0f;  // 调整这个阈值来检测摇动的灵敏度
    public float resetThreshold = 0.5f;  // 调整这个阈值来检测设备是否回正
    private bool isShaking = false;

    public float tiltThreshold = 0.5f; // 旋转阈值

    private bool isTilting = false;

    void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
    }

    void Update()
    {
        //Shake();
        DetectTilt();
    }

    private void Shake()
    {
        //print(Input.acceleration);

        // 获取设备在X轴上的加速度
        float accelerationX = Input.acceleration.x;

        // 检测向左晃动
        if (accelerationX < -shakeThreshold && !isShaking)
        {
            isShaking = true;
            Debug.Log("向左晃动");
        }
        // 检测向右晃动
        else if (accelerationX > shakeThreshold && !isShaking)
        {
            isShaking = true;
            Debug.Log("向右晃动");
        }

        // 检测设备是否回正
        if (Mathf.Abs(accelerationX) < resetThreshold && isShaking)
        {
            isShaking = false;
            Debug.Log("回正");
        }
        //print(Input.acceleration);
    }

    private void DetectTilt()
    {
        float xRotationRate = Input.gyro.rotationRateUnbiased.x;
        //print(xRotationRate);
        // 检测是否有足够的旋转
        if (xRotationRate > tiltThreshold && !isTilting)
        {
            isTilting = true;
            Debug.Log("向前倾斜");
        }
        else if (xRotationRate < -tiltThreshold && !isTilting)
        {
            isTilting = true;
            Debug.Log("向后倾斜");
        }

        // 检测设备是否回正
        if (Mathf.Abs(xRotationRate) < resetThreshold && isTilting)
        {
            isTilting = false;
            Debug.Log("回正");
        }
    }
}