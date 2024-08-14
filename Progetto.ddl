-- Database Section
-- ________________ 

create database PosteItalianeDatabase;

-- Selezionare il database appena creato
use PosteItalianeDatabase;

-- Tables Section
-- _____________ 

create table UTENTE (
     CF varchar(16) not null,
     Nome varchar(255) not null,
     Cognome varchar(255) not null,
     Ind_Via varchar(255) not null,
     Ind_Civico varchar(10) not null,
     Ind_Residenza varchar(255) not null,
     Ind_Provincia varchar(255) not null,
     primary key (CF));

create table ACCOUNT (
     CF varchar(16) not null,
     Amministratore boolean not null,
     BloccoAccount boolean not null,
     Email varchar(255) not null,
     Password varchar(255) not null,
     primary key (CF));

create table OPERAZIONE (
     Id numeric(10, 0) not null,
     Descrizione varchar(255) not null,
     primary key (Id));

create table UFFICIO_POSTALE (
     Id numeric(10, 0) not null,
     Loc_Id numeric(10, 0) not null,
     Ind_Via varchar(255) not null,
     Ind_Civico varchar(10) not null,
     Ind_Comune varchar(255) not null,
     Ind_Regione varchar(255) not null,
     Ind_Provincia varchar(255) not null,
     Apertura time not null,
     Chiusura time not null,
     primary key (Id),
     constraint FKLocazione_ID unique (Loc_Id));

CREATE TABLE AUTORIZZAZIONE (
    Sed_Id NUMERIC(10, 0) NOT NULL,
    Id NUMERIC(10, 0) NOT NULL,
    PRIMARY KEY (Sed_Id, Id)
);
     

create table BANCOMAT (
     Id numeric(10, 0) not null,
     Commissione float not null,
     Tipo varchar(16) not null,
     Via varchar(100),
     Comune varchar(30),
     Provincia varchar(30),
     Civico varchar(5),
     Regione varchar(30),
     primary key (Id));

create table CARTA (
     BloccoCarta boolean not null,
     NumeroIdentificativo varchar(16) not null,
     Ccv numeric(3, 0) not null,
     Pin numeric(4, 0) not null,
     Scadenza date not null,
     Saldo float not null,
     Iban varchar(32) not null,
     Tipo varchar(10) not null, 
     CF varchar(16) not null,
     primary key (NumeroIdentificativo));

create table NOTIFICA (
     Id INT AUTO_INCREMENT not null,
     Titolo varchar(255) not null,
     Testo varchar(255) not null,
     Letta boolean not null, 
     CF varchar(16) not null,
     primary key (Id));

create table PRENOTAZIONE (
     Ticket varchar(255) not null,
     Data datetime not null,
     Sed_Id numeric(10,0) not null,
     Id numeric(10, 0) not null,
     CF varchar(16) not null,
     primary key (Data));

create table RECENSIONE (
     Id numeric(10, 0) not null,
     Testo varchar(255) not null,
     Voto numeric(1, 0) not null,
     Data datetime not null,
     CF varchar(16) not null,
     Ass_Id numeric(10, 0) not null,
     primary key (Id));

create table TIPO_TRANSAZIONE (
     CodTransazione varchar(16) not null,
     Tipo varchar(32) not null,
     Commissione float(2),
     TipologiaPagamento varchar(32),
     Ente varchar(32),
     IbanDestinatario varchar(32),
     Causale varchar(32),
     NumeroIdentificativo varchar(16),
     Id numeric(10,0),
     primary key (CodTransazione));

create table TRANSAZIONE (
     CodTransazione varchar(16) not null,
     Importo float(2) not null,
     Data datetime not null,
     NumeroIdentificativo varchar(16) not null,
     primary key (CodTransazione));

-- Constraints Section
-- ___________________ 

alter table ACCOUNT add constraint FKIscrizione_FK
     foreign key (CF)
     references UTENTE (CF);

alter table AUTORIZZAZIONE add constraint FKTipo_FK
     foreign key (Id)
     references OPERAZIONE (Id);

alter table AUTORIZZAZIONE add constraint FKSede
     foreign key (Sed_Id)
     references UFFICIO_POSTALE (Id);

alter table CARTA add constraint FKDispone_FK
     foreign key (CF)
     references UTENTE (CF);

alter table NOTIFICA add constraint FKAvviso_FK
     foreign key (CF)
     references ACCOUNT (CF);

alter table PRENOTAZIONE add constraint FKPrenotazioneUfficio_FK
     foreign key (Sed_Id, Id)
     references AUTORIZZAZIONE (Sed_Id, Id);

alter table PRENOTAZIONE add constraint FKServizio_FK
     foreign key (CF)
     references ACCOUNT (CF);

alter table RECENSIONE add constraint FKEffettua_FK
     foreign key (CF)
     references ACCOUNT (CF);

alter table RECENSIONE add constraint FKAssegna_FK
     foreign key (Ass_Id)
     references UFFICIO_POSTALE (Id);

alter table TIPO_TRANSAZIONE add constraint FKRicevente_FK
     foreign key (NumeroIdentificativo)
     references CARTA (NumeroIdentificativo);

alter table TIPO_TRANSAZIONE add constraint FKPresso_FK
     foreign key (Id)
     references BANCOMAT (Id);

alter table TIPO_TRANSAZIONE add constraint FKCategoriaTransazione_FK
     foreign key (CodTransazione)
     references TRANSAZIONE (CodTransazione);

alter table TRANSAZIONE add constraint FKMovimentoBancario_FK
     foreign key (NumeroIdentificativo)
     references CARTA (NumeroIdentificativo);

alter table UFFICIO_POSTALE add constraint FKLocazione_FK
     foreign key (Loc_Id)
     references BANCOMAT (Id);


-- Index Section
-- _____________ 

create unique index FKIscrizione_IND
     on ACCOUNT (CF);

create unique index ID_AUTORIZZAZIONE_IND
     on AUTORIZZAZIONE (Sed_Id, Id);

create unique index ID_BANCOMAT_IND
     on BANCOMAT (Id);

create unique index ID_CARTA_IND
     on CARTA (NumeroIdentificativo);

create index FKDispone_IND
     on CARTA (CF);

create unique index ID_NOTIFICA_IND
     on NOTIFICA (Id);

create index FKAvviso_IND
     on NOTIFICA (CF);

create unique index ID_OPERAZIONE_IND
     on OPERAZIONE (Id);

create unique index ID_PRENOTAZIONE_IND
     on PRENOTAZIONE (Data);

create index FKPrenotazioneUfficio_IND
     on PRENOTAZIONE (Sed_Id, Id);

create index FKServizio_IND
     on PRENOTAZIONE (CF);

create unique index ID_RECENSIONE_IND
     on RECENSIONE (Id);

create index FKEffettua_IND
     on RECENSIONE (CF);

create index FKAssegna_IND
     on RECENSIONE (Ass_Id);

create index FKRicevente_IND
     on TIPO_TRANSAZIONE (NumeroIdentificativo);

create index FKPresso_IND
     on TIPO_TRANSAZIONE (Id);

create unique index FKCategoriaTransazione_IND
     on TIPO_TRANSAZIONE (CodTransazione);

create unique index ID_TRANSAZIONE_IND
     on TRANSAZIONE (CodTransazione);

create index FKMovimentoBancario_IND
     on TRANSAZIONE (NumeroIdentificativo);

create unique index ID_UFFICIO_POSTALE_IND
     on UFFICIO_POSTALE (Id);

create unique index ID_UTENTE_IND
     on UTENTE (CF);