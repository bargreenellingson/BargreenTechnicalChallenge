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


-- So to reconcile matches and differences between inventory and accounting tables we can write a query that selects the data from one table, 
-- stores it in a variable and compare it with the other table, then perform an update statement if there's a mismatch. We can also add SQL triggers
-- using transactions that updates the accounting balance everytime there's an update/insert happens on inventory balance.
SELECT * FROM ...