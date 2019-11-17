using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCoolDown : MonoBehaviour
{
    public string abilityButtonName = "ButtonName";
    public Image darkMask;

    public Ability ability;
    [SerializeField] private GameObject weaponHolder;
    private Image myButtonImage;
    private AudioSource abilitySource;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;
    private Ability aSelected;

    void Start()
    {
        Initialize(ability, weaponHolder);
    }

    public void Initialize(Ability selectedAbility, GameObject weaponHolder)
    {
        ability = selectedAbility;
        myButtonImage = GetComponent<Image>();
        abilitySource = GetComponent<AudioSource>();
        myButtonImage.sprite = ability.aSprite;
        darkMask.sprite = ability.aSprite;
        coolDownDuration = ability.aBaseCoolDown;
        ability.Initialize(weaponHolder);
        AbilityReady();
        aSelected = selectedAbility;
    }

    void Update()
    {
        if (ability != aSelected)
            Initialize(ability, weaponHolder);
        bool coolDownComplete = (Time.time > nextReadyTime);
        if(coolDownComplete)
        {
            AbilityReady();
            if (abilityButtonName == "horShoot" || abilityButtonName == "verShoot")
            {
                Vector2 dir = Vector2.zero;
                    dir.x = Input.GetAxisRaw("horShoot");
                    dir.y = Input.GetAxisRaw("verShoot");
                if (dir != Vector2.zero)
                {
                    if (dir.magnitude > 1)
                        dir /= dir.magnitude;
                    ButtonTriggered(dir);
                }
            }
        }
        else
        {
            CoolDown();
        }
    }

    private void AbilityReady()
    {
        darkMask.enabled = false;
    }

    private void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCd = Mathf.Round(coolDownTimeLeft);
        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    private void ButtonTriggered(Vector2 dir)
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        darkMask.enabled = true;

        abilitySource.clip = ability.aSound;
        abilitySource.Play();
        ability.TriggerAbility(dir);
    }

}
