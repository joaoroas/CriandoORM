using CriandoORM;
using System;
using System.Collections.Generic;
using System.Text;

namespace UsandoORM
{
    class Pessoa : CType
    {
        [Table(IsNotOnDatabase = true)]
        public override string ConnectionString => @"Server=localhost\SQLEXPRESS;Database=Desafio21Dias;Trusted_Connection=True";

        public string Name { get; set; }
        public string Endereco { get; set; }
    }
}
