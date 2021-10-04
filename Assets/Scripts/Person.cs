using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Person : MonoBehaviour
{
    public List<Questable> RelianceList;
    public List<Questable> ImportantRelianceList;
    private float _annoyance;
    public float _maxAnnoyance;
    public float AnnoyanceRate;
    public float AnnoyanceDecreaseRate;
    public Slider AnnoyanceBar;
    public List<Sprite> PeopleSkins;

    void Start()
    {
        AnnoyanceBar.maxValue = _maxAnnoyance;
        AnnoyanceBar.minValue = 0;
        AnnoyanceBar.value = 0;
        GetComponent<SpriteRenderer>().sprite = PeopleSkins[Random.Range(0, PeopleSkins.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        bool annoyed = ShouldIBeAnnoyed();
        if (annoyed)
        {
            _annoyance += AnnoyanceRate * Time.deltaTime;

            if (_annoyance > _maxAnnoyance)
            {
                AnnoyanceBar.value = _maxAnnoyance;
                GameManager.Instance.GameLose(this);
            }
            else
            {
                AnnoyanceBar.gameObject.SetActive(true);     
            }

            AnnoyanceBar.value = _annoyance;
        }
        else if (!annoyed && _annoyance != 0) {
            _annoyance -= AnnoyanceDecreaseRate * Time.deltaTime;
            if (_annoyance < 0)
            {
                _annoyance = 0;
                AnnoyanceBar.gameObject.SetActive(false);
            }
            AnnoyanceBar.value = _annoyance;
        }
    }

    bool ShouldIBeAnnoyed() {
        foreach (Questable Q in ImportantRelianceList)
        {
            if (Q.IsActive) {
                return true;
            }
        }

        foreach (Questable Q in RelianceList)
        {
            if (!Q.IsActive) {
                return false;
            }
        }
        return true;
    }
}
