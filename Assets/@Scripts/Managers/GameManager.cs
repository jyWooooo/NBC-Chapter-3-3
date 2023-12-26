using Photon.Pun;
using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Start()
    {
        base.Start();

        // 1. ���ҽ� �ε�
        ResourceLoad((key, count, total) =>
        {
            if (count == total)
            {
                // 2. ���� ���� ����
                PhotonConnect(() =>
                {
                    // 3. ��, ĳ����, �� ���� (���� ����)
                    GameStart();
                });
            }
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

        CreateMap();
        PlayerSpawn();
        BallSpawn();
    }

    private void BallSpawn()
    {
        var ballPrefab = ResourceManager.Instance.GetCache<GameObject>("Ball.prefab");
        Instantiate(ballPrefab);
    }

    private void CreateMap()
    {
        var floorPrefab = ResourceManager.Instance.GetCache<GameObject>("Floor.prefab");
        Instantiate(floorPrefab);
    }

    private void SetPhotonPrefabPool()
    {
        // 1. �÷��̾� prefab ���
        var playerPrefab = ResourceManager.Instance.GetCache<GameObject>("Player.prefab");
        var pool = PhotonNetwork.PrefabPool as DefaultPool;
        pool.ResourceCache.Add("Player.prefab", playerPrefab);
    }

    public void PlayerSpawn()
    {
        // PhotonNetwork.PrefabPool�� �̿��� ����
        var playerInstance = PhotonNetwork.Instantiate("Player.prefab", Vector3.zero, Quaternion.identity);
        var player = playerInstance.GetComponent<Player>();
        player.SetInfo(ResourceManager.Instance.GetCache<GameObject>("XBot.fbx"), ResourceManager.Instance.GetCache<RuntimeAnimatorController>("Humanoid.anim"));
    }
}