using System;
using Cysharp.Threading.Tasks;
using HKFeedback;
using HKFeedback.Extensions;
using R3;
using UnityEngine;

namespace OneHourGames.SlimeSlash
{
    public class SceneController : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private IFeedback<SceneContext>[] initializeSequences = null!;

        [SerializeReference, SubclassSelector]
        private IFeedback<SceneContext>[] titleSequences = null!;

        [SerializeReference, SubclassSelector]
        private IFeedback<SceneContext>[] gameSequences = null!;

        [SerializeReference, SubclassSelector]
        private IFeedback<SceneContext>[] resultSequences = null!;

        private async UniTaskVoid Start()
        {
            var context = new SceneContext(this);
            await initializeSequences.PlayAsync(context, destroyCancellationToken);

            while (!destroyCancellationToken.IsCancellationRequested)
            {
                await titleSequences.PlayAsync(context, destroyCancellationToken);
                await gameSequences.PlayAsync(context, destroyCancellationToken);
                await resultSequences.PlayAsync(context, destroyCancellationToken);
            }
        }

        [Serializable]
        public class SceneContext : IProvider<SceneContext>
        {
            public SceneController SceneController { get; }

            public ReactiveProperty<float> GameTime { get; } = new(0.0f);

            public SceneContext(SceneController sceneController)
            {
                SceneController = sceneController;
            }

            public SceneContext Provide() => this;
        }
    }
}
