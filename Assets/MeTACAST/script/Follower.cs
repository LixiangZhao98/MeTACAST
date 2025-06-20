// using UnityEngine;
// using UnityEngine.Serialization;
//
// public class Followers : MonoBehaviour
// {
// public GameObject target;
// public Vector3 posOffset;
// public Vector3 rotOffset;
// public void Update()
// {
//     this.transform.position=target.transform.position+posOffset;
//     this.transform.eulerAngles=target.transform.eulerAngles+rotOffset;
// }
// }
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject target;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    
    [Tooltip("smooth following?")]
    public bool smoothFollow = true;
    
    [Tooltip("smooth speed")]
    public float smoothSpeed = 5f;
    
    private void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned!");
            return;
        }
        
        // 计算目标位置和旋转
        Vector3 desiredPosition = target.transform.TransformPoint(positionOffset);
        Quaternion desiredRotation = target.transform.rotation * Quaternion.Euler(rotationOffset);
        
        // 更新位置
        if (smoothFollow)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, smoothSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = desiredPosition;
            transform.rotation = desiredRotation;
        }
    }
}