DELETE FROM Translations WHERE Value='Test' OR Value='Testing';
DELETE FROM ForeignKeys WHERE ForeignKeyForTypes LIKE '%IForeignKey%' AND (ForeignKeyValue='Test' OR ForeignKeyValue='Testing' OR ForeignKeyValue='ForeignKeyToFoodGroup');

CREATE TEMPORARY TABLE DeleteFoodGroups AS (
	SELECT
		fg.FoodGroupIdentifier AS FoodGroupIdentifier
	FROM FoodGroups AS fg
	LEFT JOIN ForeignKeys AS fk ON fk.ForeignKeyForIdentifier=fg.FoodGroupIdentifier
	WHERE
		fk.ForeignKeyForIdentifier IS NULL);

DELETE FROM FoodGroups
WHERE FoodGroupIdentifier IN (SELECT FoodGroupIdentifier FROM DeleteFoodGroups);

DROP TEMPORARY TABLE DeleteFoodGroups;

CREATE TEMPORARY TABLE DeleteFoodItmes AS (
	SELECT
		fi.FoodItemIdentifier AS FoodItemIdentifier
	FROM FoodItems AS fi
	LEFT JOIN ForeignKeys AS fk ON fk.ForeignKeyForIdentifier=fi.FoodItemIdentifier
	WHERE
		fk.ForeignKeyForIdentifier IS NULL);
        
DELETE FROM FoodItems
WHERE FoodItemIdentifier IN (SELECT FoodItemIdentifier FROM DeleteFoodItmes);

DROP TEMPORARY TABLE DeleteFoodItmes;