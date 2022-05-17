using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthDisplay : MonoBehaviour
{
    #region Instance Handling

    private static EnemyHealthDisplay instance;

    public static EnemyHealthDisplay Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    #endregion

    private Transform enemyInfoParent;
    private GameObject enemyHealthTemplate;
    private float initialWidth;

    private Dictionary<string, GameObject> activeHealths;

    private void OnEnable()
    {
        activeHealths = new Dictionary<string, GameObject>();

        enemyInfoParent = transform.Find("EnemyInfo");
        enemyHealthTemplate = enemyInfoParent.Find("EnemyHealthTemplate").gameObject;
        initialWidth = enemyHealthTemplate.transform.Find("Background").Find("HP_Fill").GetComponent<RectTransform>().rect.width;
    }

    public void SetHealth(string _name, float _hp, float _max)
    {
        if (!activeHealths.ContainsKey(_name))
        {
            var newObj = Instantiate(enemyHealthTemplate, enemyInfoParent);

            newObj.transform.Find("BossName").GetComponent<Text>().text = _name;
            var fill = newObj.transform.Find("Background").Find("HP_Fill");
            fill.GetComponent<RectTransform>().sizeDelta = new Vector2(initialWidth * (_hp / _max), fill.GetComponent<RectTransform>().sizeDelta.y);
            newObj.transform.Find("Background").Find("HP_Text").GetComponent<Text>().text = _hp.ToString();
            newObj.SetActive(true);

            activeHealths.Add(_name, newObj);
        }
        else
        {
            var obj = activeHealths[_name];

            obj.transform.Find("BossName").GetComponent<Text>().text = _name;
            var fill = obj.transform.Find("Background").Find("HP_Fill");
            fill.GetComponent<RectTransform>().sizeDelta = new Vector2(initialWidth * (_hp / _max), fill.GetComponent<RectTransform>().sizeDelta.y);
            obj.transform.Find("Background").Find("HP_Text").GetComponent<Text>().text = _hp.ToString();
            obj.SetActive(true);

            if(_hp <= 0f)
            {
                DeleteHealth(_name);
            }
        }
    }

    public void DeleteHealth(string _name)
    {
        StartCoroutine(Delete(_name));
    }

    IEnumerator Delete(string _name)
    {
        yield return new WaitForSeconds(2f);        
        Destroy(activeHealths[_name]);
        activeHealths.Remove(_name);
    }
}
