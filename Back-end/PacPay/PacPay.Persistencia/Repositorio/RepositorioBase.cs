﻿using Microsoft.EntityFrameworkCore;
using PacPay.Dominio.Entidades;
using PacPay.Dominio.Interfaces;
using PacPay.Infra.Contexto;
using System;

namespace PacPay.Infra.Repositorio
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : EntidadeBase
    {
        protected readonly AppDbContexto Contexto;

        public RepositorioBase(AppDbContexto contexto)
        {
            Contexto = contexto;
        }

        public void Adicionar(T entidade)
        {
            try
            {
                entidade.DataCriacao = DateTime.Now.ToUniversalTime();

                Contexto.Add(entidade.Cliente.Endereco);
                Contexto.Add(entidade.Cliente);
                Contexto.Add(entidade);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Atualizar(T entidade)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ContaEsxiste(string documento, CancellationToken cancellationToken)
        {
            bool existe = await Contexto.Contas.AnyAsync(c => c.Cliente.Documento == documento, cancellationToken);

            return existe;
        }

        public void Excluir(T entidade)
        {
            throw new NotImplementedException();
        }
    }
}