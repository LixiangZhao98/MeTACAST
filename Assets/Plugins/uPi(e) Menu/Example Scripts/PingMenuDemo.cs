using UnityEngine;
using uPIe;
using System.Collections;

public class PingMenuDemo : MonoBehaviour
{
    [SerializeField]
    private string triggerButtonName = "TogglePingMenu";

    [SerializeField]
    private uPIeMenu relatedMenu;

    public string TriggerButtonName
    {
        get { return triggerButtonName; }
        set { triggerButtonName = value; }
    }

    public uPIeMenu RelatedMenu
    {
        get { return relatedMenu; }
        set { relatedMenu = value; }
    }

    private void Update()
    {
        if (Input.GetButtonDown(triggerButtonName))
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
        else if (Input.GetButtonUp(triggerButtonName))
        {
            relatedMenu.ConfirmCurrentSelection();
            relatedMenu.gameObject.SetActive(false);
        }
    }

    public void LogMessage(string msg)
    {
        Debug.Log("Ping Menu Demo: " + msg);
    }
}