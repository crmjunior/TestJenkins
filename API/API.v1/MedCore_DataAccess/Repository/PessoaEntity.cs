using System.Collections.Generic;
using MedCore_DataAccess.Entidades;
using MedCore_DataAccess.Model;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using MedCore_DataAccess.Util;

namespace MedCore_DataAccess.Repository
{
    public class PessoaEntity : IDataAccess<Pessoa>
    {
		public List<Pessoa> GetPermissions( int roleId )
		{
			using ( var ctx = new DesenvContext() )
			{
				var permissoes = ( from perm in ctx.tblCtrlPanel_AccessControl_Persons_X_Roles
								   where perm.intRoleId == roleId
								   select new Pessoa
								   {
									   ID = ( int )perm.intContactId
								   } ).ToList( );

				return permissoes;
			}

		}

		public List<Pessoa> GetByFilters( Pessoa registro )
		{

			var ctx = new DesenvContext();

			var consulta = from pessoa in ctx.tblPersons
						   where String.IsNullOrEmpty( registro.Register ) || pessoa.txtRegister == registro.Register
						   //join cliente in ctx.tblClients on pessoa.intContactID equals cliente.intClientID
						   select new
						   {
							   pessoa.intContactID,
							   pessoa.txtRegister,
							   pessoa.txtName
						   };

			if ( registro.ID > 0 )
				consulta = consulta.Where( b => b.intContactID == registro.ID );

			/* if (!string.IsNullOrEmpty(registro.Register))
                 consulta = consulta.Where(b => b.txtRegister == registro.Register);*/

			if ( !string.IsNullOrEmpty( registro.Nome ) )
				consulta = consulta.Where( b => b.txtName.Contains( registro.Nome ) );


			var lst = new Pessoas( );

			foreach ( var valor in consulta )
			{
				Pessoa p = new Pessoa( ) { ID = valor.intContactID, Nome = valor.txtName.Trim( ), Register = (valor.txtRegister!=null?valor.txtRegister.Trim( ):valor.txtRegister) };
				lst.Add( p );
			}



			return lst;
		}

		public Pessoa GetDadosByEmail( string email )
		{
			using ( DesenvContext ctx = new DesenvContext() )
			{
				var consulta = ( from p in ctx.tblPersons
								 where p.txtEmail1 == email || p.txtEmail2 == email || p.txtEmail3 == email
								 select new Pessoa
								 {
									 ID = p.intContactID,
									 Nome = p.txtName.Trim( ),
									 Register = p.txtRegister.Trim( )
								 } ).FirstOrDefault( );

				return consulta ?? new Pessoa( );
			}
		}

		public List<Pessoa> GetAll( )
		{
			return GetByFilters( new Pessoa( ) );
		}

		public int Insert( Pessoa registro )
		{
			throw new NotImplementedException( );
		}

		public int Update( Pessoa registro )
		{
			throw new NotImplementedException( );
		}

		public int Delete( Pessoa registro )
		{
			throw new NotImplementedException( );
		}

		public Pessoa.EnumTipoPessoa GetPersonType(string register)
        {
            if (!RedisCacheManager.CannotCache(RedisCacheConstants.Pessoa.KeyGetPersonType))
            {
                var Key = string.Format("{0}:{1}", RedisCacheConstants.Pessoa.KeyGetPersonType, register);

                if (RedisCacheManager.HasAny(Key))
                {
                    return RedisCacheManager.GetItemObject<Pessoa.EnumTipoPessoa>(Key);
                }

                var tipoPessoa = GetPersonTypeDB(register);
                RedisCacheManager.SetItemObject(Key, tipoPessoa, TimeSpan.FromDays(1));

                return tipoPessoa;
            }
            else
            {
                return GetPersonTypeDB(register);
            }
        }

		/// <summary>
		/// Utilizado para saber se é Funcionário(Professor ou Funcionário Recursos) ou Cliente
		/// </summary>
		/// <param name="register">CPF do Usuário</param>
		/// <param name="aplicacao">Qual aplicação utilizada</param>
		/// <returns>TipoPessoa (Enumerador)</returns>
		public Pessoa.EnumTipoPessoa GetPersonTypeDB( string register )
		{
			var ctx = new DesenvContext();
			var person = ( from p in ctx.tblPersons where p.txtRegister == register select p.intContactID ).FirstOrDefault( );
			var professor = ctx.tblEmployee_Sector.Any( a => a.intSectorID == 1 && a.intEmployeeID == person );
			var funcionario = ctx.tblConcurso_Recurso_Funcionarios.Any( a => a.intEmployeeID == person );

			if ( professor )
				return Pessoa.EnumTipoPessoa.Professor;
			else if ( funcionario )
				return Pessoa.EnumTipoPessoa.Funcionario;
			else if ( person != 0 )
				return Pessoa.EnumTipoPessoa.Cliente;
			else
				return Pessoa.EnumTipoPessoa.NaoExiste;
		}

		#region Prospects Normal
		public int SetProspect( Prospect prosp )
		{
			try
			{
				using ( var ctx = new DesenvContext() )
				{
					if ( ctx.tblProspects.Any( p =>
							p.txtEmail.Trim( ).Equals( prosp.Email.Trim( ) ) ||
							p.txtName.Trim( ).Equals( prosp.Nome.Trim( ) )
						)
					)
					{
						return 0;
					}

					var lstId = ctx.tblProspects.Any( ) ? ctx.tblProspects.Max( p => p.intProspectID ) + 1 : 1;

					ctx.tblProspects.Add( new tblProspects
					{
						intProspectID = lstId,
						txtName = prosp.Nome,
						intSex = prosp.Sexo,
						txtZipCode = prosp.Cep,
						txtAddress = prosp.Endereco,
						txtNumero = prosp.EnderecoNumero,
						txtEnderecoReferencia = prosp.EnderecoReferencia,
						txtAddressComplement = prosp.Complemento,
						txtNeighbourhood = prosp.Bairro,
						txtCity = prosp.Cidade,
						intStateId = prosp.IdEstado,
						txtCel = prosp.Telefone,
						txtEmail = prosp.Email,
						txtInstitution = prosp.Instituicao,
						dteCadastro = DateTime.Now
					} );
					ctx.SaveChanges( );

					return 1;
				}
			}
			catch
			{
				throw;
			}
		}

		public int SetRemoverProspect( Prospect prosp )
		{
			try
			{
				using ( var ctx = new DesenvContext() )
				{
					tblProspects entidade = ctx.tblProspects
						.FirstOrDefault( p => p.txtEmail.Trim( ).Equals( prosp.Email.Trim( ) ) );

					if ( entidade != null )
					{
						ctx.tblProspects.Remove( entidade );
					}

					ctx.SaveChanges( );

					return 1;
				}
			}
			catch
			{
				throw;
			}
		}

		public List<Prospect> GetProspects( )
		{
			try
			{
				using ( var ctx = new DesenvContext() )
				{
					var prospects = ( from p in ctx.tblProspects
									  select new Prospect
									  {
										  ID = p.intProspectID,
										  Nome = p.txtName,
										  Sexo = p.intSex,
										  Cep = p.txtZipCode,
										  Endereco = p.txtAddress,
										  EnderecoNumero = p.txtNumero,
										  EnderecoReferencia = p.txtEnderecoReferencia,
										  Complemento = p.txtAddressComplement,
										  Bairro = p.txtNeighbourhood,
										  Cidade = p.txtCity,
										  IdEstado = p.intStateId,
										  Telefone = p.txtCel,
										  Email = p.txtEmail,
										  Instituicao = p.txtInstitution
									  } ).ToList( );

					return prospects;
				}
			}
			catch
			{
				throw;
			}

		}

		public List<Prospect> GetProspectsPorEmail( string email )
		{
			if ( string.IsNullOrWhiteSpace( email ) )
			{
				return null;
			}

			try
			{
				using ( var ctx = new DesenvContext() )
				{
					var prospects = ( from p in ctx.tblProspects
									  where p.txtEmail.Equals( email )
									  select new Prospect
									  {
										  ID = p.intProspectID,
										  Nome = p.txtName,
										  Sexo = p.intSex,
										  Cep = p.txtZipCode,
										  Endereco = p.txtAddress,
										  EnderecoNumero = p.txtNumero,
										  EnderecoReferencia = p.txtEnderecoReferencia,
										  Complemento = p.txtAddressComplement,
										  Bairro = p.txtNeighbourhood,
										  Cidade = p.txtCity,
										  IdEstado = p.intStateId,
										  Telefone = p.txtCel,
										  Email = p.txtEmail,
										  Instituicao = p.txtInstitution
									  } ).ToList( );

					return prospects;
				}
			}
			catch
			{
				throw;
			}
		}

		public List<Prospect> SetProspectsFiltrar( Prospect prosp )
		{
			if ( prosp == null || ( string.IsNullOrWhiteSpace( prosp.Nome ) && string.IsNullOrWhiteSpace( prosp.Email ) ) )
			{
				return null;
			}

			try
			{
				using ( var ctx = new DesenvContext() )
				{
					var prospects = ( from p in ctx.tblProspects
									  where
										(
											prosp.Nome == null ||
											prosp.Nome.Equals( string.Empty ) ||
											p.txtName.Equals( prosp.Nome )
										) &&
										(
											prosp.Email == null ||
											prosp.Email.Equals( string.Empty ) ||
											p.txtEmail.Equals( prosp.Email )
										)
									  select new Prospect
									  {
										  ID = p.intProspectID,
										  Nome = p.txtName,
										  Sexo = p.intSex,
										  Cep = p.txtZipCode,
										  Endereco = p.txtAddress,
										  EnderecoNumero = p.txtNumero,
										  EnderecoReferencia = p.txtEnderecoReferencia,
										  Complemento = p.txtAddressComplement,
										  Bairro = p.txtNeighbourhood,
										  Cidade = p.txtCity,
										  IdEstado = p.intStateId,
										  Telefone = p.txtCel,
										  Email = p.txtEmail,
										  Instituicao = p.txtInstitution
									  } ).ToList( );

					return prospects;
				}
			}
			catch
			{
				throw;
			}
		}
		#endregion

		#region Prospect Adaptamed
		public int SetProspectAdaptamed( ProspectAdaptamed prosp )
		{
			try
			{
				using ( var ctx = new DesenvContext() )
				{
					if ( ctx.tblProspectsAdaptamed.Any( p => p.txtNome.Trim( ) == prosp.Nome.Trim( ) ) )
					{
						return 0;
					}

					var lstId = ctx.tblProspectsAdaptamed.Any( ) ? ctx.tblProspectsAdaptamed.Max( p => p.intProspectID ) + 1 : 1;

					ctx.tblProspectsAdaptamed.Add( new tblProspectsAdaptamed
					{
						intProspectID = lstId,
						txtNome = prosp.Nome,
						txtEmail = prosp.Email,
						txtTelefone = prosp.Telefone,
						txtEndereco = prosp.Endereco,
						txtEnderecoComplemento = prosp.Complemento,
						txtCep = prosp.Cep,
						txtBairro = prosp.Bairro,
						txtCidade = prosp.Cidade,
						intPaisID = prosp.IdPais,
						txtFaculdade = prosp.Faculdade,
						dteCadastro = DateTime.Now,
						intAnoFormatura = prosp.AnoFormatura
					} );

					ctx.SaveChanges( );

					return 1;
				}
			}
			catch
			{
				throw;
			}
		}

		public int SetRemoverProspectAdaptamed( ProspectAdaptamed prosp )
		{
			try
			{
				using ( var ctx = new DesenvContext() )
				{
					tblProspectsAdaptamed entidade = ctx.tblProspectsAdaptamed.FirstOrDefault( p => p.txtNome.Trim( ) == prosp.Nome.Trim( ) );

					if ( entidade != null )
					{
						ctx.tblProspectsAdaptamed.Remove( entidade );
					}

					ctx.SaveChanges( );

					return 1;
				}
			}
			catch
			{
				throw;
			}
		}

		#endregion

		public List<Pessoa> GetAllByNomeOrEmail( Pessoa pessoa )
		{
			var ctx = new DesenvContext();

			var consulta = from p in ctx.tblPersons
						   select new
						   {
							   Id = p.intContactID,
							   Name = p.txtName,
							   Email1 = p.txtEmail1,
							   Email2 = p.txtEmail2,
							   Email3 = p.txtEmail3,
							   Register = p.txtRegister
						   };

			//E-mail like...
			if ( !string.IsNullOrEmpty( pessoa.Email ) )
				consulta = consulta.Where(
					b => b.Email1.Contains( pessoa.Email ) ||
					b.Email2.Contains( pessoa.Email ) ||
					b.Email3.Contains( pessoa.Email )
				);

			//Nome da pessoa identica
			if ( !string.IsNullOrEmpty( pessoa.Nome ) )
				consulta = consulta.Where( b => b.Name.Contains( pessoa.Nome ) );

			var lst = new List<Pessoa>( );

			var listaPessoa = consulta.ToList( );

			foreach ( var valor in listaPessoa )
			{
				Pessoa p = new Pessoa( )
				{
					ID = valor.Id,
					Nome = valor.Name.Trim( ),
					Email = valor.Email1,
					Email2 = valor.Email2,
					Email3 = valor.Email3,
					Register = valor.Register
				};
				lst.Add( p );
			}

			return lst;

		}

		public List<Pessoa> PesquisaPreBlacklist( Pessoa pessoa )
		{
			var ctx = new DesenvContext();

			var consulta = from p in ctx.tblPersons
						   select new
						   {
							   Id = p.intContactID,
							   Name = p.txtName,
							   Email1 = p.txtEmail1,
							   Email2 = p.txtEmail2,
							   Email3 = p.txtEmail3,
							   Register = p.txtRegister
						   };

			//E-mail like...
			if ( !string.IsNullOrEmpty( pessoa.Email ) )
				consulta = consulta.Where(
					b => b.Email1.Contains( pessoa.Email ) ||
					b.Email2.Contains( pessoa.Email ) ||
					b.Email3.Contains( pessoa.Email )
				);

			//Nome da pessoa identica
			if ( !string.IsNullOrEmpty( pessoa.Nome ) )
				consulta = consulta.Where( b => b.Name.Contains( pessoa.Nome ) );

			var lst = new List<Pessoa>( );

			var listaPessoa = consulta.ToList( );

			foreach ( var valor in listaPessoa )
			{
				Pessoa p = new Pessoa( )
				{
					ID = valor.Id,
					Nome = valor.Name.Trim( ),
					Email = valor.Email1,
					Email2 = valor.Email2,
					Email3 = valor.Email3,
					Register = valor.Register
				};
				lst.Add( p );
			}

			return lst;

		}

        public Pessoa GetByClientLogin(string clientLogin)
        {
            using (var ctx = new DesenvContext())
            {
                var consulta = (from p in ctx.tblPersons
                                where p.txtClientLogin == clientLogin
                                select new Pessoa
                                {
                                    ID = p.intContactID,
                                    Nome = p.txtName.Trim(),
                                    Register = p.txtRegister.Trim()
                                }).FirstOrDefault();

                return consulta;
            }
        }      

		public static string GetNomeResumido( int intContactID )
		{
			var ctx = new DesenvContext();

			var consulta = ctx.Set<msp_API_NomeResumido_Result>().FromSqlRaw("msp_API_NomeResumido @intContactID = {0}", intContactID).ToList().FirstOrDefault().txtName;
			return consulta;
		}
    }
}