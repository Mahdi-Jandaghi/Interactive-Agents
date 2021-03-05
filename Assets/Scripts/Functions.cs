using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functoins : MonoBehaviour
{
    public static List<List<Vector3>> Dis(List<List<Vector3>> incomigpoints) {
        List<List<Vector3>> pList = new List<List<Vector3>>();

        for (int i = 0; i < incomigpoints.Count; i++) {

            //List to store points
            List<Vector3> innerPlist = new List<Vector3>();

            //adding the first point
            innerPlist.Add(incomigpoints[i][0]);

            //adding the next points in case (checking if 3 consecutive points fall in the same line logic)
            for (int j = 0; j < incomigpoints[i].Count - 2; j++) {
                Vector3 a = incomigpoints[i][j + 1] - incomigpoints[i][j];
                Vector3 b = incomigpoints[i][j + 2] - incomigpoints[i][j + 1];

                if (Vector3.Cross(a, b).magnitude != 0) {
                    innerPlist.Add(incomigpoints[i][j + 1]);
                }

            }

            //adding the last point
            innerPlist.Add(incomigpoints[i][incomigpoints[i].Count - 1]);
            //adding the pointlists to list
            pList.Add(innerPlist);

        }

        return pList;
    }
    //public static List<List<Vector3>> GetVertices(List<GameObject> incomingObj,)
    public static List<List<Vector2>> Partition(int HorizontalDivision, int VerticalDivision) {
        List<List<Vector2>> UvList = new List<List<Vector2>>();

        float HI = (float)1 / HorizontalDivision; // Horizontal Interval
        float VI = (float)1 / VerticalDivision;    // Vertical Interval

        List<float> HList = new List<float>();    //Adding Horizontal Divisions intoList
        List<float> VList = new List<float>();    //Adding Vertical Divisions intoList

        for (int i = 0; i <= HorizontalDivision; i++) {
            float x = i * HI;
            HList.Add(x);
        }
        for (int j = 0; j <= VerticalDivision; j++) {
            float y = j * VI;
            VList.Add(y);
        }


        for (int k = 0; k < VerticalDivision; k++) {
            for (int L = 0; L < HorizontalDivision; L++) {
                Vector2 point1 = new Vector2(HList[L], VList[k + 1]);
                Vector2 point2 = new Vector2(HList[L + 1], VList[k + 1]);
                Vector2 point3 = new Vector2(HList[L + 1], VList[k]);
                Vector2 point4 = new Vector2(HList[L], VList[k]);

                List<Vector2> Uv = new List<Vector2>();
                Uv.Add(point1);
                Uv.Add(point2);
                Uv.Add(point3);
                Uv.Add(point4);

                UvList.Add(Uv);
            }
        }
        return UvList;
    }
    public static List<Vector3> Sort(List<Vector3> inList) {
        List<Vector3> FinalList = new List<Vector3>();
        FinalList.Add(new Vector3(0, 0, 0));
        FinalList.Add(new Vector3(0, 0, 0));
        FinalList.Add(new Vector3(0, 0, 0));
        FinalList.Add(new Vector3(0, 0, 0));

        List<int> FirstScoreList = new List<int>() { 0, 0, 0, 0 };
        List<float> SecondScoreList = new List<float>() { 0, 0, 0, 0 };

        //finding plane of mesh
        Vector3 cross = Vector3.Cross(inList[0] - inList[1], inList[0] - inList[2]).normalized;

        if (Mathf.Abs(cross[0]) == 1) {
            Debug.Log("pass 0");
            //Y Test
            for (int i = 0; i < 4; i++) {
                if (System.Math.Round(inList[i][1], 1) > System.Math.Round(inList[0][1], 1)) {
                    FirstScoreList[i] += 1;
                }
                if (System.Math.Round(inList[i][1], 1) > System.Math.Round(inList[1][1], 1)) {
                    FirstScoreList[i] += 1;
                }
                if (System.Math.Round(inList[i][1], 1) > System.Math.Round(inList[2][1], 1)) {
                    FirstScoreList[i] += 1;
                }
                if (System.Math.Round(inList[i][1], 1) > System.Math.Round(inList[3][1], 1)) {
                    FirstScoreList[i] += 1;
                }

            }

            //Z Test
            for (int i = 0; i < 4; i++) {
                if (System.Math.Round(inList[i][2], 1) > System.Math.Round(inList[0][2], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
                if (System.Math.Round(inList[i][2], 1) > System.Math.Round(inList[1][2], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
                if (System.Math.Round(inList[i][2], 1) > System.Math.Round(inList[2][2], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
                if (System.Math.Round(inList[i][2], 1) > System.Math.Round(inList[3][2], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
            }

        }
        if (Mathf.Abs(cross[1]) == 1) {
            Debug.Log("pass 1");
            //X
            for (int i = 0; i < 4; i++) {
                if (System.Math.Round(inList[i][0], 1) > System.Math.Round(inList[0][0], 1)) {
                    FirstScoreList[i] += 1;
                }
                if (System.Math.Round(inList[i][0], 1) > System.Math.Round(inList[1][0], 1)) {
                    FirstScoreList[i] += 1;
                }
                if (System.Math.Round(inList[i][0], 1) > System.Math.Round(inList[2][0], 1)) {
                    FirstScoreList[i] += 1;
                }
                if (System.Math.Round(inList[i][0], 1) > System.Math.Round(inList[3][0], 1)) {
                    FirstScoreList[i] += 1;
                }

            }
            //Z Test
            for (int i = 0; i < 4; i++) {
                if (System.Math.Round(inList[i][2], 1) > System.Math.Round(inList[0][2], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
                if (System.Math.Round(inList[i][2], 1) > System.Math.Round(inList[1][2], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
                if (System.Math.Round(inList[i][2], 1) > System.Math.Round(inList[2][2], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
                if (System.Math.Round(inList[i][2], 1) > System.Math.Round(inList[3][2], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
            }

        }
        if (Mathf.Abs(cross[2]) == 1) {
            Debug.Log("pass 2");
            //Y Test
            for (int i = 0; i < 4; i++) {
                if (System.Math.Round(inList[i][1], 1) > System.Math.Round(inList[0][1], 1)) {
                    FirstScoreList[i] += 1;
                }
                if (System.Math.Round(inList[i][1], 1) > System.Math.Round(inList[1][1], 1)) {
                    FirstScoreList[i] += 1;
                }
                if (System.Math.Round(inList[i][1], 1) > System.Math.Round(inList[2][1], 1)) {
                    FirstScoreList[i] += 1;
                }
                if (System.Math.Round(inList[i][1], 1) > System.Math.Round(inList[3][1], 1)) {
                    FirstScoreList[i] += 1;
                }

            }

            //X Test
            for (int i = 0; i < 4; i++) {
                if (System.Math.Round(inList[i][0], 1) > System.Math.Round(inList[0][0], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
                if (System.Math.Round(inList[i][0], 1) > System.Math.Round(inList[1][0], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
                if (System.Math.Round(inList[i][0], 1) > System.Math.Round(inList[2][0], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
                if (System.Math.Round(inList[i][0], 1) > System.Math.Round(inList[3][0], 1)) {
                    SecondScoreList[i] += 0.5f;
                }
            }

        }


        List<float> Aggregate = new List<float>() {  FirstScoreList[0] + SecondScoreList[0],
                                                     FirstScoreList[1] + SecondScoreList[1],
                                                     FirstScoreList[2] + SecondScoreList[2],
                                                     FirstScoreList[3] + SecondScoreList[3]
                                            };
        //sorting
        Debug.Log(Aggregate[0]);
        Debug.Log(Aggregate[1]);
        Debug.Log(Aggregate[2]);
        Debug.Log(Aggregate[3]);


        Debug.Log(FirstScoreList[0]);
        Debug.Log(FirstScoreList[1]);
        Debug.Log(FirstScoreList[2]);
        Debug.Log(FirstScoreList[3]);

        Debug.Log(SecondScoreList[0]);
        Debug.Log(SecondScoreList[1]);
        Debug.Log(SecondScoreList[2]);
        Debug.Log(SecondScoreList[3]);

        for (int i = 0; i < 4; i++) {
            FinalList[(int)Aggregate[i]] = inList[i];
        }
        return FinalList;
    }
}
