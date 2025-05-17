using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineOutput : MonoBehaviour
{
    [SerializeField] private GameObject level1Spline;
    [SerializeField] private GameObject level2Spline;
    [SerializeField] private GameObject level3BigSpline;
    [SerializeField] private GameObject level3SmallSpline;
    
    private SplineContainer level1Splinecontainer;
    private SplineContainer level2Splinecontainer;
    private SplineContainer level3BigSplinecontainer;
    private SplineContainer level3SmallSplinecontainer;
    
    private SplineExtrude level1Extrude;
    private SplineExtrude level2Extrude;
    private SplineExtrude level3BigExtrude;
    private SplineExtrude level3SmallExtrude;

    private void Awake()
    {
        level1Splinecontainer = level1Spline.GetComponent<SplineContainer>();
        level2Splinecontainer = level2Spline.GetComponent<SplineContainer>();
        level3BigSplinecontainer = level3BigSpline.GetComponent<SplineContainer>();
        level3SmallSplinecontainer = level3SmallSpline.GetComponent<SplineContainer>();
        
        level1Extrude = level1Spline.GetComponent<SplineExtrude>();
        level2Extrude = level2Spline.GetComponent<SplineExtrude>();
        level3BigExtrude = level3BigSpline.GetComponent<SplineExtrude>();
        level3SmallExtrude = level3SmallSpline.GetComponent<SplineExtrude>();
    }
    private List<string> GetSplineKnotsDataAsString(Spline spline)
    {
        List<string> splineKnotsStringList = new List<string>();
        IEnumerable<BezierKnot> bezierKnots = spline.Knots;
        foreach (var k in bezierKnots)
        {
            string positionData = $"{k.Position.x},{k.Position.y},{k.Position.z}";
            Quaternion rotation = k.Rotation;
            Vector3 eularRotation = rotation.eulerAngles;
            string rotationData = $"{eularRotation.x},{eularRotation.y},{eularRotation.z}";
            string tangentInData = $"{k.TangentIn.x},{k.TangentIn.y},{k.TangentIn.z}";
            string tangentOutData = $"{k.TangentOut.x},{k.TangentOut.y},{k.TangentOut.z}";
            string row = string.Join(",", new string[] { positionData, rotationData, tangentInData, tangentOutData });
            //Debug.Log(row);
            splineKnotsStringList.Add(row);
        }
        return splineKnotsStringList;
    }

    private List<string> GetLevel3SplineKnotsDataAsString(SplineContainer bigSplineContainer, SplineContainer smallSplineContainer)
    {
        List<string> level3SplineKnotsStringList = new List<string>();
        for (int i = 0; i < 3; i++)
        {
            // Create individual entries for each knot in the spline
            List<string> knotDataList = GetSplineKnotsDataAsString(bigSplineContainer.Splines[i]);
            foreach (var knotData in knotDataList)
            {
                level3SplineKnotsStringList.Add($"3,{level3BigExtrude.Radius},{knotData}");
            }
        }
        
        // Small spline container - first spline
        List<string> smallKnotDataList = GetSplineKnotsDataAsString(smallSplineContainer.Splines[0]);
        foreach (var knotData in smallKnotDataList)
        {
            level3SplineKnotsStringList.Add($"3,{level3SmallExtrude.Radius},{knotData}");
        }
        
        // Remaining big splines (3-4)
        for (int i = 3; i < 5; i++)
        {
            List<string> knotDataList = GetSplineKnotsDataAsString(bigSplineContainer.Splines[i]);
            foreach (var knotData in knotDataList)
            {
                level3SplineKnotsStringList.Add($"3,{level3BigExtrude.Radius},{knotData}");
            }
        }
        
        // Small spline container - second spline
        smallKnotDataList = GetSplineKnotsDataAsString(smallSplineContainer.Splines[1]);
        foreach (var knotData in smallKnotDataList)
        {
            level3SplineKnotsStringList.Add($"3,{level3SmallExtrude.Radius},{knotData}");
        }
        
        // Last big spline
        List<string> lastKnotDataList = GetSplineKnotsDataAsString(bigSplineContainer.Splines[5]);
        foreach (var knotData in lastKnotDataList)
        {
            level3SplineKnotsStringList.Add($"3,{level3BigExtrude.Radius},{knotData}");
        }
        
        return level3SplineKnotsStringList;
    }

    public List<string> Getlevel1Line()
    {
        List<string> level1LineList = new List<string>();
        
        // Get all knot data strings
        List<string> knotDataList = GetSplineKnotsDataAsString(level1Splinecontainer.Spline);
        
        // Add radius info to each knot data string
        foreach (var knotData in knotDataList)
        {
            level1LineList.Add($"1,{level1Extrude.Radius},{knotData}");
        }
        
        return level1LineList;
    }
    
    public List<string> Getlevel2Line()
    {
        List<string> level2LineList = new List<string>();
        
        // Get all knot data strings
        List<string> knotDataList = GetSplineKnotsDataAsString(level2Splinecontainer.Spline);
        
        // Add radius info to each knot data string
        foreach (var knotData in knotDataList)
        {
            level2LineList.Add($"2,{level2Extrude.Radius},{knotData}");
        }
        
        return level2LineList;
    }
    
    public List<string> Getlevel3Line()
    {
        return GetLevel3SplineKnotsDataAsString(level3BigSplinecontainer, level3SmallSplinecontainer);
    }
    
}
