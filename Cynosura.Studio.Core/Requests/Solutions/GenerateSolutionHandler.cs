﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cynosura.Core.Data;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Generator;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class GenerateSolutionHandler : IRequestHandler<GenerateSolution>
    {
        private readonly CodeGenerator _codeGenerator;
        private readonly IEntityRepository<Solution> _solutionRepository;

        public GenerateSolutionHandler(CodeGenerator codeGenerator, 
            IEntityRepository<Solution> solutionRepository)
        {
            _codeGenerator = codeGenerator;
            _solutionRepository = solutionRepository;
        }

        public async Task<Unit> Handle(GenerateSolution request, CancellationToken cancellationToken)
        {
            var solution = await _solutionRepository.GetEntities()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (solution != null)
            {
                var accessor = new SolutionAccessor(solution.Path);
                await _codeGenerator.GenerateSolutionAsync(solution.Path, solution.Name, accessor.Metadata.TemplateName);
            }
            return Unit.Value;
        }

    }
}
