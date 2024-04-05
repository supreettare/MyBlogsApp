MyBlogsApp is a simple .NET Core-based microservices app that showcases how to use various features such as creating a partially implemented service that allows for creating new posts with comments and fetching posts by ID, along with the post comments.

The app also uses JWT tokens for authentication, rate-limits each API call, and uses Redis for caching to improve performance.

The entire app is containerized in a Docker container along with all its dependencies, including SQL Server and Redis.

The below high-level architecture diagram shows the various components of this app and how they are structured.

![image](https://github.com/supreettare/MyBlogsApp/assets/284847/b4b9ae37-006e-4041-b3e2-25f0c26e22c6)

Here is a brief summary of each of the components and what they do: 

- Client Apps
Browsers/Mobile Apps: Interface through which users interact with the application. They send requests to and receive responses from the Application Layer.

- Application Layer - WebApp
Posts Controller: Acts as an API endpoint; it processes incoming HTTP requests, invokes the appropriate actions in the Service Layer, and returns HTTP responses.

- Services - CQRS Pattern:
    - IPostCommandService: Handles all write operations (create, update, delete) for posts and comments, following the Command aspect of CQRS.
    - IPostQueryService: Manages read operations (retrieve posts and comments), embodying the Query aspect of CQRS.

- Data Layer
    - MS SQL Server: Stores, retrieves, and manages post and comment data as the primary persistent data store.
    - Redis Server: Provides a fast, in-memory data store for caching, reducing the number of queries to the SQL database and improving read performance.

- Infrastructure Layer
    - Docker: A platform used to containerize and isolate the Application and Data Layers, ensuring consistency and ease of deployment across different environments.
    - Each component operates within its scope, collaboratively supporting the application's functionality while adhering to the separation of concerns dictated by the CQRS pattern.


**Running the app: **
- You can run & test the app via Postman. To do this, follow the steps below. 
1. Download the Postman package json from here https://github.com/supreettare/MyBlogsApp/blob/master/EmpowerIDDemoSupreet.postman_collection.json
2. Import the postman collectoion package to your Postman app.
3. You will notice 3 APIs, use them in the below order: 
    a.) http://empiddemo.centralindia.azurecontainer.io:8080/api/auth/token => Run this to get an Auth Token 
    b.) http://empiddemo.centralindia.azurecontainer.io:8080/api/posts/ => Use this api to create new Blog Posts with comments, following JSON pattern is used 

{
  "title": "Azure DB Entry 1",
  "content": "This should work",
  "comments": [
    {
      "author": "Supreet",
      "content": "Does this work?"
    }
  ]
}

    c.) http://empiddemo.centralindia.azurecontainer.io:8080/api/posts/{id} => Use this API to fetch Posts with comments by passing the Id. 

**Additional Notes: **

- The first time the posts will be fetched from the DB & then will be cached. 
- The default rate limit to APIs is 5 APIs per minute 


 

 


