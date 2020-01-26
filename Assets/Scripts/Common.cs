using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common
{
    static Vector3 m_v3VecResult;

    // public static Vector3 RoataeToPos(Vector3 v_target, float v_angle)
    // {
    //     float fAngle = v_angle * Mathf.Deg2Rad;
    //     float fNewX = v_target.x * Mathf.Cos(fAngle) - v_target.z * Mathf.Sin(fAngle);
    //     float fNewZ = v_target.x * Mathf.Sin(fAngle) + v_target.z * Mathf.Cos(fAngle);

    //     m_vecResult.x = fNewX;
    //     m_vecResult.y = v_target.y;
    //     m_vecResult.z = fNewZ;
        
    //     return m_vecResult; 
    // }

    public static float DisForVector3(Vector3 v_vec)
    {
        return Mathf.Pow(Mathf.Sqrt(Mathf.Pow(v_vec.x, 2) + Mathf.Pow(v_vec.y, 2) + Mathf.Pow(v_vec.z, 2)), 2);
    }

    public static Vector3 RoataeToPosAxisY(Vector3 v_target, Vector3 v_ref, float v_angle)
    {
        float fAngle = v_angle * Mathf.Deg2Rad;
        float fNewX = (v_target.x - v_ref.x) * Mathf.Cos(fAngle) - (v_target.z - v_ref.z) * Mathf.Sin(fAngle);
        float fNewZ = (v_target.x - v_ref.x) * Mathf.Sin(fAngle) + (v_target.z - v_ref.z) * Mathf.Cos(fAngle);

        m_v3VecResult.x = fNewX;
        m_v3VecResult.y = v_target.y;
        m_v3VecResult.z = fNewZ;
        
        return m_v3VecResult; 
    }

    static float GetVecLength(Vector2 v_vec)
    {
        return Mathf.Sqrt((v_vec.x * v_vec.x) + (v_vec.y * v_vec.y));
    }

    static float GetVecDot(Vector2 v_vecA, Vector2 v_vecB)
    {
        return (v_vecA.x * v_vecB.x) + (v_vecA.y * v_vecB.y);
    }

    public static float GetProjectLenghtOnto(Vector2 v_projectVec, Vector2 v_projectBaseVec)
    {
        float fDotProduct = GetVecDot(v_projectVec, v_projectBaseVec);
        float fProjectBaseVecLength = GetVecLength(v_projectBaseVec);
        return fDotProduct / fProjectBaseVecLength;
    }

    public static Vector2 GetNormalL(Vector2 v_vec)
    {
        return new Vector2(-v_vec.y, v_vec.x);
    }

    public static Vector2 GetNormalR(Vector2 v_vec)
    {
        return new Vector2(v_vec.y, -v_vec.x);
    }
}
