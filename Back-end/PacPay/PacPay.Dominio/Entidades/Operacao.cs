﻿using PacPay.Dominio.Entidades.Enums;
using PacPay.Dominio.Excecoes;
using PacPay.Dominio.Excecoes.Mensagens;
using PacPay.Dominio.Interfaces;

namespace PacPay.Dominio.Entidades
{
    public class Operacao
    {
        public Guid Id { get; private set; }
        public decimal Valor { get; set; }
        public Transacoes TipoOperacao { get; set; }
        public Guid IdContaOrigem { get; set; }
        public Guid IdContaDestino { get; set; }
        public DateTime DataOperacao { get; set; }

        public void Deposito(decimal valor, Conta conta, IRepositorioConta repositorioConta, IRepositorioOperacao repositorioOperacao, ICommitDados commitDados, CancellationToken cancellationToken)
        {
            if (valor <= 0) throw new DominioExcecao(OperacoesErr.ValorInvalido);

            Valor = valor;
            TipoOperacao = Transacoes.Deposito;
            IdContaOrigem = conta.Id;
            IdContaDestino = conta.Id;
            DataOperacao = DateTime.Now.ToUniversalTime();

            conta.Saldo += valor;
            conta.UltimaAtualizacao = DateTime.Now.ToUniversalTime();
            conta.AtualizarConta(repositorioConta);

            repositorioOperacao.Deposito(this);

            commitDados.Commit(cancellationToken);
        }

        public void Saque(decimal valor, Conta conta, IRepositorioConta repositorioConta, IRepositorioOperacao repositorioOperacao, ICommitDados commitDados, CancellationToken cancellationToken)
        {
            if (valor <= 0) throw new DominioExcecao(OperacoesErr.ValorInvalido);
            if (valor > conta.Saldo) throw new DominioExcecao(OperacoesErr.SaldoInsuficiente);

            Valor = valor;
            TipoOperacao = Transacoes.Saque;
            IdContaOrigem = conta.Id;
            IdContaDestino = conta.Id;
            DataOperacao = DateTime.Now.ToUniversalTime();

            conta.Saldo -= valor;
            conta.UltimaAtualizacao = DateTime.Now.ToUniversalTime();
            conta.AtualizarConta(repositorioConta);

            repositorioOperacao.Saque(this);
            commitDados.Commit(cancellationToken);
        }
    }
}