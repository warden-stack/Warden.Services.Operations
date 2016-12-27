using System;
using System.Threading.Tasks;
using Warden.Common.Types;
using Warden.Services.Operations.Domain;
using Warden.Services.Operations.Repositories;

namespace Warden.Services.Operations.Services
{
    public class OperationService : IOperationService
    {
        private readonly IOperationRepository _operationRepository;

        public OperationService(IOperationRepository operationRepository)
        {
            _operationRepository = operationRepository;
        }

        public async Task<Maybe<Operation>> GetAsync(Guid requestId)
            => await _operationRepository.GetAsync(requestId);

        public async Task CreateAsync(Guid requestId, string name, string userId, string origin, string resource,
            DateTime createdAt)
        {
            var operation = new Operation(requestId, name, userId, origin, resource, createdAt);
            await _operationRepository.AddAsync(operation);
        }

        public async Task ProcessAsync(Guid requestId)
            => await UpdateAsync(requestId, x => x.Process());

        public async Task RejectAsync(Guid requestId, string code, string message)
            => await UpdateAsync(requestId, x => x.Reject(code, message));

        public async Task CompleteAsync(Guid requestId, string message = null)
            => await UpdateAsync(requestId, x => x.Complete(message));

        private async Task UpdateAsync(Guid requestId, Action<Operation> update)
        {
            var operation = await _operationRepository.GetAsync(requestId);
            if (operation.HasNoValue)
                return;

            update(operation.Value);
            await _operationRepository.UpdateAsync(operation.Value);
        }
    }
}