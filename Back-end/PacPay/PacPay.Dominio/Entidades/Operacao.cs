﻿namespace PacPay.Dominio.Entidades
{
    public class Operacao
    {
        public Guid Id { get; private set; }
        public decimal Valor { get; set; }

        public Guid IdContaOrigem { get; set; }

        public Guid IdContaDestino { get; set; }
        public DateTimeOffset DataOperacao { get; set; }
    }
}