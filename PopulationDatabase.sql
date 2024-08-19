-- Assicurati di usare il database creato
use PosteItalianeDatabase;


-- Inserire un utente
insert into UTENTE (CF, Nome, Cognome, Ind_Via, Ind_Civico, Ind_Residenza, Ind_Provincia)
values ('ABCDGD32G35H567I', 'Gianmarco', 'Di maria', 'Via Franco', '9', 'Milano', 'MI');

-- Inserire un account associato all'utente
insert into ACCOUNT (CF,Amministratore, BloccoAccount, Email, Password)
values ('ABCDGD32G35H567I', 1, 0, 'admin@admin.com', 'admin');

-- Inserire un utente
insert into UTENTE (CF, Nome, Cognome, Ind_Via, Ind_Civico, Ind_Residenza, Ind_Provincia)
values ('ABCDEF12G34H567I', 'Mario', 'Rossi', 'Via Roma', '10', 'Milano', 'MI');
INSERT INTO UTENTE (CF, Nome, Cognome, Ind_Via, Ind_Civico, Ind_Residenza, Ind_Provincia) VALUES
('RSSMRA85M01H501Z', 'Mario', 'Peppino', 'Via Roma', '10', 'Roma', 'RM'),
('VRDLGI85S01H501Y', 'Luigi', 'Verdi', 'Corso Italia', '20', 'Milano', 'MI'),
('BNCLRA85M01H501X', 'Laura', 'Bianchi', 'Via Garibaldi', '5', 'Napoli', 'NA'),
('PLMLGU85M01H501W', 'Giorgio', 'Palmi', 'Via Dante', '15', 'Torino', 'TO'),
('MNCLGI85S01H501V', 'Gianni', 'Mancini', 'Via Mazzini', '30', 'Firenze', 'FI');
-- Inserire un account associato all'utente
insert into ACCOUNT (CF, Amministratore, BloccoAccount, Email, Password)
values ('ABCDEF12G34H567I',false, 0, 'mario.rossi@example.com', 'password123');
INSERT INTO ACCOUNT (CF, Amministratore, BloccoAccount, Email, Password) VALUES
('RSSMRA85M01H501Z',false, FALSE, 'mario.rossi@example.com', 'password123'),
('VRDLGI85S01H501Y',false, FALSE, 'luigi.verdi@example.com', 'password123'),
('BNCLRA85M01H501X',false, FALSE, 'laura.bianchi@example.com', 'password123'),
('PLMLGU85M01H501W',false, FALSE, 'giorgio.palmi@example.com', 'password123'),
('MNCLGI85S01H501V',false, FALSE, 'gianni.mancini@example.com', 'password123');
-- Verificare che i dati siano stati inseriti correttamente
select * from ACCOUNT;

INSERT INTO CARTA (BloccoCarta, NumeroIdentificativo, Ccv, Pin, Scadenza, Saldo, Iban, Tipo, CF)
VALUES (0, '1234567890123456', 123, 1234, '2029-12-31', 10000.00, 'IT60X0542811101000000123456', 'BancoPosta', 'ABCDEF12G34H567I'),
		(0, '1234567890121478', 321, 1873, '2025-12-31', 150.00, 'IT60X0542811101000000123457', 'PostePay', 'ABCDEF12G34H567I');

INSERT INTO TRANSAZIONE (CodTransazione, Importo, Data, NumeroIdentificativo)
VALUES ('T1', 50.00, '2023-01-01 10:30:00', '1234567890123456'),
       ('T2', 25.75, '2023-01-15 14:45:00', '1234567890123456'),
       ('T3', 100.00, '2023-02-01 09:00:00', '1234567890123456'),
       ('T4', 85.00, '2022-01-10 10:30:00', '1234567890123456'),
       ('T5', 35.00, '2024-08-15 10:30:00', '1234567890123456');

INSERT INTO TIPO_TRANSAZIONE (CodTransazione, Tipo, Commissione, TipologiaPagamento, Ente, IbanDestinatario, Causale, NumeroIdentificativo, Id)
VALUES ('T1', 'Pagamento', null, 'online', 'Amazon', null, null, '1234567890123456', null),
       ('T2', 'Pagamento', null, 'online', 'Ebay', null, null, '1234567890123456', null),
       ('T3', 'Pagamento', null, 'online', 'Subito', null, null, '1234567890123456', null),
       ('T4', 'Deposito', 2, null, null, null, null, '1234567890123456', null),
       ('T5', 'Pagamento', null, 'carburante', 'GEP Carburanti', null, null, '1234567890123456', null);
       
INSERT INTO TRANSAZIONE (CodTransazione, Importo, Data, NumeroIdentificativo)
VALUES 
    ('T6', 75.50, '2024-08-05 11:30:00', '1234567890123456'),
    ('T7', 150.00, '2024-08-10 13:15:00', '1234567890123456'),
    ('T8', 200.00, '2024-08-20 16:45:00', '1234567890123456');

INSERT INTO TIPO_TRANSAZIONE (CodTransazione, Tipo, Commissione, TipologiaPagamento, Ente, IbanDestinatario, Causale, NumeroIdentificativo, Id)
VALUES 
    ('T6', 'Pagamento', null, 'online', 'Zalando', null, null, '1234567890123456', null),
    ('T7', 'Pagamento', null, 'POS', 'Ikea', null, null, '1234567890123456', null),
    ('T8', 'Bonifico', 1, null, 'Enel', 'IT60X0542811101000000123456', 'Pagamento bolletta', '1234567890123456', null);


INSERT INTO TRANSAZIONE (CodTransazione, Importo, Data, NumeroIdentificativo)
VALUES
    ('T9', 100.00, '2024-08-01 10:30:00', '1234567890123456'),
    ('T10', 50.00, '2024-08-15 14:00:00', '1234567890123456'),
    ('T11', 25.00, '2024-08-20 09:45:00', '1234567890123456');
    
INSERT INTO BANCOMAT (Id, Commissione, Tipo, Via, Comune, Provincia, Civico, Regione)
VALUES (1001, 1, 'PostaMat', null, null, null, null, null);

INSERT INTO BANCOMAT (Id, Commissione, Tipo, Via, Comune, Provincia, Civico, Regione)
VALUES (1002, 1, 'PostaMat', null, null, null, null, null);

INSERT INTO BANCOMAT (Id, Commissione, Tipo, Via, Comune, Provincia, Civico, Regione)
VALUES (1003, 1.0, 'PostaMat', null, null, null, null, null);

INSERT INTO BANCOMAT (Id, Commissione, Tipo, Via, Comune, Provincia, Civico, Regione)
VALUES (1004, 1, 'PostaMat', null, null, null, null, null);

INSERT INTO BANCOMAT (Id, Commissione, Tipo, Via, Comune, Provincia, Civico, Regione)
VALUES (1005, 1, 'PostaMat', null, null, null, null, null);
    
INSERT INTO TIPO_TRANSAZIONE (CodTransazione, Tipo, Commissione, TipologiaPagamento, Ente, IbanDestinatario, Causale, NumeroIdentificativo, Id)
VALUES
    ('T9', 'prelievo', 1.00, null, null, null, null, '1234567890123456', 1001),
    ('T10', 'prelievo', 1.00, null, null, null, null, '1234567890123456', 1002),
    ('T11', 'Prelievo', 1.00, null, null, null, null, '1234567890123456', 1003);
SELECT TRANSAZIONE.*,TIPO_TRANSAZIONE.* 
FROM
    TRANSAZIONE
INNER JOIN
    TIPO_TRANSAZIONE ON TRANSAZIONE.CodTransazione = TIPO_TRANSAZIONE.CodTransazione
WHERE
    TRANSAZIONE.NumeroIdentificativo = '1234567890123456';




INSERT INTO UFFICIO_POSTALE (Id, Loc_Id, Ind_Via, Ind_Civico, Ind_Comune, Ind_Regione, Ind_Provincia, Apertura, Chiusura) 
VALUES (1, 1001, 'Via Roma', '10', 'Roma', 'Lazio', 'RM', '08:00:00', '18:00:00');

INSERT INTO UFFICIO_POSTALE (Id, Loc_Id, Ind_Via, Ind_Civico, Ind_Comune, Ind_Regione, Ind_Provincia, Apertura, Chiusura) 
VALUES (2, 1002, 'Corso Vittorio Emanuele', '25', 'Milano', 'Lombardia', 'MI', '09:00:00', '17:00:00');

INSERT INTO UFFICIO_POSTALE (Id, Loc_Id, Ind_Via, Ind_Civico, Ind_Comune, Ind_Regione, Ind_Provincia, Apertura, Chiusura) 
VALUES (3, 1003, 'Piazza Garibaldi', '5', 'Napoli', 'Campania', 'NA', '08:30:00', '16:30:00');

INSERT INTO UFFICIO_POSTALE (Id, Loc_Id, Ind_Via, Ind_Civico, Ind_Comune, Ind_Regione, Ind_Provincia, Apertura, Chiusura) 
VALUES (4, 1004, 'Via Dante', '15', 'Torino', 'Piemonte', 'TO', '08:00:00', '18:00:00');

INSERT INTO UFFICIO_POSTALE (Id, Loc_Id, Ind_Via, Ind_Civico, Ind_Comune, Ind_Regione, Ind_Provincia, Apertura, Chiusura) 
VALUES (5, 1005, 'Via Mazzini', '30', 'Firenze', 'Toscana', 'FI', '09:00:00', '17:00:00');

INSERT INTO OPERAZIONE (Id, Descrizione) VALUES (1, 'Bollettino');
INSERT INTO OPERAZIONE (Id, Descrizione) VALUES (2, 'Posta e pacchi');
INSERT INTO OPERAZIONE (Id, Descrizione) VALUES (3, 'Versamenti e altri pagamenti');
INSERT INTO OPERAZIONE (Id, Descrizione) VALUES (4, 'SPID');
INSERT INTO OPERAZIONE (Id, Descrizione) VALUES (5, 'Altro');



-- Esempio di popolamento della tabella AUTORIZZAZIONE con valori invertiti
-- Inserimento dati con valori di Sed_Id validi dalla tabella UFFICIO_POSTALE
INSERT INTO AUTORIZZAZIONE (Sed_Id, Id) VALUES (1, 1);
INSERT INTO AUTORIZZAZIONE (Sed_Id, Id) VALUES (2, 1);
INSERT INTO AUTORIZZAZIONE (Sed_Id, Id) VALUES (3, 2);
INSERT INTO AUTORIZZAZIONE (Sed_Id, Id) VALUES (4, 2);
INSERT INTO AUTORIZZAZIONE (Sed_Id, Id) VALUES (5, 3);

INSERT INTO AUTORIZZAZIONE (Sed_Id, Id) VALUES (1, 4);
INSERT INTO AUTORIZZAZIONE (Sed_Id, Id) VALUES (2, 4);
INSERT INTO AUTORIZZAZIONE (Sed_Id, Id) VALUES (3, 1);
INSERT INTO AUTORIZZAZIONE (Sed_Id, Id) VALUES (4, 5);
INSERT INTO AUTORIZZAZIONE (Sed_Id, Id) VALUES (5, 5);


-- Popolamento della tabella RECENSIONE

INSERT INTO RECENSIONE (Id, Testo, Voto, Data, CF, Ass_Id) 
VALUES (1, 'Ottimo servizio, personale molto cortese.', 5, '2023-08-01 10:30:00', 'RSSMRA85M01H501Z', 1);

INSERT INTO RECENSIONE (Id, Testo, Voto, Data, CF, Ass_Id) 
VALUES (2, 'Servizio buono, ma tempi di attesa lunghi.', 3, '2023-08-02 11:00:00', 'VRDLGI85S01H501Y', 2);

INSERT INTO RECENSIONE (Id, Testo, Voto, Data, CF, Ass_Id) 
VALUES (3, 'Esperienza negativa, poca disponibilità del personale.', 2, '2023-08-03 09:45:00', 'BNCLRA85M01H501X', 3);

INSERT INTO RECENSIONE (Id, Testo, Voto, Data, CF, Ass_Id) 
VALUES (4, 'Ufficio pulito e ordinato, ma il servizio potrebbe migliorare.', 4, '2023-08-04 15:20:00', 'PLMLGU85M01H501W', 4);

INSERT INTO RECENSIONE (Id, Testo, Voto, Data, CF, Ass_Id) 
VALUES (5, 'Personale scortese, esperienza pessima.', 1, '2023-08-05 14:00:00', 'MNCLGI85S01H501V', 5);

INSERT INTO RECENSIONE (Id, Testo, Voto, Data, CF, Ass_Id) 
VALUES (6, 'Servizio eccellente, ritornerò sicuramente.', 5, '2023-08-06 10:00:00', 'RSSMRA85M01H501Z', 1);

INSERT INTO RECENSIONE (Id, Testo, Voto, Data, CF, Ass_Id) 
VALUES (7, 'Soddisfatto del servizio, ma tempi di attesa troppo lunghi.', 3, '2023-08-07 11:15:00', 'VRDLGI85S01H501Y', 2);

INSERT INTO RECENSIONE (Id, Testo, Voto, Data, CF, Ass_Id) 
VALUES (8, 'Personale gentile e disponibile, ma ufficio poco pulito.', 3, '2023-08-08 09:30:00', 'BNCLRA85M01H501X', 3);

INSERT INTO RECENSIONE (Id, Testo, Voto, Data, CF, Ass_Id) 
VALUES (9, 'Servizio nella media, niente di speciale.', 3, '2023-08-09 16:00:00', 'PLMLGU85M01H501W', 4);

INSERT INTO RECENSIONE (Id, Testo, Voto, Data, CF, Ass_Id) 
VALUES (10, 'Esperienza molto positiva, consiglio questo ufficio.', 4, '2023-08-10 13:45:00', 'MNCLGI85S01H501V', 5);
