using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject myPrefab;  // 在Unity编辑器中将你的预制体拖到这里

    void Start()
    {
        // 检查是否分配了预制体
        if (myPrefab == null)
        {
            Debug.LogError("未分配预制体！");
            return;
        }

        
    }

    public void SpawnCube()
    {
        // 生成随机位置
        float y = Random.Range(0.5f, 4.5f);
        float z = Random.Range(-4.5f, 4.5f);

        // 指定x值为9
        float x = 9f;

        // 生成预制体
        Vector3 spawnPosition = new Vector3(x, y, z);
        Instantiate(myPrefab, spawnPosition, Quaternion.identity);
    }


    public void TargetDestroy()
    {
        SpawnCube();
        Destroy(this);
    }
}
