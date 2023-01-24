using UnityEngine;
using UnityEngine.UI;

public class KeyConfig : MonoBehaviour
{

    [Range (-5,5)]
    public float cursorSpeed = 0;
    public Slider cursorSpeedSlider;
    public bool rotateInverseX=false;
    public bool rotateInverseY=false;
    public KeyCode jump = KeyCode.Space;
    public KeyCode[] wsad = new KeyCode[]{KeyCode.W,KeyCode.S,KeyCode.A,KeyCode.D};
    public KeyCode[] arrow = new KeyCode[]{KeyCode.UpArrow,KeyCode.DownArrow,KeyCode.LeftArrow,KeyCode.RightArrow};

    public KeyCode inventory = KeyCode.T;
    // public KeyCode dash = KeyCode.LeftControl;
    public KeyCode roll = KeyCode.LeftShift;
    public KeyCode modeChange = KeyCode.Tab;
    public KeyCode menu = KeyCode.Escape;
    public KeyCode focus = KeyCode.C;
    public KeyCode check = KeyCode.E;
    public void setCursorSpeed(){
        if (cursorSpeedSlider!=null){
            cursorSpeed = cursorSpeedSlider.value;
        }
    }
}
