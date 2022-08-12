using System;
using System.Collections.Generic;
using System.Text;

namespace CriandoORM
{
   public class TableAttribute : Attribute
    {
        public string Name { get; set; }
        public string PrimaryKey { get; set; }
        public string Collection { get; set; }
        public string ForeignKey { get; set; }
        public bool IsNotOnDatabase { get; set; }
    }
}
