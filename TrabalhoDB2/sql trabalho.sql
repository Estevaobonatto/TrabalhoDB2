--execute a criação de tabela uma por vez

-- Criação do banco de dados
CREATE DATABASE SalaoAppBanco;

-- Selecionar o banco de dados para uso
USE SalaoAppBanco;

-- Criação da tabela cidade
CREATE TABLE cidade (
    id INT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    uf CHAR(2) NOT NULL
);

-- Criação da tabela cliente
CREATE TABLE cliente (
    id INT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    cpf CHAR(11) NOT NULL,
    data_nascimento DATE NOT NULL,
    cidade_id INT,
    FOREIGN KEY (cidade_id) REFERENCES cidade(id)
);

-- Criação da tabela servico
CREATE TABLE servico (
    id INT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    preco DECIMAL(10, 2) NOT NULL,
    duracao TIME NOT NULL
);
go

-- Criação da tabela funcionario
CREATE TABLE funcionario (
    id INT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    cpf CHAR(11) NOT NULL,
    data_nascimento DATE NOT NULL,
    cidade_id INT,
    FOREIGN KEY (cidade_id) REFERENCES cidade(id)
);

CREATE TABLE horario (
    id INT PRIMARY KEY,
    data_agendamento DATE NOT NULL,
    horario TIME NOT NULL,
    cliente_id INT,
    servico_id INT,
    funcionario_id INT,
    FOREIGN KEY (cliente_id) REFERENCES cliente(id),
    FOREIGN KEY (servico_id) REFERENCES servico(id),
    FOREIGN KEY (funcionario_id) REFERENCES funcionario(id)
);

INSERT INTO cidade (id, nome, uf) VALUES (1, 'São Paulo', 'SP');
INSERT INTO cidade (id, nome, uf) VALUES (2, 'Rio de Janeiro', 'RJ');
INSERT INTO cidade (id, nome, uf) VALUES (3, 'Belo Horizonte', 'MG');
INSERT INTO cidade (id, nome, uf) VALUES (4, 'Curitiba', 'PR');
INSERT INTO cidade (id, nome, uf) VALUES (5, 'Salvador', 'BA');

INSERT INTO cliente (id, nome, cpf, data_nascimento, cidade_id) VALUES (1, 'João Silva', '12345678901', '1985-04-12', 1);
INSERT INTO cliente (id, nome, cpf, data_nascimento, cidade_id) VALUES (2, 'Maria Oliveira', '23456789012', '1990-06-23', 2);
INSERT INTO cliente (id, nome, cpf, data_nascimento, cidade_id) VALUES (3, 'Carlos Souza', '34567890123', '1988-09-15', 3);
INSERT INTO cliente (id, nome, cpf, data_nascimento, cidade_id) VALUES (4, 'Ana Pereira', '45678901234', '1992-12-08', 4);
INSERT INTO cliente (id, nome, cpf, data_nascimento, cidade_id) VALUES (5, 'Lucas Santos', '56789012345', '1980-02-20', 5);


INSERT INTO servico (id, nome, preco, duracao) VALUES (1, 'Corte de Cabelo', 30.00, '00:30:00');
INSERT INTO servico (id, nome, preco, duracao) VALUES (2, 'Manicure', 20.00, '00:45:00');
INSERT INTO servico (id, nome, preco, duracao) VALUES (3, 'Pedicure', 25.00, '01:00:00');
INSERT INTO servico (id, nome, preco, duracao) VALUES (4, 'Massagem', 50.00, '01:30:00');
INSERT INTO servico (id, nome, preco, duracao) VALUES (5, 'Depilação', 40.00, '01:00:00');

INSERT INTO funcionario (id, nome, cpf, data_nascimento, cidade_id) VALUES (1, 'Pedro Ramos', '67890123456', '1985-03-15', 1);
INSERT INTO funcionario (id, nome, cpf, data_nascimento, cidade_id) VALUES (2, 'Carla Lima', '78901234567', '1990-08-22', 2);
INSERT INTO funcionario (id, nome, cpf, data_nascimento, cidade_id) VALUES (3, 'Rafael Alves', '89012345678', '1988-07-30', 3);
INSERT INTO funcionario (id, nome, cpf, data_nascimento, cidade_id) VALUES (4, 'Fernanda Costa', '90123456789', '1992-05-10', 4);
INSERT INTO funcionario (id, nome, cpf, data_nascimento, cidade_id) VALUES (5, 'Paula Rocha', '01234567890', '1980-11-25', 5);

INSERT INTO horario (id, data_agendamento, horario, cliente_id, servico_id, funcionario_id) VALUES (1, '2024-07-05', '10:00:00', 1, 1, 1);
INSERT INTO horario (id, data_agendamento, horario, cliente_id, servico_id, funcionario_id) VALUES (2, '2024-07-06', '11:00:00', 2, 2, 2);
INSERT INTO horario (id, data_agendamento, horario, cliente_id, servico_id, funcionario_id) VALUES (3, '2024-07-07', '12:00:00', 3, 3, 3);
INSERT INTO horario (id, data_agendamento, horario, cliente_id, servico_id, funcionario_id) VALUES (4, '2024-07-08', '13:00:00', 4, 4, 4);
INSERT INTO horario (id, data_agendamento, horario, cliente_id, servico_id, funcionario_id) VALUES (5, '2024-07-09', '14:00:00', 5, 5, 5);
INSERT INTO horario (id, data_agendamento, horario, cliente_id, servico_id, funcionario_id) VALUES (6, '2024-07-05', '15:00:00', 1, 2, 1);
INSERT INTO horario (id, data_agendamento, horario, cliente_id, servico_id, funcionario_id) VALUES (7, '2024-07-06', '16:00:00', 2, 3, 2);
INSERT INTO horario (id, data_agendamento, horario, cliente_id, servico_id, funcionario_id) VALUES (8, '2024-07-07', '17:00:00', 3, 4, 3);
INSERT INTO horario (id, data_agendamento, horario, cliente_id, servico_id, funcionario_id) VALUES (9, '2024-07-08', '18:00:00', 4, 5, 4);
INSERT INTO horario (id, data_agendamento, horario, cliente_id, servico_id, funcionario_id) VALUES (10, '2024-07-09', '19:00:00', 5, 1, 5);

-- Stored Procedure para inserir cliente
CREATE PROCEDURE sp_InserirCliente
    @ID INT,
    @NOME VARCHAR(255),
    @CPF CHAR(11),
    @DATA_NASCIMENTO DATE,
    @CIDADE_ID INT
AS
BEGIN
    INSERT INTO cliente (id, nome, cpf, data_nascimento, cidade_id)
    VALUES (@ID, @NOME, @CPF, @DATA_NASCIMENTO, @CIDADE_ID);
END;
GO

-- Stored Procedure para atualizar cliente
CREATE PROCEDURE sp_AtualizarCliente
    @ID INT,
    @NOME VARCHAR(255),
    @CPF CHAR(11),
    @DATA_NASCIMENTO DATE,
    @CIDADE_ID INT
AS
BEGIN
    UPDATE cliente
    SET nome = @NOME, cpf = @CPF, data_nascimento = @DATA_NASCIMENTO, cidade_id = @CIDADE_ID
    WHERE id = @ID;
END;
GO

-- Stored Procedure para excluir cliente
CREATE PROCEDURE sp_ExcluirCliente
    @ID INT
AS
BEGIN
    DELETE FROM cliente WHERE id = @ID;
END;
GO

-- Stored Procedure para consultar cliente por ID
CREATE PROCEDURE sp_ConsultarCliente
    @ID INT
AS
BEGIN
    SELECT id, nome, cpf, data_nascimento, cidade_id
    FROM cliente
    WHERE id = @ID;
END;
GO

-- Stored Procedure para carregar todos os clientes
CREATE PROCEDURE sp_CarregarClientes
AS
BEGIN
    SELECT c.id, c.nome, c.cpf, c.data_nascimento, c.cidade_id, ci.nome AS cidade_nome
    FROM cliente c
    JOIN cidade ci ON c.cidade_id = ci.id;
END;
GO

CREATE PROCEDURE sp_InserirFuncionario
    @ID INT,
    @NOME NVARCHAR(100),
    @CPF NVARCHAR(11),
    @DATA_NASCIMENTO DATE,
    @CIDADE_ID INT
AS
BEGIN
    INSERT INTO funcionario (id, nome, cpf, data_nascimento, cidade_id)
    VALUES (@ID, @NOME, @CPF, @DATA_NASCIMENTO, @CIDADE_ID)
END
GO

CREATE PROCEDURE sp_AtualizarFuncionario
    @ID INT,
    @NOME NVARCHAR(100),
    @CPF NVARCHAR(11),
    @DATA_NASCIMENTO DATE,
    @CIDADE_ID INT
AS
BEGIN
    UPDATE funcionario
    SET nome = @NOME, cpf = @CPF, data_nascimento = @DATA_NASCIMENTO, cidade_id = @CIDADE_ID
    WHERE id = @ID
END
GO

CREATE PROCEDURE sp_ExcluirFuncionario
    @ID INT
AS
BEGIN
    DELETE FROM funcionario WHERE id = @ID
END
GO

CREATE PROCEDURE sp_ConsultarFuncionario
    @ID INT
AS
BEGIN
    SELECT id, nome, cpf, data_nascimento, cidade_id
    FROM funcionario
    WHERE id = @ID
END
GO

CREATE PROCEDURE sp_CarregarFuncionarios
AS
BEGIN
    SELECT f.id, f.nome, f.cpf, f.data_nascimento, f.cidade_id, ci.nome AS cidade_nome
    FROM funcionario f
    JOIN cidade ci ON f.cidade_id = ci.id
END
GO

-- Stored Procedure para Inserir Horário
CREATE PROCEDURE sp_InserirHorario
    @DATA_AGENDAMENTO DATETIME,
    @HORARIO TIME,
    @CLIENTE_ID INT,
    @SERVICO_ID INT,
    @FUNCIONARIO_ID INT
AS
BEGIN
    INSERT INTO horario (data_agendamento, horario, cliente_id, servico_id, funcionario_id)
    VALUES (@DATA_AGENDAMENTO, @HORARIO, @CLIENTE_ID, @SERVICO_ID, @FUNCIONARIO_ID);
END
GO

-- Stored Procedure para Atualizar Horário
CREATE PROCEDURE sp_AtualizarHorario
    @ID INT,
    @DATA_AGENDAMENTO DATETIME,
    @HORARIO TIME,
    @CLIENTE_ID INT,
    @SERVICO_ID INT,
    @FUNCIONARIO_ID INT
AS
BEGIN
    UPDATE horario
    SET data_agendamento = @DATA_AGENDAMENTO,
        horario = @HORARIO,
        cliente_id = @CLIENTE_ID,
        servico_id = @SERVICO_ID,
        funcionario_id = @FUNCIONARIO_ID
    WHERE id = @ID;
END
GO

-- Stored Procedure para Excluir Horário
CREATE PROCEDURE sp_ExcluirHorario
    @ID INT
AS
BEGIN
    DELETE FROM horario WHERE id = @ID;
END
GO

-- Stored Procedure para Consultar Horário
CREATE PROCEDURE sp_ConsultarHorario
    @ID INT
AS
BEGIN
    SELECT id, data_agendamento, horario, cliente_id, servico_id, funcionario_id
    FROM horario
    WHERE id = @ID;
END
GO

-- Stored Procedure para Inserir Produto
CREATE PROCEDURE sp_InserirProduto
    @NOME NVARCHAR(100),
    @PRECO DECIMAL(18, 2)
AS
BEGIN
    INSERT INTO servico (nome, preco)
    VALUES (@NOME, @PRECO);
END
GO

-- Stored Procedure para Atualizar Produto
CREATE PROCEDURE sp_AtualizarProduto
    @ID INT,
    @NOME NVARCHAR(100),
    @PRECO DECIMAL(18, 2)
AS
BEGIN
    UPDATE servico
    SET nome = @NOME,
        preco = @PRECO
    WHERE id = @ID;
END
GO

-- Stored Procedure para Excluir Produto
CREATE PROCEDURE sp_ExcluirProduto
    @ID INT
AS
BEGIN
    DELETE FROM horario WHERE servico_id = @ID;
    DELETE FROM servico WHERE id = @ID;
END
GO

-- Stored Procedure para Consultar Produto
CREATE PROCEDURE sp_ConsultarProduto
    @ID INT
AS
BEGIN
    SELECT id, nome, preco
    FROM servico
    WHERE id = @ID;
END
GO

-- Criar novo trigger na tabela cliente
CREATE TRIGGER trg_VerificarFormatoCPFEDateCliente
ON cliente
FOR INSERT, UPDATE
AS
BEGIN
    -- Verifica se o CPF tem exatamente 11 dígitos numéricos
    IF EXISTS (SELECT 1 FROM inserted WHERE LEN(cpf) != 11 OR cpf NOT LIKE '%[^0-9]%')
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR ('CPF inválido. Deve ter exatamente 11 dígitos numéricos.', 16, 1);
        RETURN;
    END

    -- Verifica se a data de nascimento não é maior que 01/01/2017
    IF EXISTS (SELECT 1 FROM inserted WHERE data_nascimento > '2017-01-01')
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR ('Data de nascimento inválida. Deve ser menor ou igual a 01/01/2017.', 16, 1);
        RETURN;
    END
END;
GO

-- Criar novo trigger na tabela funcionario
CREATE TRIGGER trg_VerificarFormatoCPFEDateFuncionario
ON funcionario
FOR INSERT, UPDATE
AS
BEGIN
    -- Verifica se o CPF tem exatamente 11 dígitos numéricos
    IF EXISTS (SELECT 1 FROM inserted WHERE LEN(cpf) != 11 OR cpf NOT LIKE '%[^0-9]%')
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR ('CPF inválido. Deve ter exatamente 11 dígitos numéricos.', 16, 1);
        RETURN;
    END

    -- Verifica se a data de nascimento não é maior que 01/01/2017
    IF EXISTS (SELECT 1 FROM inserted WHERE data_nascimento > '2017-01-01')
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR ('Data de nascimento inválida. Deve ser menor ou igual a 01/01/2017.', 16, 1);
        RETURN;
    END
END;
GO

