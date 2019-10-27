using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRef : MonoBehaviour
{
    public GameObject player;

    #region
    public static PlayerRef instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion


}
