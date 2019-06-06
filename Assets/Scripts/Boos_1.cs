using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boos_1 : LandMonster {

    public float Near = 6.0f;
    public float Close =8.0f;
    public float Far = 12.0f;    

    //技能形态
    protected enum SkillStatus
    {
        Ready,
        Busy,
        Attack,
        DashAttack,
        ProjectFire
    }

    //当前的技能状态
    SkillStatus SkillStatu=SkillStatus.Ready;

    //冲刺攻击位置
    Vector3 dashPosition;

    //火焰喷射状态
    bool isFireExplore = false;


    // Use this for initialization
    void Start () {
        //初始化赋值
        GameManagerObject = GameObject.FindGameObjectsWithTag("GameManager")[0];
        GameManager = GameManagerObject.GetComponent<GameManager>();
        PlayerObject = GameManager.Player;
        TargetPlayer = PlayerObject.GetComponent<Player>();
        DoorObject = GameManager.Door;
        Door = DoorObject.GetComponent<Door>();

        /*自身属性赋值*/
        MonsterAnimator = GetComponent<Animator>();
        MonsterTransform = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {
        switch (SkillStatu)
        {
            case SkillStatus.Ready:
                {
                    //如果当前没有施放技能，则根据当前状态选择技能
                    float distance = caculateTheDistance();
                    //Debug.Log("dis:" + distance);
                    if (distance < Near)
                    {
                        //就在Player附近
                        float choose = Random.Range(0.0f, 1.0f);
                        if (choose < 0.2f)
                        {
                            //使用喷射火焰
                            projectFire();
                        }
                        else
                        {
                            //普通攻击
                            chaseAndAttackPlayer();
                        }
                    }
                    else if (distance < Close)
                    {
                        //中等距离
                        float choose = Random.Range(0.0f, 1.0f);
                        if (choose < 0.5f)
                        {
                            //使用喷射火焰
                            projectFire();
                        }
                        else
                        {
                            //冲刺攻击
                            dashAttack();
                        }
                    }
                    else if (distance < Far)
                    {
                        //较远距离
                        float choose = Random.Range(0.0f, 1.0f);
                        if (choose < 0.6f)
                        {
                            //喷射火焰
                            dashAttack();
                        }
                        else
                        {
                            //使用喷射火焰
                            projectFire();
                        }
                    }
                    else
                    {
                        //非常遥远
                        float choose = Random.Range(0.0f, 1.0f);
                        //Debug.Log("choose:" + choose);
                        if (choose < 0.9f)
                        {
                            //冲刺攻击
                            dashAttack();
                        }
                        else
                        {
                            //使用喷射火焰
                            projectFire();
                        }
                    }
                    break;
                }
            case SkillStatus.Busy:
                {
                    chaseAndAttackPlayer();
                    break;
                }
            case SkillStatus.DashAttack:
                {
                    dashAttack();
                    break;
                }
            case SkillStatus.Attack:
                {
                    attack();
                    break;
                }
            case SkillStatus.ProjectFire:
                {
                    projectFire();
                    break;
                }
        }



    }

    /*计算与玩家的直线距离*/
    protected float caculateTheDistance()
    {
        Vector3 PlayerPosition = PlayerObject.GetComponent<Transform>().position;

        float xDistance = MonsterTransform.position.x - PlayerPosition.x;
        float yDistance = MonsterTransform.position.z - PlayerPosition.z;

        float distance = Mathf.Sqrt(xDistance * xDistance + yDistance * yDistance);

        return distance;
    }

    /*普通攻击*/
    protected void attack()
    {
        if(SkillStatu!=SkillStatus.Attack)
        {
            //开始攻击
            SkillStatu = SkillStatus.Attack;
            //Debug.Log("attack!"); //执行攻击
            MonsterAnimator.SetTrigger("SwordAttack");            
            
        }
        else
        {
            //正在攻击
            AnimatorClipInfo[] animatorClipInfos = MonsterAnimator.GetCurrentAnimatorClipInfo(0);
            if (animatorClipInfos[0].clip.name=="Idel")
            {
                //已经攻击完毕
                //Debug.Log("攻击完毕！");
                SkillStatu = SkillStatus.Busy;
                StartCoroutine(recoverFromSkill(3));
            }
            else
            {

            }
        }
    }

    /*冲刺爪击*/
    protected void dashAttack()
    {
        SkillStatu = SkillStatus.DashAttack;
        //Debug.Log("dashAttack!"); //执行冲刺、攻击
        SkillStatu = SkillStatus.Busy;
        StartCoroutine(recoverFromSkill(5));
    }

    /*喷射火焰*/
    protected void projectFire()
    {
        SkillStatu = SkillStatus.ProjectFire;
        //Debug.Log("projectFire!"); //执行动画，产生发射物
        SkillStatu = SkillStatus.Busy;
        StartCoroutine(recoverFromSkill(4));
    }

    protected IEnumerator recoverFromSkill(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SkillStatu = SkillStatus.Ready;
    }

}
