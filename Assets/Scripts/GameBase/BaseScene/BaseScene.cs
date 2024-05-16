using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScene : MonoBehaviour
{
    public virtual void EnterScene()
    {
        print("Switch to " + GetType().Name);
    }

    public virtual void ExitScene()
    {
        UIManager.Instance.ClearPanel();
        Destroy(gameObject);
    }
}
