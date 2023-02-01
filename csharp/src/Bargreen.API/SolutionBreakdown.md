

# Bargreen Interview Problem

We're presented with a few requirements:
-First modify our API to utilize dependency injection and avoid tightly coupled classes.
-Second, modify controller methods to use the async/await patterns.
-And finally, third, create an inventory reconcilliation function that returns a list of items and their total value in hand and total value in accounting balance

## Problem 1, Dependency injection
I used a constructer dependency injection type and specified the implementation in the startup. This allows the InventoryController to be loosely coupled with the service and have the implementation specified at the startup.

## Problem 2, async/await patterns.
This one is more tricky. The implementation of the methods in our service is synchronous, so to modify our APIs to be asynchronous requires something special.
We're using Task.Run which is a bad practice especially in Web APIs for multitude of reasons.
Reasons being reducing performance/scalability by blocking threadpool, potentially causing deadlocks, etc.
However due to lack of naturally asynchronous code such as using a database context framework or a streamreader,
to convert our APIs to use the await/async pattern, I used Task.Run
It unblocks the current thread by offloading this task to another thread and allows for asynchronous execution.
In a real world implementation using something such as Entity Framework, we don't need Task.Run.

## Problem 3, reconciliation method
Here I did a simple LINQ query that generates a sum of item numbers in all of their regions by adding up their PricePerItem * QuantityInHand.
Then looping through account balances, checking if item number exists on hand, and creating a new InventoryReconciliation record with both values. Otherwise, creating a record with one value. I modified the class to have the value fields nullable as it seems that one list may not have another list's item number.

## Problem 4, reconciliation using SQL
We have an Update statement to update matching item numbers with latest balances.
We have an Insert statement to insert new inventory items into accounting with their balances.
We have a Delete statement that deletes records from accounting when it no longer exists in inventory.