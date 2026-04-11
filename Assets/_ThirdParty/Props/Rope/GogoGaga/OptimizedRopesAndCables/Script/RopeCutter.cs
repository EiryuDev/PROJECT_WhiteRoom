using UnityEngine;
using GogoGaga.OptimizedRopesAndCables;

public class RopeCutter : MonoBehaviour
{
    private Rope _rope;
    bool _RopeCut = false;

    void Start()
    {
        _rope = GetComponent<Rope>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name);
        if (other.CompareTag("sword"))
        {
            CutRope(other.ClosestPoint(transform.position));
            Debug.Log("Rope cut at position: " + other.ClosestPoint(transform.position));
        }
    }

    public void CutRope(Vector3 cutPosition)
    {
        if (_RopeCut)
        {
            return;
        }
        _RopeCut = true;
        float t = _rope.GetClosestT(cutPosition);

        GameObject cutPointA = CreatePhysicalPoint(cutPosition);
        GameObject cutPointB = CreatePhysicalPoint(cutPosition);

        SpawnHalfRope(_rope.StartPoint, cutPointA.transform, _rope.ropeLength * t);
        SpawnHalfRope(cutPointB.transform, _rope.EndPoint, _rope.ropeLength * (1 - t));

        Destroy(this.gameObject);
    }

    private void SpawnHalfRope(Transform start, Transform end, float length)
    {
        GameObject newRopeObj = Instantiate(gameObject, transform.parent);
        Rope newRope = newRopeObj.GetComponent<Rope>();

        Destroy(newRopeObj.GetComponent<RopeCutter>());

        newRope.SetStartPoint(start, true);
        newRope.SetEndPoint(end, true);
        newRope.ropeLength = length;
    }

    private GameObject CreatePhysicalPoint(Vector3 position)
    {
        GameObject point = new GameObject("CutPoint");
        point.transform.position = position;
        Rigidbody rb = point.AddComponent<Rigidbody>();
        BoxCollider collider = point.AddComponent<BoxCollider>();
        collider.size = new Vector3(0.1f, 0.1f, 0.1f);

        rb.AddExplosionForce(5f, position, 1f);
        return point;
    }
}