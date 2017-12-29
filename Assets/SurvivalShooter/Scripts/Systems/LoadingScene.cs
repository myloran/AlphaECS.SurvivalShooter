using System.Collections;
using AlphaECS.Unity;
using UniRx;
using UnityEngine.SceneManagement;

namespace AlphaECS.SurvivalShooter {
    public class LoadingScene : SystemBehaviour {
        public override void Initialize() {
            EventSystem.OnEvent<LoadScene>().Subscribe(scene => 
                LoadScene(scene.Name)).AddTo(this);
            EventSystem.OnEvent<UnloadScene>().Subscribe(scene => 
                StartCoroutine(UnloadSceneAsync(scene.Name))).AddTo(this);
        }

        void LoadScene(string name) { SceneManager.LoadScene(name); }

        IEnumerator LoadSceneAsync(string name) {
            var asyncOperation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            while (!asyncOperation.isDone) {
                yield return null;
            }
            yield return null;
        }

        IEnumerator UnloadSceneAsync(string name) {
            var isUnloaded = SceneManager.UnloadScene(name);
            while (!isUnloaded) {
                yield return null;
            }
            yield return null;
        }
    }
}