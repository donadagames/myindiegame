using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    [Header("Left Stick")]
    #region LEFT STICK 

    [SerializeField] GameObject[] positionFocus;
    private Direction currentDirection;

    private void SetPositionFocus(int focusIndex)
    {
        for (int i = 0; i < positionFocus.Length; i++)
        {
            positionFocus[i].SetActive(false);
        }
        if (focusIndex < positionFocus.Length)
        {
            positionFocus[focusIndex].SetActive(true);
        }
    }

    public void SetPositionFocusUI(Direction direction)
    {
        if (direction == currentDirection) return;
        else
        {
            switch (direction)
            {
                case Direction.UpLeft:
                    currentDirection = Direction.UpLeft;
                    SetPositionFocus(0);
                    break;
                case Direction.UpRight:
                    currentDirection = Direction.UpRight;
                    SetPositionFocus(1);
                    break;
                case Direction.DownLeft:
                    currentDirection = Direction.DownLeft;
                    SetPositionFocus(2);
                    break;
                case Direction.DownRight:
                    currentDirection = Direction.DownRight;
                    SetPositionFocus(3);
                    break;
                case Direction.Default:
                    currentDirection = Direction.Default;
                    SetPositionFocus(10);
                    break;
                default:
                    currentDirection = Direction.Default;
                    SetPositionFocus(10);
                    break;
            }
        }
    }
    #endregion
}

public enum Direction
{
    UpLeft, DownLeft, UpRight, DownRight, Default
}
