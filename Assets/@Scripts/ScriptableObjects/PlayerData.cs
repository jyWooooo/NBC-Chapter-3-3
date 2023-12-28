using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Object/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [field: SerializeField] public float InvincibleTime { get; private set; }
    [field: SerializeField] public float InvincibleFlashInterval { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float MaxPower { get; private set; }
    [field: SerializeField] public float BallDetectRange { get; private set; }
    [field: SerializeField] public float KickCooltime { get; private set; }
}