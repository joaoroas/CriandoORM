using CriandoORM;
using System;

namespace UsandoORM
{
    class Program
    {
        static void Main(string[] args)
        {
            var pessoa = new Pessoa();
            pessoa.Name = "João Ricardo";
            pessoa.Endereco = "Rua São Paulo Apostolo";
            pessoa.Save();

            //new Service(pessoa).Save();
        }
    }
}
