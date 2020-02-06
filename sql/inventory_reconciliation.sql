--Create a table to hold inventory balances:
declare @inventory table (
    ItemNumber varchar(50)  not null,
    WarehouseLocation varchar(50) not null,
    QuantityOnHand int not null,
    PricePerItem decimal(18,2) not null
)

--Create a table to hold accounting balances: 
declare @accounting table (
    ItemNumber varchar(50)  not null,
    TotalAccountingValue decimal(18,2) not null
)

--Mock up some inventory balances
INSERT INTO @inventory VALUES ('ABC123', 'WLA1', 312, 7.5)
INSERT INTO @inventory VALUES ('ABC123', 'WLA2', 146, 7.5)
INSERT INTO @inventory VALUES ('ZZZ99', 'WLA3', 47, 13.99)
INSERT INTO @inventory VALUES ('zzz99', 'WLA4', 91, 13.99)
INSERT INTO @inventory VALUES ('xxccM', 'WLA5', 32, 245.25)
INSERT INTO @inventory VALUES ('xxddM', 'WLA6', 15, 747.47)

--Mock up some accounting balances
INSERT INTO @accounting VALUES ('ABC123', 3435)
INSERT INTO @accounting VALUES ('ZZZ99', 1930.62)
INSERT INTO @accounting VALUES ('xxccM', 7602.75)
INSERT INTO @accounting VALUES ('fbr77', 17.99)

--TODO-CHALLENGE: Write a query to reconcile matches/differences between the inventory and accounting tables

-- Needed to keep the decimals as decimals, otherwise they rounded up.
SET NUMERIC_ROUNDABORT OFF 

-- This code would have been much easier if I wasn't fighting the collation so much.
-- By default the server collation is case insensitive which really messes up sums for ItemNumbers
-- that are identical except for case.

select I.ItemNumber AS InventoryItemNumber,
    A.ItemNumber AS AccountingItemNumber,
    TotalInventoryValue,
    A.TotalAccountingValue,
    CASE WHEN TotalInventoryValue = A.TotalAccountingValue Then 'Yes' ELSE 'No' end AS Is_Balanced
FROM @inventory AS I
JOIN (SELECT
    ItemNumber COLLATE Latin1_General_CS_AI AS ItemNumber,
    SUM((PricePerItem * QuantityOnHand)) as TotalInventoryValue
    FROM @inventory
    group by ItemNumber COLLATE Latin1_General_CS_AI) as inv
    ON inv.ItemNumber = I.ItemNumber
FULL OUTER JOIN @accounting as A
ON I.ItemNumber = A.ItemNumber
GROUP BY I.ItemNumber, A.TotalAccountingValue, A.ItemNumber, TotalInventoryValue
order by I.ItemNumber;