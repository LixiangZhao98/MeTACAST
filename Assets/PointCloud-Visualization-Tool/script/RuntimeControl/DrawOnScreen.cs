
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class DrawOnScreen : MonoBehaviour
    {
        public LineRenderer lineRenderer;
        private Vector3 mousePosition;
        private bool isDrawing = false; 
        public float zValue = -0.001f;
        public float distThre = 0.003f;
        public float width = 0.001f;
    
        public void Init(LineRenderer lr)
        {
            lineRenderer = lr;
            lineRenderer.positionCount = 0;
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
            isDrawing = false;
        }
        void Start()
        {
            Init(lineRenderer);
        }
    
        void Update()
        {
            Draw();
        }
        private Vector3 oldMousePosition;
        private void Draw()
        {
    
    
            if (Input.GetMouseButton(0))
    
            {
    
                if (!isDrawing)
                {
                    lineRenderer.positionCount = 0;
                    isDrawing = true;
                }
                mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, Camera.main.nearClipPlane + zValue));
                Vector3 localMousePosition = lineRenderer.gameObject.transform.InverseTransformPoint(mousePosition);
                if ((oldMousePosition - mousePosition).magnitude > distThre)
                {
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, localMousePosition);
                    oldMousePosition = mousePosition;
                }
            }
    
            else
            {
                if (isDrawing)
                {
                    //lineRenderer.positionCount = 0;
                    isDrawing = false;
                }
               
            }
    
    
        }
    }

