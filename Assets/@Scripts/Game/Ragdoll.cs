using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] _rigids;
    [SerializeField] private float _iifeTime = 10f;

    private void Awake()
    {
        _rigids = GetComponentsInChildren<Rigidbody>();
    }

    private void Start()
    {
        StartCoroutine(CoLifeTimeLapse());
    }

    private IEnumerator CoLifeTimeLapse()
    {
        yield return new WaitForSeconds(_iifeTime);
        Destroy(gameObject);
    }

    public void RandomAddForce(GameManager.BallStateStruct ballState)
    {
        foreach (var rigidbody in _rigids)
            rigidbody.velocity = ballState.velocity;
    }
}