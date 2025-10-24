using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class DeleteProductCommand
    {
        public Guid Id { get; set; }
    }

    public class DeleteProductResult
    {
        [JsonPropertyName("old_item_id")]
        public Guid Id { get; set; }

        [JsonPropertyName("removed")]
        public bool Removed { get; set; }
    }

    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, Result<DeleteProductResult>>
    {
        private IUnitOfWork unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        { 
            this.unitOfWork = unitOfWork; 
        }

        public async Task<Result<DeleteProductResult>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.ProductRepository.GetByIdAsync(command.Id);

            if (item == null || item.IsDeleted)
            {
                return Result<DeleteProductResult>.Failure(Error.NotFoundError($"can not find service with id {command.Id}"), "Failed while processing request.");
            }

            item.IsDeleted = true;

            if (await unitOfWork.ProductRepository.Update(item) != null)
            {
                return Result<DeleteProductResult>.Success(new DeleteProductResult { Id = command.Id, Removed = true });
            }

            return Result<DeleteProductResult>.Failure(Error.UnhandledError("Context save failed, please check service detail in database"), "Failed while processing request.");
        }
    }
}
