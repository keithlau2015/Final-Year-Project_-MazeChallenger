using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void SwordSwingSound()
    {
        FindObjectOfType<SoundManager>().PlayWeaponAttackEffect(0);
    }

    public void SpearStingSound()
    {
        FindObjectOfType<SoundManager>().PlayWeaponAttackEffect(1);
    }
}
