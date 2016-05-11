using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class Chest : MonoBehaviour, IPointerDownHandler
{
    public ChestKey key;
    public Text debugText;

    Transform _playerTransform;
    WeaponLibrary _weaponLibrary;

    private readonly float _showUpTime = 1f;
    private readonly float _lingerTime = 0.5f;
    private readonly float _toPlayerTime = 0.4f;

    private readonly float chestActiveDistance = 3f;
    private readonly float chestActiveAngle = 60f;
    private bool active;
    private Animator animator;

    void Awake()
    {
        _weaponLibrary = GameObject.Find("Weapon Library").GetComponent<WeaponLibraryBehavior>().weaponLibrary;
    }

	// Use this for initialization
	void Start () {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        active = false;
        animator = GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        Debug.Log("Clicking the chest");
        // first test distance, angle, then verify the key
        if (!active 
            && Vector3.Distance(transform.position, _playerTransform.position) < chestActiveDistance 
            && Vector3.Angle(_playerTransform.position - transform.position, transform.forward) < chestActiveAngle)
        {
            // verify key
            PlayerInventory inventory = _playerTransform.GetComponent<PlayerInventory>();
            if (inventory.holdChestKey
                && inventory.chestKey == key)
            {
                active = true;
                animator.SetTrigger("Open Chest");
                string result = GameObject.Find("Twitch Vote").GetComponent<VotingSystem>().CloseVoteAndGetResult(key.vote.title);
                Debug.Log("Vote result: " + result);

                // spawn the weapon
                SpawnWeapon(result);

                // disable way point
                OnOpen();
            }
        }
    }

    // TODO: get inventory other than weapons from chests
    void SpawnWeapon(string optionName)
    {
        int weaponIndex = key.vote.GetOptionIndexByName(optionName);
        Weapon weapon = _weaponLibrary.weapons[weaponIndex];

        
        StartCoroutine(HandoverWeapon(weapon));

        GameObject weaponPrefab = Resources.Load("Prefabs/" + weapon.prefabName) as GameObject;
        if (weaponPrefab != null)
        {
            GameObject displayModel = Instantiate(weaponPrefab, transform.position, transform.rotation) as GameObject;
            displayModel.transform.DOMoveY(1f, _showUpTime);
            displayModel.transform.DOScale(3f, _showUpTime);
            displayModel.transform.DOMove(_playerTransform.position, _toPlayerTime).SetDelay(_showUpTime + _lingerTime);
            displayModel.transform.DOScale(0.2f, _toPlayerTime).SetDelay(_showUpTime + _lingerTime);
            Destroy(displayModel, _showUpTime + _toPlayerTime + _lingerTime);
        }
    }

    void OnEnable()
    {
        GameObject wayPointObject = GameObject.Find("Way Point");
        if (wayPointObject != null)
        {
            wayPointObject.GetComponent<WayPoint>().target = gameObject;
            wayPointObject.GetComponent<UnityEngine.UI.Image>().enabled = true;
        }
    }

    void OnOpen()
    {
        GameObject wayPointObject = GameObject.Find("Way Point");
        if (wayPointObject != null)
        {
            wayPointObject.GetComponent<UnityEngine.UI.Image>().enabled = false;
            wayPointObject.GetComponent<WayPoint>().target = null;
        }
    }

    IEnumerator HandoverWeapon(Weapon weapon)
    {
        yield return new WaitForSeconds(_showUpTime + _toPlayerTime + _lingerTime);
        _playerTransform.GetComponent<CharacterAttack>().EquipWeapon(weapon, debugText);
    }
}
