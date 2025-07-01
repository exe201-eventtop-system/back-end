using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class RemoveProductPackageCommand
    {
        public Guid PackageId { get; set; }
    }

    public class RemoveProductPackageResult
    {
    }

    public class RemoveProductPackageHandler : ICommandHandler<RemoveProductPackageCommand, Result<RemoveProductPackageResult>>
    {
        private IUnitOfWork unitOfWork;

        public RemoveProductPackageHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<RemoveProductPackageResult>> Handle(RemoveProductPackageCommand command, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.PackageRepository.GetByIdAsync(command.PackageId);

            if (item == null)
            {
                return Result<RemoveProductPackageResult>
                    .Failure(Error.NotFoundError($"Can not find service package with id {command.PackageId}"), "Failed while processing request");
            }

            if(await unitOfWork.PackageRepository.Remove(command.PackageId))
            {
                return Result<RemoveProductPackageResult>.Success(new RemoveProductPackageResult(), "Successfully removed service package");
            }

            return Result<RemoveProductPackageResult>
                    .Failure(Error.UnhandledError($"Can not remove service package with id {command.PackageId}"), "Failed while processing request");
        }
    }
}
