using Photon.Pun;
using UnityEngine;

public class Ball : MonoBehaviourPun
{
    private Rigidbody _rigid;
    private Vector3 _velocity;
    [SerializeField] private float _basePower;
    private GameObject _kicker;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _velocity = _rigid.velocity;
    }

    [PunRPC]
    public void Shoot(Vector3 dir, float power)
    {
        if (photonView.IsMine)
            _rigid.velocity = _basePower * power * dir;
    }

    [PunRPC]
    public void Catch(Vector3 pos)
    {
        if (photonView.IsMine)
        {
            transform.position = pos + Vector3.up * transform.localScale.x / 2;
            _rigid.velocity = Vector3.zero;
            _rigid.angularVelocity = Vector3.zero;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            if (collision.contacts.Length > 0)
            {
                Debug.Log(collision.contacts.Length + " " + collision.contacts[0].otherCollider.name);
                var normal = collision.contacts[0].normal;
                var reflect = Vector3.Reflect(_velocity.normalized, normal);
                reflect.y = 0f;
                if (_rigid == null)
                    return;
                _rigid.velocity = _velocity.magnitude * reflect.normalized;
                _rigid.angularVelocity = Vector3.zero;
                _kicker = null;
            }
        }
    }
}