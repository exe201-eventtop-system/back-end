using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class BaseEntity<T> where T : struct
    {
        [Key, Column("id", TypeName = "uniqueidentifier")]
        public T Id { get; set; }

        // TODO: Fix circular dependency where invoking Equal(object) cause stackoverflow
        /* public static bool operator ==(BaseEntity<T> A, object B)
        {
            if (object.ReferenceEquals(A, B))
            {
                return true;
            }
            var castedB = (BaseEntity<T>) B;

            if ((A == null && castedB != null) || (A != null && castedB == null))
            {
                return false;
            }

            return A.Id.Equals(castedB.Id);
        }

        public static bool operator !=(BaseEntity<T> A,object B)
        {
            if (object.ReferenceEquals(A, B))
            {
                return false;
            }
            var castedB = (BaseEntity<T>)B;

            if ((A == null && castedB != null) && (A != null && castedB == null))
            {
                return true;
            }


            return !A.Id.Equals(castedB.Id);
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }*/

        public override int GetHashCode()
        {
            return Id.GetHashCode(); // Magic numbers, that's all.
        }
    }
}
