using UnityEngine;
using uPIe;

[RequireComponent(typeof(uPIeMenu))]
public class CustomInputGamepad : MonoBehaviour
{
    private uPIeMenu menu;

    private void Awake()
    {
        menu = GetComponent<uPIeMenu>();
    }

    private void Start()
    {
        menu.ControlWithGamepad = true;
        menu.UseCustomInputSystem = true;
    }

    private void Update()
    {
        menu.CustomInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}