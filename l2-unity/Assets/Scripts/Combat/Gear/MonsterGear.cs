using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MonsterGear : Gear
{
    private const string BONE_CLAVICLE_R_NAME = "Bip01 R UpperArm";
    private const string BONE_CLAVICLE_L_NAME = "Bip01 L UpperArm";

    private Transform clavicleRBone;
    private Transform clavicleLBone;
    public Transform GetClavicledRBone()
    {
        if (clavicleRBone == null)
        {
            clavicleRBone = FindRecursiveBone(BONE_CLAVICLE_R_NAME);
        }
        return clavicleRBone;
    }

    public Transform GetClavicledLBone()
    {
        if (clavicleLBone == null)
        {
            clavicleLBone = FindRecursiveBone(BONE_CLAVICLE_L_NAME);
        }
        return clavicleLBone;
    }

    public Transform DetermineHitSide(Vector3 hitPoint, Transform target)
    {
        Transform rightBone = GetClavicledRBone();
        Transform leftBone = GetClavicledLBone();

        if (rightBone == null || leftBone == null)
        {
            Debug.LogWarning("Clavicle bones not found!");
            return null;
        }

  
        Vector3 hitDirection = hitPoint - target.position;
        hitDirection.Normalize(); 
        Vector3 characterRight = target.right;
        float dotProduct = Vector3.Dot(characterRight, hitDirection);

        if (Mathf.Abs(dotProduct) < 0.2f)
        {
            float rightDistance = Vector3.Distance(hitPoint, rightBone.position);
            float leftDistance = Vector3.Distance(hitPoint, leftBone.position);
            Transform selectBone = (rightDistance < leftDistance) ? rightBone : leftBone;
            Debug.Log($"Raycater hit calc Center hit select bone > {selectBone} < | Right Distance: {rightDistance}, Left Distance: {leftDistance}");
            return selectBone;
        }


        Transform selectedBone = (dotProduct > 0) ? rightBone : leftBone;
        Debug.Log($"Raycater hit calc Hit detected on {(dotProduct > 0 ? "Right" : "Left")} side");
        return selectedBone;
    }

    public void SetPositionArrowRandomlyNearCenter(GameObject attach, Transform targetBone,  Vector3 hitDirection)
    {
 
        Quaternion originalRotation = attach.transform.rotation;

        float[] possibleLengths = { 0f, 0.1f, 0.2f };
        float arrowLength = possibleLengths[Random.Range(0, possibleLengths.Length)];


        Vector3 arrowCenterPosition = targetBone.position - (hitDirection * (arrowLength * 0.5f));
        attach.transform.position = arrowCenterPosition;
        attach.transform.SetParent(targetBone);    
        attach.transform.forward = hitDirection;
        attach.transform.rotation = originalRotation;
    }









}
