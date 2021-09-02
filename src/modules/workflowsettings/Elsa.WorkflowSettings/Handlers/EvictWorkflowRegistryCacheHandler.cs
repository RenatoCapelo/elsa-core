using System.Threading;
using System.Threading.Tasks;
using Elsa.Caching;
using Elsa.Decorators;
using Elsa.Services;
using Elsa.WorkflowSettings.Abstractions.Events;
using MediatR;

namespace Elsa.WorkflowSettings.Handlers
{
    public class EvictWorkflowRegistryCacheHandler :
        INotificationHandler<WorkflowSettingsSaved>,
        INotificationHandler<ManyWorkflowSettingsSaved>,
        INotificationHandler<WorkflowSettingsDeleted>,
        INotificationHandler<ManyWorkflowSettingsDeleted>
    {
        private readonly ITriggerIndexer _triggerIndexer;
        private readonly ICacheSignal _cacheSignal;

        public EvictWorkflowRegistryCacheHandler(ITriggerIndexer triggerIndexer, ICacheSignal cacheSignal)
        {
            _triggerIndexer = triggerIndexer;
            _cacheSignal = cacheSignal;
        }

        public async Task Handle(WorkflowSettingsSaved notification, CancellationToken cancellationToken)
        {
            await HandleWorkflowCacheAndTriggerIndexer(cancellationToken);
        }

        public async Task Handle(ManyWorkflowSettingsSaved notification, CancellationToken cancellationToken)
        {
            await HandleWorkflowCacheAndTriggerIndexer(cancellationToken);
        }

        public async Task Handle(WorkflowSettingsDeleted notification, CancellationToken cancellationToken)
        {
            await HandleWorkflowCacheAndTriggerIndexer(cancellationToken);
        }

        public async Task Handle(ManyWorkflowSettingsDeleted notification, CancellationToken cancellationToken)
        {
            await HandleWorkflowCacheAndTriggerIndexer(cancellationToken);
        }

        private async Task HandleWorkflowCacheAndTriggerIndexer(CancellationToken cancellationToken)
        {
            await _cacheSignal.TriggerTokenAsync(CachingWorkflowRegistry.CacheKey);
            await _triggerIndexer.IndexTriggersAsync(cancellationToken);
        }
    }
}