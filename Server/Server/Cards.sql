CREATE TABLE "CARDS"
(
	"CARD_NAME" CHAR(40) NOT NULL PRIMARY KEY,
	"COST" INT,
	"STRENGTH" INT,
	"HEALTH" INT,
	"CARD_TYPE" CHAR(7),
	"WAGE_MOD" INT,
	"WAGE_BONUS" INT,
	CONSTRAINT CHK_Valid_Type CHECK (CARD_TYPE in ('Unit','Utility','Lord'))
);

INSERT INTO CARDS VALUES ('The Executioner', 0, 0, 0, 'Lord', 1, 0);
INSERT INTO CARDS VALUES ('Black Market', 0, 0, 0, 'Utility', 1, 0);
INSERT INTO CARDS VALUES ('Bloated Body', 4, 1, 1, 'Unit', 1, 0);
INSERT INTO CARDS VALUES ('Combat Medic', 4, 1, 2, 'Unit', 1, 0);
INSERT INTO CARDS VALUES ('Dying Nobleman', 3, 1, 1, 'Unit', 1, 0);
INSERT INTO CARDS VALUES ('Frog Elder', 3, 2, 2, 'Unit', 1, 0);
INSERT INTO CARDS VALUES ('Illusion Frog', 3, 2, 2, 'Unit', 1, 0);
INSERT INTO CARDS VALUES ('Negotiator', 4, 1, 2, 'Unit', 0, 0);
INSERT INTO CARDS VALUES ('Sleight of Hand', 0, 0, 0, 'Utility', 1, 0);
INSERT INTO CARDS VALUES ('Trinkets & Baubles', 0, 0, 0, 'Utility', 1, 0);
