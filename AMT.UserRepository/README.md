Unit of work and Repository pattern are design patterns used in development, specially in applications with Databases.
These pattern provide structure and organized approach to manage data access, improve maintainability and promoting 
separation of concerns.
The unit of work pattern is used to managed transactions and the connection to a database.
The repository pattern provides an abstraction layer over data access operations. It separates the data access code 
from the business logic. 
In this project we implement the Generic Repository and apply it to each object within the database.
the Unit of work will be used by any application through Dependency Injection to have access to the functionality
of each object.
