using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using HKFeedback;
using HKFeedback.Providers;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OneHourGames.SlimeSlash
{
    [Serializable]
    public sealed class GameLogicAsync<TContext> : IFeedback<TContext> where TContext : IProvider<SceneController.SceneContext>
    {
        [SerializeReference, SubclassSelector]
        private IProvider<float> gameTime = new Constant<float>(0.0f);

        [SerializeReference, SubclassSelector]
        private IProvider<float> mousePositionUpdateInterval = new Constant<float>(1.0f);

        [SerializeReference, SubclassSelector]
        private IProvider<SlashController> slashControllerPrefab = new Constant<SlashController>(null);

        [SerializeReference, SubclassSelector]
        private IProvider<Camera> worldCamera = new Constant<Camera>(null);

        private Vector2 oldMousePosition;

        public UniTask PlayAsync(TContext context, CancellationToken cancellationToken)
        {
            var sceneContext = context.Provide();
            sceneContext.GameTime.Value = gameTime.Provide();
            oldMousePosition = Mouse.current.position.ReadValue();
            Observable.Interval(TimeSpan.FromSeconds(mousePositionUpdateInterval.Provide()), cancellationToken: cancellationToken)
                .Subscribe(this, static (_, @this) =>
                {
                    var newMousePosition = Mouse.current.position.ReadValue();
                    var screenPosition = new Vector3(newMousePosition.x, newMousePosition.y, 1.0f);
                    var worldPosition = @this.worldCamera.Provide().ScreenToWorldPoint(screenPosition);
                    var slashController = @this.slashControllerPrefab.Provide().Spawn(newMousePosition, @this.oldMousePosition);
                    slashController.transform.localPosition = worldPosition;
                    @this.oldMousePosition = newMousePosition;
                })
                .RegisterTo(cancellationToken);

            return UniTask.Never(cancellationToken);
        }
    }
}
