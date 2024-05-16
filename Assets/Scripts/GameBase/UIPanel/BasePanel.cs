using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    public virtual void ShowMe()
    {
        gameObject.SetActive(true);
    }
    public virtual void HideMe()
    {
        gameObject.SetActive(false);
    }
}
