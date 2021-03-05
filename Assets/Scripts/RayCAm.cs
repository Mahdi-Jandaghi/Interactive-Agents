using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCAm : MonoBehaviour
{
    
    public LineRender myLineRender;
    public GameObject cursorTip;
    private float s = 30f;
    public int a;

    public RayCAm(GameObject incomingLineSamle) 
        {
        myLineRender = new LineRender(incomingLineSamle);          
    }

    public GameObject ray2Center(Camera incomingcamera) {

        
        RaycastHit hitInfo;
        Ray ray = incomingcamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        
        bool bHit = Physics.Raycast(ray, out hitInfo);

        Vector3 start = ray.origin + new Vector3(0.0f, -0.5f, 0.6f);                //start point
        Vector3 target = ray.origin + ray.direction * s;                            //end point

        return myLineRender.CreateLine(new List<Vector3>() { start, target });


        
        //Ray ray2 = incomingcamera.ScreenPointToRay(Input.mousePosition);  //for whenever we want to shoot ray from camera to mouse pos 
        //Vector3 cameraPos = incomingcamera.transform.position;
        //Transform tr = incomingcamera.transform;
        //cameraPos = tr.TransformPoint(cameraPos);
        //if (bHit)
        //{
        //target = hitInfo.point;
        //}
    }


    public Vector3 ray1(Camera incomingcamera) {
        
        Ray ray = incomingcamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        if (cursorTip == null)  cursorTip = GameObject.CreatePrimitive(PrimitiveType.Sphere);            //create the cursor tip
        
        Destroy(cursorTip.GetComponent<SphereCollider>());
        
        //using scrollwheel to  move the cursortip
        float d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0) s += 0.7f;
        if (d < 0) s -= 0.7f;


        Vector3 target = ray.origin + ray.direction * s;

        if (Input.GetKey(KeyCode.X)) {                               //if x is pressed the target is hit point pos
            RaycastHit hitInfo;
            bool bHit = Physics.Raycast(ray, out hitInfo);
            if (bHit) {
                target = hitInfo.point;
            }

        }
        cursorTip.transform.position = target;
        return target;
    }
}
