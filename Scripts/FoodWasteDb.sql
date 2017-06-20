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
	HandlesPayments BIT NOT NULL,
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
	IF((SELECT COUNT(*) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='DataProviders' AND Column_Name='HandlesPayments') = 0) THEN
		ALTER TABLE DataProviders ADD HandlesPayments BIT NULL;
		UPDATE DataProviders SET HandlesPayments=0;
		ALTER TABLE DataProviders MODIFY HandlesPayments BIT NOT NULL;
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
		INSERT INTO DataProviders (DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier) VALUES('5A1B9283-6406-44DF-91C5-F2FB83CC9A42','DTU Fødevareinstituttet',0,'4980BD1C-17D5-4E77-ABA5-BC6E065E6155');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = 'AA99AFDD-4FF4-48E8-9AA9-B34D00F5DF11') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('AA99AFDD-4FF4-48E8-9AA9-B34D00F5DF11','4980BD1C-17D5-4E77-ABA5-BC6E065E6155','807E904D-FDF9-418D-9745-B73821B8D07A','Data source for food groups and food data: http://www.foodcomp.dk');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '851F635C-F87E-44E8-A1AA-26C249F8CCC0') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('851F635C-F87E-44E8-A1AA-26C249F8CCC0','4980BD1C-17D5-4E77-ABA5-BC6E065E6155','978C7318-AD0A-459C-BEE0-1803A94F50D7','Datakilde for fødevare grupperinger og fødevaredata: http://www.foodcomp.dk');
	END IF;
	IF((SELECT COUNT(*) FROM DataProviders WHERE DataProviderIdentifier = '9FF5EB98-B475-4FEB-A621-0DFBEA881552') = 0) THEN
		INSERT INTO DataProviders (DataProviderIdentifier,Name,HandlesPayments,DataSourceStatementIdentifier) VALUES('9FF5EB98-B475-4FEB-A621-0DFBEA881552','PayPal',1,'9D7CF514-380D-4BD9-822D-7CE70B30D061');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = 'FFE92175-FF98-4CE7-8CD6-89B08DF1A8D6') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('FFE92175-FF98-4CE7-8CD6-89B08DF1A8D6','9D7CF514-380D-4BD9-822D-7CE70B30D061','807E904D-FDF9-418D-9745-B73821B8D07A','N/A');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '8B86E508-F7A2-4F01-8FD5-F14854FD0435') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('8B86E508-F7A2-4F01-8FD5-F14854FD0435','9D7CF514-380D-4BD9-822D-7CE70B30D061','978C7318-AD0A-459C-BEE0-1803A94F50D7','N/A');
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
	IF((SELECT COUNT(*) FROM StaticTexts WHERE StaticTextType=2) = 0) THEN
		INSERT INTO StaticTexts (StaticTextIdentifier,StaticTextType,SubjectTranslationIdentifier,BodyTranslationIdentifier) VALUES('651D2814-3D4D-41D6-88CB-C0F8C43BFA8C',2,'D722639D-3680-4273-A5CF-BA6337C7678F','D2C5C01C-9F7C-4E70-B6BE-832EACB31B8F');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '15FD83CA-B141-483C-8AF0-D27008A00353') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('15FD83CA-B141-483C-8AF0-D27008A00353','D722639D-3680-4273-A5CF-BA6337C7678F','807E904D-FDF9-418D-9745-B73821B8D07A','Privacy Policy');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '497B32FD-E521-4342-BDFC-11A5B8CE2A9E') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('497B32FD-E521-4342-BDFC-11A5B8CE2A9E','D2C5C01C-9F7C-4E70-B6BE-832EACB31B8F','807E904D-FDF9-418D-9745-B73821B8D07A','<html>As a household member, you will own the data registered by you within the households where you are a member. The data registered by you within each household will be stored as part of our project and our project can use and share your data with our third party vendors. When we share data, we will anonymize all the data so it will not be possible to trace it back to you.<br><br>Our project will own all the aggregated results we can create using your data. When we make aggregated results, we will anonymize all the data so it will not be possible to trace it back to you.</html>');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '590FF843-B1A7-4BAE-81F9-9C0637D1ED62') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('590FF843-B1A7-4BAE-81F9-9C0637D1ED62','D722639D-3680-4273-A5CF-BA6337C7678F','978C7318-AD0A-459C-BEE0-1803A94F50D7','Fortrolighedspolitik');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = 'D922E53F-A311-4CE8-BDAC-9BC5293F182E') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('D922E53F-A311-4CE8-BDAC-9BC5293F182E','D2C5C01C-9F7C-4E70-B6BE-832EACB31B8F','978C7318-AD0A-459C-BEE0-1803A94F50D7','<html>Som husstandsmedlem, vil du eje de data, der er registreret af dig i de husstande, hvor du er medlem. De data, der er registreret af dig i hver husstand, vil blive gemt som en del af vores projekt og vores projekt kan bruge og dele dine data med vore tredjepartsleverandører. Når vi deler data, vil vi anonymisere alle data, så det ikke vil være muligt at spore dem tilbage til dig.<br><br>Vores projekt vil eje alle de aggregerede resultater, vi kan skabe ved hjælp af dine data. Når vi laver aggregerede resultater, vil vi anonymisere alle data, så det ikke vil være muligt at spore dem tilbage til dig.</html>');
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

CREATE TABLE IF NOT EXISTS Payments (
	PaymentIdentifier CHAR(36) NOT NULL,
	StakeholderIdentifier CHAR(36) NOT NULL,
	StakeholderType TINYINT NOT NULL,
	DataProviderIdentifier CHAR(36) NOT NULL,
	PaymentTime DATETIME NOT NULL,
	PaymentReference NVARCHAR(128) NOT NULL,
	PaymentReceipt LONGTEXT NULL,
	CreationTime DATETIME NOT NULL,
	PRIMARY KEY (PaymentIdentifier),
	UNIQUE INDEX IX_Payments_DataProvider_PaymentReference (DataProviderIdentifier,PaymentReference),
	INDEX IX_StakeholderIdentifier_PaymentTime (StakeholderIdentifier,PaymentTime),
	FOREIGN KEY FK_Payments_DataProviderIdentifier (DataProviderIdentifier) REFERENCES DataProviders (DataProviderIdentifier) ON DELETE CASCADE ON UPDATE CASCADE
);

DROP PROCEDURE IF EXISTS AlterPayments;
DELIMITER $$
CREATE PROCEDURE AlterPayments()
BEGIN
	IF((SELECT COUNT(*) FROM information_schema.Statistics WHERE Table_Schema=DATABASE() AND Table_Name='Payments' AND Index_Name='IX_StakeholderIdentifier_PaymentTime') = 0) THEN
		CREATE INDEX IX_StakeholderIdentifier_PaymentTime ON Payments (StakeholderIdentifier,PaymentTime);
	END IF;
END $$
DELIMITER ;
CALL AlterPayments();
DROP PROCEDURE AlterPayments;

DROP PROCEDURE IF EXISTS GrantRightsForPayments;
DELIMITER $$
CREATE PROCEDURE GrantRightsForPayments(IN hostName CHAR(60), IN databaseName CHAR(64), IN userName CHAR(16))
BEGIN
	SET @sql = CONCAT('GRANT SELECT,INSERT,UPDATE,DELETE ON ', databaseName, '.Payments TO "', userName, '"@"', hostName, '"');
	PREPARE statement FROM @sql;
	EXECUTE statement;
END $$
DELIMITER ;
CALL GrantRightsForPayments(@HostName, DATABASE(), @ServiceUserName);
DROP PROCEDURE GrantRightsForPayments;

CREATE TABLE IF NOT EXISTS HouseholdMembers (
	HouseholdMemberIdentifier CHAR(36) NOT NULL,
	MailAddress NVARCHAR(128) NOT NULL,
	Membership TINYINT NOT NULL,
	MembershipExpireTime DATETIME NULL,
	ActivationCode NVARCHAR(64) NOT NULL,
	ActivationTime DATETIME NULL,
	PrivacyPolicyAcceptedTime DATETIME NULL,
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
	IF((SELECT COUNT(*) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='HouseholdMembers' AND Column_Name='Membership') = 0) THEN
		ALTER TABLE HouseholdMembers ADD Membership TINYINT NULL;
		UPDATE HouseholdMembers SET Membership=1;
		ALTER TABLE HouseholdMembers MODIFY Membership TINYINT NOT NULL;
	END IF;
	IF((SELECT COUNT(*) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='HouseholdMembers' AND Column_Name='MembershipExpireTime') = 0) THEN
		ALTER TABLE HouseholdMembers ADD MembershipExpireTime DATETIME NULL;
	END IF;
	IF((SELECT COUNT(*) FROM information_schema.Columns WHERE Table_Schema=DATABASE() AND Table_Name='HouseholdMembers' AND Column_Name='PrivacyPolicyAcceptedTime') = 0) THEN
		ALTER TABLE HouseholdMembers ADD PrivacyPolicyAcceptedTime DATETIME NULL;
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

CREATE TABLE IF NOT EXISTS Households (
	HouseholdIdentifier CHAR(36) NOT NULL,
	Name NVARCHAR(64) NOT NULL,
	Descr NVARCHAR(2048) NULL,
	CreationTime DATETIME NOT NULL,
	PRIMARY KEY (HouseholdIdentifier),
	INDEX IX_Households_Name_CreationTime (Name,CreationTime)
);

DROP PROCEDURE IF EXISTS GrantRightsForHouseholds;
DELIMITER $$
CREATE PROCEDURE GrantRightsForHouseholds(IN hostName CHAR(60), IN databaseName CHAR(64), IN userName CHAR(16))
BEGIN
	SET @sql = CONCAT('GRANT SELECT,INSERT,UPDATE,DELETE ON ', databaseName, '.Households TO "', userName, '"@"', hostName, '"');
	PREPARE statement FROM @sql;
	EXECUTE statement;
END $$
DELIMITER ;
CALL GrantRightsForHouseholds(@HostName, DATABASE(), @ServiceUserName);
DROP PROCEDURE GrantRightsForHouseholds;

CREATE TABLE IF NOT EXISTS MemberOfHouseholds (
	MemberOfHouseholdIdentifier CHAR(36) NOT NULL,
	HouseholdMemberIdentifier CHAR(36) NOT NULL,
	HouseholdIdentifier CHAR(36) NOT NULL,
	CreationTime DATETIME NOT NULL,
	PRIMARY KEY (MemberOfHouseholdIdentifier),
	UNIQUE INDEX IX_MemberOfHouseholds_HouseholdMemberIdentifier (HouseholdMemberIdentifier,HouseholdIdentifier),
	UNIQUE INDEX IX_MemberOfHouseholds_HouseholdIdentifier (HouseholdIdentifier,HouseholdMemberIdentifier),
	FOREIGN KEY FK_MemberOfHouseholds_HouseholdMemberIdentifier (HouseholdMemberIdentifier) REFERENCES HouseholdMembers (HouseholdMemberIdentifier) ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY FK_MemberOfHouseholds_HouseholdIdentifier (HouseholdIdentifier) REFERENCES Households (HouseholdIdentifier) ON DELETE CASCADE ON UPDATE CASCADE
);

DROP PROCEDURE IF EXISTS GrantMemberOfHouseholds;
DELIMITER $$
CREATE PROCEDURE GrantMemberOfHouseholds(IN hostName CHAR(60), IN databaseName CHAR(64), IN userName CHAR(16))
BEGIN
	SET @sql = CONCAT('GRANT SELECT,INSERT,UPDATE,DELETE ON ', databaseName, '.MemberOfHouseholds TO "', userName, '"@"', hostName, '"');
	PREPARE statement FROM @sql;
	EXECUTE statement;
END $$
DELIMITER ;
CALL GrantMemberOfHouseholds(@HostName, DATABASE(), @ServiceUserName);
DROP PROCEDURE GrantMemberOfHouseholds;

CREATE TABLE IF NOT EXISTS StorageTypes (
	StorageTypeIdentifier CHAR(36) NOT NULL,
	SortOrder TINYINT NOT NULL,
	Temperature TINYINT NOT NULL,
	TemperatureRangeStartValue TINYINT NOT NULL,
	TemperatureRangeEndValue TINYINT NOT NULL,
	Creatable BIT NOT NULL,
	Editable BIT NOT NULL,
	Deletable BIT NOT NULL,
	PRIMARY KEY (StorageTypeIdentifier)
);

DROP PROCEDURE IF EXISTS InsertDataIntoStorageTypes;
DELIMITER $$
CREATE PROCEDURE InsertDataIntoStorageTypes()
BEGIN
	IF((SELECT COUNT(*) FROM StorageTypes WHERE StorageTypeIdentifier = '3CEA8A7D-01A4-40BF-AB96-F70354015352') = 0) THEN
		INSERT INTO StorageTypes (StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable) VALUES('3CEA8A7D-01A4-40BF-AB96-F70354015352',1,5,-5,10,1,1,1);
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '00A98681-217A-45DD-8510-27BC359369A3') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('00A98681-217A-45DD-8510-27BC359369A3','3CEA8A7D-01A4-40BF-AB96-F70354015352','807E904D-FDF9-418D-9745-B73821B8D07A','Refrigerator');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = 'A29B688E-D620-4C58-96C6-4020AADFD840') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('A29B688E-D620-4C58-96C6-4020AADFD840','3CEA8A7D-01A4-40BF-AB96-F70354015352','978C7318-AD0A-459C-BEE0-1803A94F50D7','Køleskab');
	END IF;
	IF((SELECT COUNT(*) FROM StorageTypes WHERE StorageTypeIdentifier = '959A0D7D-A034-405C-8F6E-EF49ED5E7553') = 0) THEN
		INSERT INTO StorageTypes (StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable) VALUES('959A0D7D-A034-405C-8F6E-EF49ED5E7553',2,-10,-25,0,1,1,1);
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = 'FCAAD78E-31DD-4D6C-9DE3-B8261D251799') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('FCAAD78E-31DD-4D6C-9DE3-B8261D251799','959A0D7D-A034-405C-8F6E-EF49ED5E7553','807E904D-FDF9-418D-9745-B73821B8D07A','Freezer');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '1C1E756D-ADAF-4450-881C-5112ABE917FD') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('1C1E756D-ADAF-4450-881C-5112ABE917FD','959A0D7D-A034-405C-8F6E-EF49ED5E7553','978C7318-AD0A-459C-BEE0-1803A94F50D7','Fryser');
	END IF;
	IF((SELECT COUNT(*) FROM StorageTypes WHERE StorageTypeIdentifier = '0F78276B-87D1-4660-8708-A119C5DAA3A9') = 0) THEN
		INSERT INTO StorageTypes (StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable) VALUES('0F78276B-87D1-4660-8708-A119C5DAA3A9',3,20,0,45,1,1,1);
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = 'EC4C26E2-0832-4107-BED7-DD66367B442A') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('EC4C26E2-0832-4107-BED7-DD66367B442A','0F78276B-87D1-4660-8708-A119C5DAA3A9','807E904D-FDF9-418D-9745-B73821B8D07A','Kitchen Cabinets');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '8D8A075B-AAD4-4E29-B785-E19B287C22B7') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('8D8A075B-AAD4-4E29-B785-E19B287C22B7','0F78276B-87D1-4660-8708-A119C5DAA3A9','978C7318-AD0A-459C-BEE0-1803A94F50D7','Køkkenskabe');
	END IF;
	IF((SELECT COUNT(*) FROM StorageTypes WHERE StorageTypeIdentifier = 'B5A0B40D-1709-48D9-83F2-E87D54ED80F5') = 0) THEN
		INSERT INTO StorageTypes (StorageTypeIdentifier,SortOrder,Temperature,TemperatureRangeStartValue,TemperatureRangeEndValue,Creatable,Editable,Deletable) VALUES('B5A0B40D-1709-48D9-83F2-E87D54ED80F5',4,0,0,0,0,0,0);
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = 'B752FA23-6D58-4005-AA6B-6302A7417C68') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('B752FA23-6D58-4005-AA6B-6302A7417C68','B5A0B40D-1709-48D9-83F2-E87D54ED80F5','807E904D-FDF9-418D-9745-B73821B8D07A','Shopping Basket');
	END IF;
	IF((SELECT COUNT(*) FROM Translations WHERE TranslationIdentifier = '2566C2CA-578C-4E92-96B3-81274447C07E') = 0) THEN
		INSERT INTO Translations (TranslationIdentifier,OfIdentifier,InfoIdentifier,Value) VALUES('2566C2CA-578C-4E92-96B3-81274447C07E','B5A0B40D-1709-48D9-83F2-E87D54ED80F5','978C7318-AD0A-459C-BEE0-1803A94F50D7','Indkøbskurv');
	END IF;
END $$
DELIMITER ;
CALL InsertDataIntoStorageTypes();
DROP PROCEDURE InsertDataIntoStorageTypes;

DROP PROCEDURE IF EXISTS CreateIX_StorageTypes_SortOrder;
DELIMITER $$
CREATE PROCEDURE CreateIX_StorageTypes_SortOrder()
BEGIN
	IF((SELECT COUNT(*) FROM information_schema.Statistics WHERE Table_Schema=DATABASE() AND Table_Name='StorageTypes' AND Index_Name='IX_StorageTypes_SortOrder') = 0) THEN
		CREATE UNIQUE INDEX IX_StorageTypes_SortOrder ON StorageTypes (SortOrder);
	END IF;
END $$
DELIMITER ;
CALL CreateIX_StorageTypes_SortOrder();
DROP PROCEDURE CreateIX_StorageTypes_SortOrder;

CREATE TABLE IF NOT EXISTS Storages (
	StorageIdentifier CHAR(36) NOT NULL,
	HouseholdIdentifier CHAR(36) NOT NULL,
	SortOrder TINYINT NOT NULL,
	StorageTypeIdentifier CHAR(36) NOT NULL,
	Descr NVARCHAR(2048) NULL,
	Temperature TINYINT NOT NULL,
	CreationTime DATETIME NOT NULL,
	PRIMARY KEY (StorageIdentifier),
	UNIQUE INDEX IX_Storages_HouseholdIdentifier_SortOrder (HouseholdIdentifier,SortOrder),
	FOREIGN KEY FK_Storages_HouseholdIdentifier (HouseholdIdentifier) REFERENCES Households (HouseholdIdentifier) ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY FK_Storages_StorageTypeIdentifier (StorageTypeIdentifier) REFERENCES StorageTypes (StorageTypeIdentifier) ON DELETE CASCADE ON UPDATE CASCADE
);

DROP PROCEDURE IF EXISTS GrantStorages;
DELIMITER $$
CREATE PROCEDURE GrantStorages(IN hostName CHAR(60), IN databaseName CHAR(64), IN userName CHAR(16))
BEGIN
	SET @sql = CONCAT('GRANT SELECT,INSERT,UPDATE,DELETE ON ', databaseName, '.Storages TO "', userName, '"@"', hostName, '"');
	PREPARE statement FROM @sql;
	EXECUTE statement;
END $$
DELIMITER ;
CALL GrantStorages(@HostName, DATABASE(), @ServiceUserName);
DROP PROCEDURE GrantStorages;

DROP PROCEDURE IF EXISTS InsertDataIntoStorages;
DELIMITER $$
CREATE PROCEDURE InsertDataIntoStorages()
BEGIN
	CREATE TEMPORARY TABLE IF NOT EXISTS Temp AS ( 
		SELECT
			household.HouseholdIdentifier AS HouseholdIdentifier
		FROM Households AS household
		LEFT JOIN Storages AS storage ON storage.HouseholdIdentifier=household.HouseholdIdentifier
		WHERE
			storage.HouseholdIdentifier IS NULL);

	DROP TABLE Temp;
END $$
DELIMITER ;
CALL InsertDataIntoStorages();
DROP PROCEDURE InsertDataIntoStorages;
