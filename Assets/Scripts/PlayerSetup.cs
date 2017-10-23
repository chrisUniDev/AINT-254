using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] componetsToDisable;

    Camera sceneCam;

	void Start () {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componetsToDisable.Length; i++)
            {
                componetsToDisable[i].enabled = false;
            }
        }
        else
        {
            sceneCam = Camera.main;
            if (sceneCam != null)
            {
                sceneCam.gameObject.SetActive(false);
            }
            
        }
	}

    private void OnDisable()
    {
        if (sceneCam != null)
        {
            sceneCam.gameObject.SetActive(true);
        }
    }


}
