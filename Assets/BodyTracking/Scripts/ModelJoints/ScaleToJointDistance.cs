using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToJointDistance : MonoBehaviour
{
    [SerializeField] int frameInterval = 5;
    [SerializeField] float scalar = 1f;
    [SerializeField] Camera bodyTrackingCamera;

    [Space]
    [SerializeField] PoseEstimations poseEstimations;

    [Space]
    [SerializeField] Transform transformToBeScaled;

    [Space]
    [SerializeField] string poseFrom;
    [SerializeField] string poseTo;

    [Space]
    [SerializeField] Transform jointFrom;
    [SerializeField] Transform jointTo;

    [Space]
    [SerializeField] bool debug;

    PoseEstimate pointFrom;
    PoseEstimate pointTo;

    float originalScale = 1;
    //float lastScale = 1;

    private void Start()
    {
        pointFrom = poseEstimations.GetPose(poseFrom);
        pointTo = poseEstimations.GetPose(poseTo);

        originalScale = transformToBeScaled.localScale.x;

        //StartCoroutine(IScaleTransform());
    }

    private void OnEnable()
    {
        StartCoroutine(IScaleTransform());
    }

    IEnumerator IScaleTransform()
    {
        yield return WaitFor.Frames(frameInterval);
        ScaleTransform();
        StartCoroutine(IScaleTransform());
    }

    private float GetDistanceVariation()
    {
        pointFrom = poseEstimations.GetPose(poseFrom);
        pointTo = poseEstimations.GetPose(poseTo);
        //  Check if poses are null, if so, return.
        if (pointFrom == null || pointTo == null)
        {
            Debug.Log("Points do not exist");
            return -1;
        }
        //  Divide dP / dJ (=dPJ).
        float distanceBetweenPoints = DistanceBetweenPoints();
        float distanceBetweenJoints = DistanceBetweenJoints();
        if (debug)
        {
            Debug.Log($"DistanceBetweenPoints = {distanceBetweenPoints}\nDistanceBetweenJoints = {distanceBetweenJoints}");
        }
        return distanceBetweenPoints / distanceBetweenJoints;
    }


    //  Multiply originalScale * dPJ * scalar * Vector3.one
    private void ScaleTransform()
    {
        float distanceVariation = GetDistanceVariation();

        if (distanceVariation < 0)
        {
            return;
        }

        float scaleValue = originalScale * distanceVariation * scalar;
        //float scaleValue = originalScale * distanceVariation * scalar * (Camera.main.pixelWidth / poseEstimations.CameraResolution.x);
        if (scaleValue > 0)
            transformToBeScaled.localScale = scaleValue * transformToBeScaled.localScale;
        //lastScale = transformToBeScaled.localScale.x;
        poseEstimations.CurrentScale = transformToBeScaled.localScale.x;

        if (debug)
        {
            Debug.Log($"distanceVariation = {distanceVariation}");
            Debug.Log($"scaleValue = {scaleValue}");
        }
    }

    //  Calculate distance between points (dP).
    private float DistanceBetweenPoints()
    {
        return Vector2.Distance(new Vector2((float)pointFrom.point.x, (float)pointFrom.point.y), new Vector2((float)pointTo.point.x, (float)pointTo.point.y));
    }

    //  Convert joint positions to screen space.
    //  Calculate distance between screen space joints (dJ).
    private float DistanceBetweenJoints()
    {
        Vector2 jointFromScreenSpace = bodyTrackingCamera.WorldToScreenPoint(jointFrom.position);
        Vector2 jointToScreenSpace = bodyTrackingCamera.WorldToScreenPoint(jointTo.position);

        if (debug)
        {
            Debug.Log($"jointFromScreenSpace = {jointFromScreenSpace}");
            Debug.Log($"jointToScreenSpace = {jointToScreenSpace}");
        }

        return Vector2.Distance(jointFromScreenSpace, jointToScreenSpace);
    }
}
