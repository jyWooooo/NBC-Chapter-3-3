using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneUI : UI_Scene
{
    #region Bind Enums...
    enum Objects
    {
        objHelpUI,
    }

    enum Texts
    {

    }

    enum Images
    {

    }

    enum Buttons
    {
        btnStart,
        btnHelp,
    }
    #endregion

    private Button _btnStart;
    private Button _btnHelp;
    private GameObject _objHelpPopUp;

    public override bool Initialize()
    {
        if (!base.Initialize())
            return false;

        // Binding..
        BindContent();
        BindEvents();

        _objHelpPopUp.SetActive(false);

        return true;
    }

    private void BindContent()
    {
        BindObject(typeof(Objects));
        _objHelpPopUp = GetObject((int)Objects.objHelpUI);

        BindButton(typeof(Buttons));
        _btnStart = GetButton((int)Buttons.btnStart);
        _btnHelp = GetButton((int)Buttons.btnHelp);
    }

    private void BindEvents()
    {
        _btnStart.gameObject.BindEvent<PointerEventData>(OnStartButtonClick, EventTriggerType.PointerClick, PointerEventData.InputButton.Left);
        _btnHelp.gameObject.BindEvent<PointerEventData>(OnHelpButtonEnter, EventTriggerType.PointerEnter, null);
        _btnHelp.gameObject.BindEvent<PointerEventData>(OnHelpButtonExit, EventTriggerType.PointerExit, null);
    }

    private void OnHelpButtonEnter(PointerEventData data)
    {
        _objHelpPopUp.SetActive(true);
    }

    private void OnHelpButtonExit(PointerEventData data)
    {
        _objHelpPopUp.SetActive(false);
    }

    private void OnStartButtonClick(PointerEventData data)
    {
        SceneManager.LoadScene("MainScene");
    }
}