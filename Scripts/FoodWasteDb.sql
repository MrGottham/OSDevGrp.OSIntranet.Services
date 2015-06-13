SET @HostName = '%';
SET @ServiceUserName = 'ODBC';

DROP PROCEDURE IF EXISTS GrantRightsForFoodWasteDb;
DELIMITER $$
CREATE PROCEDURE GrantRightsForFoodWasteDb(IN hostName CHAR(60), IN databaseName CHAR(64), IN userName CHAR(16))
BEGIN
	SET @sql = CONCAT('GRANT SELECT ON ', databaseName, '.* TO "', userName, '"@"', hostName, '"');
	PREPARE statement FROM @sql;
	EXECUTE statement;
END $$
DELIMITER ;
CALL GrantRightsForFoodWasteDb(@HostName, DATABASE(), @ServiceUserName);
DROP PROCEDURE GrantRightsForFoodWasteDb;

CREATE TABLE IF NOT EXISTS TranslationInfos (
	TranslationInfoIdentifier VARCHAR(40) NOT NULL,
	CultureName VARCHAR(5) NOT NULL,
	PRIMARY KEY (TranslationInfoIdentifier)
);

DROP PROCEDURE IF EXISTS CreateIX_TranslationInfos_CultureName;
DELIMITER $$
CREATE PROCEDURE CreateIX_TranslationInfos_CultureName()
BEGIN
	IF((SELECT COUNT(*) FROM information_schema.Statistics WHERE Table_Schema=DATABASE() AND Table_Name='TranslationInfos' AND Index_Name='IX_TranslationInfos_CultureName') = 0) THEN
		CREATE UNIQUE INDEX IX_TranslationInfos_CultureName ON TranslationInfos (CultureName);
	END IF;
END $$
DELIMITER ;
CALL CreateIX_TranslationInfos_CultureName();
DROP PROCEDURE CreateIX_TranslationInfos_CultureName;

DROP PROCEDURE IF EXISTS InsertDataIntoTranslationInfos;
DELIMITER $$
CREATE PROCEDURE InsertDataIntoTranslationInfos()
BEGIN
	IF((SELECT COUNT(*) FROM TranslationInfos WHERE CultureName='en-US') = 0) THEN
		INSERT INTO TranslationInfos (TranslationInfoIdentifier,CultureName) VALUES('807E904D-FDF9-418D-9745-B73821B8D07A','en-US');
	END IF;
	IF((SELECT COUNT(*) FROM TranslationInfos WHERE CultureName='da-DK') = 0) THEN
		INSERT INTO TranslationInfos (TranslationInfoIdentifier,CultureName) VALUES('978C7318-AD0A-459C-BEE0-1803A94F50D7','da-DK');
	END IF;
END $$
DELIMITER ;
CALL InsertDataIntoTranslationInfos();
DROP PROCEDURE InsertDataIntoTranslationInfos;

CREATE TABLE IF NOT EXISTS Translations (
	TranslationIdentifier VARCHAR(40) NOT NULL,
	OfIdentifier VARCHAR(40) NOT NULL,
	InfoIdentifier VARCHAR(40) NOT NULL,
	Value VARCHAR(4096) NOT NULL,
	PRIMARY KEY (TranslationIdentifier),
	UNIQUE INDEX IX_Translations_OfIdentifier_InfoIdentifier (OfIdentifier,InfoIdentifier),
	FOREIGN KEY FK_Translations_InfoIdentifier (InfoIdentifier) REFERENCES TranslationInfos (TranslationInfoIdentifier) MATCH FULL ON DELETE CASCADE ON UPDATE CASCADE
);

DROP PROCEDURE IF EXISTS GrantRightsForTranslations;
DELIMITER $$
CREATE PROCEDURE GrantRightsForTranslations(IN hostName CHAR(60), IN databaseName CHAR(64), IN userName CHAR(16))
BEGIN
	SET @sql = CONCAT('GRANT SELECT,INSERT,UPDATE,DELETE ON ', databaseName, '.Translations TO "', userName, '"@"', hostName, '"');
	PREPARE statement FROM @sql;
	EXECUTE statement;
END $$
DELIMITER ;
CALL GrantRightsForTranslations(@HostName, DATABASE(), @ServiceUserName);
DROP PROCEDURE GrantRightsForTranslations;

CREATE TABLE IF NOT EXISTS DataProviders (
	DataProviderIdentifier VARCHAR(40) NOT NULL,
	Name VARCHAR(256) NOT NULL,
	DataSourceStatementIdentifier VARCHAR(40) NOT NULL,
	PRIMARY KEY (DataProviderIdentifier),
	UNIQUE INDEX IX_DataProviders_DataSourceStatementIdentifier (DataSourceStatementIdentifier)
);

DROP PROCEDURE IF EXISTS InsertDataIntoDataProviders;
DELIMITER $$
CREATE PROCEDURE InsertDataIntoDataProviders()
BEGIN
	IF((SELECT COUNT(*) FROM DataProviders WHERE DataProviderIdentifier = '5A1B9283-6406-44DF-91C5-F2FB83CC9A42') = 0) THEN
		INSERT INTO DataProviders (DataProviderIdentifier,Name,DataSourceStatementIdentifier) VALUES('5A1B9283-6406-44DF-91C5-F2FB83CC9A42','DTU Fødevareinstituttet','4980BD1C-17D5-4E77-ABA5-BC6E065E6155');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = 'AA99AFDD-4FF4-48E8-9AA9-B34D00F5DF11') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('AA99AFDD-4FF4-48E8-9AA9-B34D00F5DF11','4980BD1C-17D5-4E77-ABA5-BC6E065E6155','807E904D-FDF9-418D-9745-B73821B8D07A','Data source for food groups and food data: http://www.foodcomp.dk');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '851F635C-F87E-44E8-A1AA-26C249F8CCC0') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('851F635C-F87E-44E8-A1AA-26C249F8CCC0','4980BD1C-17D5-4E77-ABA5-BC6E065E6155','978C7318-AD0A-459C-BEE0-1803A94F50D7','Datakilde for fødevare grupperinger og fødevaredata: http://www.foodcomp.dk');
	END IF;
END $$
DELIMITER ;
CALL InsertDataIntoDataProviders();
DROP PROCEDURE InsertDataIntoDataProviders;
