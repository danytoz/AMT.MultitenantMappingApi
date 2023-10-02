This is a generic repository that sets the actual contract of what the repository can actually do. 
in this case I am defining that the object that represents the object in the DB can have any type of ID
It could be a unique identifier, string value, int value, as long as it can be used as a primary key.
It also defines basic functionality for each object that we should be able to do, for example find object by its ID
get all, find objects by filter, find the first value based on a filter.
It is currently using EF core but applying the interface we could easily switch this to a different type of ORM like Nhibernate
The BaseModel is the minimal properties the objects should have. 
