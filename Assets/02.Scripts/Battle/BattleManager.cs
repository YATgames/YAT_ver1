using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class BattleManager : MonoBehaviour
{
    private static BattleManager _instace;

    public static BattleManager instance
    {
        get
        {
            if (_instace == null) return null;
            return _instace;
        }
    }

    private Transform tf_CharacterP; // 캐릭터 파티의 부모객체
    private Transform tf_EnemyP; // 적 파티의 부모객체 ( 인간형 적도 있을 수있음)

    [SerializeField] private List<BaseCharacter> curCharacterList = new List<BaseCharacter>();
    [SerializeField] private List<BaseEnemy> curEnemyList = new List<BaseEnemy>();

    private Transform baseMoveActor;


    // Canvas
    private Slider slider_Enemyhp;
    private float maxSliderValue;

    private Text text_enemysHP;
    private int enemysHP;

    private void Awake()
    {
        if (_instace == null)
        {
            _instace = this;
            //DontDestroyOnLoad(this);
        }

        baseMoveActor = GameObject.Find("BaseMoveActor").GetComponent<Transform>();

        tf_CharacterP = GameObject.Find("party_Character").GetComponent<Transform>();
        tf_EnemyP = GameObject.Find("party_Enemy").GetComponent<Transform>();

        slider_Enemyhp = GameObject.Find("Slider_EnemyHP").GetComponent<Slider>();
        text_enemysHP = GameObject.Find("text_EnemysHP").GetComponent<Text>();


        for (int i = 0; i < tf_EnemyP.childCount; i++)
        {
            curEnemyList.Add(tf_EnemyP.GetChild(i).gameObject.GetComponent<BaseEnemy>());
            enemysHP += curEnemyList[i].Get_enemyData();
        }

        for (int i = 0; i < tf_CharacterP.childCount; i++)
        {
            curCharacterList.Add(tf_CharacterP.GetChild(i).gameObject.GetComponent<BaseCharacter>());
        }

        // Canvas
        canvasInit();

        // TODO : GameManager 에서 정보 가져오기
        GetListObject();
    }

    private void Update()
    {
        // 타겟 설정함수 호출
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (text_enemysHP != null)
            {
            }
            else
            {
                Debug.Log("Null");
            }

            //text_enemysHP.text = enemysHP.ToString();
            //Debug.Log(curEnemyList.Count);
            Debug.Log("List : " + curEnemyList[0].transform.name);
            SetTarget();
        }
    }


    public void BM_Attack(int damage)
    {
        // 현재 설정된 타겟의 hp를 깎음으로써 총 hp 도 낮아지게된다.
        // 그렇게 해야 하나씩 죽어가겠지?
        if (enemysHP > 1)
        {
            enemysHP -= damage;
            text_enemysHP.text = enemysHP.ToString();
        }
        else
        {
            // TODO : 엔딩 진행 슬로우모션 동작시켜야댐
        }
    }

    void canvasInit()
    {
        if (curEnemyList.Count >= 1) // 체력 값 할당 오류 방지
        {
            text_enemysHP.text = enemysHP.ToString();
            slider_Enemyhp.wholeNumbers = true;
            maxSliderValue = enemysHP;
            slider_Enemyhp.maxValue = maxSliderValue;
            slider_Enemyhp.value = maxSliderValue;
        }
    }

    void GetListObject()
    {
        // TODO : 선택씬에서 캐릭터를 선택할 수 있게되면 리스트에서 받아오는 방식으로
        Debug.Log("GetListObject");
        //curCharacterList.AddFirst();
    }

    void SetTarget()
    {
        // TODO : 캐릭터 타겟 설정
        curCharacterList[0].p_Target = curEnemyList[0];
    }
}