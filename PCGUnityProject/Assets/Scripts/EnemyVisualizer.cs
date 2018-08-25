using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyVisualizer : MonoBehaviour
{
    [SerializeField]
    private EnemyGenerator _generator;
    [SerializeField]
    private Button _btnNewGen;
    [SerializeField]
    private Text _txtNumGen;
    private Camera _camera;

    public void OnClickNewGen()
    {
        List<EnemySpace> enemyList = _generator.CreateNewWave();
        int i;
        float x, y;
        for (i = 0, x = 0.1f, y = 0.25f; i < enemyList.Count; i++, x += 0.07f)
        {
            enemyList[i].Revive();
            if (x >= 0.8f)
            {
                x = 0.07f;
                y += 0.15f;
            }
            Vector3 position = _camera.ViewportToWorldPoint(new Vector3(x, y));
            position.z = 0f;
            enemyList[i].transform.position = position;
        }

    }

    // Use this for initialization
    void Start()
    {
        _btnNewGen.onClick.AddListener(OnClickNewGen);
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    private void OnGUI()
    {
        _txtNumGen.text = _generator.NumGeneration.ToString();
    }


}
