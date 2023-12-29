using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitHandler : MonoBehaviour
{
    private Player _parent;

    private void Awake()
    {
        _parent = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var pv = _parent.photonView;

        if (pv.IsMine)
        {
            var ball = other.GetComponentInParent<Ball>();
            if (ball != null)
            {
                if (!_parent.CheckKickerID(ball.KickerID))
                    pv.RPC("OnHit", RpcTarget.All);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var pv = _parent.photonView;

        if (pv.IsMine)
        {
            var ball = other.GetComponentInParent<Ball>();
            if (ball != null)
            {
                if (_parent.CheckKickerID(ball.KickerID))
                    ball.photonView.RPC("DeleteKickerID", RpcTarget.All);
            }
        }
    }
}