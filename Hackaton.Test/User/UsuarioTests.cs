using Bogus;
using Hackaton.API.DTO.Inputs.Usuario;
using Hackaton.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Test.Usuario
{
    public class UsuarioTests
    {
        private readonly Faker _faker;

        public UsuarioTests()
        {
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void Should_Register_Infos_Not_Be_Null()
        {
            // Arrange
            var usuario = new CadastrarInput();

            // Act
            usuario.Nome = _faker.Person.FirstName;
            usuario.Email = _faker.Person.Email;
            usuario.Senha = string.Empty;

            // Assert
            Assert.NotNull(usuario.Senha);
        }
    }
}
