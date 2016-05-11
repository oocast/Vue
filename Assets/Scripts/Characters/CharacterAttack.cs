using UnityEngine;
using System.Collections;
using System.Linq;

public class CharacterAttack : MonoBehaviour
{
    // TODO: this is just for DEBUG
    ShowAttackRange showAttackRange;
    PlayerAnimation playerAnimation;
    SoundSystem soundSystem;

    private Weapon _weapon;
    private MeleeSkill _basicAttackSkill = null;

    public Transform weaponModelSlot;

    /// <summary>
    /// Character is staggerred until
    /// </summary>
    [HideInInspector]
    public float staggerBeforeThisTime;
    private float _skillHitTime;

    private bool _skillHit = false;

    CountLock attackLock;

    float[] abilityCoolDownFins;

    private string _weaponNameGeneral;

    void Awake()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        var soundSystemObj = GameObject.Find("Sound System");
        if (soundSystemObj != null)
        {
            soundSystem = soundSystemObj.GetComponent<SoundSystem>();
        }
    }

    // Use this for initialization
    void Start () {
        attackLock = new CountLock();

        staggerBeforeThisTime = 0;
        abilityCoolDownFins = new float[2];

        showAttackRange = GetComponentInChildren<ShowAttackRange>();
        weaponModelSlot = transform.GetChild(1).Find("group6/group5/joint4/joint3/joint14/joint9/joint10/joint23/Weapon Model Slot");
        EquipWeapon(GameObject.Find("Weapon Library").GetComponent<WeaponLibraryBehavior>().weaponLibrary.weapons[0]);
    }
	
	// Update is called once per frame
	void Update () {
	    if (_basicAttackSkill != null)
        {
            if (Time.time > _skillHitTime && _skillHit == false)
            {
                _skillHit = true;
                Collider[] colliders = Physics.OverlapSphere(transform.position, _basicAttackSkill.brandishDistanceMax);
                // TODO: check angle with boundary, not center
                GameObject[] enemiesInRange = (from collider in colliders
                                               where collider.tag == "Enemy"
                                                  && collider.isTrigger == false
                                                  && Vector3.Distance(collider.bounds.ClosestPoint(transform.position), transform.position) > _basicAttackSkill.brandishDistanceMin
                                                  && Vector3.Angle(collider.bounds.ClosestPoint(transform.position) - transform.position, transform.forward) < _basicAttackSkill.halfBrandishAngle
                                               select collider.gameObject).ToArray();

                if (enemiesInRange.Length > 0)
                {
                    // Handheld.Vibrate();
                    soundSystem.PlaySound(transform.position, _weaponNameGeneral + " Hit");
                }
                foreach (GameObject enemy in enemiesInRange)
                {
                    enemy.GetComponent<ICharacterHealth>().TakeDamage(_basicAttackSkill.damage, _basicAttackSkill.targetStaggerTime, _basicAttackSkill.moveDistance, transform);
                }
            }
            else if (Time.time > staggerBeforeThisTime)
            {
                _basicAttackSkill = null;
                _skillHit = false;
                showAttackRange.FlushAttackRange();
            }
        }
	}

    public void EquipWeapon(Weapon weapon, UnityEngine.UI.Text debugText = null)
    {
        // destroy old weapon model
        if (weaponModelSlot.childCount > 0)
        {
            Destroy(weaponModelSlot.GetChild(0).gameObject);
        }

        // equip new weapon
        if (_weapon != null)
        {
            weapon.abilities = _weapon.abilities;
        }
        _weapon = weapon;
        _weapon.Initialize(debugText);
        if (_weapon.prefabName.Length > 2)
        {
            GameObject weaponPrefab = Resources.Load("Prefabs/" + _weapon.prefabName) as GameObject;
            if (weaponPrefab != null)
            {
                GameObject weaponModel = Instantiate(weaponPrefab, weaponModelSlot.position, weaponModelSlot.rotation) as GameObject;
                weaponModel.transform.SetParent(weaponModelSlot);
            }
        }

        // TODO: sound effect of changing weapon
        if (weapon.weaponName.Contains("Laser Sword"))
        {
            _weaponNameGeneral = "Laser Sword";
        }
        else if (weapon.weaponName.Contains("Sword"))
        {
            _weaponNameGeneral = "Sword";
        }
        else if (weapon.weaponName.Contains("Warhammer"))
        {
            _weaponNameGeneral = "Warhammer";
        }
        else if (weapon.weaponName.Contains("Frying Pan"))
        {
            _weaponNameGeneral = "Frying Pan";
        }
    }

    public void WeaponAttack()
    {
        if (Time.time > staggerBeforeThisTime && attackLock.value == false)
        {
            int weaponAttackIndex = 0;
            _basicAttackSkill = _weapon.GetWeaponAttackSkill(out weaponAttackIndex);
            playerAnimation.SetWeaponAttack(weaponAttackIndex);
            staggerBeforeThisTime = Time.time + _basicAttackSkill.selfStaggerTime;
            _skillHitTime = Time.time + _basicAttackSkill.hitTime;
            _skillHit = false;
            // TODO: change the range hit to animation or vfx
            showAttackRange.DrawAttackRange(_basicAttackSkill.halfBrandishAngle,
                                            _basicAttackSkill.brandishDistanceMax,
                                            _basicAttackSkill.brandishDistanceMin);
            //StartCoroutine("BasickAttackSkill");
            if (soundSystem != null)
            {
                soundSystem.PlaySound(transform.position, _weaponNameGeneral + " Swing");
            }
            Debug.Log("Attack with " + _basicAttackSkill.skillName);
        }
        else
        {
            Debug.Log("Self staggerring or skill caused attack lock");
        }
    }

    public void UseAbility(int index)
    {
        if (attackLock.value == false)
        {
            Debug.Log("Use ability " + index);
            float currentTime = Time.time;
            if (currentTime > staggerBeforeThisTime && currentTime > abilityCoolDownFins[index])
            {
                if (_weapon.abilities.Length > index)
                {
                    _weapon.abilities[index].ActivateSkill(gameObject);
                    abilityCoolDownFins[index] = currentTime + _weapon.abilities[index].coolDown;
                    GameObject cooldownObject = GameObject.Find("/Canvas/Ability " + index + "/CoolDown");
                    if (cooldownObject != null)
                    {
                        cooldownObject.GetComponent<CoolDown>().SetTimer(_weapon.abilities[index].coolDown);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Attack lock locked");
        }
    }

    public void UpdateWeaponAbility(int ability0Index, int ability1Index)
    {
        _weapon.UpdateAbilities(ability0Index, ability1Index);
        for (int i = 0; i < 2; i++)
        {
            GameObject cooldownObject = GameObject.Find("/Canvas/Ability " + i + "/CoolDown");
            if (cooldownObject != null)
            {
                cooldownObject.GetComponent<CoolDown>().ClearTimer();
            }
        }
    }

    public void LockAttack()
    {
        attackLock.Lock();
    }

    public void UnlockAttack()
    {
        attackLock.Unlock();
    }

    /*
    IEnumerator BasickAttackSkill()
    {
        showAttackRange.DrawAttackRange(_basicAttackSkill.halfBrandishAngle,
                                        _basicAttackSkill.brandishDistanceMax,
                                        _basicAttackSkill.brandishDistanceMin);
        yield return new WaitForSeconds(_basicAttackSkill.hitTime);

    }
    */
}
