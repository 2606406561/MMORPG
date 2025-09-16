using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField, Header("武器主人标识符")] public int id;//谁的武器
    [SerializeField,Header("命中伤害")] public int damage;
    [SerializeField, Header("武器主人位置")] public Transform ts;
    [SerializeField, Header("破韧值")] public int value;
    private void OnTriggerEnter(Collider other)
    {
        AiEnemy ae = other.GetComponent<AiEnemy>();
        if (ae != null)
        {
            ae.Hurt(ts, damage, id, value);
            
        }
        else
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if(pc != null)
            {
                pc.Hurt(ts, damage, id);
            }
        }
    }
}
