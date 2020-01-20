using UnityEngine;

public class RotateDemo : MonoBehaviour
{
    [SerializeField] GameObject m_objTarget;
    Vector3 m_v3OriPos = new Vector3(10, 0, 1);
    [SerializeField] float m_fAngle;

    void Start()
    {
        m_fAngle = 0;
        m_objTarget.transform.position = m_v3OriPos;
    }

    void Update()
    {
        Vector3 v3ResultVec = Common.RoataeToPos(m_v3OriPos, m_objTarget.transform.localEulerAngles.y);
        m_objTarget.transform.position = v3ResultVec;

        Debug.Log("v3ResultVec: " + v3ResultVec);
    }
}
