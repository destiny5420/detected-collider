using System.Collections;
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
        // string sMsg1 = r_r1.boundData.maxX > r_r2.boundData.minX ? r_r1.name + " maxX > " + r_r2.name + " minX: true" : r_r1.name + " maxX > " + r_r2.name + " minX: false";
        // string sMsg2 = r_r2.boundData.maxX > r_r1.boundData.minX ? r_r2.name + " maxX > " + r_r1.name + " minX: true" : r_r2.name + " maxX > " + r_r1.name + " minX: false";
        // string sMsg3 = r_r1.boundData.maxY > r_r1.boundData.minY ? r_r1.name + " maxY > " + r_r2.name + " minY: true" : r_r1.name + " maxY > " + r_r2.name + " minY: false";
        // string sMsg4 = r_r2.boundData.maxY > r_r1.boundData.minY ? r_r2.name + " maxY > " + r_r1.name + " minY: true" : r_r2.name + " maxY > " + r_r1.name + " minY: false";
        // Debug.LogWarning(sMsg1);
        // Debug.LogWarning(sMsg2);
        // Debug.LogWarning(sMsg3);
        // Debug.LogWarning(sMsg4);

        if (
        r_r1.boundData.maxX > r_r2.boundData.minX && 
        r_r2.boundData.maxX > r_r1.boundData.minX && 
        r_r1.boundData.maxY > r_r1.boundData.minY && 
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

    void OnDrawGizmosSelected()
    {

    }
}
