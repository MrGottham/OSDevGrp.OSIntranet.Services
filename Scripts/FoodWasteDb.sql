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
	TranslationInfoIdentifier CHAR(36) NOT NULL,
	CultureName CHAR(5) NOT NULL,
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

DROP PROCEDURE IF EXISTS AlterTranslationInfos;
DELIMITER $$
CREATE PROCEDURE AlterTranslationInfos()
BEGIN
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='TranslationInfos' AND Column_Name='TranslationInfoIdentifier') = 'varchar') THEN
		ALTER TABLE TranslationInfos MODIFY TranslationInfoIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='TranslationInfos' AND Column_Name='CultureName') = 'varchar') THEN
		ALTER TABLE TranslationInfos MODIFY CultureName CHAR(5) NOT NULL;
	END IF;
END $$
DELIMITER ;
CALL AlterTranslationInfos();
DROP PROCEDURE AlterTranslationInfos;

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
	TranslationIdentifier CHAR(36) NOT NULL,
	OfIdentifier CHAR(36) NOT NULL,
	InfoIdentifier CHAR(36) NOT NULL,
	Value NVARCHAR(4096) NOT NULL,
	PRIMARY KEY (TranslationIdentifier),
	UNIQUE INDEX IX_Translations_OfIdentifier_InfoIdentifier (OfIdentifier,InfoIdentifier),
	FOREIGN KEY FK_Translations_InfoIdentifier (InfoIdentifier) REFERENCES TranslationInfos (TranslationInfoIdentifier) ON DELETE CASCADE ON UPDATE CASCADE
);

DROP PROCEDURE IF EXISTS AlterTranslations;
DELIMITER $$
CREATE PROCEDURE AlterTranslations()
BEGIN
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='Translations' AND Column_Name='TranslationIdentifier') = 'varchar') THEN
		ALTER TABLE Translations MODIFY TranslationIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='Translations' AND Column_Name='OfIdentifier') = 'varchar') THEN
		ALTER TABLE Translations MODIFY OfIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='Translations' AND Column_Name='InfoIdentifier') = 'varchar') THEN
		ALTER TABLE Translations MODIFY InfoIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='Translations' AND Column_Name='Value') = 'varchar') THEN
		ALTER TABLE Translations MODIFY Value NVARCHAR(4096) NOT NULL;
	END IF;
END $$
DELIMITER ;
CALL AlterTranslations();
DROP PROCEDURE AlterTranslations;

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
	DataProviderIdentifier CHAR(36) NOT NULL,
	Name NVARCHAR(256) NOT NULL,
	DataSourceStatementIdentifier CHAR(36) NOT NULL,
	PRIMARY KEY (DataProviderIdentifier),
	UNIQUE INDEX IX_DataProviders_DataSourceStatementIdentifier (DataSourceStatementIdentifier)
);

DROP PROCEDURE IF EXISTS AlterDataProviders;
DELIMITER $$
CREATE PROCEDURE AlterDataProviders()
BEGIN
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='DataProviders' AND Column_Name='DataProviderIdentifier') = 'varchar') THEN
		ALTER TABLE DataProviders MODIFY DataProviderIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='DataProviders' AND Column_Name='Name') = 'varchar') THEN
		ALTER TABLE DataProviders MODIFY Name NVARCHAR(256) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='DataProviders' AND Column_Name='DataSourceStatementIdentifier') = 'varchar') THEN
		ALTER TABLE DataProviders MODIFY DataSourceStatementIdentifier CHAR(36) NOT NULL;
	END IF;
END $$
DELIMITER ;
CALL AlterDataProviders();
DROP PROCEDURE AlterDataProviders;

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

CREATE TABLE IF NOT EXISTS StaticTexts (
	StaticTextIdentifier CHAR(36) NOT NULL,
	StaticTextType TINYINT NOT NULL,
	SubjectTranslationIdentifier CHAR(36) NOT NULL,
	BodyTranslationIdentifier CHAR(36) NULL,
	PRIMARY KEY (StaticTextIdentifier),
	UNIQUE INDEX IX_StaticTexts_StaticTextType (StaticTextType)
);

DROP PROCEDURE IF EXISTS InsertDataIntoStaticTexts;
DELIMITER $$
CREATE PROCEDURE InsertDataIntoStaticTexts()
BEGIN
	IF((SELECT COUNT(*) FROM StaticTexts WHERE StaticTextType=1) = 0) THEN
		INSERT INTO StaticTexts (StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier) VALUES('5F30DF2A-63BC-4FFA-9EF9-6B847C795A85',1,'4529AE8A-7DCA-47CE-8F07-4C4273FC0361','CBFA2654-065E-454A-BA09-8A4BA9EB3CF5');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '28CB67E3-769F-4483-ABB9-92754E10B538') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('28CB67E3-769F-4483-ABB9-92754E10B538','4529AE8A-7DCA-47CE-8F07-4C4273FC0361','807E904D-FDF9-418D-9745-B73821B8D07A','Welcome to the Minimize Food Waste Project');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '5A8D95AA-4537-40DF-94F9-2361ED6D234C') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('5A8D95AA-4537-40DF-94F9-2361ED6D234C','CBFA2654-065E-454A-BA09-8A4BA9EB3CF5','807E904D-FDF9-418D-9745-B73821B8D07A','<html><h1>Welcome to the Minimize Food Waste Project</h1><br><br>You have been created as a household member in the Minimize Food Waste Project. The next step is to activate your household member account on our website.<br><br>Your activation code is: <b>[ActivationCode]</b><br><br>Yours sincerely<br>The Minimize Food Waste Project<br><br><h2>[PrivacyPoliciesSubject]</h2><br>[PrivacyPoliciesBody]</html>');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = 'DE0785F1-F331-4B79-B454-019912F32E9D') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('DE0785F1-F331-4B79-B454-019912F32E9D','4529AE8A-7DCA-47CE-8F07-4C4273FC0361','978C7318-AD0A-459C-BEE0-1803A94F50D7','Velkommen til Projektet Formindsk Madspild');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '440DDE45-9D3A-4AF2-9995-7D6005D373B5') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('440DDE45-9D3A-4AF2-9995-7D6005D373B5','CBFA2654-065E-454A-BA09-8A4BA9EB3CF5','978C7318-AD0A-459C-BEE0-1803A94F50D7','<html><h1>Velkommen til Projektet Formindsk Madspild</h1><br><br>Du er blevet oprettet som husstandsmedlem i Projektet Formindsk Madspild. Dit næste skridt er at aktivere din konto på vores hjemmeside.<br><br>Din aktiveringskode er: <b>[ActivationCode]</b><br><br>Venlig hilsen<br>Projektet Formindsk Madspild<br><br><h2>[PrivacyPoliciesSubject]</h2><br>[PrivacyPoliciesBody]</html>');
	END IF;
END $$
DELIMITER ;
CALL InsertDataIntoStaticTexts();
DROP PROCEDURE InsertDataIntoStaticTexts;

CREATE TABLE IF NOT EXISTS ForeignKeys (
	ForeignKeyIdentifier CHAR(36) NOT NULL,
	DataProviderIdentifier CHAR(36) NOT NULL,
	ForeignKeyForIdentifier CHAR(36) NOT NULL,
	ForeignKeyForTypes NVARCHAR(128) NOT NULL,
	ForeignKeyValue NVARCHAR(128) NOT NULL,
	PRIMARY KEY (ForeignKeyIdentifier),
	UNIQUE INDEX IX_ForeignKeys_ForeignKeyForTypes (DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyForTypes),
	UNIQUE INDEX IX_ForeignKeys_ForeignKeyValue (DataProviderIdentifier,ForeignKeyForIdentifier,ForeignKeyValue),
	FOREIGN KEY FK_ForeignKeys_DataProviderIdentifier (DataProviderIdentifier) REFERENCES DataProviders (DataProviderIdentifier) ON DELETE CASCADE ON UPDATE CASCADE
);

DROP PROCEDURE IF EXISTS AlterForeignKeys;
DELIMITER $$
CREATE PROCEDURE AlterForeignKeys()
BEGIN
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='ForeignKeys' AND Column_Name='ForeignKeyIdentifier') = 'varchar') THEN
		ALTER TABLE ForeignKeys MODIFY ForeignKeyIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='ForeignKeys' AND Column_Name='DataProviderIdentifier') = 'varchar') THEN
		ALTER TABLE ForeignKeys MODIFY DataProviderIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='ForeignKeys' AND Column_Name='ForeignKeyForIdentifier') = 'varchar') THEN
		ALTER TABLE ForeignKeys MODIFY ForeignKeyForIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='ForeignKeys' AND Column_Name='ForeignKeyForTypes') = 'varchar') THEN
		ALTER TABLE ForeignKeys MODIFY ForeignKeyForTypes NVARCHAR(128) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='ForeignKeys' AND Column_Name='ForeignKeyValue') = 'varchar') THEN
		ALTER TABLE ForeignKeys MODIFY ForeignKeyValue NVARCHAR(128) NOT NULL;
	END IF;
END $$
DELIMITER ;
CALL AlterForeignKeys();
DROP PROCEDURE AlterForeignKeys;

DROP PROCEDURE IF EXISTS GrantRightsForForeignKeys;
DELIMITER $$
CREATE PROCEDURE GrantRightsForForeignKeys(IN hostName CHAR(60), IN databaseName CHAR(64), IN userName CHAR(16))
BEGIN
	SET @sql = CONCAT('GRANT SELECT,INSERT,UPDATE,DELETE ON ', databaseName, '.ForeignKeys TO "', userName, '"@"', hostName, '"');
	PREPARE statement FROM @sql;
	EXECUTE statement;
END $$
DELIMITER ;
CALL GrantRightsForForeignKeys(@HostName, DATABASE(), @ServiceUserName);
DROP PROCEDURE GrantRightsForForeignKeys;

CREATE TABLE IF NOT EXISTS FoodGroups (
	FoodGroupIdentifier CHAR(36) NOT NULL,
	ParentIdentifier CHAR(36) NULL,
	IsActive BIT NOT NULL,
	PRIMARY KEY (FoodGroupIdentifier),
	INDEX IX_FoodGroups_ParentIdentifier (ParentIdentifier),
	FOREIGN KEY FK_FoodGroups_ParentIdentifier (ParentIdentifier) REFERENCES FoodGroups (FoodGroupIdentifier) ON DELETE CASCADE ON UPDATE CASCADE
);

DROP PROCEDURE IF EXISTS AlterFoodGroups;
DELIMITER $$
CREATE PROCEDURE AlterFoodGroups()
BEGIN
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='FoodGroups' AND Column_Name='FoodGroupIdentifier') = 'varchar') THEN
		ALTER TABLE FoodGroups MODIFY FoodGroupIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='FoodGroups' AND Column_Name='ParentIdentifier') = 'varchar') THEN
		ALTER TABLE FoodGroups MODIFY ParentIdentifier CHAR(36) NULL;
	END IF;
END $$
DELIMITER ;
CALL AlterFoodGroups();
DROP PROCEDURE AlterFoodGroups;

DROP PROCEDURE IF EXISTS GrantRightsForFoodGroups;
DELIMITER $$
CREATE PROCEDURE GrantRightsForFoodGroups(IN hostName CHAR(60), IN databaseName CHAR(64), IN userName CHAR(16))
BEGIN
	SET @sql = CONCAT('GRANT SELECT,INSERT,UPDATE,DELETE ON ', databaseName, '.FoodGroups TO "', userName, '"@"', hostName, '"');
	PREPARE statement FROM @sql;
	EXECUTE statement;
END $$
DELIMITER ;
CALL GrantRightsForFoodGroups(@HostName, DATABASE(), @ServiceUserName);
DROP PROCEDURE GrantRightsForFoodGroups;

CREATE TABLE IF NOT EXISTS FoodItems (
	FoodItemIdentifier CHAR(36) NOT NULL,
	IsActive BIT NOT NULL,
	PRIMARY KEY (FoodItemIdentifier)
);

DROP PROCEDURE IF EXISTS AlterFoodItems;
DELIMITER $$
CREATE PROCEDURE AlterFoodItems()
BEGIN
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='FoodItems' AND Column_Name='FoodItemIdentifier') = 'varchar') THEN
		ALTER TABLE FoodItems MODIFY FoodItemIdentifier CHAR(36) NOT NULL;
	END IF;
END $$
DELIMITER ;
CALL AlterFoodItems();
DROP PROCEDURE AlterFoodItems;

DROP PROCEDURE IF EXISTS GrantRightsForFoodItems;
DELIMITER $$
CREATE PROCEDURE GrantRightsForFoodItems(IN hostName CHAR(60), IN databaseName CHAR(64), IN userName CHAR(16))
BEGIN
	SET @sql = CONCAT('GRANT SELECT,INSERT,UPDATE,DELETE ON ', databaseName, '.FoodItems TO "', userName, '"@"', hostName, '"');
	PREPARE statement FROM @sql;
	EXECUTE statement;
END $$
DELIMITER ;
CALL GrantRightsForFoodItems(@HostName, DATABASE(), @ServiceUserName);
DROP PROCEDURE GrantRightsForFoodItems;

CREATE TABLE IF NOT EXISTS FoodItemGroups (
	FoodItemGroupIdentifier CHAR(36) NOT NULL,
	FoodItemIdentifier CHAR(36) NOT NULL,
	FoodGroupIdentifier CHAR(36) NOT NULL,
	IsPrimary BIT NOT NULL,
	PRIMARY KEY (FoodItemGroupIdentifier),
	UNIQUE INDEX IX_FoodItemGroups_FoodItemIdentifier_FoodGroupIdentifier (FoodItemIdentifier,FoodGroupIdentifier),
	FOREIGN KEY FK_FoodItemGroups_FoodItemIdentifier (FoodItemIdentifier) REFERENCES FoodItems (FoodItemIdentifier) ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY FK_FoodItemGroups_FoodGroupIdentifier (FoodGroupIdentifier) REFERENCES FoodGroups (FoodGroupIdentifier) ON DELETE CASCADE ON UPDATE CASCADE
);

DROP PROCEDURE IF EXISTS AlterFoodItemGroups;
DELIMITER $$
CREATE PROCEDURE AlterFoodItemGroups()
BEGIN
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='FoodItemGroups' AND Column_Name='FoodItemGroupIdentifier') = 'varchar') THEN
		ALTER TABLE FoodItemGroups MODIFY FoodItemGroupIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='FoodItemGroups' AND Column_Name='FoodItemIdentifier') = 'varchar') THEN
		ALTER TABLE FoodItemGroups MODIFY FoodItemIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='FoodItemGroups' AND Column_Name='FoodGroupIdentifier') = 'varchar') THEN
		ALTER TABLE FoodItemGroups MODIFY FoodGroupIdentifier CHAR(36) NOT NULL;
	END IF;
END $$
DELIMITER ;
CALL AlterFoodItemGroups();
DROP PROCEDURE AlterFoodItemGroups;

DROP PROCEDURE IF EXISTS GrantRightsForFoodItemGroups;
DELIMITER $$
CREATE PROCEDURE GrantRightsForFoodItemGroups(IN hostName CHAR(60), IN databaseName CHAR(64), IN userName CHAR(16))
BEGIN
	SET @sql = CONCAT('GRANT SELECT,INSERT,UPDATE,DELETE ON ', databaseName, '.FoodItemGroups TO "', userName, '"@"', hostName, '"');
	PREPARE statement FROM @sql;
	EXECUTE statement;
END $$
DELIMITER ;
CALL GrantRightsForFoodItemGroups(@HostName, DATABASE(), @ServiceUserName);
DROP PROCEDURE GrantRightsForFoodItemGroups;

CREATE TABLE IF NOT EXISTS HouseholdMembers (
	HouseholdMemberIdentifier CHAR(36) NOT NULL,
	MailAddress NVARCHAR(128) NOT NULL,
	ActivationCode NVARCHAR(64) NOT NULL,
	ActivationTime DATETIME NULL,
	CreationTime DATETIME NOT NULL,
	PRIMARY KEY (HouseholdMemberIdentifier),
	UNIQUE INDEX IX_HouseholdMembers_MailAddress (MailAddress)
);

DROP PROCEDURE IF EXISTS AlterHouseholdMembers;
DELIMITER $$
CREATE PROCEDURE AlterHouseholdMembers()
BEGIN
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='HouseholdMembers' AND Column_Name='HouseholdMemberIdentifier') = 'varchar') THEN
		ALTER TABLE HouseholdMembers MODIFY HouseholdMemberIdentifier CHAR(36) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='HouseholdMembers' AND Column_Name='MailAddress') = 'varchar') THEN
		ALTER TABLE HouseholdMembers MODIFY MailAddress NVARCHAR(128) NOT NULL;
	END IF;
	IF((SELECT LOWER(Data_Type) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='HouseholdMembers' AND Column_Name='ActivationCode') = 'varchar') THEN
		ALTER TABLE HouseholdMembers MODIFY ActivationCode NVARCHAR(64) NOT NULL;
	END IF;
END $$
DELIMITER ;
CALL AlterHouseholdMembers();
DROP PROCEDURE AlterHouseholdMembers;

DROP PROCEDURE IF EXISTS GrantRightsForHouseholdMembers;
DELIMITER $$
CREATE PROCEDURE GrantRightsForHouseholdMembers(IN hostName CHAR(60), IN databaseName CHAR(64), IN userName CHAR(16))
BEGIN
	SET @sql = CONCAT('GRANT SELECT,INSERT,UPDATE,DELETE ON ', databaseName, '.HouseholdMembers TO "', userName, '"@"', hostName, '"');
	PREPARE statement FROM @sql;
	EXECUTE statement;
END $$
DELIMITER ;
CALL GrantRightsForHouseholdMembers(@HostName, DATABASE(), @ServiceUserName);
DROP PROCEDURE GrantRightsForHouseholdMembers;
