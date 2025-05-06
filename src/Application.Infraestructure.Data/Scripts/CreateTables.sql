IF OBJECT_ID('dbo.StatusProduto', 'U') IS NULL
	CREATE TABLE StatusProduto(
		Codigo INT NOT NULL CONSTRAINT PK_StatusProduto PRIMARY KEY,
		Descricao VARCHAR(150) NOT NULL
	)


IF NOT EXISTS(SELECT 1 FROM StatusProduto WHERE Codigo = 0)
	INSERT INTO StatusProduto (Codigo, Descricao) VALUES(0, 'Disponível')

IF NOT EXISTS(SELECT 1 FROM StatusProduto WHERE Codigo = 1)
	INSERT INTO StatusProduto (Codigo, Descricao) VALUES(1, 'Fora de estoque')

IF OBJECT_ID('dbo.CategoriaProduto', 'U') IS NULL
	CREATE TABLE CategoriaProduto(
		Id INT IDENTITY(1,1) CONSTRAINT PK_CategoriaProduto PRIMARY KEY,
		Descricao VARCHAR(150) NOT NULL
	)

IF NOT EXISTS(SELECT 1 FROM CategoriaProduto WHERE Descricao = 'Bebidas')
	INSERT INTO CategoriaProduto (Descricao) VALUES('Bebidas')

IF NOT EXISTS(SELECT 1 FROM CategoriaProduto WHERE Descricao = 'Frios e Laticínios')
	INSERT INTO CategoriaProduto (Descricao) VALUES('Frios e Laticínios')

IF NOT EXISTS(SELECT 1 FROM CategoriaProduto WHERE Descricao = 'Higiene e Beleza')
	INSERT INTO CategoriaProduto (Descricao) VALUES('Higiene e Beleza')

IF NOT EXISTS(SELECT 1 FROM CategoriaProduto WHERE Descricao = 'Limpeza')
	INSERT INTO CategoriaProduto (Descricao) VALUES('Limpeza')

IF NOT EXISTS(SELECT 1 FROM CategoriaProduto WHERE Descricao = 'Padaria e Confeitaria')
	INSERT INTO CategoriaProduto (Descricao) VALUES('Padaria e Confeitaria')

IF NOT EXISTS(SELECT 1 FROM CategoriaProduto WHERE Descricao = 'Carnes e Aves')
	INSERT INTO CategoriaProduto (Descricao) VALUES('Carnes e Aves')

IF NOT EXISTS(SELECT 1 FROM CategoriaProduto WHERE Descricao = 'Frutas, Legumes e Verduras')
	INSERT INTO CategoriaProduto (Descricao) VALUES('Frutas, Legumes e Verduras')

IF NOT EXISTS(SELECT 1 FROM CategoriaProduto WHERE Descricao = 'Congelados')
	INSERT INTO CategoriaProduto (Descricao) VALUES('Congelados')

IF NOT EXISTS(SELECT 1 FROM CategoriaProduto WHERE Descricao = 'Mercearia')
	INSERT INTO CategoriaProduto (Descricao) VALUES('Mercearia')

IF NOT EXISTS(SELECT 1 FROM CategoriaProduto WHERE Descricao = 'Pet Shop')
	INSERT INTO CategoriaProduto (Descricao) VALUES('Pet Shop')


IF OBJECT_ID('dbo.Produto', 'U') IS NULL
	CREATE TABLE Produto(
		Id INT IDENTITY(1,1) CONSTRAINT PK_Produto PRIMARY KEY,
		Nome VARCHAR(250) NOT NULL,
		CodigoStatusProduto INT NOT NULL
			CONSTRAINT FK_Produto_CodigoStatusProduto_StatusProduto_Codigo
			FOREIGN KEY(CodigoStatusProduto)
			REFERENCES StatusProduto (Codigo)
			CONSTRAINT DF_Produto_StatusProduto DEFAULT(0),
		IdCategoria INT NOT NULL
			CONSTRAINT FK_Produto_IdCategoria_CategoriaProduto_Id
			FOREIGN KEY(IdCategoria)
			REFERENCES CategoriaProduto (Id),

		Valor DECIMAL(15,2) NOT NULL,
		QuantidadeEstoque INT NOT NULL,

		DataCadastro DATETIME NOT NULL CONSTRAINT DF_Produto_DataCriacao DEFAULT(SYSDATETIME()),
		Descricao VARCHAR(800) NULL,
		DataUltimaVenda DATETIME NULL
	)

IF OBJECT_ID('dbo.Usuario', 'U') IS NULL
	CREATE TABLE Usuario(
		Id INT IDENTITY(1,1) CONSTRAINT PK_Usuario PRIMARY KEY,
		Nome VARCHAR(250) NOT NULL,
		Email VARCHAR(250) NOT NULL,
		Senha VARCHAR(250) NOT NULL,
		DataCadastro DATETIME NOT NULL CONSTRAINT DF_Usuario_DataCriacao DEFAULT(SYSDATETIME())
	)