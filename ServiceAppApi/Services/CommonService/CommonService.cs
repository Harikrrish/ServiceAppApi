

using ServiceAppApi.Models;
using ServiceAppApi.Repositories.UnitOfWork;

namespace ServiceAppApi.Services.CommonService
{
    public class CommonService : ICommonService
    {
        #region Injectors
        private IRepository<Product> ProductRepository { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }
        #endregion

        #region Constructor
        public CommonService(IRepository<Product> productRepository, IUnitOfWork unitOfWork)
        {
            ProductRepository = productRepository;
            UnitOfWork = unitOfWork;
        }
        #endregion

        #region SaveProduct
        public void SaveProduct(Product product)
        {
            if (product.ProductId > 0)
                ProductRepository.Add(product);
            else
                ProductRepository.Attach(product);

            Commit();
        }
        #endregion

        private void Commit()
        {
            UnitOfWork.Commit();
        }
    }
}
