using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MedCore_DataAccess.Entidades
{
    [DataContract(Name = "CartaoRespostaFiltro", Namespace = "a")]
    public class CartaoRespostaFiltro
    {
        public CartaoRespostaFiltro()
        {
            Estado = new List<CartaoRespostaFiltroItem<string>>();
            Concurso = new List<CartaoRespostaFiltroItem<string>>();
            Ano = new List<CartaoRespostaFiltroItem<int>>();
            Impressas = new CartaoRespostaFiltroItem<string>();
            Favoritas = new CartaoRespostaFiltroItem<string>();
            Anotacoes = new CartaoRespostaFiltroItem<string>();   
            Incorretas = new CartaoRespostaFiltroItem<string>();
            NaoRespondidas = new CartaoRespostaFiltroItem<string>();

        }

        [DataMember(Name = "ExercicioId", EmitDefaultValue = true)]
        public int ExercicioId { get; set; }

        [DataMember(Name = "ClientId", EmitDefaultValue = true)]
        public int ClientId { get; set; }

        [DataMember(Name = "TipoExercicioID", EmitDefaultValue = true)]
        public int TipoExercicioID { get; set; }

        [DataMember(Name = "Estado", EmitDefaultValue = true)]
        public List<CartaoRespostaFiltroItem<string>> Estado { get; set; }

        [DataMember(Name = "Concurso", EmitDefaultValue = true)]
        public List<CartaoRespostaFiltroItem<string>> Concurso { get; set; }

        [DataMember(Name = "Ano", EmitDefaultValue = true)]
        public List<CartaoRespostaFiltroItem<int>> Ano { get; set; }

        [DataMember(Name = "Impressas", EmitDefaultValue = true)]
        public CartaoRespostaFiltroItem<string> Impressas { get; set; }

        [DataMember(Name = "Favoritas", EmitDefaultValue = true)]
        public CartaoRespostaFiltroItem<string> Favoritas { get; set; }

        [DataMember(Name = "Anotacoes", EmitDefaultValue = true)]
        public CartaoRespostaFiltroItem<string> Anotacoes { get; set; }

        [DataMember(Name = "Incorretas", EmitDefaultValue = true)]
        public CartaoRespostaFiltroItem<string> Incorretas { get; set; }

        [DataMember(Name = "NaoRespondidas", EmitDefaultValue = true)]
        public CartaoRespostaFiltroItem<string> NaoRespondidas { get; set; }

        [DataMember(Name = "QuantidadeTotal", EmitDefaultValue = true)]
        public int QuantidadeTotal { get; set; }

        [DataMember(Name = "Questoes", EmitDefaultValue = true)]
        public List<int> Questoes { get; set; }

        [DataMember(Name = "ApostilaId", EmitDefaultValue = true)]
        public int ApostilaId { get; set; }

        [DataMember(Name = "AnoProduto", EmitDefaultValue = true)]
        public int AnoProduto { get; set; }
    }

    [DataContract(Name = "CartaoRespostaFiltroItem", Namespace = "a")]
    public class CartaoRespostaFiltroItem<T>
    {
        public CartaoRespostaFiltroItem()
        {
            Questoes = new List<int>();
        }

        [DataMember(Name = "Item", EmitDefaultValue = true)]
        public T Item { get; set; }

        [DataMember(Name = "Quantidade", EmitDefaultValue = true)]
        public int Quantidade { get; set; }

        [DataMember(Name = "Ativo", EmitDefaultValue = true)]
        public bool Ativo { get; set; }

        [DataMember(Name = "Questoes", EmitDefaultValue = true)]
        public List<int> Questoes { get; set; }
    }
}