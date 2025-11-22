using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace TheFlux.Core.Scripts.Extensions
{
    public static class DoTweenExtensions
    {
        public static async UniTask WithCancellationSafe(this Tween tween, CancellationToken cancellationToken)
        {
            KillTweenImmediatelyWhenTokenIsCanceled(tween, cancellationToken); // the tween is killed 1 frame after the token is canceled, so this prevents it
            await UniTask.WaitUntil(() => !tween.active || tween.IsComplete(), cancellationToken: cancellationToken); 
            cancellationToken.ThrowIfCancellationRequested(); // when the cancellationToken is cancelled the tween stops, BUT there is no throw so we throw afterwards
        }

        private static void KillTweenImmediatelyWhenTokenIsCanceled(this Tween tween, CancellationToken cancellationToken)
        {
            cancellationToken.Register(() =>
            {
                if (tween != null && tween.IsActive())
                {
                    tween.Kill();
                }
            });
        }
    }
}