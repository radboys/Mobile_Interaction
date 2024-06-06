using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireButton : MonoBehaviour
{
    public void Fire()
    {
        RandomTarget.Fire?.Invoke();
    }
}
