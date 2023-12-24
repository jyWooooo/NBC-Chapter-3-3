using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private bool _isInitailized = false;
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                _instance.Initialize();
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            _instance.Initialize();
        }
    }

    protected virtual void Start()
    {
        if (_instance != this)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    public virtual bool Initialize()
    {
        if (_isInitailized) return false;

        var root = GameObject.Find("@Managers");
        if (root == null)
            root = new("@Managers");
        transform.parent = root.transform;

        _isInitailized = true;
        return true;
    }
}