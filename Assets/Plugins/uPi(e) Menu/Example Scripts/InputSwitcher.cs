using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using uPIe;

public class InputSwitcher : MonoBehaviour
{
    public string MouseDescriptionText = "mouse";
    public string GamepadDescriptionText = "gamepad";
    public Canvas CanvasForGroup;
    private static bool useGamepad = false;

    [SerializeField]
    private List<uPIeMenu> menus = new List<uPIeMenu>();

    [SerializeField]
    private Text buttonText;

    [SerializeField]
    private KeyCode switchKey = KeyCode.Space;

    public List<uPIeMenu> Menu
    {
        get { return menus; }
        set { menus = value; }
    }

    public Text ButtonText
    {
        get { return buttonText; }
        set { buttonText = value; }
    }

    public KeyCode SwitchKey
    {
        get { return switchKey; }
        set { switchKey = value; }
    }

    private void Start()
    {
        foreach (var menu in menus)
        {
            menu.ControlWithGamepad = useGamepad;
        }

        SetButtonText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(SwitchKey))
        {
            SwitchInputDevice();
        }
    }

    public void SwitchInputDevice()
    {
        useGamepad = !useGamepad;
        foreach (var menu in menus)
        {
            menu.ControlWithGamepad = useGamepad;
            menu.Deselect();
        }

        SetButtonText();
        if (CanvasForGroup == null) return;

        var group = CanvasForGroup.GetComponent<CanvasGroup>();
        if (useGamepad)
        {
            if (group == null)
            {
                group = CanvasForGroup.gameObject.AddComponent<CanvasGroup>();
            }

            group.interactable = true;
            group.blocksRaycasts = false;
            group.ignoreParentGroups = false;
        }
        else
        {
            if (group != null)
            {
                Destroy(group);
            }
        }
    }

    private void SetButtonText()
    {
        buttonText.text = "Press " + switchKey.ToString() + " to " + (useGamepad ? "switch to " + MouseDescriptionText : "switch to " + GamepadDescriptionText);
    }
}