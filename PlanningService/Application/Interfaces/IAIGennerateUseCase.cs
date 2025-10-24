using Application.Commons;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAIGennerateUseCase
    {
        Task<Result<Planning>> GenerateScriptAsync(Guid userId,string script);
        Task<Result<string>> GeneratChat( string script);
        Task<Result<Planning>> AcceptPlanning(Guid planningId, Guid userid);
    }
}
