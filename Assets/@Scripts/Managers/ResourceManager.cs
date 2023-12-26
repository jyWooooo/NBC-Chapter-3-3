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

        var operation = Addressables.LoadAssetAsync<T>(key);
        operation.Completed += op =>
        {
            _resources.TryAdd(key, op.Result);
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

    public T GetCache<T>(string key) where T : UnityEngine.Object
    {
        if (!_resources.TryGetValue(key, out var resource))
            return null;
        return resource as T;
    }

    public void Release(string key)
    {
        if (_resources.TryGetValue(key, out var resource))
        {
            Addressables.Release(resource);
            _resources.Remove(key);
        }
    }
}