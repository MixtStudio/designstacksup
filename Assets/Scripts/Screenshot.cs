using UnityEngine;
using System.Collections;

public class Screenshot : MonoBehaviour
{
	private void Update()
	{
        if (Input.GetKeyDown("k"))
            ScreenCapture.CaptureScreenshot("SS3", 4);
    }
}