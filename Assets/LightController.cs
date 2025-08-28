using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light lightComponent;

    [Header("Materials to control emission")]
    public Material[] materials;
    public bool isRoot;

    void Start()
    {
        lightComponent = GetComponent<Light>();
        if (lightComponent == null)
        {
            Debug.LogWarning("No Light component found on this GameObject.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && lightComponent != null)
        {
            lightComponent.enabled = !lightComponent.enabled;
            if (isRoot) SetMaterialsEmission(lightComponent.enabled);
        }
    }

    private void SetMaterialsEmission(bool enable)
    {
        foreach (var mat in materials)
        {
            if (mat != null)
            {
                if (enable)
                {
                    mat.EnableKeyword("_EMISSION");
                }
                else
                {
                    mat.DisableKeyword("_EMISSION");
                }
            }
        }
    }
    void OnDestroy()
    {
        foreach (var mat in materials) mat.EnableKeyword("_EMISSION");
    }
}
