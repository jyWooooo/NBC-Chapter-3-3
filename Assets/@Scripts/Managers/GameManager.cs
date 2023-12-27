using Photon.Pun;
using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject ball;
    private Rigidbody _ballRigid;

    public struct BallStateStruct
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
    }

    // �� ����ü�� �̿��� ���� ������ ��� ������ ���
    public BallStateStruct ballState = new()
    {
        position = new Vector3(0, 10, 0),
        rotation = Quaternion.identity,
        velocity = new Vector3(0, 0, 0),
    };

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

    private void Update()
    {
        if (ball == null)
        {
            ball = GameObject.FindWithTag("Ball");
            if (ball != null)
                _ballRigid = ball.GetComponent<Rigidbody>();
            return;
        }

        // �� ���� ����
        ballState.position = ball.transform.position;
        ballState.rotation = ball.transform.rotation;
        ballState.velocity = _ballRigid.velocity;
    }

    public void PhotonConnect(Action callback = null)
    {
        PhotonConnector.Instance.OnComplete += callback;
        PhotonConnector.Instance.ConnectNow();
    }

    public void ResourceLoad(Action<string, int, int> callback = null)
    {
        ResourceManager.Instance.LoadAllAsync<UnityEngine.Object>("Game", callback);
    }

    public void GameStart()
    {
        // Addressable���� Load�� prefab�� PhotonNetwork.PrefabPool�� ����ؾ���.
        SetPhotonPrefabPool();

        CreateMap();
        PlayerSpawn();
        BallSpawn();

        Camera.main.clearFlags = CameraClearFlags.Skybox;
    }

    public void BallSpawn()
    {
        // Host������ �� ����
        if (PhotonNetwork.IsMasterClient)
        {
            ball = PhotonNetwork.Instantiate("Ball.prefab", ballState.position, ballState.rotation);
            _ballRigid = ball.GetComponent<Rigidbody>();
            _ballRigid.velocity = ballState.velocity;
        }
    }

    private void CreateMap()
    {
        var floorPrefab = ResourceManager.Instance.GetCache<GameObject>("Floor.prefab");
        Instantiate(floorPrefab);
    }

    private void SetPhotonPrefabPool()
    {
        var pool = PhotonNetwork.PrefabPool as DefaultPool;

        // 1. �÷��̾� prefab ���
        var playerPrefab = ResourceManager.Instance.GetCache<GameObject>("Player.prefab");
        pool.ResourceCache.Add("Player.prefab", playerPrefab);

        // 2. Ball prefab ���
        var ballPrefab = ResourceManager.Instance.GetCache<GameObject>("Ball.prefab");
        pool.ResourceCache.Add("Ball.prefab", ballPrefab);

        //// 3. Model fbx ���
        //var modelFbx = ResourceManager.Instance.GetCache<GameObject>("XBot.fbx");
        //pool.ResourceCache.Add("XBot.fbx", modelFbx);
    }

    public void PlayerSpawn()
    {
        // PhotonNetwork.PrefabPool�� �̿��� ����
        PhotonNetwork.Instantiate("Player.prefab", Vector3.zero, Quaternion.identity);
    }
}