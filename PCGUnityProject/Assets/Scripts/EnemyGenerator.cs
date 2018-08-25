using UnityEngine;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private EnemyFactory _factory;
    [Space]
    [Header("Evolutionary Algorithm Settings")]
    [SerializeField, Range(1, 50)]
    private int _mu = 10;
    [SerializeField, Range(1, 50)]
    private int _lambda = 10;
    [SerializeField, Range(1, 10)]
    private int _maxGenerations = 5;
    private int _numGeneration = 0;
    private List<EnemySpace> _enemyList;

    public int NumGeneration
    {
        get { return _numGeneration; }
    }

    /// <summary>
    /// 
    /// </summary>
    public List<EnemySpace> CreateNewWave()
    {
        if (_numGeneration >= _maxGenerations)
            return new List<EnemySpace>();
        
        if (_numGeneration == 0)
            _enemyList = CreateInitialWave(_mu, _lambda, _factory);
        else
            _enemyList = CreateEvolvedWave(_mu, _lambda, _enemyList, _factory);

        _numGeneration += 1;
        return _enemyList;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private List<EnemySpace> CreateInitialWave(int mu, int lambda, EnemyFactory factory)
    {
        int poolSize = mu + lambda;
        List<EnemySpace> wave = new List<EnemySpace>(poolSize);
        int templateSize = factory.TemplateSize;
        if (templateSize == 0)
            return wave;

        for (int i = 0; i < poolSize; i++)
        {
            int type = i % templateSize;
            EnemySpace e = _factory.Create(type);
            wave.Add(e);
        }
        wave = Shuffle(wave);
        return wave;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mu"></param>
    /// <param name="lambda"></param>
    /// <param name="list"></param>
    private List<EnemySpace> CreateEvolvedWave(int mu, int lambda, List<EnemySpace> list, EnemyFactory factory)
    {
        list.Sort(); // fitness function
        list.RemoveRange(0, lambda);
        list = Shuffle(list);
        int poolSize = mu + lambda;
        bool isRandom = lambda > mu;
        List<EnemySpace> wave = new List<EnemySpace>(poolSize);
        EnemySpace e;
        EnemyTemplate t;
        int index;

        for (int i = 0; i < lambda; i++)
        {
            if (isRandom)
                index = Random.Range(0, list.Count - 1);
            else
                index = i % list.Count;

            t = list[index].Template;
            e = factory.Create(t);
            e.Mutate();
            wave.Add(e);
        }

        wave.AddRange(list);
        return wave;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Init()
    {
        _numGeneration = 0;
        if (_enemyList == null)
            return;
        foreach (EnemySpace e in _enemyList)
        {
            Destroy(e.gameObject);
        }
        _enemyList.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public List<EnemySpace> Shuffle(List<EnemySpace> list)
    {
        List<EnemySpace> shuffledList = new List<EnemySpace>(list.Count);
        int r;
        EnemySpace e;
        while (list.Count != 0)
        {
            r = Random.Range(0, list.Count - 1);
            e = list[r];
            shuffledList.Add(e);
            list.RemoveAt(r);
        }
        return shuffledList;
    }

    private void Awake()
    {
        _enemyList = new List<EnemySpace>();
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
