﻿using Zenject;
using AlphaECS.SurvivalShooter;
using AlphaECS.Unity;
using UnityEngine.UI;
using UniRx;
using AlphaECS;

public class ScoreHUD : ComponentBehaviour {
    [Inject] private Scoring ScoringSystem { get; set; }
    Text ScoreText;

    public override void Initialize(IEventSystem eventSystem) {
        base.Initialize(eventSystem);//-

        ScoreText = GetComponent<Text>();
        ScoringSystem.Score.DistinctUntilChanged().Subscribe(value => {
            ScoreText.text = "Score: " + value.ToString();
        }).AddTo(this);
    }
}