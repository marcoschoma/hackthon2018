using CrossCutting.Model;
using CrossCutting.Persistence;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.DataAccess
{
    public class ProductRepository : IProductRepository
    {
        private IMongoDatabase _db;

        public ProductRepository(IMongoDatabase db)
        {
            _db = db;
        }

        public void Save(Product product)
        {
            var collection = _db.GetCollection<Product>("Product");
            collection.ReplaceOne(
                filter: new BsonDocument("Sku", product.Sku),
                options: new UpdateOptions { IsUpsert = true },
                replacement: product);
        }
    }
}
