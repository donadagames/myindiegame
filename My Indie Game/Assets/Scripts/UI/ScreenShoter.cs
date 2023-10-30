using UnityEngine;

public class ScreenShoter : MonoBehaviour
{
    [SerializeField]
    private string path;
    private string fileName;
    [SerializeField]
    [Range(1, 5)]
    private int size = 1;

    // Update is called once per frame
    public void Photo()
    {
        fileName = "screenshot ";
        fileName += System.Guid.NewGuid().ToString() + ".png";

        ScreenCapture.CaptureScreenshot(path + fileName, size);
    }
}

