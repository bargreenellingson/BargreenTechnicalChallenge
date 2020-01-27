# Bargreen-Ellingson Technical Aptitude Challenge ooh la la la

The tasks described below are meant to gauge your experience and expertise in scenarios you may actually encounter at Bargreen. 

You should begin by forking this repository.  When you've completed as much as possible, open a pull request in the original repo so we can review your changes. In addition to leaving comments in your code, feel free to leave comments in your PR about how you approached the problem, how you followed SOLID principles, or other useful context.

#### Context
This solution models a theoretical world where items are stored in a warehouse, and there is an accounting system that keeps track of the current value of all items in inventory. 
Unfortunately, the system that tracks warehouse quantities by location is separate from the accounting system that tracks values. Sometimes these 2 systems get out of sync and so we need a way to reconcile them! 
The **InventoryService** class contains two classes that model the primary domain objects of these 2 systems. 


## Please complete the following tasks to the best of your ability:

#### In the Bargreen.sln solution in the csharp folder:
  - Update the **InventoryController** to run asynchronously following the async/await pattern
  - Modify the **Bargreen.API** project and **InventoryController** to use dependency injection
  - Implement the method **ReconcileInventoryToAccounting** in the **InventoryService** class. The method should return information that indicates where inventory balances match or mismatch the accounting balances. 
  - Write unit tests to verify the output of the algorithm you wrote in the step before.
#### In the sql folder...
  - Write a SQL query to compare inventory balances to accounting records and find discrepancies
#### Also...
  - Refactor any methods or architecture as you see fit based on best practices. Be sure to explain what/why in your PR comments.


When you are done, the csharp solution should build successfully and all tests should pass.


