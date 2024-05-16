using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesLoader : MonoSingleton<ResourcesLoader>
{
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="name">资源路径</param>
    /// <returns>加载的资源</returns>
    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        if (res == null)
        {
            Debug.LogError($"Resource not found: {name}");
            return null;
        }

        if (res is GameObject)
        {
            return GameObject.Instantiate(res);
        }
        else
        {
            return res;
        }
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="name">资源路径</param>
    /// <param name="callback">加载完成后的回调</param>
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        StartCoroutine(RootLoadAsync(name, callback));
    }

    /// <summary>
    /// 异步加载资源的协程
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="name">资源路径</param>
    /// <param name="callback">加载完成后的回调</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator RootLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if (r.asset == null)
        {
            Debug.LogError($"Resource not found: {name}");
            callback(null);
            yield break;
        }

        if (r.asset is GameObject)
        {
            callback(GameObject.Instantiate(r.asset) as T);
        }
        else
        {
            callback(r.asset as T);
        }
    }
}
