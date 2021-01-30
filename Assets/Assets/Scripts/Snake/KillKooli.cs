using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillKooli : MonoBehaviour
{
    void KillKoolis()
    {
        GameManager.instance.RestartLevel();
    }
}
