public class UI_PopUp : UI_Base
{
    public override bool Initialize()
    {
        if (!base.Initialize())
            return false;

        UIManager.Instance.SetCanvas(gameObject, null);
        return true;
    }

    public virtual void ClosePopUpUI()
    {
        UIManager.Instance.ClosePopUp(this);
    }
}