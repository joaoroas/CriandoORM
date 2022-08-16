using CriandoORM;
using System;

namespace UsandoORM
{
    class Program
    {
        static void Main(string[] args)
        {
            var pessoaNova = new Pessoa() { Id = 4 };
            pessoaNova.Get();
           /* var pessoa = new Pessoa();
            pessoa.Name = "João Ricardo";
            pessoa.Endereco = "Rua São Paulo Apostolo";
            pessoa.Save();


            var pessoa1 = new Pessoa();
            pessoa.Name = "Pedro paulo";
            pessoa.Endereco = "Abraão Lauris";
            pessoa.Save();


            var pessoa2 = new Pessoa();
            pessoa.Name = "Suzana Roas";
            pessoa.Endereco = "Eugênio marquezini";
            pessoa.Save();


            var pessoa3 = new Pessoa();
            pessoa.Name = "Mariza Rebonato";
            pessoa.Endereco = "Rua são jorge";
            pessoa.Save();
           */
            //new Service(pessoa).Save();
        }
    }
}
