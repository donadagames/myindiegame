using UnityEngine;

public class GoToScene : MonoBehaviour
{
    [SerializeField] SceneData sceneToGo;
    Status status;

    public Vector3 playerPosition = new Vector3();
    public Vector3 playerRotation = new Vector3();

    public bool hasSpecificPlayerLocationToGo = false;

    private void Start()
    {
        hasEnter = false;
        status = Status.instance;
    }

    private void OnEnable()
    {
        hasEnter = false;
    }

    private bool hasEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (hasEnter == true) return;

        else
        {
            hasEnter = true;

            if(status.input.isMounted == true || status.input.isMounting)
                status.uiController.Dismount();

            if (hasSpecificPlayerLocationToGo == true)
                status.loadSceneController.LoadSceneWithSpecificPositionAndRotation(sceneToGo, playerPosition, playerRotation);
            else
                status.loadSceneController.LoadScene(sceneToGo);
        }
    }

}
