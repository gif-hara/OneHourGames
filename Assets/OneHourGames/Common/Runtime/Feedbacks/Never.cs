using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using HKFeedback;

namespace OneHourGames.Common
{
    [Serializable]
    public class Never<TContext> : IFeedback<TContext>
    {
        public UniTask PlayAsync(TContext context, CancellationToken cancellationToken)
        {
            return UniTask.Never(cancellationToken);
        }
    }
}
