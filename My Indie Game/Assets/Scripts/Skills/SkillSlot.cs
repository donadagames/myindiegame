using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public Skill skill;
    public bool isAvailable = false;
    public Image icon;

    UIController uiController;

    private void Start()
    {
        uiController = UIController.instance;
    }

    public void OnSkillButtonPressed()
    {
        if (isAvailable == false) return;

        uiController.UpdateSkillImage(skill);
        uiController.PlayDefaultAudioClip();
    }

    public void ReciveSkill(Skill _skill)
    {
        if (skill = _skill)
        {
            isAvailable = true;
            icon.color = Color.white;
        }
    }
}
