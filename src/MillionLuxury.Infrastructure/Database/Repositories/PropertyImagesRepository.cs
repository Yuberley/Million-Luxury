namespace MillionLuxury.Infrastructure.Database.Repositories;

using MillionLuxury.Domain.Properties;
using MillionLuxury.Infrastructure.Repositories;

internal sealed class PropertyImagesRepository : Repository<PropertyImage>, IPropertyImagesRepository
{
    public PropertyImagesRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
