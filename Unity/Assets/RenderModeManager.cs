using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderModeManager : MonoBehaviour
{
    // Start is called before the first frame update
    Color activeColor;
    Color originalColor;
    MeshRenderer go_renderer;
    void Start()
    {     
        go_renderer = this.GetComponent<MeshRenderer>();
        activeColor = new Color(go_renderer.material.color.r, go_renderer.material.color.g, go_renderer.material.color.b, go_renderer.material.color.a);
        originalColor = new Color(go_renderer.material.color.r, go_renderer.material.color.g, go_renderer.material.color.b, go_renderer.material.color.a);
    }

    // Update is called once per frame
    void Update()
    {
        go_renderer.material.color = activeColor;
    }

    public void OnPlayingWhileRecordingAnotherCharacter()
    {
        Color color = this.GetComponent<MeshRenderer>().material.color;
        color.r *= 0f;
        color.g *= 0f;
        color.b *= 0f;
        activeColor = color;
    }

    public void OnFinishedPlayingWhileRecordingAnotherCharacter()
    {
        activeColor = new Color(1f,1f,1f);
        go_renderer.material.color = new Color(1f, 1f, 1f);
    }
}
