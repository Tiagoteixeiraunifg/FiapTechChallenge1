﻿using Biblioteca.Infraestrutura.Ferramentas.Extensoes;
using Biblioteca.Infraestrutura.Seguranca.JWT.Interfaces;
using Biblioteca.Negocio.Entidades.Usuarios;
using Biblioteca.Negocio.Enumeradores.Usuarios;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Biblioteca.Infraestrutura.Seguranca.JWT.Servico
{
    public class ServicoDeToken : IServicoDeToken
    {
        private readonly IConfiguration _configuracao;
        private readonly ILogger<ServicoDeToken> _logger;

        public ServicoDeToken(IConfiguration configuracao, ILogger<ServicoDeToken> logger)
        {
            _configuracao = configuracao;
            _logger = logger;
        }

        public string GerarToken(Usuario usuario)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var Secretkey = Encoding.ASCII.GetBytes(_configuracao.GetSection("Secret").Value);
            var TokenDescriptor = new SecurityTokenDescriptor()
            {
                //dados do usuario passados no token
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("Nome", usuario.Nome),
                    new Claim(ClaimTypes.Role, usuario.Permissao.ToString()),
                    new Claim("Id", usuario.Id.ToString())
                }),
                //validade do token
                Expires = DateTime.Now.AddHours(8),

                //credencial do token (chave secreta)
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Secretkey),
                    SecurityAlgorithms.HmacSha256Signature)

            };

            SecurityToken TokenNovo;

            try
            {
                TokenNovo = TokenHandler.CreateToken(TokenDescriptor);
                return TokenHandler.WriteToken(TokenNovo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar Token", ex.Message);
                return "erro";
            }

        }

        public Usuario ObtenhaUsuarioDoToken(string token)
        {
            token = token.Substring(7);

            var TokenHandler = new JwtSecurityTokenHandler();
            var Secretkey = Encoding.ASCII.GetBytes(_configuracao.GetSection("Secret").Value);

            SecurityToken tokenInformado = TokenHandler.ReadToken(token);


            //var tokenvalidationparameters = new TokenValidationParameters()
            //{
            //    IssuerSigningKey = new SigningCredentials(
            //        new SymmetricSecurityKey(Secretkey),
            //        SecurityAlgorithms.HmacSha256Signature).Key,
            //};


            //var ret = TokenHandler.ValidateToken(token, tokenvalidationparameters, out var tokenValidado);
            var user = new Usuario();

            if (tokenInformado.PossuiValor()) 
            {
               

                foreach (var item in ((JwtSecurityToken)tokenInformado).Claims)
                {
                    if (item.Type.Equals("Nome")) 
                    {
                        user.Nome = item.Value.ToString();
                    }
                    if (item.Type.Equals("Id")) 
                    {
                        user.Id = int.Parse(item.Value.ToString());
                    }
                }

                return user;
            }

            return user;
        }

        public string RenoveToken(string token)
        {

            string tokenNovo = "";
            var TokenHandler = new JwtSecurityTokenHandler();
            var Secretkey = Encoding.ASCII.GetBytes(_configuracao.GetSection("Secret").Value);            
            var tokenvalidationparameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Secretkey),
                ValidateIssuer = false,
                ValidateAudience = false
                
            };
            
            var ret = TokenHandler.ValidateToken(token, tokenvalidationparameters, out var tokenValidado);

            Usuario usuario = new Usuario();

            if (ret.PossuiValor()) 
            {
                foreach (var claim in ret.Claims)
                {
                    if (claim.Type.Equals("Nome")) 
                    {
                        usuario.Nome = claim.Value.ToString();
                    }
                    if (claim.Type.Contains("/identity/claims/role"))
                    {  
                        foreach ( UsuarioPermissaoEnum tipo in Enum.GetValues(typeof(UsuarioPermissaoEnum)))
                        {
                            if (tipo.ToString().Equals(claim.Value.ToString())) 
                            {
                                usuario.Permissao = tipo;
                            }
                        }
                    }
                    if (claim.Type.Equals("Id"))
                    {
                        usuario.Id = int.Parse(claim.Value.ToString());
                    }
                }

                tokenNovo = GerarToken(usuario);
            }
           
            return tokenNovo;
        }

        public bool ValidarToken(string token)
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var Secretkey = Encoding.ASCII.GetBytes(_configuracao.GetSection("Secret").Value);
            
            SecurityToken tokenInformado = TokenHandler.ReadToken(token);

            
            var tokenvalidationparameters = new TokenValidationParameters()
            {
               IssuerSigningKey = new SigningCredentials(
                    new SymmetricSecurityKey(Secretkey),
                    SecurityAlgorithms.HmacSha256Signature).Key,
            };

          
            var ret = TokenHandler.ValidateToken(token, tokenvalidationparameters, out var tokenValidado);


            return ret.Identity.IsAuthenticated;
        }
    }
}
