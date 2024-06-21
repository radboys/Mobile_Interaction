using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using UnityEngine.SceneManagement;

public class RandomTarget : MonoBehaviour
{
    public static RandomTarget Instance { get; private set; }

    public List<GameObject> targets = new List<GameObject>();

    private float startTime;

    public float reloadCount = 0;

    public float weaponSwitchCount = 0;

    public float totalShootingCount = 0;

    public static UnityAction Fire;

    public static UnityAction Reload;

    public static UnityAction<int> SwitchWeapon;

    private GameObject continueButton;

    private void Awake()
    {
        Instance = this;
        startTime = Time.time;
    }

    private void Start()
    {
        continueButton = GameObject.Find("NextScene");
        continueButton.SetActive(false);
    }


    private void FixedUpdate()
    {
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
            string result = $"Total time: {totalTime}\nReload count: {reloadCount}\nWeapon switch count: {weaponSwitchCount}\nShots count: {totalShootingCount}";
            Debug.Log(result);

            GameManager.Instance.StoreResult(result);
            continueButton.SetActive(true);

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


}
