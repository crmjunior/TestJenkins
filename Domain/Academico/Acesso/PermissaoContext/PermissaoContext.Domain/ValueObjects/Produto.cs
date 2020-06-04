using PermissaoContext.Domain.Enums;

namespace PermissaoContext.Domain.ValueObjects
{
    public class Produto
    {


        public Produto(ECurso tipoProduto, int ano)
        {
            Curso = tipoProduto;
            Ano = ano;
        }

        public ECurso Curso { get; set; }
        public int Ano { get; set; }
    }
}