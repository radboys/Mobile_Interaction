using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    public Text level1nfo;
    public Text level2nfo;
    public Text level3nfo;
    public Text level4nfo;
    // Start is called before the first frame update
    void Start()
    {
        level1nfo.text = GameManager.Instance.level1Result;
        level2nfo.text = GameManager.Instance.level2Result;
        level3nfo.text = GameManager.Instance.level3Result;
        level4nfo.text = GameManager.Instance.level4Result;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
