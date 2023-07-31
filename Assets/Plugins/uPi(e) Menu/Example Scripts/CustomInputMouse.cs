using UnityEngine;
using uPIe;

[RequireComponent(typeof(uPIeMenu))]
public class CustomInputMouse : MonoBehaviour
{
    private uPIeMenu menu;

    private void Awake()
    {
        menu = GetComponent<uPIeMenu>();
    }

    private void Start()
    {
        menu.ControlWithGamepad = false;
        menu.UseCustomInputSystem = true;
    }

    private void Update()
    {
        menu.CustomInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
}