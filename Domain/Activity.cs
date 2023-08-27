// THIS ARE NOT NECESSARY ANYMORE BECAUSE OF THE IMPLICIT USING IN OUR CONFIGS

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// In the context of software development and Domain-Driven Design (DDD), 
// the term "domain" refers to the specific subject area or problem space that the software 
// is intended to address. It represents the real-world business or problem domain that 
//the software application is built to model, solve, or support.


// Here we defined the Entities
// entity is an object or a concept that represents a specific, distinguishable item of interest
// in the domain of the application. It can be a real-world object, such as a person, a product,
// a customer, or an event, or it can be an abstract concept, such as a user session, a transaction,
// or a permission.
// Entities are fundamental building blocks used to model the data and behaviors of the application


//  GUID property means Global Unique Identifier

// ENTITY FRAMEWORK NEEDS TO LOOK FOR SPECIFIC NAME "Id" to understand its going to be the primary key of the db table
// ALSO IT NEEDS TO EACH PROPERTY TO BE PUBLIC!


// In the context of databases, a schema refers to a logical container or namespace that 
// holds objects, such as tables, views, indexes, and other database elements. It provides a
//  way to organize and categorize database objects, making it easier to manage and access them.
namespace Domain
{

    // Entity framework needs all this properties to be public
    public class Activity
    {
        public Guid Id { get; set; }

        public string Title {get; set;}

        public DateTime Date {get; set;}

        public string Description {get; set;}

        public string Category {get; set;}

        public string City {get; set;} 

        public string Venue {get; set;}
    
    }
}