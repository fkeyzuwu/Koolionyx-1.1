﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KooliHome : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.instance.ReturnedHome();
    }
}
