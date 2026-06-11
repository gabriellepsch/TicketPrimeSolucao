-- ============================================
-- Schema TurismoPrime
-- ============================================

DROP TABLE IF EXISTS Reservas;
DROP TABLE IF EXISTS Assentos;
DROP TABLE IF EXISTS Viagens;
DROP TABLE IF EXISTS Cupons;
DROP TABLE IF EXISTS Passageiros;

-- Passageiros (antigo Usuarios)
CREATE TABLE IF NOT EXISTS "Passageiros" (
    "Id" SERIAL PRIMARY KEY,
    "Nome" VARCHAR(255) NOT NULL,
    "Email" VARCHAR(255) UNIQUE NOT NULL,
    "Cpf" VARCHAR(14) UNIQUE NOT NULL,
    "Senha" TEXT NOT NULL,
    "Telefone" VARCHAR(20),
    "Adm" BOOL NOT NULL DEFAULT FALSE,
    "DataCadastro" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Viagens (antigo Eventos)
CREATE TABLE IF NOT EXISTS "Viagens" (
    "Id" SERIAL PRIMARY KEY,
    "Origem" VARCHAR(255) NOT NULL,
    "Destino" VARCHAR(255) NOT NULL,
    "Descricao" TEXT,
    "DataSaida" TIMESTAMP NOT NULL,
    "DataRetorno" TIMESTAMP,
    "TotalAssentos" INT NOT NULL,
    -- "AssentosDisponiveis" removido: calcular via COUNT dos Assentos com Status = 'Disponivel'
    "ValorPassagem" DECIMAL(10,2) NOT NULL,
    "TipoVeiculo" VARCHAR(50) NOT NULL DEFAULT 'Convencional',
    "EmpresaTransporte" VARCHAR(255),
    "FotoUrl" TEXT,
    "Ativo" BOOLEAN DEFAULT TRUE,
    "DataCriacao" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Assentos (NOVO)
-- Status: 'Disponivel', 'Reservado', 'Vendido'
CREATE TABLE IF NOT EXISTS "Assentos" (
    "Id" SERIAL PRIMARY KEY,
    "ViagemId" INT NOT NULL,
    "Numero" INT NOT NULL,
    -- Garante que não haja dois assentos com o mesmo número na mesma viagem
    UNIQUE("ViagemId", "Numero"),
    "Categoria" VARCHAR(20) NOT NULL DEFAULT 'Corredor',
    "Status" VARCHAR(20) NOT NULL DEFAULT 'Disponivel',
    "PrecoExtra" DECIMAL(10,2) DEFAULT 0,
    "ReservaExpiracao" TIMESTAMP,
    CONSTRAINT fk_viagem
        FOREIGN KEY ("ViagemId")
        REFERENCES "Viagens"("Id")
        ON DELETE CASCADE
);

-- Cupons (mantido)
CREATE TABLE IF NOT EXISTS "Cupons" (
    "Codigo" VARCHAR(50) PRIMARY KEY,
    "PorcentagemDesconto" NUMERIC(5,2) NOT NULL CHECK (PorcentagemDesconto BETWEEN 0 AND 100),
    "ValorMinimo" NUMERIC(10,2) NOT NULL CHECK (ValorMinimo >= 0),
    "Ativo" BOOLEAN DEFAULT TRUE,
    "DataCriacao" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Reservas (adaptado)
CREATE TABLE IF NOT EXISTS "Reservas" (
    "Id" SERIAL PRIMARY KEY,
    "PassageiroId" INT NOT NULL,
    "ViagemId" INT NOT NULL,
    "AssentoId" INT NOT NULL,
    "CupomUtilizado" VARCHAR(50),
    "ValorFinalPago" NUMERIC(10,2) NOT NULL CHECK (ValorFinalPago >= 0),
    "Status" VARCHAR(20) DEFAULT 'Confirmada',
    "DataReserva" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_passageiro
        FOREIGN KEY ("PassageiroId")
        REFERENCES "Passageiros"("Id")
        ON DELETE RESTRICT,
    CONSTRAINT fk_viagem_reserva
        FOREIGN KEY ("ViagemId")
        REFERENCES "Viagens"("Id")
        ON DELETE RESTRICT,
    CONSTRAINT fk_assento
        FOREIGN KEY ("AssentoId")
        REFERENCES "Assentos"("Id")
        ON DELETE RESTRICT,
    CONSTRAINT fk_cupom_reserva
        FOREIGN KEY ("CupomUtilizado")
        REFERENCES "Cupons"("Codigo")
        ON DELETE SET NULL
);

-- Índices
CREATE INDEX idx_viagens_destino ON "Viagens"("Destino");
CREATE INDEX idx_viagens_data ON "Viagens"("DataSaida");
CREATE INDEX idx_assentos_viagem ON "Assentos"("ViagemId");
CREATE INDEX idx_reservas_passageiro ON "Reservas"("PassageiroId");
CREATE INDEX idx_reservas_viagem ON "Reservas"("ViagemId");
