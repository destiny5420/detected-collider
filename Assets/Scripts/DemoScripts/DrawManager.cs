using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    float m_fCenterHight;
    Vector3 m_v3CenterPos;
    [SerializeField] Camera m_camera;
    [SerializeField] DetectCollider m_clsDetectObj_A;
    static List<DetectCollider> m_lisData;

    void Awake()
    {
        m_lisData = new List<DetectCollider>();

        TextVec();
    }

    public static void Regist(DetectCollider r_obj)
    {
        m_lisData.Add(r_obj);
    }

    void Start()
    {

    }

    bool ComparerCollider(DetectCollider r_r1, DetectCollider r_r2)
    {
        if (
        r_r1.boundData.maxX > r_r2.boundData.minX && 
        r_r2.boundData.maxX > r_r1.boundData.minX && 
        r_r1.boundData.maxY > r_r2.boundData.minY && 
        r_r2.boundData.maxY > r_r1.boundData.minY 
        )
        {
             return true;
        }
        else
            return false;
    }

    void Update()
    {
        if (m_lisData.Count >= 2)
        {
            for (int i = 0; i < m_lisData.Count - 1; i++)
            {
                if (ComparerCollider(m_lisData[i], m_lisData[i + 1]))
                {
                    m_lisData[i].TriggerEvent(true);
                    m_lisData[i+1].TriggerEvent(true);
                }
                else
                {
                    m_lisData[i].TriggerEvent(false);
                    m_lisData[i+1].TriggerEvent(false);
                }
            }
        }
    }

    void TextVec()
    {
        Vector2 v2VecA = new Vector2(3, 4);
        Debug.LogWarning("v2VecA length: " + GetVecLength(v2VecA));
    }

    float GetVecLength(Vector2 v_vec)
    {
        return Mathf.Sqrt((v_vec.x*v_vec.x) + (v_vec.y*v_vec.y));
    }

    float GetVecDot(Vector2 v_vecA, Vector2 v_vecB)
    {
        return (v_vecA.x * v_vecB.x) + (v_vecA.y * v_vecB.y);
    }

    float GetProjectLenght(Vector2 v_projectVec, Vector2 v_projectBaseVec)
    {
        float fDotProduct = GetVecDot(v_projectVec, v_projectBaseVec);
        float fProjectBaseVecLength = GetVecLength(v_projectBaseVec);
        return fDotProduct / fProjectBaseVecLength;
    }

    void OnDrawGizmosSelected()
    {
        
    }
}
