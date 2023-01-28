using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public MoneySettings moneySettings;

    public List<Money> spawnedMoneyPrefabList;
    private void Awake()
    {
        instance = this;
        StartCoroutine(ObjectPool());
    }
    private IEnumerator SpawnIE(MoneyHolder moneyHolder)
    {
        Money money = GetMoney();
        Transform freeTransform = moneyHolder.GetCurrentTransform();
        money.transform.parent = freeTransform;
        money.transform.localPosition = Vector3.zero;
        money.gameObject.SetActive(true);
        yield return null;
    }
    public IEnumerator ObjectPool()
    {
        ResetObjectPool();

        GameObject temp;
        for (int i = 0; i < moneySettings.spawnAmount; i++)
        {
            temp = Instantiate(moneySettings.moneyPrefab, transform);
            temp.SetActive(false);
            spawnedMoneyPrefabList.Add(temp.GetComponent<Money>());
            yield return new WaitForFixedUpdate();
        }
    }
    public Money GetMoney()
    {
        return spawnedMoneyPrefabList.Where(x => !x.gameObject.activeSelf).FirstOrDefault();
    }
    public void ResetObjectPool()
    {
        spawnedMoneyPrefabList.ForEach(x => Destroy(x.gameObject));
        spawnedMoneyPrefabList.Clear();
    }
  
    public void Spawn(MoneyHolder moneyHolder)
    {
        if (!moneyHolder) { moneyHolder = FindObjectOfType<MoneyHolder>(); }
        StartCoroutine(SpawnIE(moneyHolder));
    }
}
