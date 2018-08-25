using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class EnemySpace : MonoBehaviour, System.IComparable<EnemySpace>
{
    private SpriteRenderer _renderer;
    private BoxCollider2D _collider;
    private EnemyTemplate _template;
    [SerializeField]
    private float _health;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _timer;
    [SerializeField]
    private Transform _target;



    /// <summary>
    /// 
    /// </summary>
    public EnemyTemplate Template
    {
        get { return _template; }
    }

    public float TimeAlive
    {
        get { return _timer; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="template"></param>
    public void Setup(EnemyTemplate template)
    {
        //Debug.Log(template);
        _renderer = GetComponent<SpriteRenderer>();
        //_collider = GetComponent<BoxCollider2D>();

        _template = template;
        _renderer.sprite = template.sprite;
        _health = template.initialHealth;
        _speed = template.initialSpeed;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Mutate()
    {

    }

    public void Revive()
    {
        _timer = 0f;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Kill()
    {
        gameObject.SetActive(false);
    }
    

    private void Awake()
    {
        _timer = 0f;
        Kill();
    }

    // Use this for initialization
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_health <= 0f)
            Kill();
        if (_target == null)
            return;

        Vector3 direction = _target.position - transform.position;
        direction.Normalize();
        transform.LookAt(_target);
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnMouseDown()
    {
        Kill();
    }

    public int CompareTo(EnemySpace obj)
    {
        return (int)(_timer - obj.TimeAlive);
    }
}
