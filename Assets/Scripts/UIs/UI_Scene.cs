public class UI_Scene : UI_Base
{
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        //UIManager.Instance.SetCanvas(gameObject);

        return true;
    }
}