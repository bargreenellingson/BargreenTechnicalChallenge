--Create a table to hold inventory balances:
declare @inventory table (
    ItemNumber varchar(50) not null,
    WarehouseLocation varchar(50) not null,
    QuantityOnHand int not null,
    PricePerItem decimal(18, 2) not null --Value was getting rounded up without this fix - I noticed when I was hand checking the results
)

--Create a table to hold accounting balances: 
declare @accounting table (
    ItemNumber varchar(50) not null,
    TotalInventoryValue decimal(18, 2) not null
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
/*
	Y = Yes
	N = No
*/
SELECT CASE
           WHEN [inv].[ItemNumber] IS NOT NULL THEN [inv].[ItemNumber]
           ELSE [act].[ItemNumber]
       END AS [ItemNumber]
     , ISNULL([inv].[QuantityOnHand], 0) AS [QuantityOnHand]
     , ISNULL([inv].[PricePerItem] * [inv].[QuantityOnHand], 0) [QuantityOnHandValue]
     , ISNULL([act].[TotalInventoryValue], 0) AS [AccountingInventoryValue]
     , CASE
           WHEN ISNULL([inv].[PricePerItem] * [inv].[QuantityOnHand], 0) <> ISNULL([act].[TotalInventoryValue], 0) THEN
               'N'
           ELSE 'Y'
       END AS [Match]
FROM
    (SELECT [i].[ItemNumber]
          , [i].[PricePerItem]
          , SUM([i].[QuantityOnHand]) [QuantityOnHand]
     FROM @inventory AS [i]
     GROUP BY [i].[ItemNumber]
            , [i].[PricePerItem]) AS [inv]
    FULL OUTER JOIN @accounting AS [act] ON [act].[ItemNumber] = [inv].[ItemNumber]
ORDER BY [Match] DESC