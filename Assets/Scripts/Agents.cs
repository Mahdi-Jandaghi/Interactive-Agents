using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agents : MonoBehaviour
{
    public LineRender lr;
    public List<LineRender> lineList;
    private List<LineRender> inActivelist;

    public List<GameObject> rendelineList;
    private List<GameObject> inActiverendelineList;

    public List<GameObject> CubeAgent;                    //list to store cubes at initial starting point
    private List<GameObject> inActiveCubeAgent;           //list to store inActiveCubeAgent

    private List<Vector3> library;                        //discrete vectors library

    public List<List<Vector3>> Points;
    private List<List<Vector3>> inActivePoints;
    
    public List<List<int>> PointLenghtRecorder;
    private List<List<int>> inActivePointLenghtRecorder;

    //variables for live mesh
    public List<List<Vector3>> last3points;
    public List<List<GameObject>> liveMesh;

    public GameObject empty;
    private GameObject line;
    public struct Mystruct { public List<GameObject> lrf; public List<List<Vector3>> Pointsf; };

    public Agents(GameObject incomingline) {        
        line = incomingline;

        CubeAgent = new List<GameObject>();         
        inActiveCubeAgent = new List<GameObject>();
        
        Points = new List<List<Vector3>>();
        inActivePoints = new List<List<Vector3>>();
        
        PointLenghtRecorder = new List<List<int>>();
        inActivePointLenghtRecorder = new List<List<int>>();

        lineList = new List<LineRender>();
        inActivelist = new List<LineRender>();
        

        last3points = new List<List<Vector3>>();
        liveMesh = new List<List<GameObject>>();


        library = new List<Vector3>() { };

        library.Add(new Vector3(1, 0, 0));
        library.Add(new Vector3(-1, 0, 0));
        library.Add(new Vector3(0, 1, 0));
        library.Add(new Vector3(0, -1, 0));
        library.Add(new Vector3(0, 0, 1));
        library.Add(new Vector3(0, 0, -1));
    }

    public Mystruct chase1(Vector3 Target, Camera incomingcamera) {

        //creating new AgentStream
        if (Input.GetMouseButtonDown(0)) {
            if (Input.GetKey(KeyCode.LeftControl) == false && Input.GetKey(KeyCode.LeftAlt) == false) {

                lineList.Add(new LineRender(line));              //creating a linerenderer class and adding it to the list

                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);                       //creating the cube
                cube.transform.position = Target;                                                       //setting its pos to mouseCursor location 
                cube.GetComponent<MeshRenderer>().material.color = new Color(0.7f, 0.1f, 0.1f);         //setting its color
                CubeAgent.Add(cube);                                                                    //adding it to the list

                
                Points.Add(new List<Vector3>() { Target });                                             //creating an agent stream and adding it to the list
                
                PointLenghtRecorder.Add(new List<int>() { 1 });                                         //creating its lenghtrecorder list

                
                last3points.Add(new List<Vector3>() { Target });                                        //creating last3point list

                liveMesh.Add(new List<GameObject>());                                                   //creating an empty ameobject for live mesh
            }
        }

        //moving Agents
        if (Input.GetMouseButton(1)) {
            for (int i = 0; i < Points.Count; i++) {                        //for every agent stream
                Vector3 dir = (Target - Points[i][Points[i].Count - 1]);    //direction vector to target
                float Mag = dir.magnitude;

                if (dir.magnitude < 1.01f)  continue;   //if the distance is less than the treshold, Stop


                //closest Vector
                List<float> values = new List<float>();        //list to store the distance between dir vector and discrete vectors

                for (int j = 0; j < library.Count; j++) {                   //for every discrete vector find the distance 
                    values.Add(Mathf.Abs((dir - library[j]).magnitude));
                }

                float min = Mathf.Min(values.ToArray());                    //find the minimum value                            
                dir = library[values.IndexOf(min)];                         //find the index of minimum value and get the discrete vector                                      

                Vector3 Newpos = Points[i][Points[i].Count - 1] + dir * Mag / 2;        //pos+vel  //mag/2
                Points[i].Add(Newpos);                                                  //Adding new Pos to points 

                lineList[i].CreateLine(Points[i]);                                      //creating the lines

                //recording last3point 
                if (last3points[i].Count < 2) {
                    last3points[i].Add(Newpos);
                    continue;
                }

                Vector3 Va = last3points[i][last3points[i].Count - 1] - last3points[i][last3points[i].Count - 2];
                Vector3 Vb = Newpos - last3points[i][last3points[i].Count - 1];

                //case 1 consecutive points falling in a line
                if (Vector3.Cross(Va, Vb).magnitude == 0) {
                    //replacing the last item
                    last3points[i][last3points[i].Count - 1] = Newpos;

                    //destroying the last mesh if it exist
                    if (liveMesh[i].Count > 0) {
                        Destroy(liveMesh[i][liveMesh[i].Count - 1]);
                        liveMesh[i].RemoveAt(liveMesh[i].Count - 1);
                    }

                    continue;
                }

                last3points[i].Add(Newpos);

                if (last3points[i].Count > 3) {
                    last3points[i].RemoveAt(0);
                }
            }
        }


        //recording the pointlenght
        for (int k = 0; k < Points.Count; k++) {

            PointLenghtRecorder[k].Add(Points[k].Count);

            //make sure lenght recorder always have 2 items
            if (PointLenghtRecorder[k].Count > 2) {
                PointLenghtRecorder[k].RemoveAt(0);
            }
        }

        //Removeing Agents
        if (Input.GetKey(KeyCode.LeftAlt)) {
            Ray ray = incomingcamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hitInfo;
            bool bHit = Physics.Raycast(ray, out hitInfo);

            if (bHit == true && Input.GetMouseButtonDown(0) == true) {
                int index = CubeAgent.IndexOf(hitInfo.collider.gameObject);

                //removing cube
                CubeAgent.Remove(hitInfo.collider.gameObject);
                Destroy(hitInfo.collider.gameObject);

                //removing points
                Points.RemoveAt(index);

                //remove lenghtrecorder
                PointLenghtRecorder.RemoveAt(index);

                //removing last3
                last3points.RemoveAt(index);


                //removing lines
                lineList.RemoveAt(index);
                Destroy(rendelineList[index]);
                rendelineList.RemoveAt(index);
            }
        }

        //removing all at once Activeones


        //removing inActiveones all at once 
        if (Input.GetKey(KeyCode.Delete)) {

            for (int ii = 0; ii < Points.Count; ii++) {

                Destroy(inActiveCubeAgent[ii]);
                Destroy(inActiverendelineList[ii]);

            }
            inActiveCubeAgent.Clear();
            inActivePoints.Clear();
            inActivePointLenghtRecorder.Clear();
            inActivelist.Clear();
            inActiverendelineList.Clear();
        }




        //Deactivating Agents
        int switchkey = 0;
        if (Input.GetKey(KeyCode.LeftControl)) {
            Ray ray = incomingcamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hitInfo1;
            bool bHit = Physics.Raycast(ray, out hitInfo1);

            if (bHit == true && Input.GetMouseButtonDown(0) == true) {

                if (inActiveCubeAgent.Contains(hitInfo1.collider.gameObject)) {
                    switchkey = 1;
                }
                switch (switchkey) {
                    case 0:

                        int index1 = CubeAgent.IndexOf(hitInfo1.collider.gameObject);
                        //moving cube to deactivate list
                        CubeAgent.Remove(hitInfo1.collider.gameObject);
                        inActiveCubeAgent.Add(hitInfo1.collider.gameObject);
                        hitInfo1.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.grey);

                        //moving points to deavtivate list
                        inActivePoints.Add(Points[index1]);
                        Points.RemoveAt(index1);
                        //Moving lenghtrecorder to deactivelist
                        inActivePointLenghtRecorder.Add(PointLenghtRecorder[index1]);
                        PointLenghtRecorder.RemoveAt(index1);

                        //moving lines to deactivate list
                        inActivelist.Add(lineList[index1]);
                        lineList.RemoveAt(index1);
                        inActiverendelineList.Add(rendelineList[index1]);
                        rendelineList.RemoveAt(index1);
                        break;

                    case 1:

                        int index2 = inActiveCubeAgent.IndexOf(hitInfo1.collider.gameObject);
                        //moving cube to active list

                        inActiveCubeAgent.Remove(hitInfo1.collider.gameObject);
                        CubeAgent.Add(hitInfo1.collider.gameObject);
                        hitInfo1.collider.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);

                        //moving points to active list
                        Points.Add(inActivePoints[index2]);
                        inActivePoints.RemoveAt(index2);
                        //moving lenghtrecorders into activeList
                        PointLenghtRecorder.Add(inActivePointLenghtRecorder[index2]);
                        inActivePointLenghtRecorder.RemoveAt(index2);


                        //moving lines to active list
                        lineList.Add(inActivelist[index2]);
                        inActivelist.RemoveAt(index2);
                        rendelineList.Add(inActiverendelineList[index2]);
                        inActiverendelineList.RemoveAt(index2);
                        break;
                }
            }
        }
        List<List<Vector3>> c = new List<List<Vector3>>();

        //mergeing both active and inactive list for mesh creation.
        c.AddRange(Points);
        c.AddRange(inActivePoints);

        //Returning the struct
        Mystruct pi = new Mystruct();
        pi.lrf = rendelineList;
        pi.Pointsf = c;
        return pi;
    }

    public List<List<GameObject>> LiveMesh(Material inmeshMaterial) {
        for (int i = 0; i < PointLenghtRecorder.Count; i++) {

            // if any list has less than 3 points continue
            if (last3points[i].Count < 3) {
                continue;
            }

            //if no point added to list continue
            if (PointLenghtRecorder[i][PointLenghtRecorder[i].Count - 1] == PointLenghtRecorder[i][PointLenghtRecorder[i].Count - 2]) {
                continue;
            }

            GameObject empty = new GameObject();
            List<Vector3> V = new List<Vector3>();


            //List<Vector2> UVs = new List<Vector2>();    //list for uvs
            //List<Vector3> normals = new List<Vector3>();

            //adding v4
            Vector3 vectorV4 = last3points[i][2] - last3points[i][1];
            Vector3 v4 = last3points[i][0] + vectorV4;

            V.Add(last3points[i][0]);
            V.Add(last3points[i][1]);
            V.Add(last3points[i][2]);
            V.Add(v4);

            Debug.Log(V[0]);
            Debug.Log(V[1]);
            Debug.Log(V[2]);
            Debug.Log(V[3]);
            //Sorting The itmes
            List<Vector3> Sorted = new List<Vector3>(Functoins.Sort(V));

            Debug.Log(Sorted[0]);
            Debug.Log(Sorted[1]);
            Debug.Log(Sorted[2]);
            Debug.Log(Sorted[3]);


            //getting the normal vector
            Vector3 normal = Vector3.Cross(Sorted[0] - Sorted[1], Sorted[0] - Sorted[2]);
            //copying the sorted list
            List<Vector3> Sorted2 = new List<Vector3>(Sorted);

            //if (normal.magnitude < 0.01)
            //{
            //continue;
            //}

            if (normal.magnitude > 5) {
                normal = normal.normalized * 5;
            }

            normal = normal.normalized * 0.1f;

            //if (normal.magnitude > 15)
            //{
            // continue;
            //}

            Debug.Log(normal.magnitude);
            //moving the copied list
            Sorted2[0] -= normal;
            Sorted2[1] -= normal;
            Sorted2[2] -= normal;
            Sorted2[3] -= normal;

            Sorted.AddRange(Sorted2);

            List<Vector3> Vertices = new List<Vector3>()
            {
                    //frontface
                    Sorted[0],
                    Sorted[1],
                    Sorted[2],
                    Sorted[3],
                    //BackFace
                    Sorted2[0],
                    Sorted2[1],
                    Sorted2[2],
                    Sorted2[3],
                    //SideFace1
                    Sorted2[0],
                    Sorted[0],
                    Sorted2[2],
                    Sorted[2],
                    //SideFace2
                    Sorted2[1],
                    Sorted[1],
                    Sorted2[3],
                    Sorted[3],
                    //top face
                    Sorted[2],
                    Sorted[3],
                    Sorted2[2],
                    Sorted2[3],
                    //bottom face
                    Sorted[0],
                    Sorted[1],
                    Sorted2[0],
                    Sorted2[1]
                };

            List<int> tr = new List<int>()
            {
                    //frontface
                    0,1,3,
                    0,3,2,
                    //backFace
                    4,7,5,
                    4,6,7,
                    //Side Face 1
                    8,9,11,
                    8,11,10,
                    //Side Face 2
                    12,15,13,
                    12,14,15,
                    //top face
                    16,17,19,
                    16,19,18,
                    //bottom face
                    20,23,21,
                    20,22,23,
                };             //list for triangles


            //tr.Add(0);
            //tr.Add(1);
            //tr.Add(2);

            //tr.Add(0);
            //tr.Add(2);
            //tr.Add(3);

            //UVs.Add(new Vector2(0, 1));
            //UVs.Add(new Vector2(0, 0));
            //UVs.Add(new Vector2(1, 0));
            //UVs.Add(new Vector2(1, 1));


            //normals.Add(normal*-1);
            //normals.Add(normal * -1);
            //normals.Add(normal * -1);
            //normals.Add(normal * -1);

            Mesh mesh1 = new Mesh();
            mesh1.vertices = Vertices.ToArray();
            //mesh1.normals = normals.ToArray();
            //mesh1.uv = UVs.ToArray();
            mesh1.triangles = tr.ToArray();


            mesh1.RecalculateNormals();
            mesh1.RecalculateTangents();
            mesh1.RecalculateBounds();

            empty.AddComponent<MeshFilter>();
            empty.AddComponent<MeshRenderer>();


            empty.GetComponent<MeshFilter>().mesh = mesh1;
            empty.GetComponent<MeshRenderer>().material = inmeshMaterial;
            empty.AddComponent<MeshCollider>();

            liveMesh[i].Add(Instantiate(empty));
            Destroy(empty);
        }

        return liveMesh;
    }

}

