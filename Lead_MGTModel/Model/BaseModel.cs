using Lead_MGTModel.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lead_MGTModel.Models
{
    public abstract class BaseModel<T>
    {
        public Lead_GWEntities LeadMGT_Entities;
        public BaseModel()
        {
            LeadMGT_Entities = new Lead_GWEntities();
        }
        public abstract List<T> GetAllElement();
        public abstract bool Insert(T obj);
        public abstract bool Update(T obj);
        public abstract bool Delete(Guid Id);

    }
}
