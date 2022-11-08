using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkillSpawn : MonoBehaviour
{

    [SerializeField] int AmountOfSkills;

    [SerializeField] private float r;

    [SerializeField] GameObject skillBallPrefab;

    [SerializeField] private Transform parentTransform;

    [SerializeField] private Transform calibratePos;
    [SerializeField] private GameStateManager Man;

    [SerializeField] List<GameObject> SkillBalls = new List<GameObject>();

    [SerializeField] GameObject centerOfTheStage;

    [SerializeField] float spawnCooldown = 6f;
    [SerializeField] int skillsPerSpawn = 1;
	// Use this for initialization
	void Start ()
    {
        Man = GameStateManager.Manager;
        InvokeRepeating("SkillSpawning", spawnCooldown, spawnCooldown);
    }
	
	// Update is called once per frame
	void Update ()
    {
        ChangeR();
        skillsPerSpawn = GameStateManager.Manager.GetPlayerBunch();
    }

    public GameObject GetCenter()
    {
        return centerOfTheStage;
    }

    public List<GameObject> GetSkillBalls()
    {
        return SkillBalls;
    }

    public int GetAmountOfSkills()
    {
        return AmountOfSkills;
    }

    public void SetAmountOfSkills(int i)
    {
        AmountOfSkills = i;
    }

    private float GetScaleKoef()
    {
        return Man.GetTimeToEnd() / Man.GetFullGameTime();
    }

    Vector3 RandomCircle(Transform cal, float radius)
    {
        float ang = Random.value * 360 * Mathf.Deg2Rad;
        float rad = Random.Range(0, 0.9f*radius);
        Vector3 temp = cal.position + new Vector3(Mathf.Sin(ang), 0f, Mathf.Cos(ang)) * rad;
        return temp;
    }

    void SkillSpawning()
    {
        for (int i = 0; i < skillsPerSpawn; i++)
        {
            GameObject SkillBall = Instantiate(skillBallPrefab, RandomCircle(calibratePos, r), Quaternion.identity);
            SkillBall sk = SkillBall.GetComponent<SkillBall>();
            sk.SetSkillSpawn(gameObject.GetComponent<SkillSpawn>());
            SkillBalls.Add(SkillBall);
        }
    }

    void ChangeR()
    {
        if (r >= 1)
        r = (parentTransform.localScale.x / 2);        
    }

    public void SetSkillPerSpawn(int i)
    {
        skillsPerSpawn = i;
    }


}

