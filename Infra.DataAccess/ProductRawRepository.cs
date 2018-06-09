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
    public class ProductRawRepository : IRepository<ProductRawData>
    {
        private IMongoDatabase _db;
        protected string CollectionName = "ProductRaw";

        public ProductRawRepository(IMongoDatabase db)
        {
            _db = db;
        }

        public void Save(ProductRawData product)
        {
            var collection = _db.GetCollection<ProductRawData>(CollectionName);
            collection.ReplaceOne(
                filter: new BsonDocument {
                    { "Sku", product.Sku },
                    { "Vendor", product.Vendor },
                    { "Platform", product.Platform }
                },
                options: new UpdateOptions { IsUpsert = true },
                replacement: product);
        }
    }
}
