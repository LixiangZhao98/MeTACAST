// using System.Collections.Generic;
// using UnityEngine;
// using HTC.UnityPlugin.Vive;
//
// [RequireComponent(typeof(LineRenderer))]
// public class ControllerDraw : MonoBehaviour
// {
//    
//     public Transform marker;
//     private Transform pointCloudData;
//     LineRenderer line;
//     bool m_init = false;
//     private List<Vector3> pointsList;
//     private List<float> drawTime; //start from the draw beginning
//     private float startTime;
//     public float pointDistance = 0.005f;
//
//  
//     // void Start()
//     // {
//     //     line = this.GetComponent<LineRenderer>();
//     //     pointsList = new List<Vector3>();
//     //     drawTime = new List<float>();
//     //     pointCloudData=this.transform.parent;
//     // }
//
//
//     public void Init()
//     {
//        
//         line = this.GetComponent<LineRenderer>();
//         pointsList = new List<Vector3>();
//         drawTime = new List<float>();
//         pointCloudData=this.transform.parent;
//         
//         if (m_init) return;
//         m_init = true;
//         line.positionCount = 0;
//         pointsList.Clear();
//         drawTime.Clear();
//         startTime = Time.time;
//        
//     }
//
//   
//     void Update()
//     {
//         if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
//         {
//             Init();
//         }
//
//         if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
//         {
//            
//             Vector3 v = pointCloudData.transform.InverseTransformPoint(marker.position);
//             
//             if (pointsList.Count != 0)
//             {
//                 if (Vector3.Magnitude(v - pointsList[pointsList.Count - 1]) > pointDistance)
//                 {
//                     pointsList.Add(v);
//                     drawTime.Add(Time.time - startTime);
//                 }
//             }
//             else
//             {
//                 pointsList.Add(v );
//                 drawTime.Add(Time.time - startTime);
//             }
//
//             line.positionCount = pointsList.Count;
//
//             for (int i = 0; i < pointsList.Count; i++)
//             {
//                 line.SetPosition(i, pointsList[i] );
//
//             }
//         }
//
//         if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger)&&MenuController.currentMode==MenuController.Mode.Default)
//         {
//             
//             m_init = false;
//         }
//     }
//
//     
//
//     public List<Vector3> GetPointList()
//     {
//
//         return pointsList;
//     }
//
//     public void Initiate()
//     {
//         m_init = false;
//         Init();
//     }
// }
//
//
//
//
//


using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

[RequireComponent(typeof(LineRenderer))]
public class ControllerDraw : MonoBehaviour
{
    [Header("References")]
    public Transform marker;  // Marker transform that will draw the line
    private Transform pointCloudData;  // Parent transform for coordinate space
    private LineRenderer line;  // Line Renderer component
    
    [Header("Drawing Settings")]
    [Tooltip("Minimum distance between consecutive points")]
    public float pointDistance = 0.005f;
    
    // Drawing data
    private List<Vector3> pointsList = new List<Vector3>();
    private List<float> drawTime = new List<float>();  // Timestamps from drawing start
    private float startTime;
    private bool isDrawing = false;

    private void Awake()
    {
        // Initialize components
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
        pointCloudData = transform.parent;
    }

    void Update()
    {
        HandleDrawingInput();
    }

    /// <summary>
    /// Handles all drawing input logic
    /// </summary>
    private void HandleDrawingInput()
    {
        // Only respond in default mode
        if (MenuController.currentMode != MenuController.Mode.Default) return;

        // Trigger pressed - start drawing
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
        {
            StartDrawing();
        }

        // Trigger held - continue drawing
        if (isDrawing && ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger))
        {
            ContinueDrawing();
        }

        // Trigger released - stop drawing
        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger))
        {
            StopDrawing();
        }
    }

    /// <summary>
    /// Initializes a new drawing session
    /// </summary>
    private void StartDrawing()
    {
        pointsList.Clear();
        drawTime.Clear();
        line.positionCount = 0;
        startTime = Time.time;
        isDrawing = true;
        
        // Add first point
        AddPoint(marker.position);
    }

    /// <summary>
    /// Adds new points while drawing
    /// </summary>
    private void ContinueDrawing()
    {
        Vector3 currentPos = marker.position;
        Vector3 lastPos = pointsList.Count > 0 ? pointsList[pointsList.Count - 1] : currentPos;

        // Only add new point if moved sufficiently
        if (Vector3.Magnitude(currentPos - pointCloudData.transform.TransformPoint(lastPos)) >  pointDistance)
        {
            AddPoint(currentPos);
        }
    }

    /// <summary>
    /// Cleans up after drawing ends
    /// </summary>
    private void StopDrawing()
    {
        isDrawing = false;
    }

    /// <summary>
    /// Adds a new point to the line
    /// </summary>
    /// <param name="position">World space position to add</param>
    private void AddPoint(Vector3 position)
    {
        pointsList.Add(position);
        // Convert to local space
        Vector3 localPos = pointCloudData.transform.InverseTransformPoint(position);
        
        drawTime.Add(Time.time - startTime);
        
        // Update line renderer
        line.positionCount = pointsList.Count;
        line.SetPosition(pointsList.Count - 1, localPos);
    }

    /// <summary>
    /// Gets the list of drawn points
    /// </summary>
    /// <returns>Copy of the points list</returns>
    public List<Vector3> GetPointList()
    {
        return new List<Vector3>(pointsList);  // Return copy to prevent external modification
    }

    /// <summary>
    /// Resets the drawing state
    /// </summary>
    public void ResetDrawing()
    {
        isDrawing = false;
        pointsList.Clear();
        drawTime.Clear();
        line.positionCount = 0;
    }
}