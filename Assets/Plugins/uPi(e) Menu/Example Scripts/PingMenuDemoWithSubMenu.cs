using UnityEngine;
using uPIe;

public class PingMenuDemoWithSubMenu : MonoBehaviour
{
    [SerializeField]
    private KeyCode triggerKey = KeyCode.LeftAlt;

    [SerializeField]
    private uPIeMenu relatedMenu;

    [SerializeField]
    private uPIeMenu subMenu;

    public KeyCode TriggerKey
    {
        get { return triggerKey; }
        set { triggerKey = value; }
    }

    public uPIeMenu RelatedMenu
    {
        get { return relatedMenu; }
        set { relatedMenu = value; }
    }

    public uPIeMenu SubMenu
    {
        get { return subMenu; }
        set { subMenu = value; }
    }

    private void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (!relatedMenu.ControlWithGamepad)
            {
                relatedMenu.transform.position = Input.mousePosition;
            }
            else
            {
                relatedMenu.transform.localPosition = Vector3.zero;
            }

            relatedMenu.gameObject.SetActive(true);
        }
        else if (Input.GetKeyUp(triggerKey))
        {
            relatedMenu.ConfirmCurrentSelection();
            subMenu.ReturnToSuperMenu(relatedMenu);
            relatedMenu.gameObject.SetActive(false);
        }
    }

    public void LogMessage(string msg)
    {
        Debug.Log("Ping Menu Demo: " + msg);
    }
}