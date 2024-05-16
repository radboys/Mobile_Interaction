using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum E_UI_Layer
{
    Bottom,
    Middle,
    Top,
    System,
}

/// <summary>
/// UI层级
/// </summary>

/// <summary>
/// UI管理器
/// 1.管理所有显示的面板
/// 2.提供给外部 显示和隐藏等等接口
/// </summary>
public class UIManager : MonoSingleton<UIManager>
{
    private Dictionary<string, BasePanel> UIPanels = new();

    private Transform bottomLayer;
    private Transform middleLayer;
    private Transform topLayer;
    private Transform systemLayer;

    //记录我们UI的Canvas父对象 方便以后外部可能会使用它
    private RectTransform canvas;

    private void Start()
    {
        //尝试加载Canvas
        GameObject obj = ResourcesLoader.Instance.Load<GameObject>("UI/Canvas");
        if (obj == null)
        {
            //如果加载失败，手动创建Canvas
            obj = CreateCanvas();
        }

        canvas = obj.transform as RectTransform;
        DontDestroyOnLoad(obj);

        //找到各层
        bottomLayer = canvas.Find("Bot");
        middleLayer = canvas.Find("Mid");
        topLayer = canvas.Find("Top");
        systemLayer = canvas.Find("System");

        //尝试加载EventSystem
        obj = ResourcesLoader.Instance.Load<GameObject>("UI/EventSystem");
        if (obj == null)
        {
            //如果加载失败，手动创建EventSystem
            obj = CreateEventSystem();
        }

        DontDestroyOnLoad(obj);
    }

    private GameObject CreateCanvas()
    {
        GameObject canvasObj = new GameObject("Canvas");
        canvasObj.layer = LayerMask.NameToLayer("UI");
        Canvas canvasComponent = canvasObj.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        CreateLayer(canvasObj.transform, "Bot");
        CreateLayer(canvasObj.transform, "Mid");
        CreateLayer(canvasObj.transform, "Top");
        CreateLayer(canvasObj.transform, "System");

        return canvasObj;
    }

    private void CreateLayer(Transform parent, string name)
    {
        GameObject layer = new GameObject(name);
        layer.transform.SetParent(parent);
        RectTransform rect = layer.AddComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localScale = Vector3.one;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private GameObject CreateEventSystem()
    {
        GameObject eventSystemObj = new GameObject("EventSystem");
        eventSystemObj.layer = LayerMask.NameToLayer("UI");
        eventSystemObj.AddComponent<EventSystem>();
        eventSystemObj.AddComponent<StandaloneInputModule>();
        return eventSystemObj;
    }

    /// <summary>
    /// 通过层级枚举 得到对应层级的父对象
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bottom:
                return this.bottomLayer;
            case E_UI_Layer.Middle:
                return this.middleLayer;
            case E_UI_Layer.Top:
                return this.topLayer;
            case E_UI_Layer.System:
                return this.systemLayer;
        }
        return null;
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示在哪一层</param>
    /// <param name="callBack">当面板预设体创建成功后 你想做的事</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer = E_UI_Layer.Middle, UnityAction<T> callBack = null) where T : BasePanel
    {
        if (UIPanels.ContainsKey(panelName))
        {
            UIPanels[panelName].ShowMe();
            // 处理面板创建完成后的逻辑
            callBack?.Invoke(UIPanels[panelName] as T);
            //避免面板重复加载 如果存在该面板 即直接显示 调用回调函数后  直接return 不再处理后面的异步加载逻辑
            return;
        }

        ResourcesLoader.Instance.LoadAsync<GameObject>("UI/Panels/" + panelName, (obj) =>
        {
            //把他作为 Canvas的子对象
            //并且 要设置它的相对位置
            //找到父对象 你到底显示在哪一层
            Transform father = GetLayerFather(layer);
            //设置父对象  设置相对位置和大小
            obj.transform.SetParent(father);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //得到预设体身上的面板脚本
            T panel = obj.GetComponent<T>();
            // 处理面板创建完成后的逻辑
            callBack?.Invoke(panel);

            panel.ShowMe();

            //把面板存起来
            UIPanels.Add(panelName, panel);
        });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (UIPanels.ContainsKey(panelName))
        {
            UIPanels[panelName].HideMe();
        }
    }

    /// <summary>
    /// 得到某一个已经显示的面板 方便外部使用
    /// </summary>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (UIPanels.ContainsKey(name))
            return UIPanels[name] as T;
        return null;
    }

    public void ClearPanel()
    {
        if (UIPanels.Count != 0)
        {
            foreach (var kvp in UIPanels)
            {
                GameObject.Destroy(UIPanels[kvp.Key].gameObject);
            }
            UIPanels.Clear();
        }
    }
}
