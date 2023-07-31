using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SimpleCameraLook : MonoBehaviour
{
    public Vector2 Sensitivity = new Vector2(40f, 40f);
    public Vector2 LookRotationClamp = new Vector2(40f, 40f);
    private Vector3 initialRotation;

    private void Start()
    {
        initialRotation = ClampRotation360(transform.rotation.eulerAngles);
    }

    private void Update()
    {
        var lookVector = new Vector2(Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Mouse X"),
                                     Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Mouse Y"));
        lookVector = Vector2.Scale(lookVector, Sensitivity) * Time.deltaTime;
        var rotationBefore = transform.rotation.eulerAngles;
        transform.Rotate(new Vector3(0f, lookVector.x, 0f), Space.World);
        transform.Rotate(new Vector3(-lookVector.y, 0f, 0f), Space.Self);

        var look = GetLookDelta();
        var clampedLook = transform.rotation.eulerAngles;
        if (look.x > LookRotationClamp.x)
        {
            clampedLook.x = rotationBefore.x;
        }

        if (look.y > LookRotationClamp.y)
        {
            clampedLook.y = rotationBefore.y;
        }

        transform.rotation = Quaternion.Euler(clampedLook);
    }

    private Vector3 GetLookDelta()
    {
        return GetDeltaRotation(ClampRotation360(transform.rotation.eulerAngles), initialRotation);
    }

    private Vector3 GetDeltaRotation(Vector3 source, Vector3 destination)
    {
        source.x = Mathf.Abs(Mathf.DeltaAngle(source.x, destination.x));
        source.y = Mathf.Abs(Mathf.DeltaAngle(source.y, destination.y));
        source.z = Mathf.Abs(Mathf.DeltaAngle(source.z, destination.z));
        return source;
    }

    private Vector3 ClampRotation360(Vector3 eulerRotation)
    {
        return new Vector3(ClampAngle360(eulerRotation.x),
                           ClampAngle360(eulerRotation.y),
                           ClampAngle360(eulerRotation.z));
    }

    private float ClampAngle360(float angle)
    {
        angle %= 360f;
        return angle % 360;
    }
}