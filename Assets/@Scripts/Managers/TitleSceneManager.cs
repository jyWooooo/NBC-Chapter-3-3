using System;

public class TitleSceneManager : Singleton<TitleSceneManager>
{
    protected override void Start()
    {
        base.Start();

        // 1. 리소스 로드
        ResourceLoad((key, count, total) =>
        {
            if (count == total)
            {
                // 2. UI 생성
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