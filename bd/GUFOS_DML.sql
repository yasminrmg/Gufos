USE GufosBD;

INSERT INTO TipoUsuario	(Titulo)
VALUES					('Administrador'),
						('Aluno');


INSERT INTO Usuario (Nome, Email ,Senha, IdTIpoUsuario)
VALUES ('Administrador', 'adm@adm.com','123',1),
		('Ariel','ariel@email.com', '123',2);


INSERT INTO Localizacao
	(CNPJ, RazaoSocial, Endereco)
VALUES 
	('12345678901234', 'Escola SENAI de Informática', 'Al. Barão de Limeira, 539');

INSERT INTO Categoria
	(Titulo,Ativo)
VALUES
	('Desenvolvimento',1),
	('HTML + CSS',1),
	('Marketing',1);

INSERT INTO Evento
	(Titulo,IdCategoria,AcessoLivre,DataEvento,IdLocalizacao)
VALUES
	('C#', 2, 0, '2019-08-07T18:00:00',1),
	('Estrutura Semântica', 2, 1, GETDATE(), 1);

INSERT INTO PresencaEvento
	(IdEvento, IdUsuario, PresencaStatus)
VALUES
	(1,2,'AGUARDANDO'),
	(1,1,'CONFIRMADO')

select* from TipoUsuario
select * from Usuario
select * from Categoria