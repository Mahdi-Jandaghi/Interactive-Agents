using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRender : MonoBehaviour {

    private GameObject LineSample;

    public LineRender(GameObject incomingLineSamle)                      //constructor
        {
        
        LineSample = Instantiate(incomingLineSamle);
    }

    public GameObject CreateLine(List<Vector3> incomingpList) {

        LineSample.GetComponent<LineRenderer>().positionCount = incomingpList.Count;
        LineSample.GetComponent<LineRenderer>().SetPositions(incomingpList.ToArray());
    
        return LineSample;
    }
}
