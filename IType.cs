using System;
using System.Collections.Generic;
using System.Text;

namespace CriandoORM
{
    public class IType : IConnectionString
    {
        [Table(PrimaryKey ="id")]
        public virtual int Id { get; set; }
    }
}
