using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRInput;

public class XRInputsManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateXRInputsManager()
    {
        GameObject manager = new GameObject("XRInputsManager");
        manager.AddComponent<XRInputsManager>();
        DontDestroyOnLoad(manager);
    }

    void Update() => XRInputs.Update();
}
