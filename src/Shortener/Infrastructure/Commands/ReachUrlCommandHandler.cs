using MediatR;
using Microsoft.EntityFrameworkCore;
using Shortener.Infrastructure.Context;

namespace Shortener.Infrastructure.Commands
{
    public class ReachUrlDto
    {
        public string Message { get; set; } = string.Empty;
    }

    public class ReachUrlCommand : IRequest<ReachUrlDto>
    {
        public string ShortPath { get; set; }
        public ReachUrlCommand(string shortPath)
        {
            ShortPath = shortPath;
        }
    }

    public class ReachUrlCommandHandler : IRequestHandler<ReachUrlCommand, ReachUrlDto>
    {
        private readonly AppDbContext dbContext;
        public ReachUrlCommandHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<ReachUrlDto> Handle(ReachUrlCommand request, CancellationToken cancellationToken)
        {
            var url = await dbContext.Urls.FirstOrDefaultAsync(p => p.ShortPath == request.ShortPath);
            if (url == null)
            {
                return new() { Message = string.Empty };
            }
            url.RequestCounter++;
            url.LastRequestedDate = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
            return new() { Message = url.LongUrl };
        }
    }
}
