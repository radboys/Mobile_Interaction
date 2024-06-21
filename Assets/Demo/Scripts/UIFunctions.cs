using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFunctions : MonoBehaviour
{
    public GameObject reloadButton;

    public GameObject previousButton;

    public GameObject nextButton;

    private void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":reloadButton.SetActive(true);
                previousButton.SetActive(true);
                nextButton.SetActive(true);
                break;

            case "Level2":
                reloadButton.SetActive(false);
                previousButton.SetActive(true);
                nextButton.SetActive(true);
                break;

            case "Level3":
                reloadButton.SetActive(true);
                previousButton.SetActive(false);
                nextButton.SetActive(false);
                break;

            case "Level4":
                reloadButton.SetActive(false);
                previousButton.SetActive(false);
                nextButton.SetActive(false);
                break;
        }
    }
    public void Fire()
    {
        RandomTarget.Fire?.Invoke();
    }

    public void Reload()
    {
        RandomTarget.Reload?.Invoke();
    }

    public void SwitchWeapon(int direction)
    {
        RandomTarget.SwitchWeapon?.Invoke(direction);
    }

    public void NextScene()
    {
        GameManager.Instance.ChangeScene();
    }
}
