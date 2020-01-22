using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTest_Normal : MonoBehaviour
{
    Vector3 m_v3OriPos = Vector3.zero;
    public Transform m_tranTargetVec;
    Vector3 m_v3NewVec;
    Vector2 m_v2NormalL;
    Vector2 m_v2NormalR;

    void Awake()
    {
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_v3NewVec = m_tranTargetVec.position - m_v3OriPos;
        m_v2NormalL = Common.GetNormalL(new Vector2(m_v3NewVec.x, m_v3NewVec.z));
        float fLengthL = Mathf.Sqrt(Mathf.Pow(m_v2NormalL.x, 2) + Mathf.Pow(m_v2NormalL.y, 2));
        m_v2NormalL = m_v2NormalL / fLengthL;

        m_v2NormalR = Common.GetNormalR(new Vector2(m_v3NewVec.x, m_v3NewVec.z));
        float fLengthR = Mathf.Sqrt(Mathf.Pow(m_v2NormalR.x, 2) + Mathf.Pow(m_v2NormalR.y, 2));
        m_v2NormalR = m_v2NormalR / fLengthR;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(m_v3NewVec.x, 0.0f, m_v3NewVec.z));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(m_v2NormalL.x, 0.0f, m_v2NormalL.y));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(m_v2NormalR.x, 0.0f, m_v2NormalR.y));
    }
}
