using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{

    
    public Camera myCamera;
    public GameObject trailLine;
    public GameObject CursorLine;
    private RayCAm myraycam;
    private GameObject cameraRay;
    private Agents myAgents;
    private int switch1 = 0;

    public Material CanvasMaterial;
    public Material ImageMaterial;
    public Material VideoMaterial;

    void Start()
    {
        Cursor.visible = !Cursor.visible;
        myraycam = new RayCAm(CursorLine);
        myAgents = new Agents(trailLine);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))  Cursor.visible = !Cursor.visible;      //Mouse Cursor control

        myraycam.ray2Center(myCamera);                                           //updating camera ray
        Vector3 cursorPos = myraycam.ray1(myCamera);                             //creating cursor + returning its position



        if (Input.GetKeyDown(KeyCode.Alpha1)) { switch1 = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { switch1 = 2; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { switch1 = 3; }
        if (Input.GetKeyDown(KeyCode.Alpha0)) { switch1 = 4; }


        List<List<Vector3>> c = myAgents.chase1(cursorPos, myCamera).Pointsf;
        myAgents.LiveMesh(CanvasMaterial);

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

    }
}
