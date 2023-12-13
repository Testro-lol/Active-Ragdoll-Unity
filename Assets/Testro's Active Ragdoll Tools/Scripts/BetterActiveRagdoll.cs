using UnityEngine;

public class BetterActiveRagdoll : MonoBehaviour
{
    public bool isBalanced;

    [Header("Assignables")]
    [SerializeField] ConfigurableJoint hipJoint;
    [SerializeField] ConfigurableJoint torsoJoint;
    [SerializeField] ConfigurableJoint[] kneeJoints;

    [Header("Hip Balance")]
    public float balanceStrength;
    public float balanceEase;

   
    [Header("Body Strength")]
    public float torsoStrength;
    public float torsoEase;

    [Header("Knee Strength")]
    public float kneeStrength;
    public float kneeEase;

    private void Start()
    {
        isBalanced = true;
    }

    public void Update()
    {
        if (isBalanced)
        {
            // Hip Stuff
            JointDrive zHipDrive = hipJoint.angularYZDrive;
            JointDrive xHipDrive = hipJoint.angularXDrive;

            zHipDrive.positionSpring = balanceStrength;
            xHipDrive.positionSpring = balanceStrength;

            zHipDrive.positionDamper = balanceEase;
            xHipDrive.positionDamper = balanceEase;

            hipJoint.angularYZDrive = zHipDrive;
            hipJoint.angularXDrive = xHipDrive;

            // Torso Stuff
            JointDrive zTorsoDrive = torsoJoint.angularYZDrive;
            JointDrive xTorsoDrive = torsoJoint.angularXDrive;

            zTorsoDrive.positionSpring = torsoStrength;
            xTorsoDrive.positionSpring = torsoStrength;

            zTorsoDrive.positionDamper = torsoEase;
            xTorsoDrive.positionDamper = torsoEase;

            torsoJoint.angularYZDrive = zTorsoDrive;
            torsoJoint.angularXDrive = xTorsoDrive;

            // Knee Stuff
            foreach(ConfigurableJoint joints in kneeJoints)
            {
                JointDrive zKneeDrive = joints.angularYZDrive;
                JointDrive xKneeDrive = joints.angularXDrive;

                zKneeDrive.positionSpring = kneeStrength;
                xKneeDrive.positionSpring = kneeStrength;

                zKneeDrive.positionDamper = kneeEase;
                xKneeDrive.positionDamper = kneeEase;

                joints.angularYZDrive = zKneeDrive;
                joints.angularXDrive = xKneeDrive;
            }
        }
    }
}
