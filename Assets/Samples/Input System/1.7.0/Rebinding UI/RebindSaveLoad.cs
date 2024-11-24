using UnityEngine;
using UnityEngine.InputSystem;

public class RebindSaveLoad : MonoBehaviour
{
    public InputActionAsset actions;

    public void OnEnable()
    {
        Debug.Log("I AM IN THE ENABLE FUNCTION");
        Debug.Log("text1e");
        Debug.Log(this.name);
        Debug.Log("text2e");
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);
    }

    public void OnDisable()
    {
        Debug.Log("I AM IN THE DISABLE FUNCTION", this);
        Debug.Log("text1d");
        Debug.Log(this.name);
        Debug.Log("text2d");
        var rebinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
}
