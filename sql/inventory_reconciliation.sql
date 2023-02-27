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
/**
1. Simplify Inventory table to add values that are in multiple warehouses. Format it to look like the accounting table
    a. take all values in the table, merge them appropriately, then add them into a new table
    I was not able to write and test my query because on my machine I was not able to set up and run a sql db. The following has not been thoroughly tested and Is not guaranteed to work.
**/
SELECT 
    inventory.ItemNumber,
    SUM(inventory.QuantityOnHand *
    inventory.PricePerItem) as ItemVal
FROM @inventory inventory
LEFT JOIN @accounting accounting on accounting.ItemNumber = inventory.ItemNumber
group by inventory.ItemNumber, accounting.TotalInventoryValue
HAVING SUM(inventory.QuantityOnHand * inventory.PricePerItem) != accounting.TotalInventoryValue;