using Photon.Pun;
using UnityEngine.EventSystems;

public class RespawnPopUpUI : UI_PopUp
{
    enum Buttons
    {
        btnRespawn,
    }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.btnRespawn).gameObject.BindEvent<PointerEventData>(Respawn);

        return true;
    }

    private void Respawn(PointerEventData data)
    {
        GameManager.Instance.MyPlayer.GetComponent<Player>().photonView.RPC("Respawn", RpcTarget.All);
        UIManager.Instance.ClosePopUp(this);
    }
}