-- ============================================================
-- Script DDL — TripPrime (Supabase / PostgreSQL)
-- Domínio: Viagens, Veiculos, Assentos, Passagens, Usuarios, Cupons
-- Ordem: respeitar dependências de FK (sem FK primeiro)
-- ============================================================

-- 1. Tabelas sem dependências de FK

CREATE TABLE IF NOT EXISTS "Usuarios" (
    "Id" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(255) NOT NULL,
    "Email" VARCHAR(255) UNIQUE NOT NULL,
    "Cpf" VARCHAR(11) UNIQUE NOT NULL,
    "Senha" TEXT NOT NULL,
    "Adm" BOOLEAN NOT NULL DEFAULT FALSE
);

CREATE TABLE IF NOT EXISTS "Veiculos" (
    "Id" SERIAL PRIMARY KEY,
    "Modelo" VARCHAR(255) NOT NULL,
    "Placa" VARCHAR(20) UNIQUE NOT NULL,
    "Capacidade" INT NOT NULL,
    "Tipo" VARCHAR(50) NOT NULL,
    "Linhas" INT NOT NULL,
    "Colunas" INT NOT NULL
);

CREATE TABLE IF NOT EXISTS "Cupons" (
    "Id" SERIAL PRIMARY KEY,
    "Codigo" VARCHAR(50) UNIQUE NOT NULL,
    "PercentualDesconto" INT NOT NULL CHECK ("PercentualDesconto" BETWEEN 0 AND 100)
);

-- 2. Tabelas com FK → Veiculos

CREATE TABLE IF NOT EXISTS "Assentos" (
    "Id" SERIAL PRIMARY KEY,
    "VeiculoId" INT NOT NULL,
    "Numero" VARCHAR(10) NOT NULL,
    "Tipo" VARCHAR(50) NOT NULL,
    "Status" VARCHAR(50) NOT NULL DEFAULT 'Disponível',
    CONSTRAINT fk_assentos_veiculo
        FOREIGN KEY ("VeiculoId")
        REFERENCES "Veiculos"("Id")
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    CONSTRAINT unq_assento_veiculo UNIQUE ("VeiculoId", "Numero")
);

CREATE TABLE IF NOT EXISTS "Viagens" (
    "Id" SERIAL PRIMARY KEY,
    "Origem" VARCHAR(255) NOT NULL,
    "Destino" VARCHAR(255) NOT NULL,
    "DataPartida" TIMESTAMP NOT NULL,
    "DataChegada" TIMESTAMP NOT NULL,
    "DataVolta" TIMESTAMP,
    "Descricao" TEXT,
    "VeiculoId" INT NOT NULL,
    "PrecoBase" NUMERIC(10,2) NOT NULL,
    "FotoUrl" TEXT,
    CONSTRAINT fk_viagens_veiculo
        FOREIGN KEY ("VeiculoId")
        REFERENCES "Veiculos"("Id")
        ON DELETE RESTRICT
        ON UPDATE CASCADE
);

-- 3. Tabelas com FK múltiplas

CREATE TABLE IF NOT EXISTS "Passagens" (
    "Id" SERIAL PRIMARY KEY,
    "ViagemId" INT NOT NULL,
    "AssentoId" INT NOT NULL,
    "UsuarioCpf" VARCHAR(11) NOT NULL,
    "PrecoPago" NUMERIC(10,2) NOT NULL CHECK ("PrecoPago" >= 0),
    "CupomUtilizado" VARCHAR(50),
    "Status" VARCHAR(50) NOT NULL DEFAULT 'Ativa',
    "DataCompra" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "DataExpiracaoReserva" TIMESTAMP,
    CONSTRAINT fk_passagens_viagem
        FOREIGN KEY ("ViagemId")
        REFERENCES "Viagens"("Id")
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    CONSTRAINT fk_passagens_assento
        FOREIGN KEY ("AssentoId")
        REFERENCES "Assentos"("Id")
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    CONSTRAINT fk_passagens_usuario
        FOREIGN KEY ("UsuarioCpf")
        REFERENCES "Usuarios"("Cpf")
        ON DELETE RESTRICT
        ON UPDATE CASCADE,
    CONSTRAINT fk_passagens_cupom
        FOREIGN KEY ("CupomUtilizado")
        REFERENCES "Cupons"("Codigo")
        ON DELETE SET NULL
        ON UPDATE CASCADE,
    CONSTRAINT unq_passagem_assento UNIQUE ("AssentoId", "Status")
);

-- 4. Índices para otimizar consultas frequentes

CREATE INDEX IF NOT EXISTS idx_usuarios_cpf ON "Usuarios"("Cpf");
CREATE INDEX IF NOT EXISTS idx_viagens_origem ON "Viagens"("Origem");
CREATE INDEX IF NOT EXISTS idx_viagens_destino ON "Viagens"("Destino");
CREATE INDEX IF NOT EXISTS idx_viagens_data ON "Viagens"("DataPartida");
CREATE INDEX IF NOT EXISTS idx_viagens_veiculo ON "Viagens"("VeiculoId");
CREATE INDEX IF NOT EXISTS idx_assentos_veiculo ON "Assentos"("VeiculoId");
CREATE INDEX IF NOT EXISTS idx_assentos_status ON "Assentos"("Status");
CREATE INDEX IF NOT EXISTS idx_passagens_usuario ON "Passagens"("UsuarioCpf");
CREATE INDEX IF NOT EXISTS idx_passagens_viagem ON "Passagens"("ViagemId");
CREATE INDEX IF NOT EXISTS idx_cupons_codigo ON "Cupons"("Codigo");
