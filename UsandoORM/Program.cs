using CriandoORM;
using System;
using System.Collections.Generic;

namespace UsandoORM
{
    class Program
    {
        static void Main(string[] args)
        {
            var pessoas = new Service(new Pessoa()).All();
            foreach (Pessoa pessoa in pessoas)
            {
                Console.WriteLine($"Id: {pessoa.Id} Nome: {pessoa.Name} Endereço: {pessoa.Endereco}");
                Console.WriteLine("==================================================");
            }


            //var pessoa1 = new Pessoa();
            //pessoa1.Id = 10;
            //pessoa1.Destroy();
            /*pessoa1.Name = "Pedro paulo";
            pessoa1.Endereco = "Abraão Lauris";
            pessoa1.Save();*/

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
