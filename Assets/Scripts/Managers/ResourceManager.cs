using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<string, UnityEngine.Object> _resources = new();

    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (_resources.TryGetValue(key, out var resource))
        {
            callback?.Invoke(resource as T);
            return;
        }

        string loadKey = key;
        //if (key.Contains(".sprite"))
        //    loadKey = $"{key}[{key.Replace(".sprite", "")}]";

        var operation = Addressables.LoadAssetAsync<T>(loadKey);
        operation.Completed += op =>
        {
            _resources.Add(key, op.Result);
            callback?.Invoke(op.Result);
        };
    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> callback = null) where T : UnityEngine.Object
    {
        var operation = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        operation.Completed += op =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                LoadAsync<T>(result.PrimaryKey, obj =>
                {
                    loadCount++;
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }

    public T Load<T>(string key) where T : UnityEngine.Object
    {
        if (!_resources.TryGetValue(key, out var resource))
            return null;
        return resource as T;
    }

    public void UnLoad(string key)
    {
        if (_resources.TryGetValue(key, out var resource))
        {
            Addressables.Release(resource);
            _resources.Remove(key);
        }
    }

    public GameObject Instantiate(string key) => Instantiate<GameObject>(key);

    public T Instantiate<T>(string key) where T : UnityEngine.Object
    {
        var obj = Load<T>(key);
        if (obj == null)
        {
            Debug.LogError($"{nameof(ResourceManager)}: {key} Load failed.");
            return null;
        }
        return Instantiate(obj);
    }

    new public void Destroy(UnityEngine.Object obj)
    {
        // 어떻게 동작하는 걸까 ?
        // 단순히 Destroy(gameObject)와 어떤 차이점이 있을까 .
        UnityEngine.Object.Destroy(obj);
    }
}