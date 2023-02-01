--Create a table to hold inventory balances:
declare @inventory table (
    ItemNumber varchar(50) not null,
    WarehouseLocation varchar(50) not null,
    QuantityOnHand int not null,
    PricePerItem decimal not null
)

--Create a table to hold accounting balances: 
declare @accounting table (
    ItemNumber varchar(50) not null,
    TotalInventoryValue decimal not null
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

-- Updates matching Item Numbers
UPDATE @accounting
SET TotalInventoryValue = (SELECT SUM(PricePerItem * QuantityOnHand)
                            FROM @inventory
                            WHERE @accounting.ItemNumber = @inventory.ItemNumber)

-- Adds missing item numbers to accounting
INSERT INTO @accounting
SELECT ItemNumber, SUM(PricePerItem * QuantityOnhand)
FROM @inventory inv
LEFT JOIN @accounting acc ON acc.ItemNumber = inv.ItemNumber
WHERE acc.ItemNumber IS NULL
Group By ItemNumber

-- Delete account balances not existing in inventory balances.
DELETE FROM @accounting
WHERE ItemNumber IN ( SELECT ItemNumber
                        FROM @accounting as acc
                        LEFT JOIN @inventory as inv
                        ON acc.ItemNumber = inv.ItemNumebr
                        WHERE inv.ItemNumber IS NULL)
