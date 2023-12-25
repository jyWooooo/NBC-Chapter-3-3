using Photon.Pun;
using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Start()
    {
        base.Start();

        // 1. ���� ���� ����
        PhotonConnect(() =>
        {
            // 2. ���ҽ� �ε�
            ResourceLoad((key, count, total) =>
            {
                if (count == total)
                {
                    // 3. ��, ĳ����, �� ���� (���� ����)
                    GameStart();
                }
            });
        });
    }

    public void PhotonConnect(Action callback = null)
    {
        PhotonConnector.Instance.OnComplete += callback;
        PhotonConnector.Instance.ConnectNow();
    }

    public void ResourceLoad(Action<string, int, int> callback = null)
    {
        ResourceManager.Instance.LoadAllAsync<UnityEngine.Object>("Preload", callback);
    }

    public void GameStart()
    {
        // Addressable���� Load�� prefab�� PhotonNetwork.PrefabPool�� ����ؾ���.
        SetPhotonPrefabPool();

        PlayerSpawn();
    }

    private void SetPhotonPrefabPool()
    {
        // 1. �÷��̾� prefab ���
        var playerPrefab = ResourceManager.Instance.GetCache<GameObject>("Player.prefab");
        var pool = PhotonNetwork.PrefabPool as DefaultPool;
        pool.ResourceCache.Add("Player.prefab", playerPrefab);

        // 2. �� prefab ���

    }

    public void PlayerSpawn()
    {
        // PhotonNetwork.PrefabPool�� �̿��� ����
        var playerInstance = PhotonNetwork.Instantiate("Player.prefab", Vector3.zero, Quaternion.identity);
        var player = playerInstance.GetComponent<Player>();
        player.SetInfo(ResourceManager.Instance.GetCache<GameObject>("XBot.fbx"), ResourceManager.Instance.GetCache<RuntimeAnimatorController>("Humanoid.anim"));
    }
}