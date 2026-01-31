using Godot;
using System;

public partial class SkeletonUtils : Node
{
    /// <summary>
    /// Updates the pin joint bias on all PhysicalBone3D nodes inside a PhysicalBoneSkeleton3D.
    /// Optional parameters allow locking linear and angular axes.
    /// </summary>
    public static void UpdatePhysicalBonePinJointBias(
        PhysicalBoneSimulator3D skeleton,
        float biasValue,
        bool lockLinearX = false,
        bool lockLinearY = false,
        bool lockLinearZ = false,
        bool lockAngularX = false,
        bool lockAngularY = false,
        bool lockAngularZ = false
    )
    {
        if (skeleton == null)
        {
            GD.PrintErr("Skeleton reference is null.");
            return;
        }
       
        foreach (Node child in skeleton.GetChildren())
        {
            
            if (child is PhysicalBone3D bone)
            {
                // child.SetParam(PinJoint3D.Param.Bias)
                
                // Joint3D joint = bone.Joint;
                
                // // Only modify if it's a PinJoint3D
                // if (joint is PinJoint3D pin)
                // {
                //     // Update bias
                //     pin.Bias = biasValue;

                //     // Apply linear lock settings
                //     pin.SetParam(PinJoint3D.Param.LinearLimitX, lockLinearX ? 0f : 1f);
                //     pin.SetParam(PinJoint3D.Param.LinearLimitY, lockLinearY ? 0f : 1f);
                //     pin.SetParam(PinJoint3D.Param.LinearLimitZ, lockLinearZ ? 0f : 1f);

                //     // Apply angular lock settings
                //     pin.SetParam(PinJoint3D.Param.AngularLimitX, lockAngularX ? 0f : 1f);
                //     pin.SetParam(PinJoint3D.Param.AngularLimitY, lockAngularY ? 0f : 1f);
                //     pin.SetParam(PinJoint3D.Param.AngularLimitZ, lockAngularZ ? 0f : 1f);
                // }
            }
        }
    }
}