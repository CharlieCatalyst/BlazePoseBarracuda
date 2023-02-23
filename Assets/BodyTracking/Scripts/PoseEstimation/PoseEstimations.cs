using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PoseEstimation")]
public class PoseEstimations : ScriptableObject
{
    Dictionary<PoseName, PoseEstimate> poseEstimations = new Dictionary<PoseName, PoseEstimate>()
    {

        { PoseName.nose, new PoseEstimate() },
        { PoseName.left_eye_inner, null },
        { PoseName.left_eye, null },
        { PoseName.left_eye_outer, null },
        { PoseName.right_eye_inner, null },
        { PoseName.right_eye, null },
        { PoseName.right_eye_outer, null },
        { PoseName.left_ear, null },
        { PoseName.right_ear, null },
        { PoseName.mouth_left, null },
        { PoseName.mouth_right, null },
        { PoseName.left_shoulder, null },
        { PoseName.right_shoulder, null },
        { PoseName.left_elbow, null },
        { PoseName.right_elbow, null },
        { PoseName.left_wrist, null },
        { PoseName.right_wrist, null },
        { PoseName.left_pinky, null },
        { PoseName.right_pinky, null },
        { PoseName.left_index, null },
        { PoseName.right_index, null },
        { PoseName.left_thumb, null },
        { PoseName.right_thumb, null },
        { PoseName.left_hip, null },
        { PoseName.right_hip, null },
        { PoseName.left_knee, null },
        { PoseName.right_knee, null },
        { PoseName.left_ankle, null },
        { PoseName.right_ankle, null },
        { PoseName.left_heel, null },
        { PoseName.right_heel, null },
        { PoseName.left_foot_index, null },
        { PoseName.right_foot_index, null },
        { PoseName.visible, null},
        { PoseName.neck, null}
    };


    [SerializeField] Vector2 cameraResolution = new Vector2(720, 1280);
    public Vector2 CameraResolution
    {
        get { return cameraResolution; }
        set { cameraResolution = value; }
    }

    float currentScale = 1;
    public float CurrentScale
    {
        get { return currentScale; }
        set { currentScale = value; }
    }

    public void Add(PoseName poseName, PoseEstimate poseEstimate)
    {
        poseEstimations.Add(poseName, poseEstimate);
    }

    public void UpdatePose(PoseName poseNameToUpdate, PoseEstimate poseEstimate)
    {
        poseEstimations[poseNameToUpdate] = poseEstimate;
    }

    public PoseEstimate GetPose(string pointName)
    {
        return poseEstimations[(PoseName)Enum.Parse(typeof(PoseName), pointName)];
    }
    public PoseEstimate GetPose(PoseName pointName)
    {
        return poseEstimations[pointName];
    }

    public List<PoseName> GetAllPoseNames()
    {
        List<PoseName> poseNames = new List<PoseName>();
        foreach (KeyValuePair<PoseName, PoseEstimate> pose in poseEstimations)
        {
            poseNames.Add(pose.Key);
        }

        return poseNames;
    }

    public string GetStringList()
    {
        string output = "";
        foreach (KeyValuePair<PoseName, PoseEstimate> pose in poseEstimations)
        {
            if (pose.Value != null)
            {
                output = output + pose.Key + " : " + pose.Value.ToString() + "\n";
            }
        }
        return output;
    }

}

[Serializable]
public class PoseEstimate
{
    public Point point = new Point();
    public float relativeDepth; //  Relative to the hips.
    public float visibilityCoefficient;
}