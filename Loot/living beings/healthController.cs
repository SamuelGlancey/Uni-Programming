using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthController : MonoBehaviour
{
    public float currentHealth = 100;
    public float MaxHealth = 100;
    public Animator anim;
    public bool isDead;
    public Slider healthBar;
    bool lerpDamage;
    bool lerpHeal;
    public SkinnedMeshRenderer mesh;
    public Color HealthColor;

    // Start is called before the first frame update
    void Start()
    {
        //sets health to full health
        currentHealth = MaxHealth;
        //update slider maximum value
        healthBar.maxValue = MaxHealth;
        //update sliders current value
        healthBar.value = currentHealth;
        //update colour of the healthbar
        healthBar.fillRect.GetComponent<Image>().color = HealthColor;

    }

    // Update is called once per fram
    void Update()
    {
        //makes the model change from red to white over time after being hit
        if (lerpDamage)
        {
            mesh.material.color = Color.Lerp(mesh.material.color, Color.white, 5*Time.deltaTime);
            if(mesh.material.color == Color.white)
            {
                lerpDamage = false;
            }
        }
        //makes healthbar fade from green to white after healing
        if (lerpHeal)
        {
            healthBar.fillRect.GetComponent<Image>().color = Color.Lerp(healthBar.fillRect.GetComponent<Image>().color, HealthColor, 3 * Time.deltaTime);
            if(healthBar.fillRect.GetComponent<Image>().color == Color.blue)
            {
                lerpHeal = false;
            }
        }
    }

    //takes health away and does the appropriate checks
    public void takeDamage(float damage)
    {
        //make model nearly red
        mesh.material.color = new Color(0.8f, 0, 0);
        //lerp from red to white in update method
        lerpDamage = true;
        //minus damage from health
        currentHealth -= damage;
        
        if (healthBar != null)
        {
            //update slider with the health value
            healthBar.value = currentHealth;

            //check for death
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                //make the bar disappear
                healthBar.transform.parent.gameObject.SetActive(false);
                Die();
            }
        }
    }

    //adds an amount to the health
    public void Heal(float amount = 50f)
    {
        //turn health bar green
        healthBar.fillRect.GetComponent<Image>().color = new Color(0,0.8f,0);
        //add the amount to the health
        currentHealth += amount;
        //lerp from green to white in update method
        lerpHeal = true;
        //check if health is higher than the max health
        if (currentHealth >= MaxHealth)
        {
            currentHealth = MaxHealth;
        }
        
        if (healthBar != null)
        {
            //update slider with health value
            healthBar.value = currentHealth;
        }
        //check if dead
        if(currentHealth > 0)
        {
            isDead = false;
        }
    }
    public void Die()
    {
        // this boolean is used by other scripts
        isDead = true;
    }
}
