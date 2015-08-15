using System.Data.Entity;
using System.Linq;
using Lte.Parameters.Abstract;
using Lte.Parameters.Entities;

namespace Lte.Parameters.Concrete
{
    public class EFBtsRepository : LightWeightRepositroyBase<CdmaBts>, IBtsRepository
    {
        protected override DbSet<CdmaBts> Entities
        {
            get { return context.Btss; }
        }
    }

    public class EFENodebPhotoRepository : IENodebPhotoRepository
    {
        private readonly EFParametersContext context = new EFParametersContext();

        public IQueryable<ENodebPhoto> Photos 
        { 
            get { return context.ENodebPhotos; }
        }

        public void AddOnePhoto(ENodebPhoto photo)
        {
            context.ENodebPhotos.Add(photo);
        }

        public bool RemoveOnePhoto(ENodebPhoto photo)
        {
            return (context.ENodebPhotos.Remove(photo) != null);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
