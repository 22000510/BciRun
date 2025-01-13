using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class AttackerManager : MonoBehaviour
{   
    public List<GameObject> Attacker = new List<GameObject>();
    public GameObject[] objs;
    public int objCnt = 1;
    private void Awake()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            for (int j = 0; j < objCnt; j++)
            {
                Attacker.Add(CreateObj(objs[i], transform));
            }
        }
    }
    void Start()
    {
        StartCoroutine(CreateAttacker());
    }

     IEnumerator CreateAttacker()
    {
        while (true)
        {
            Attacker[SelectDeactivateAttacker()].SetActive(true);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }
    int SelectDeactivateAttacker()
    {
        List<int> deactiveList = new List<int>();
        for (int i = 0; i < Attacker.Count; i++)
        {
            // 비활성화된 것을 추리기
            if (!Attacker[i].activeSelf)
                deactiveList.Add(i);
        }

        int num = 0;
        if (deactiveList.Count > 0)
            num = deactiveList[Random.Range(0, deactiveList.Count)];

        return num;
    }

    GameObject CreateObj(GameObject obj, Transform parent)
    {
        GameObject copy = Instantiate(obj);
        copy.transform.SetParent(parent);
        copy.SetActive(false);
        return copy;
    }
}
