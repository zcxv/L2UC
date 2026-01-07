using UnityEngine;

public class TrackedSword
{
    public Transform basePt;
    public Transform tipPt;
    public float range;

    public TrackedSword(Transform basePt , Transform tipPt, float range)
    {
        this.basePt = basePt;
        this.tipPt = tipPt;
        this.range = range;
    }

    public Vector3 PositionBasePt()
    {
        return basePt.position;
    }

    public Vector3 PositiontipPt()
    {
        return tipPt.position;
    }
}
