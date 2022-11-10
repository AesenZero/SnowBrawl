using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { Neutral, Shooting, GoingForSkill, GoingToEnemy, GoingToClosestEnemy, GoingAwayFromBorder, Evading }
public class EnemyAI : MonoBehaviour
{


    [SerializeField] GameObject centerOfTheStage;
    [SerializeField] EnemyState state;
    [SerializeField] GameObject compass;
    [SerializeField] EnemyShootingSystem ESS;
    [SerializeField] EnemyMovement EM;
    [SerializeField] EnemyReload ER;
    [SerializeField] EnemySkillChanger ESC;
    [SerializeField] GameObject closestEnemyInSight;
    [SerializeField] GameObject closestSkillInSight;
    [SerializeField] GameObject closestBulletInSight;
    [SerializeField] GameObject target;
    [SerializeField] GameObject enemyLookingAtYou;
    [SerializeField] float rangeAngle;
    [SerializeField] float range;
    [SerializeField] float compassAngle;



    float currentRad { get { return GameStateManager.Manager.GetStageRad(); } }
    float distToCen { get { return Vector3.Distance(transform.position, centerOfTheStage.transform.position); } }
    float stagePanicValue { get { return distToCen / currentRad; } }

    bool skillEnoughPanic { get { return ER.IsSkillValueEnough(); } }

    [SerializeField] float evadingTime;
    [SerializeField] float maxEvadingTime;

    bool oneTimeShoot = false;

    EnemyState lastState;
    // Start is called before the first frame update
    void Start()
    {
        ESS = GetComponent<EnemyShootingSystem>();
        EM = GetComponent<EnemyMovement>();
        ER = GetComponent<EnemyReload>();
        ESC = GetComponent<EnemySkillChanger>();
        centerOfTheStage = GameStateManager.Manager.GetCenterOfStage();
        transform.LookAt(centerOfTheStage.transform);
    }

    // Update is called once per frame
    void Update()
    {

        EvadeEnemySkills(CheckIfMovingEnemySkillInSight());
        closestEnemyInSight = CheckingIfSomethingInSight(getAllPlayers());
        closestSkillInSight = CheckingIfSomethingInSight(getAllSkills(),"Bullet");
        closestBulletInSight = CheckingIfSomethingInSight(getAllSkills(), "SkillBall");
        if (IsSkillUsable(4, 0) == 4)
        {
            Invoke("MineBomb", Random.Range(5f, 15f));
        }
        if (target != null)
        {
            SetTarget(target);
            bool tempBool = IsThisSkillCloserThenEnemy(closestSkillInSight, target);
            if (tempBool && state != EnemyState.GoingAwayFromBorder && !CheckIfSomeoneLookYourWay())
            {
                SetTarget(closestSkillInSight);
                state = EnemyState.GoingForSkill;
            }
        }
        GoAwayFromTheBorder();



        if (state != EnemyState.Shooting && CheckIfSomeoneLookYourWay() && state != EnemyState.GoingAwayFromBorder)
        {
            if (Vector3.Angle(transform.forward, enemyLookingAtYou.transform.position - transform.position) > Vector3.Angle(enemyLookingAtYou.transform.forward, transform.position - enemyLookingAtYou.transform.position))
            {
                SetTarget(enemyLookingAtYou);
                evadingTime = Random.Range(maxEvadingTime / 2f, maxEvadingTime);
                state = EnemyState.Evading;
            }
            else
            {
                SetTarget(enemyLookingAtYou);
                state = EnemyState.GoingToEnemy;
            }

        }

        if (state == EnemyState.GoingAwayFromBorder)
        {
            MoveToTarget(centerOfTheStage, true);
            if (stagePanicValue < 0.8)
            {
                target = null;
                state = lastState;
            }
        }

        switch (state)
        {



            case EnemyState.Neutral:
                {
                    if (closestSkillInSight != null && closestEnemyInSight != null)
                    {
                        if ((skillEnoughPanic || Vector3.Distance(transform.position, closestSkillInSight.transform.position) >= Vector3.Distance(transform.position, closestEnemyInSight.transform.position) - range) && ESS.GetBullets() != 0)
                        {
                            SetTarget(closestEnemyInSight);
                            state = EnemyState.GoingToEnemy;
                            break;
                        }
                        else
                        {
                            SetTarget(closestSkillInSight);
                            state = EnemyState.GoingForSkill;
                            break;
                        }

                    }
                    else if (closestSkillInSight != null && (!skillEnoughPanic))
                    {
                        SetTarget(closestSkillInSight);
                        state = EnemyState.GoingForSkill;
                        break;
                    }
                    else if (closestEnemyInSight != null && ESS.GetBullets() != 0)
                    {
                        SetTarget(closestEnemyInSight);
                        state = EnemyState.GoingToEnemy;
                        break;
                    }
                    else if (ESS.GetBullets() < ESS.GetBulletMax() && closestBulletInSight!=null)
                    {
                        SetTarget(closestBulletInSight);
                        state = EnemyState.GoingForSkill;
                        break;
                    }

                    SetTarget(centerOfTheStage);
                    if (Vector3.Distance(transform.position, centerOfTheStage.transform.position) > 0.5f)
                    {
                        MoveToTarget(centerOfTheStage, true);
                    }
                    else EM.Rotate(1);
                }
                break;

            case EnemyState.Shooting:
                {
                    if (target == null) state = EnemyState.Neutral;
                    if (ESS.GetBullets() == 0)
                    {
                        state = EnemyState.Neutral;
                        break;
                    }
                    if (Mathf.Abs(RotateToTarget(0.2f)) <= 0) ESS.Shoot();
                    if (Vector3.Distance(transform.position, target.transform.position) > range || oneTimeShoot)
                    {
                        oneTimeShoot = false;
                        if (target == null) state = EnemyState.Neutral;
                        else state = EnemyState.GoingToEnemy;
                    }
                }
                break;


            case EnemyState.GoingForSkill:
                {
                    if (target == null)
                    {
                        state = EnemyState.Neutral;
                        break;
                    }
                    MoveToTarget(GetSkillPos(target), true);

                }
                break;

            case EnemyState.GoingToEnemy:
                {
                    if (target == null)
                    {
                        state = EnemyState.Neutral;
                        break;
                    }

                    if (IsAngleRightForEnemy(GetSkillPos(target), target.transform.localScale.x))
                    {
                        bool tempBool = ChooseSkill(target);
                        if (Vector3.Distance(transform.position, target.transform.position) < range || tempBool)
                        {
                            if (tempBool) oneTimeShoot = true;
                            state = EnemyState.Shooting;
                            break;
                        }

                        
                        
                    }
                    MoveToTarget(GetSkillPos(target));
                }
                break;




            case EnemyState.Evading:
                {
                    if (target == null)
                    {
                        state = EnemyState.Neutral;
                        break;
                    }

                    Evading(target);
                    evadingTime -= Time.deltaTime;

                    if (evadingTime <= 0) state = EnemyState.Neutral;

                }
                break;

        }
    }

    List<GameObject> getAllPlayers()
    {
        return GameStateManager.Manager.GetPlayers();
    }

    List<GameObject> getAllSkills()
    {
        return GameStateManager.Manager.GetSpawner().GetSkillBalls();
    }

    GameObject CheckingIfSomethingInSight(List<GameObject> Things, string ignoreTag = "ignore")
    {
        GameObject temp = null;
        float minDist = 10000f;
        float tempF = 0f;
        foreach (GameObject en in Things)
        {
            if (en == null || en == gameObject || en.tag == ignoreTag || en.name == gameObject.name + " Mine") continue;
            Vector3 dirToThing = en.transform.position - transform.position;
            tempF = Vector3.Distance(en.transform.position, transform.position);
            if (Vector3.Angle(transform.forward, dirToThing) < rangeAngle / 2f && tempF < minDist)
            {

                minDist = tempF;
                temp = en;
            }
        }
        return temp;
    }



    BulletSkill CheckIfMovingEnemySkillInSight()
    {
        float minDist = 10000f;
        float tempF = 0f;
        foreach (BulletSkill sk in GameStateManager.Manager.GetShotSkills())
        {
            if (sk.GetInstigator() == this || sk.IsItStatic()) continue;
            Vector3 dirToSkill = GetSkillPos(sk.gameObject) - transform.position;
            tempF = Vector3.Distance(GetSkillPos(sk.gameObject), transform.position);

            if (Vector3.Angle(transform.forward, dirToSkill) < rangeAngle / 2f && tempF < minDist && Vector3.Angle(GetSkillPos(sk.transform.forward), transform.position - GetSkillPos(sk.gameObject)) < 30f)
            {
                return sk;
            }

        }
        return null;
    }

    void EvadeEnemySkills(BulletSkill skill)
    {

        if (skill == null || skill.GetInstigator() == null) return;
        GameObject tempInst = skill.GetInstigator();
        switch (skill.GetKey())
        {
            case 0:
                {
                    if (Vector3.Distance(transform.position, tempInst.transform.position) < range)
                    {
                        SetTarget(tempInst);
                        MoveToTarget(tempInst);
                        return;
                    }
                    else
                    {
                        SetTarget(tempInst);
                        MoveToTarget(tempInst, false, -1);
                        return;
                    }
                }

            case 1:


            case 2:
                {
                    SetTarget(tempInst);
                    evadingTime = Random.Range(maxEvadingTime / 2f, maxEvadingTime);
                    state = EnemyState.Evading;
                    return;
                }

            case 3:
                {
                    EM.ChangeStopper(2f);

                }
                break;
        }
    }

    bool CheckIfSomeoneLookYourWay()
    {
        float minDist = range * 1.4f;
        float tempF = 0f;
        foreach (GameObject en in getAllPlayers())
        {
            if (en == null || en == gameObject) continue;
            Vector3 dirToEnemy = en.transform.position - transform.position;
            tempF = Vector3.Distance(en.transform.position, transform.position);
            if (Vector3.Angle(transform.forward, dirToEnemy) < rangeAngle / 2f && tempF < minDist && Vector3.Angle(en.transform.forward, transform.position - en.transform.position) < 1f)
            {
                enemyLookingAtYou = en;
                return true;
            }


        }
        enemyLookingAtYou = null;
        return false;
    }


    void Evading(GameObject t)
    {
        float tempAng = Vector3.SignedAngle(transform.forward, GetSkillPos(t) - transform.position, transform.up);
        if (Mathf.Abs(tempAng) < 60f)
        {
            EM.Rotate(-Mathf.Sign(tempAng));
        }

        EM.Move(Vector3.forward);
    }


    void GoAwayFromTheBorder()
    {
        if (stagePanicValue >= 0.92f && state != EnemyState.GoingAwayFromBorder)
        {
            lastState = state;
            target = centerOfTheStage;
            state = EnemyState.GoingAwayFromBorder;
        }
    }


    public EnemyState GetState()
    {
        return state;
    }

    int RotateToTarget(float minAng)
    {
        if(target == null)
        {
            state = EnemyState.Neutral;
            return 0;
        }
        compassAngle = Vector3.SignedAngle(transform.forward, GetSkillPos(target) - transform.position, transform.up);
        if (Mathf.Abs(compassAngle) > minAng) EM.Rotate(Mathf.Sign(compassAngle));
        return (int)compassAngle;
    }


    void SetTarget(GameObject g)
    {
        target = g;

    }


    Vector3 GetSkillPos(GameObject g, float f = -0.283f)
    {
        return new Vector3(g.transform.position.x, f, g.transform.position.z);
    }

    Vector3 GetSkillPos(Vector3 g, float f = -0.283f)
    {
        return new Vector3(g.x, f, g.z);
    }

    bool IsThisSkillCloserThenEnemy(GameObject sk, GameObject en)
    {
        if (sk == null || en == null) return false;
        return (Vector3.Distance(transform.position, sk.transform.position) <= Vector3.Distance(transform.position, en.transform.position) - range);
    }

    void MoveToTarget(GameObject t, bool isSkill = false, int coef = 1)
    {
        int angle = (int)Vector3.SignedAngle(transform.forward, t.transform.position - transform.position, transform.up);

        if (Mathf.Abs(angle) <= 1)
        {
            if (Mathf.Abs(RotateToTarget(0.2f)) <= 1 && (Vector3.Distance(transform.position, t.transform.position) > range || isSkill)) EM.Move(coef * Vector3.forward);
        }
        else if (Mathf.Abs(angle) > 1 && Mathf.Abs(angle) <= 45)
        {
            EM.Rotate(Mathf.Sign(angle));
            if (Vector3.Distance(transform.position, t.transform.position) > range || isSkill) EM.Move(coef * Vector3.forward);
        }
        else if (Mathf.Abs(angle) > 45 && Mathf.Abs(angle) <= 180)
        {
            EM.Rotate(Mathf.Sign(angle));
            EM.Move(coef * -Vector3.forward);
        }

    }

    void MoveToTarget(Vector3 t, bool isSkill = false, int coef = 1)
    {
        int angle = (int)Vector3.SignedAngle(transform.forward, t - transform.position, transform.up);

        if (Mathf.Abs(angle) <= 1)
        {
            if (Mathf.Abs(RotateToTarget(0.2f)) <= 1 && (Vector3.Distance(transform.position, t) > range || isSkill)) EM.Move(coef * Vector3.forward);
        }
        else if (Mathf.Abs(angle) > 1 && Mathf.Abs(angle) <= 45)
        {
            EM.Rotate(Mathf.Sign(angle));
            if (Vector3.Distance(transform.position, t) > range || isSkill) EM.Move(coef * Vector3.forward);
        }
        else if (Mathf.Abs(angle) > 45 && Mathf.Abs(angle) <= 180)
        {
            EM.Rotate(Mathf.Sign(angle));
            EM.Move(coef * -Vector3.forward);
        }

    }

    bool IsAngleRightForEnemy(Vector3 t, float scale, float coef = 1)
    {
        int te1 = (int)Vector3.Angle(t - transform.position, transform.forward);
        int te2 = (int)Mathf.Atan((coef * (scale / 2f)) / Vector3.Distance(transform.position, t));
        return te1 <= te2;
    }



    bool ChooseSkill(GameObject t, bool forceSkill = false)
    {
        if ((t == null || t.tag != "Player") && !forceSkill)
        {
            ESC.ChangeSkill(0);
            return false;
        }
        Character_Movement CM = t.GetComponent<Character_Movement>();
        float tempDist = Vector3.Distance(transform.position, t.transform.position);
        if (tempDist < 0.8f * range)
        {
            if (!CM.IsItMoving()) ESC.ChangeSkill(IsSkillUsable(1, IsSkillUsable(3, IsSkillUsable(2, 0))));
            else ESC.ChangeSkill(IsSkillUsable(3, 0));
        }
        else if (tempDist >= 0.8f * range && tempDist <= range)
        {
            if (!CM.IsItMoving()) ESC.ChangeSkill(IsSkillUsable(2, 0));
            else ESC.ChangeSkill(IsSkillUsable(3, IsSkillUsable(2, 0)));
        }
        else if (tempDist > range && tempDist <= GameStateManager.Manager.GetStageRad())
        {
            if (!CM.IsItMoving()) ESC.ChangeSkill(IsSkillUsable(1, 0));
            else ESC.ChangeSkill(IsSkillUsable(3, IsSkillUsable(1, IsSkillUsable(2, 0))));
        }
        else if (tempDist > GameStateManager.Manager.GetStageRad())
        {
            ESC.ChangeSkill(IsSkillUsable(1, IsSkillUsable(3, 0)));
        }

        if (ESS.GetKeyOfBall() == 0) return false;
        else return true;

    }

    int IsSkillUsable(int key, int returnKey)
    {
        if (ESC.GetSkills()[key].IsActive()) return key;
        else return returnKey;
    }

    void MineBomb()
    {
        if (ESS.GetBullets() == 0) return;
        ESC.ChangeSkill(4);
        ESS.Shoot();
    }

}
