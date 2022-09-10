using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class InflationController : MonoBehaviour
{
    public float minimumShaderVariableValue;
    public float maximumShaderVariableValue;
    public float TimeToCycle;
    [SerializeField]
    Material material;

    // Update is called once per frame
    void Update()
    {
        float shaderValue = (Mathf.Sin((Time.realtimeSinceStartup * Mathf.PI) / TimeToCycle) + 1) / 2;

        shaderValue = shaderValue * (maximumShaderVariableValue - minimumShaderVariableValue) + minimumShaderVariableValue;

        material.SetFloat("_InflationFactor", shaderValue);
    }
}
