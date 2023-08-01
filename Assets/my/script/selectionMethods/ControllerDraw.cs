using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

[RequireComponent(typeof(LineRenderer))]
public class ControllerDraw : MonoBehaviour
{
   
    public Transform drawPos;
    public GameObject PointCloudMeshinHand;
    LineRenderer line;
    bool m_init = false;
    private List<Vector3> pointsList;
    private List<float> drawTime; //start from the draw beginning
    private float startTime;
    public float PointDistance = 0.005f;
    public float lineWidth;

 
    void Start()
    {
        lineWidth = 0.005f;
        line = this.GetComponent<LineRenderer>();
        
        pointsList = new List<Vector3>();
        drawTime = new List<float>();
     
    }


    private void _Init()
    {
       
        if (m_init) return;
        m_init = true;
        line.positionCount = 0;
        line.SetWidth(lineWidth, lineWidth);
        pointsList.Clear();
        drawTime.Clear();
        startTime = Time.time;
       
    }

  
    void Update()
    {
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
        {
            _Init();
        }

        if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Trigger))
        {
           
            Vector3 v = PointCloudMeshinHand.transform.InverseTransformPoint(drawPos.position);
            if (pointsList.Count != 0)
            {
                if (Vector3.Magnitude(v - pointsList[pointsList.Count - 1]) > PointDistance/PointCloudMeshinHand.transform.localScale.x )
                {
                    pointsList.Add(v);
                    drawTime.Add(Time.time - startTime);
                }
            }
            else
            {
                pointsList.Add(v );
                drawTime.Add(Time.time - startTime);
            }

            line.positionCount = pointsList.Count;

            for (int i = 0; i < pointsList.Count; i++)
            {
                line.SetPosition(i, pointsList[i] );

            }
        }

        if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.Trigger))
        {
            
            m_init = false;
        }
    }

    

    public List<Vector3> GetPointList()
    {

        return pointsList;
    }

    public void Initiate()
    {
        m_init = false;
        _Init();
    }
}





