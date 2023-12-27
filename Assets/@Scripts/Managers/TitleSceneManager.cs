using System;

public class TitleSceneManager : Singleton<TitleSceneManager>
{
    protected override void Start()
    {
        base.Start();

        // 1. ���ҽ� �ε�
        ResourceLoad((key, count, total) =>
        {
            if (count == total)
            {
                // 2. UI ����
                ShowTitleSceneUI();
            }
        });
    }

    public void ResourceLoad(Action<string, int, int> callback = null)
    {
        ResourceManager.Instance.LoadAllAsync<UnityEngine.Object>("Title", callback);
    }

    public void ShowTitleSceneUI()
    {
        UIManager.Instance.ShowSceneUI<TitleSceneUI>();
    }
}