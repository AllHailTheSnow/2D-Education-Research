using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public TMP_Text damageText;

    public float lifeTime = 1f;
    public float moveSpeed = 1f;
    public float placementJitter = 0.5f;

    private void Update()
    {
        Destroy(gameObject, lifeTime);
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }

    public void SetDamage(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0f);
    }
}
