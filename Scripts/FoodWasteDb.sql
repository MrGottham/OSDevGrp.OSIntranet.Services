CREATE TABLE IF NOT EXISTS TranslationInfos (
	TranslationInfoIdentifier VARCHAR(40) NOT NULL,
	CultureName VARCHAR(5) NOT NULL,
	PRIMARY KEY (TranslationInfoIdentifier)
);
IF SELECT COUNT(*) FROM information_schema.X WHERE X=DATABASE() THEN
BEGIN
	CREATE UNIQUE INDEX IF NOT EXISTS IX_TranslationInfos_CultureName ON TranslationInfos (CultureName);
END IF;

