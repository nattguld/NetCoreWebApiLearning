using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models.Contexts
{
    public class PornhubVideoCommentContext : DbContext
    {
        public PornhubVideoCommentContext(DbContextOptions<PornhubVideoCommentContext> options)
            : base(options) {
        }

        public DbSet<PornhubVideoCommentItem> PornhubVideoCommentItems { get; set; }
    }
}
