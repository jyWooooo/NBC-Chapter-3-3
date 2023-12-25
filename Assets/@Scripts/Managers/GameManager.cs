using Photon.Pun;
using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected override void Start()
    {
        base.Start();

        // 1. 포톤 서버 연결
        PhotonConnect(() =>
        {
            // 2. 리소스 로드
            ResourceLoad((key, count, total) =>
            {
                if (count == total)
                {
                    // 3. 맵, 캐릭터, 공 생성 (게임 시작)
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
        // Addressable에서 Load한 prefab을 PhotonNetwork.PrefabPool에 등록해야함.
        SetPhotonPrefabPool();

        PlayerSpawn();
    }

    private void SetPhotonPrefabPool()
    {
        // 1. 플레이어 prefab 등록
        var playerPrefab = ResourceManager.Instance.GetCache<GameObject>("Player.prefab");
        var pool = PhotonNetwork.PrefabPool as DefaultPool;
        pool.ResourceCache.Add("Player.prefab", playerPrefab);

        // 2. 공 prefab 등록

    }

    public void PlayerSpawn()
    {
        // PhotonNetwork.PrefabPool을 이용해 생성
        var playerInstance = PhotonNetwork.Instantiate("Player.prefab", Vector3.zero, Quaternion.identity);
        var player = playerInstance.GetComponent<Player>();
        player.SetInfo(ResourceManager.Instance.GetCache<GameObject>("XBot.fbx"), ResourceManager.Instance.GetCache<RuntimeAnimatorController>("Humanoid.anim"));
    }
}