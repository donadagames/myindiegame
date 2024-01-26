using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    InputHandler inputHandler;
    AudioSource audioSource;
    Player player;
    [SerializeField] private AudioClip[] meleeAttackAudioClips;
    [SerializeField] private AudioClip[] grassStepAudioClips;
    [SerializeField] private AudioClip[] waterStepAudioClips;
    [SerializeField] private AudioClip[] cutWoodAudioClips;
    [SerializeField] private AudioClip[] jumpAudioClips;
    [SerializeField] private AudioClip[] swimmingAudioClips;
    [SerializeField] private AudioClip[] deathAudioClips;
    [SerializeField] private AudioClip[] damageAudioClips;
    [SerializeField] private AudioClip[] swordHitAudioClips;
    [SerializeField] private AudioClip[] landAudioClips;
    [SerializeField] private AudioClip[] miningAudioClips;
    [SerializeField] private AudioClip splashAudioClip;
    [SerializeField] private AudioClip levelUpClip;
    [SerializeField] private AudioClip[] openCrateAudioClips;

    [Header("Skills")]
    [SerializeField] AudioClip fireballAudioClip;
    [SerializeField] AudioClip cureAudioClip;
    [SerializeField] AudioClip blastAudioClip;
    [SerializeField] AudioClip iceAudioClip;
    [SerializeField] AudioClip meteoreAudioClip;
    [SerializeField] AudioClip speakFire;
    [SerializeField] AudioClip[] speakCure;
    [SerializeField] AudioClip speakBurn;
    [SerializeField] AudioClip[] speakIce;
    [SerializeField] AudioClip[] speaklevelUp;
    [SerializeField] AudioClip[] healdAudioClips;
    private void Start()
    {
        inputHandler = InputHandler.instance;
        player = GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
    }

    private AudioClip GetRandomAudioClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }

    public void StepSound()
    {
        if (inputHandler.footIsOnWater)
        {
            audioSource.PlayOneShot(GetRandomAudioClip(waterStepAudioClips));
        }
        else
        {
            audioSource.PlayOneShot(GetRandomAudioClip(grassStepAudioClips));
        }
    }

    public void SplashSound()
    {
        audioSource.PlayOneShot(splashAudioClip);
    }

    public void JumpSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(jumpAudioClips));
    }

    public void MeleeAttackSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(meleeAttackAudioClips));
    }

    public void CutWoodSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(cutWoodAudioClips));
        Instantiate(player.impactWood_VFX, player.impactTransform.position, Quaternion.identity);
    }

    public void MinningSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(miningAudioClips));
        Instantiate(player.impactStone_VFX, player.impactTransform.position, Quaternion.identity);
    }

    public void MiningSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(miningAudioClips));
    }

    public void SwimmingSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(swimmingAudioClips));
    }

    public void LandSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(landAudioClips));
    }

    public void SwordHitSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(swordHitAudioClips));
    }

    public void DeathSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(deathAudioClips));
    }

    public void GetHitSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(damageAudioClips));
    }

    public void LevelUpSound()
    {
        PlayClip(levelUpClip);
    }

    public void SpeakLevelUp()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(speaklevelUp));
    }

    public void OpenCrateSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(openCrateAudioClips));
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void FireballSound()
    {
        audioSource.PlayOneShot(fireballAudioClip);
    }

    public void CureSound()
    {
        audioSource.PlayOneShot(cureAudioClip);
    }

    public void IceSound()
    {
        audioSource.PlayOneShot(iceAudioClip);
    }

    public void BlastSound()
    {
        audioSource.PlayOneShot(blastAudioClip);
    }

    public void StarfallSound()
    {
        audioSource.PlayOneShot(meteoreAudioClip);
    }

    public void SpeakFire()
    {
        audioSource.PlayOneShot(speakFire);
    }

    public void SpeakCure()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(speakCure));
    }

    public void HealingSound()
    {
        audioSource.PlayOneShot(GetRandomAudioClip(healdAudioClips));
    }


    public void SpeakBurne()
    {
        audioSource.PlayOneShot(speakBurn);
    }

    public void SpeakIce()
    {
        audioSource.PlayOneShot(speakIce[Random.Range(0, speakIce.Length)]);
    }
}
