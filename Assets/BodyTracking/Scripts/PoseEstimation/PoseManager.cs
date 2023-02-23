using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using OpenCVForUnity.CoreModule;

public class PoseManager : MonoBehaviour
{
    [SerializeField] int frameInterval = 5;
    [SerializeField] PoseEstimations poseEstimations;
    [SerializeField] GameObject posePrefab;
    [Space]
    [SerializeField] bool debug;
    [Space]
    [SerializeField] List<PoseZOffset> zOffsets;
    Dictionary<PoseName, Pose> poses = new Dictionary<PoseName, Pose>();


    private void Start()
    {
        StartCoroutine(IDelayGeneratePoses());
    }

    IEnumerator IDelayGeneratePoses()
    {
        yield return new WaitForSecondsRealtime(1f);
        GeneratePoses();
        StartCoroutine(IPositionPoses());
    }

    public void GeneratePoses()
    {
        foreach (PoseName poseName in poseEstimations.GetAllPoseNames())
        {
            GameObject poseGO = Instantiate(posePrefab);
            Pose pose = poseGO.GetComponent<Pose>();

            poseGO.name = $"Pose - {poseName}";
            pose.UpdateLabel(poseName.ToString());
            poses.Add(poseName, pose);
        }
    }

    IEnumerator IPositionPoses()
    {
        yield return WaitFor.Frames(frameInterval);
        foreach (KeyValuePair<PoseName, Pose> poseKV in poses)
        {
            poseKV.Value.SetDebugActive(debug);

            PoseEstimate poseEstimate = poseEstimations.GetPose(poseKV.Key);
            poseKV.Value.SetActive(poseEstimate != null);

            if (poseEstimate != null)
            {
                //if (poseEstimate.point.x > 0 && poseEstimate.point.y > 0)
                if (poseEstimate.visibilityCoefficient > 0.8f)
                {

                    PoseZOffset zOffset = zOffsets.FirstOrDefault(zo => zo.poseName == poseKV.Key);
                    poseKV.Value.UpdatePosition(Camera.main.ScreenToWorldPoint(new Vector3(
                       FormatPointWithResolution(poseEstimate.point).x,
                        Camera.main.pixelHeight - FormatPointWithResolution(poseEstimate.point).y
                       , 4
                       )));
                }

                else
                {
                    poseKV.Value.UpdatePosition(Vector3.zero);
                    Debug.Log($"pose: {poseKV.Key} is null");
                }
            }
        }
        StartCoroutine(IPositionPoses());
    }

    private Vector2 FormatPointWithResolution(Point point)
    {

        return new Vector2((float)point.x * Camera.main.pixelWidth, (1 - (float)point.y) * Camera.main.pixelHeight);
        //return new Vector2((float)point.x * Camera.main.pixelWidth * (Camera.main.pixelWidth / poseEstimations.CameraResolution.x), (1 - (float)point.y) * Camera.main.pixelHeight) * (Camera.main.pixelHeight / poseEstimations.CameraResolution.y);
        //return new Vector2((float)point.x * (Camera.main.pixelWidth / poseEstimations.CameraResolution.x), (float)point.y * (Camera.main.pixelHeight / poseEstimations.CameraResolution.y));
    }

    public Pose GetPose(PoseName poseName)
    {
        if (!poses.ContainsKey(poseName))
        {
            Debug.LogError($"poseName: {poseName} not found in pose dictionary");
            return null;
        }
        return poses[poseName];
    }

    public void ToggleDebug()
    {
        debug = !debug;
    }

}

[Serializable]
public class PoseZOffset
{
    public PoseName poseName;
    public float offset;
}
