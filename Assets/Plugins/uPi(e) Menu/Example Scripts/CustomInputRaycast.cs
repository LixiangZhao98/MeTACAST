using UnityEngine;
using System.Collections;
using uPIe;

public class CustomInputRaycast : MonoBehaviour
{
    [Header("Most likely you want to link the camera here")]
    [Tooltip("Rays are cast from the referenced transform in forward direction\n" + 
             "In VR applications it's best to use a child transform centered" +
             "between the two cameras.")]
    public Transform RaycastSource;
    public Camera ScreenTranslationCam;
    private uPIeMenu menu;
    private RectTransform menuTransformCache;
    private Plane menuPlane;
    private Vector3 hitpoint;
    private void Awake()
    {
        menu = GetComponent<uPIeMenu>();
    }

    private void Start()
    {
        // use directional as default. Pointer (mouse) does pretty much the same
        // but using a pointing device behaviour we have no deadzone
        menu.ControlWithGamepad = true;
        menu.UseCustomInputSystem = true;
        menuTransformCache = menu.GetComponent<RectTransform>();

        // put up a plane to cast rays to
        menuPlane = new Plane(menuTransformCache.forward, menuTransformCache.position);
    }

    private void Update()
    {
        var ray = new Ray(RaycastSource.position, RaycastSource.forward);
        var hitDistance = 0f;
        if (menuPlane.Raycast(ray, out hitDistance))
        {
            hitpoint = ray.GetPoint(hitDistance);

            // directional input
            if (menu.ControlWithGamepad)
            {
                var dist = hitpoint - menuTransformCache.position;
                menu.CustomInput = new Vector2(dist.x, dist.y);
            }
            else // direct look input
            {
                var screenPoint = ScreenTranslationCam.WorldToScreenPoint(hitpoint);
                menu.CustomInput = new Vector2(screenPoint.x, screenPoint.y);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(hitpoint, 10f);
    }
}