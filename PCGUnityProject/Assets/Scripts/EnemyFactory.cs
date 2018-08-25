using UnityEngine;

/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "EnemyFactory", menuName = "PCG/Enemy Factory")]
public class EnemyFactory : ScriptableObject
{
    public GameObject prefab;
    public EnemyTemplate[] list;

    public int TemplateSize
    {
        get
        {
            if (list == null)
                return 0;
            return list.Length;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public EnemySpace Create(int type = -1)
    {
        if (type == -1)
        {
            type = Random.Range(0, list.Length - 1);
        }
        GameObject obj = Instantiate(prefab) as GameObject;
        EnemySpace enemy = obj.GetComponent<EnemySpace>();
        enemy.Setup(list[type]);
        obj.AddComponent<BoxCollider2D>();
        return enemy;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="template"></param>
    /// <returns></returns>
    public EnemySpace Create(EnemyTemplate template)
    {
        GameObject obj = Instantiate(prefab);
        EnemySpace enemy = obj.GetComponent<EnemySpace>();
        enemy.Setup(template);
        obj.AddComponent<BoxCollider2D>();
        return enemy;
    }
}

[System.Serializable]
public class EnemyTemplate
{
    public static float TOP_SPEED;
    public static float TOP_HEALTH;
    public Sprite sprite;
    public float initialHealth;
    public float initialSpeed;

    public override string ToString()
    {
        string str = "EnemyTemplate | " + sprite.name;
        return str;
    }
}