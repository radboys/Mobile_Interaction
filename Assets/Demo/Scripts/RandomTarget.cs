using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

public class RandomTarget : MonoBehaviour
{
    public static RandomTarget Instance { get; private set; }

    public List<GameObject> targets = new List<GameObject>();

    private float startTime;

    public float reloadCount = 0;

    public float weaponSwitchCount = 0;

    public static UnityAction Fire;

    private void Awake()
    {
        Instance = this;
        startTime = Time.time;
    }

    private void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.None;
        if (SystemInfo.supportsGyroscope && !Input.gyro.enabled)
        {
            Input.gyro.enabled = true;
        }
    }

    public void HitTarget(GameObject target)
    {
        if (!targets.Remove(target))
        {
            Debug.LogError("No target included");
            return;
        }

        Destroy(target);

        if (targets.Count == 0)
        {
            float totalTime = Time.time - startTime;
            string result = $"Total time: {totalTime}\nReload count: {reloadCount}\nWeapon switch count: {weaponSwitchCount}";
            Debug.Log(result);

            // Save the result to a txt file
            SaveResultToFile(result);
            return;
        }

        var temp = targets[Random.Range(0, targets.Count)];
        temp.transform.position = RandomPosition();
        temp.SetActive(true);
    }

    private Vector3 RandomPosition()
    {
        return new Vector3(9f, Random.Range(0.5f, 4.5f), Random.Range(-4.5f, 4.5f));
    }

    private void SaveResultToFile(string result)
    {
        string path = Path.Combine(Application.persistentDataPath, "result.txt");
        try
        {
            if (File.Exists(path))
            {
                // Append to the existing file
                File.AppendAllText(path, result + "\n");
                Debug.Log("Appended result to " + path);
            }
            else
            {
                // Create a new file and write the result
                File.WriteAllText(path, result + "\n");
                Debug.Log("Created and wrote result to " + path);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save result: " + e.Message);
        }
    }
}
