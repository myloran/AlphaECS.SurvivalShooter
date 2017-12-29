using Zenject;
using AlphaECS.SurvivalShooter;
using AlphaECS.Unity;
using UnityEngine.UI;
using UniRx;
using AlphaECS;

public class ScoreHUD : ComponentBehaviour {
    [Inject] private Scoring ScoringSystem { get; set; } //there should be component, not system
    Text ScoreText;

    public override void Initialize() {
        ScoreText = GetComponent<Text>();
        ScoringSystem.Score.DistinctUntilChanged().Subscribe(value => 
            ScoreText.text = "Score: " + value.ToString()).AddTo(this);
    }
}