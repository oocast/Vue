using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EnvironmentColor : MonoBehaviour
{
    Material _groundGlow;
    Material _groundGlowCopy;
    Color _groundInitial;
    MeshRenderer[] renderers;

    void Awake()
    {
        GameObject enemyManager = GameObject.FindGameObjectWithTag("Enemy Generator");
        if (enemyManager != null)
        {
            enemyManager.GetComponent<EnemyManager>().onEnemySpawn += UpdateGroundColor;
        }
    }

    void Start()
    {
        _groundInitial = new Color(0, 1, 178 / 255f);
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    void UpdateGroundColor(int enemyCount)
    {
        foreach (MeshRenderer renderer in renderers)
        {
            //renderer.materials[1].color = Color.Lerp(_groundInitial, Color.red, enemyCount / 30f);
            // Use DOTween to smooth
            int glowMaterialIndex = 0;
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                if (renderer.materials[i].name.Contains("ground_glow"))
                {
                    glowMaterialIndex = i;
                    break;
                }
            }
            renderer.materials[glowMaterialIndex].DOColor(Color.Lerp(_groundInitial, Color.red, enemyCount / 30f), 1f);

        }
    }
}
