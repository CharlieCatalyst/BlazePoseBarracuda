using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class JointsMapping : MonoBehaviour
{
    [SerializeField] PoseManager poseManager;
    [SerializeField] FullBodyBipedIK bipedIK;

    [Space]
    [SerializeField] Transform root;

    bool trackingStarted;

    //[SerializeField] List<MappedJoint> mappedJoints;

    private void Start()
    {
        StartCoroutine(IDelaySetup());
    }

    IEnumerator IDelaySetup()
    {
        yield return WaitFor.Frames(15);
        Setup();
    }

    public void Setup()
    {
        if (poseManager.GetPose(PoseName.neck) == null)
        {
            StartCoroutine(IDelaySetup());
            return;
        }
        trackingStarted = true;
        root.parent = poseManager.GetPose(PoseName.neck).transform;
        root.localPosition = new Vector3(0, 0, 0);

        SetupIKSolvers();
    }

    private void SetupIKSolvers()
    {
        bipedIK.solver.leftHandEffector.target = poseManager.GetPose(PoseName.left_wrist).transform;
        bipedIK.solver.leftShoulderEffector.target = poseManager.GetPose(PoseName.left_shoulder).transform;
        bipedIK.solver.leftArmChain.bendConstraint.bendGoal = poseManager.GetPose(PoseName.left_elbow).transform;

        bipedIK.solver.rightHandEffector.target = poseManager.GetPose(PoseName.right_wrist).transform;
        bipedIK.solver.rightShoulderEffector.target = poseManager.GetPose(PoseName.right_shoulder).transform;
        bipedIK.solver.rightArmChain.bendConstraint.bendGoal = poseManager.GetPose(PoseName.right_elbow).transform;
    }

    private void SetWeights(Pose pose, IKEffector ikEffector)
    {
        if (pose != null)
        {
            ikEffector.positionWeight = pose.gameObject.activeSelf ? 1 : 0;
        }
    }
    private void SetWeights(Pose pose, IKConstraintBend bendConstraint)
    {
        if (pose != null)
        {
            bendConstraint.weight = pose.gameObject.activeSelf ? 1 : 0;
        }
    }

    private void Update()
    {
        if (trackingStarted)
        {
            SetWeights(poseManager.GetPose(PoseName.left_wrist), bipedIK.solver.leftHandEffector);
            SetWeights(poseManager.GetPose(PoseName.right_wrist), bipedIK.solver.rightHandEffector);
            SetWeights(poseManager.GetPose(PoseName.left_wrist), bipedIK.solver.leftArmChain.bendConstraint);
            SetWeights(poseManager.GetPose(PoseName.right_wrist), bipedIK.solver.rightArmChain.bendConstraint);
            SetWeights(poseManager.GetPose(PoseName.left_wrist), bipedIK.solver.leftShoulderEffector);
            SetWeights(poseManager.GetPose(PoseName.right_wrist), bipedIK.solver.rightShoulderEffector);
            //SetWeights(poseManager.GetPose(PoseName.left_elbow), bipedIK.solver.leftArmChain.bendConstraint);
            //SetWeights(poseManager.GetPose(PoseName.right_elbow), bipedIK.solver.rightArmChain.bendConstraint);
            //SetWeights(poseManager.GetPose(PoseName.left_shoulder), bipedIK.solver.leftShoulderEffector);
            //SetWeights(poseManager.GetPose(PoseName.right_shoulder), bipedIK.solver.rightShoulderEffector);
        }

    }
}

[Serializable]
internal class MappedJoint
{
    public string poseName;
    public Transform jointTransform;
}
