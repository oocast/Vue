using UnityEngine;
using System.Collections;
using DG.Tweening;

[System.Serializable]
public class BombSkill : AOESkill {
    public float explodeTime;
    public string bombPrefabName;
    public string rangeCircleName;

    public BombSkill()
    {
        bombPrefabName = "Bomb Prefab";
    }

    GameObject CreateBomb(Transform referenceTransform)
    {
        GameObject bombPrefab = Resources.Load("Prefabs/" + bombPrefabName) as GameObject;
        GameObject bombObject = Object.Instantiate(bombPrefab, referenceTransform.position, Quaternion.identity) as GameObject;
        return bombObject;
    }

    public override void UpdateEffect(Transform referenceTransform = null, GameObject skillCaster = null)
    {
        GameObject bombObject = CreateBomb(referenceTransform);
        Bomb bomb = bombObject.GetComponent<Bomb>();
        bomb.Initialize(this);
        bomb.SetFuse(explodeTime);

        // Show range circle
        ShowRangeCircle(referenceTransform);
    }

    void ShowRangeCircle(Transform referenceTransform)
    {
        if (referenceTransform != null && rangeCircleName.Length > 0)
        {
            GameObject rangeCirclePrefab = Resources.Load("Prefabs/" + rangeCircleName) as GameObject;
            if (rangeCirclePrefab != null)
            {
                float outerRadius = radii[radii.Length - 1];
                GameObject rangeCircleObject = Object.Instantiate(rangeCirclePrefab, referenceTransform.position, referenceTransform.rotation) as GameObject;
                rangeCircleObject.transform.localScale = Vector3.zero;
                rangeCircleObject.transform.DOScale(outerRadius, 0.3f);
                Quaternion rotationEnd = rangeCircleObject.transform.rotation;
                rangeCircleObject.transform.DORotate(rotationEnd.eulerAngles + new Vector3(0, 180f, 0), explodeTime, RotateMode.FastBeyond360).SetDelay(0.3f);
                rangeCircleObject.GetComponentInChildren<SpriteRenderer>().DOFade(0f, 0.3f).SetDelay(explodeTime);
                Object.Destroy(rangeCircleObject, explodeTime + 1f);
            }
        }
    }
}
