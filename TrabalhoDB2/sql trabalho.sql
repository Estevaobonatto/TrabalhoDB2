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
